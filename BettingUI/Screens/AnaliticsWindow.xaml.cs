using DataModels;
using MahApps.Metro.Controls;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace BettingUI.Screens
{
    /// <summary>
    /// Interaction logic for AnaliticsWindow.xaml
    /// </summary>
    public partial class AnaliticsWindow : MetroWindow
    {
        List<string> competitions = new List<string>();
        List<SportMatch> sportMatches = new List<SportMatch>();
        List<Operation> bets = new List<Operation>();



        public AnaliticsWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var database = Database.OpenNamedConnection("main-database");
            List<SportMatch> competitionsDb = database.SportMatch.All().Select(database.SportMatch.Competition.Distinct());
            competitions = competitionsDb.Select(s => s.Competition).ToList();
            CompetitionsComboBox.ItemsSource = competitions;
        }

        private void RunAnalysisButton_Click(object sender, RoutedEventArgs e)
        {
            var database = Database.OpenNamedConnection("main-database");
            sportMatches = database.SportMatch.All();

            if (CompetitionsComboBox.SelectedValue != null)
            {
                sportMatches = sportMatches.Where(w => w.Competition == (string)CompetitionsComboBox.SelectedValue).ToList();
            }

            if (!ValidateFields())
            {
                return;
            }

            double.TryParse(this.MinimumOddBetTextbox.Text, out double minimumOddValue);
            double.TryParse(this.ProfitOnBetTextbox.Text, out double profitOnBetValue);
            double.TryParse(this.InitialMoneyTextbox.Text, out double initialMoneyValue);

            int i = 0;
            var currentMoney = initialMoneyValue;
            double profits = 0;
            int rechargesFromProfits = 0;

            List<Operation> operations = new List<Operation>();
            

            sportMatches = sportMatches.Where(w => w.HtOdd >= minimumOddValue || w.DrawOdd >= minimumOddValue || w.AtOdd >= minimumOddValue).OrderBy(o=> o.Date).ToList();
            foreach (var gameMatch in sportMatches)
            {
                i++;
                //this just gets the odd we are supposed to bet into, in this one we are skipping draws double bets on draws and wind
                dynamic bestOdd = new { Odd = -1, ExpectedResult = "I" }; ;

                if (gameMatch.HtOdd > gameMatch.DrawOdd && gameMatch.HtOdd > gameMatch.AtOdd)
                {
                    bestOdd = new { Odd = gameMatch.HtOdd, ExpectedResult = "H" };
                }

                if (gameMatch.DrawOdd > gameMatch.HtOdd && gameMatch.DrawOdd > gameMatch.AtOdd)
                {
                    bestOdd = new { Odd = gameMatch.DrawOdd, ExpectedResult = "D" };
                }

                if (gameMatch.AtOdd > gameMatch.HtOdd && gameMatch.AtOdd > gameMatch.DrawOdd)
                {
                    bestOdd = new { Odd = gameMatch.AtOdd, ExpectedResult = "A" };
                }

                if (bestOdd.Odd == -1)
                {
                    continue;  
                }

                //var totalLossPlusBetProfit = initialMoneyValue - currentMoney + profitOnBetValue;
                double waveLoss = operations.OrderByDescending(b => b.Id).TakeWhile(b => b.Win == false).Select(b => b.BetValue).Sum();
                var totalLossPlusBetProfit = waveLoss + profitOnBetValue;

                var betValue = Math.Round(totalLossPlusBetProfit / bestOdd.Odd,2);

                double rechargeValue = 0;
                if (currentMoney < betValue)
                {
                    if (profits < initialMoneyValue)
                    {
                        MessageBox.Show(string.Format("busted after {0}, in the game between {1} - {2}, with a profit of {3}", gameMatch.Date, gameMatch.HomeTeam, gameMatch.AwayTeam, profits));
                        break;
                    }
                    else
                    {
                        rechargesFromProfits++;
                        rechargeValue = initialMoneyValue - currentMoney;
                        profits -= rechargeValue;
                        currentMoney += rechargeValue;

                        operations.Add(new Operation() { Id = i, OperationType = OperationType.Recharge, InitialMoney = initialMoneyValue - waveLoss, AfterBetMoney = currentMoney, Profit = profits, Recharged = rechargeValue });
                        i++;

                    }
                }


                if (gameMatch.Result == bestOdd.ExpectedResult)
                {
                    currentMoney += betValue * bestOdd.Odd;
                    profits += currentMoney - initialMoneyValue;
                    operations.Add(new Operation() { Id = i, OperationType = OperationType.Bet, InitialMoney = initialMoneyValue - waveLoss, Odd= bestOdd.Odd, BetValue = betValue, AfterBetMoney = currentMoney, Win = true, Profit = profits });

                    currentMoney = initialMoneyValue;
                }
                else if (gameMatch.Result != bestOdd.ExpectedResult)
                {
                    currentMoney -= betValue;
                    operations.Add(new Operation() { Id = i, OperationType = OperationType.Bet, InitialMoney = initialMoneyValue - waveLoss, Odd = bestOdd.Odd, BetValue = betValue, AfterBetMoney = currentMoney, Win = false, Profit = profits });
                }

                rechargeValue = 0;

                if (currentMoney <= 0)
                {
                    if (profits < initialMoneyValue)
                    {
                        MessageBox.Show(string.Format("busted after {0}, in the game between {1} - {2}, with a profit of {3}", gameMatch.Date, gameMatch.HomeTeam, gameMatch.AwayTeam, profits));
                        break;
                    }
                    else
                    {
                        rechargesFromProfits++;
                        profits -= initialMoneyValue;
                    }
                }

                //moneyFlow.Add(currentMoney);

            }

            this.BetsGrid.ItemsSource = operations;

            MessageBox.Show(string.Format("Ended with {0}€ and a profit of {1}€ and {2} recharges", currentMoney.ToString(), profits, rechargesFromProfits));
            
        }


        private bool ValidateFields()
        {
            if (string.IsNullOrWhiteSpace(this.MinimumOddBetTextbox.Text))
            {
                MessageBox.Show("Please insert MinimumOddBet");
                return false;
            }


            if (string.IsNullOrWhiteSpace(this.ProfitOnBetTextbox.Text))
            {
                MessageBox.Show("Please insert ProfitOnBet");
                return false;
            }

            if (string.IsNullOrWhiteSpace(this.InitialMoneyTextbox.Text))
            {
                MessageBox.Show("Please insert InitialMoney");
                return false;
            }

            return double.TryParse(this.MinimumOddBetTextbox.Text, out double minimumOddValue) &&
                double.TryParse(this.ProfitOnBetTextbox.Text, out double profitOnBetValue) &&
                double.TryParse(this.InitialMoneyTextbox.Text, out double initialMoneyValue);

        }


    }
}
