using DataModels;
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
    /// Interaction logic for OperationsDetailWindow.xaml
    /// </summary>
    public partial class OperationsDetailWindow : Window
    {
        public OperationsDetailWindow(List<Operation> Operations)
        {
            InitializeComponent();


        }
    }
}
