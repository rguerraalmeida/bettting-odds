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


                matches = new List<SportMatch>();

                

                using (TextReader reader = File.OpenText(openFileDialog.FileName))
                {
                    try
                    {
                        var ci = new CultureInfo("en-GB");
                        var formats = new[] { "dd/MM/yy", "dd/MM/yyyy", "d/MM/yy", "d/M/yy", "dd/M/yy", }
                            .Union(ci.DateTimeFormat.GetAllDateTimePatterns()).ToArray();
                        var csv = new CsvReader(reader);


                        while (csv.Read())
                        {
                            var match = new SportMatch();

                            string compValue;
                            Mappings.Competitions.TryGetValue(csv.GetField<string>("Div"), out compValue);
                            match.Competition = compValue ?? csv.GetField<string>("Div");
                            // CultureInfo.GetCultureInfo("en-GB")
                            match.Date = DateTime.ParseExact(csv.GetField<string>("Date"), formats, ci, DateTimeStyles.AssumeLocal);
                            //match.Date = DateTime.Parse(csv.GetField<string>("Date"));
                            match.HomeTeam = csv.GetField<string>("HomeTeam");
                            match.AwayTeam = csv.GetField<string>("AwayTeam");
                            match.HomeTeamGoals = csv.GetField<int>("FTHG");
                            match.AwayTeamGoals = csv.GetField<int>("FTAG");
                            match.Result = csv.GetField<string>("FTR");
                            match.HalfHGoals = csv.GetField<int>("HTHG");
                            match.HalfAGoals = csv.GetField<int>("HTAG");
                            match.HalfTimeResult = csv.GetField<string>("HTR");
                            // 23, 26, 29, 32, 35, 38, 41, 44, 47, 
                            match.HtOdd = Math.Round(csv.GetField<double>("B365H"), 2);
                            match.DrawOdd = Math.Round(csv.GetField<double>("B365D"), 2);
                            match.AtOdd = Math.Round(csv.GetField<double>("B365A"), 2);

                            matches.Add(match);
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
                var database = Database.OpenNamedConnection("main-database");

                foreach (var match in matches)
                {
                    database.SportMatch.Insert(match);
                }
            }

            MessageBox.Show("Save completed");
        }
    }
}
