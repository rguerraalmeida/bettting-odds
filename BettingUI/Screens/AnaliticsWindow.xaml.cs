using BettingStrategies;
using BettingStrategies.Strategies;
using DataModels;
using DataModels.Strategies;
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

using OxyPlot;
using OxyPlot.Series;


namespace BettingUI.Screens
{
    /// <summary>
    /// Interaction logic for AnaliticsWindow.xaml
    /// </summary>
    public partial class AnaliticsWindow : MetroWindow
    {
        List<Operation> operations = null;

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
            MartingaleSample();
            //ComputeStrategies();

        }

        private void ComputeStrategies()
        {
            List<int> initialValues = new List<int>() { /*100, 200, 500, 1000, */ 2000 /*, 5000 */};
            List<double> minimumOdds = Enumerable.Range(30, 1).Select(x => x / 10.0).ToList();
            List<int> profitOnBets = new List<int>() { 100 /*35, 40, 45, 50, 60, 70, 75, 80*/ };
            List<SampleData> samples = new List<SampleData>();

            List<IStrategy> strategies = new List<IStrategy>() { new MartingaleStrategie() };

            var database = Database.OpenNamedConnection("main-database");
            List<SportMatch> sportmatches = database.SportMatch.All();


            var i = 0;
            var totalIterations = strategies.Count() * initialValues.Count() * minimumOdds.Count() * profitOnBets.Count();
            this.TotalIterationsTextbox.Text = totalIterations.ToString();

            foreach (var strategy in strategies)
            {
                foreach (var value in initialValues)
                {
                    foreach (var minimum in minimumOdds)
                    {
                        foreach (var profit in profitOnBets)
                        {
                            i++;
                            //CurrentIterationTextbox.Text = i.ToString();
                            var sample = new SampleData() { StrategyName = strategy.Name, InitialValue = value, OddValue = minimum, ProfitOnBet = profit };
                            var matchesdata = sportmatches.Where(w => w.HtOdd >= minimum || w.DrawOdd >= minimum || w.AtOdd >= minimum).OrderBy(o => o.Date).ToList();

                            strategy.Calculate(matchesdata, value, minimum, profit, out double maxbetvalue, out double totalprofits, out double riskFactor);

                            sample.MaxBetValue = maxbetvalue;
                            sample.TotalProfits = totalprofits;
                            sample.RiskFactor = Math.Round(riskFactor, 2);
                            samples.Add(sample);
                        }
                    }
                }
            }

            var sampled = samples.Where(s => s.TotalProfits > s.InitialValue);
            this.BetsGrid.ItemsSource = sampled;
            InformationTextblock.Text = string.Format("sampled data: {0}", sampled.Count());


            //sampledData
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





        List<string> competitions = new List<string>();
        List<SportMatch> sportMatches = new List<SportMatch>();
        List<Operation> bets = new List<Operation>();

        private void MartingaleSample()
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
            double.TryParse(this.InitialMoneyTextbox.Text, out double currentMoney);

            int i = 0;

            operations = new List<Operation>();


            sportMatches = sportMatches.Where(w => w.HtOdd >= minimumOddValue || w.DrawOdd >= minimumOddValue || w.AtOdd >= minimumOddValue).OrderBy(o => o.Date).ToList();
            foreach (var gameMatch in sportMatches)
            {
                i++;
                //this just gets the odd we are supposed to bet into, in this one we are skipping draws double bets on draws and wins
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

                var betValue = Math.Round(totalLossPlusBetProfit / bestOdd.Odd, 2);

                if (currentMoney < betValue)
                {
                    MessageBox.Show(string.Format("busted after {0}, in the game between {1} - {2}", gameMatch.Date, gameMatch.HomeTeam, gameMatch.AwayTeam));
                    break;
                }

                if (gameMatch.Result == bestOdd.ExpectedResult)
                {
                    var beforeMoney = currentMoney;
                    currentMoney += betValue * bestOdd.Odd;
                    operations.Add(new Operation() { Id = i, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.Odd, BetValue = betValue, AfterBetMoney = currentMoney, Win = true });
                }
                else if (gameMatch.Result != bestOdd.ExpectedResult)
                {
                    var beforeMoney = currentMoney;
                    currentMoney -= betValue;
                    operations.Add(new Operation() { Id = i, OperationType = OperationType.Bet, InitialMoney = beforeMoney, Odd = bestOdd.Odd, BetValue = betValue, AfterBetMoney = currentMoney, Win = false });
                }

                if (currentMoney <= 0)
                {
                    MessageBox.Show(string.Format("busted after {0}, in the game between {1} - {2}", gameMatch.Date, gameMatch.HomeTeam, gameMatch.AwayTeam));
                    break;
                }
            }

            this.BetsGrid.ItemsSource = operations;
            double.TryParse(this.InitialMoneyTextbox.Text, out double initialMoney);
            this.MaxBetValueTextblock.Text = Math.Round(operations.Max(s => s.BetValue), 2).ToString();
            this.ProfitsValueTextblock.Text = (Math.Round(operations.Last().AfterBetMoney - initialMoney, 2)).ToString();

            SetChartData();
        }


        private void SetChartData()
        {
            if (operations != null)
            {
                if (ChartOperationsValue.SelectedIndex == 0)
                {
                    ChartLineSeries.ItemsSource = new List<DataPoint>(operations.Select((value, index) => new DataPoint((double)index, value.AfterBetMoney)));
                }
                else
                {
                    ChartLineSeries.ItemsSource = new List<DataPoint>(operations.Select((value, index) => new DataPoint((double)index, value.BetValue)));
                }
            }
        }

        private void ChartOperationsValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SetChartData();
        }
    }
}
