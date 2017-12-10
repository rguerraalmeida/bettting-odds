using CsvHelper;
using DataModels;
using Microsoft.Win32;
using Simple.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfDataImporter
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<SportMatch> matches = new List<SportMatch>();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Csv files (*.csv)|*.csv|Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (openFileDialog.ShowDialog() == true)
            {
                this.openFilePath.Content = openFileDialog.FileName;

                this.dataGrid.ItemsSource = new List<SportMatch>();

                matches = new List<SportMatch>();

                using (TextReader reader = File.OpenText(openFileDialog.FileName))
                {
                    try
                    {
                        int ID = 0;
                        var ci = new CultureInfo("en-GB");
                        var formats = new[] { "dd/MM/yy", "dd/MM/yyyy", "d/MM/yy", "d/M/yy", "dd/M/yy", }
                            .Union(ci.DateTimeFormat.GetAllDateTimePatterns()).ToArray();
                        var csv = new CsvReader(reader);

                        while (csv.Read())
                        {
                            try
                            {
                                ID++;
                                var match = new SportMatch();

                                string compValue;
                                Mappings.Competitions.TryGetValue(csv.GetField<string>("Div"), out compValue);
                                match.Competition = compValue ?? csv.GetField<string>("Div");
                                // CultureInfo.GetCultureInfo("en-GB")
                                match.Date = DateTime.ParseExact(csv.GetField<string>("Date"), formats, ci, DateTimeStyles.AssumeLocal);
                                match.Epoca = GetEpocaByDate(match.Date);
                                match.HomeTeam = csv.GetField<string>("HomeTeam");
                                match.AwayTeam = csv.GetField<string>("AwayTeam");
                                match.HomeTeamGoals = csv.GetField<int>("FTHG");
                                match.AwayTeamGoals = csv.GetField<int>("FTAG");
                                match.Result = csv.GetField<string>("FTR");
                                match.HalfHGoals = csv.GetField<int>("HTHG");
                                match.HalfAGoals = csv.GetField<int>("HTAG");
                                match.HalfTimeResult = csv.GetField<string>("HTR");
                                // 23, 26, 29, 32, 35, 38, 41, 44, 47, 
                                match.HtOdd = GetHomeTeamOdd(csv);
                                match.DrawOdd = GetDrawOdd(csv);
                                match.AtOdd = GetAwayTeamOdd(csv);

                                match.ID = ID;

                                matches.Add(match);
                            }
                            catch (Exception) {}
                        }

                        this.dataGrid.ItemsSource = matches;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }


            }
        }

        private void SaveRecordsBtn_Click(object sender, RoutedEventArgs e)
        {
            if (matches != null && matches.Count() > 0)
            {
                matches.ForEach(m => m.ID = 0);

                var database = Database.OpenNamedConnection("main-database");
                List<SportMatch> databaseMatchs = database.SportMatch.All().ToList<SportMatch>();

                foreach (var match in matches)
                {
                    if (!databaseMatchs.Any(m => m.Date.Date == match.Date.Date
                        && m.HomeTeam == match.HomeTeam
                        && m.AwayTeam == match.AwayTeam))
                    {
                        database.SportMatch.Insert(match);
                    }


                }
            }

            MessageBox.Show("Save completed");
        }

        private string GetEpocaByDate(DateTime date)
        {
            string epoca = null;
            if (date != null)
            {
                if (date.Month >= 7)
                {
                    epoca = string.Format("{0}/{1}", date.ToString("yyyy"), date.AddYears(1).ToString("yyyy"));
                }

                else if (date.Month <= 6)
                {
                    epoca = string.Format("{0}/{1}", date.AddYears(-1).ToString("yyyy"), date.ToString("yyyy"));
                }
            }

            return epoca;
        }

        private double GetHomeTeamOdd(CsvReader csv)
        {
            double htOdd = 0.0;
            if (csv.TryGetField<double>("B365H", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("BSH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("BWH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("GBH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("IWH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("LBH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("PSH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("SOH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("SBH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("SJH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("SYH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("VCH", out htOdd)) return Math.Round(htOdd, 2);
            if (csv.TryGetField<double>("WHH", out htOdd)) return Math.Round(htOdd, 2);
            return htOdd;
        }

        private double GetDrawOdd(CsvReader csv)
        {
            double drawOdd = 0.0;
            if (csv.TryGetField<double>("B365D", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("BSD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("BWD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("GBD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("IWD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("LBD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("PSD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("SOD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("SBD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("SJD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("SYD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("VCD", out drawOdd)) return Math.Round(drawOdd, 2);
            if (csv.TryGetField<double>("WHD", out drawOdd)) return Math.Round(drawOdd, 2);
            return drawOdd;
        }

        private double GetAwayTeamOdd(CsvReader csv)
        {
            double atOdd = 0.0;
            if (csv.TryGetField<double>("B365A", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("BSA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("BWA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("GBA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("IWA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("LBA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("PSA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("SOA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("SBA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("SJA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("SYA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("VCA", out atOdd)) return Math.Round(atOdd, 2);
            if (csv.TryGetField<double>("WHA", out atOdd)) return Math.Round(atOdd, 2);
            return atOdd;
        }

    }
}

