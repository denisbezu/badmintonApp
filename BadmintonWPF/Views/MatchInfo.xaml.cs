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

namespace BadmintonWPF.Views
{
    /// <summary>
    /// Interaction logic for MatchInfo.xaml
    /// </summary>
    public partial class MatchInfo : Window
    {
        public MatchInfo()
        {
            InitializeComponent();
        }
        public string Winner { get; set; }
        public bool WinnerChanged { get; set; }
        private void cancel_btn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void ok_btn_Click(object sender, RoutedEventArgs e)
        {
            if (radioButton1.IsChecked == true)
            {
                Winner = "";
                WinnerChanged = true;
                if (int.Parse(t1_1.Text) > int.Parse(t2_1.Text))
                    Winner += t2_1.Text + ", ";
                else
                    Winner += "-" + t1_1.Text + ", ";

                if (int.Parse(t1_2.Text) > int.Parse(t2_2.Text))
                    Winner += t2_2.Text;
                else
                    Winner += "-" + t1_2.Text;

                if (t1_3.Text != "" || t2_3.Text != "")
                {
                    if (int.Parse(t1_3.Text) > int.Parse(t2_3.Text))
                        Winner += ", " + t2_3.Text;
                    else
                        Winner += ", " + "-" + t1_3.Text;
                }
                Close();
            }
            else if (radioButton2.IsChecked == true)
            {
                Winner = "";
                WinnerChanged = true;
                if (int.Parse(t1_1.Text) < int.Parse(t2_1.Text))
                    Winner += t1_1.Text + ", ";
                else
                    Winner += "-" + t2_1.Text + ", ";

                if (int.Parse(t1_2.Text) < int.Parse(t2_2.Text))
                    Winner += t1_2.Text;
                else
                    Winner += "-" + t2_2.Text;

                if (t1_3.Text != "" || t2_3.Text != "")
                {
                    if (int.Parse(t1_3.Text) < int.Parse(t2_3.Text))
                        Winner += ", " + t1_3.Text;
                    else
                        Winner += ", " + "-" + t2_3.Text;
                }
                Close();
            }
            else
            {
                MessageBox.Show("Выберите победителя", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }
    }
}
