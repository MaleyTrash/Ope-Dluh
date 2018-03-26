using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace Dluh
{
    /// <summary>
    /// Interakční logika pro MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        ObservableCollection<Item> current;
        ObservableCollection<Item> future;

        DbHandler db;
        public MainWindow()
        {
            db = new DbHandler();
            InitializeComponent();
            UpdateLists();
        }

        private void UpdateLists()
        {
            current = new ObservableCollection<Item>(db.SelectWhere<Item>((i => i.Date != DateTime.MinValue)));
            future = new ObservableCollection<Item>(db.SelectWhere<Item>((i => i.Date == DateTime.MinValue)));
            aktList.ItemsSource = current;
            futList.ItemsSource = future;
            int futTotalValue = 0;
            foreach(Item i in future)
            {
                futTotalValue += i.Price;
            }
            futTotal.Content = "Celkem: " + futTotalValue.ToString()+",-";
            CheckValues();
            UpdateCurrentTotal();
        }

        private void CheckValues()
        {
            DateTime n = DateTime.Now;
            foreach(Item i in current)
            {
                double xd = (((n - i.ToDate).TotalDays / 30) * ((double)i.Multiplier / 100));
                if (DateTime.Compare(n, i.ToDate) > 0)
                {
                    double m = 1 + (((n - i.ToDate).TotalDays / 30) * ((double)i.Multiplier / 100));
                    MessageBox.Show(string.Format("Dluh za {0} překročil datum splacení! Přidán {1}% úrok.", i.Name, (int)Math.Round((m-1)*100)));
                    m *= i.Price;
                    i.Price = (int)Math.Round(m);
                }
            }
        }

        private void UpdateCurrentTotal()
        {
            if (!statsDate.SelectedDate.HasValue) return;
            int month = statsDate.SelectedDate.Value.Month;
            int year = statsDate.SelectedDate.Value.Year;
            List<Item> monthAll = current.Where(i => i.Date.Month.Equals(month)).ToList();
            List<Item> yearAll = current.Where(i => i.Date.Year.Equals(year)).ToList();
            int monthTotalValue = 0;
            foreach (Item i in monthAll)
            {
                monthTotalValue += i.Price;
            }
            int yearTotalValue = 0;
            int[] yearArray = Enumerable.Repeat(0, 12).ToArray();
            foreach (Item i in yearAll)
            {
                yearArray[i.Date.Month - 1] += i.Price;
                yearTotalValue += i.Price;
            }
            aktTotalMesic.Content = String.Format("Za měsíc ({0}): {1}", month, monthTotalValue);
            aktTotalRok.Content = String.Format("Za rok ({0}): {1}", year, yearTotalValue);
            UpdateGraph(year.ToString(), yearArray);
        }

        private void NewEntry(string name, int value, DateTime date, DateTime toDate, int mult)
        {
            db.Insert<Item>(new Item() { Name = name, Price = value, Date = date, ToDate = toDate, Multiplier = mult });
            UpdateLists();
        }

        private void aktButton_Click(object sender, RoutedEventArgs e)
        {
            string co = aktCo.Text;
            int kolik;
            int urok;
            try
            {
                kolik = int.Parse(aktKolik.Text);
                urok = int.Parse(aktUrok.Text);
            }
            catch
            {
                MessageBox.Show("POUZE ČÍSLA !!");
                return;
            }
            if(!aktKdy.SelectedDate.HasValue)
            {
                MessageBox.Show("Co to je za datum vole??");
                return;
            }
            if (!aktDoKdy.SelectedDate.HasValue)
            {
                MessageBox.Show("Co to je za datum vole??");
                return;
            }
            DateTime kdy = aktKdy.SelectedDate.Value;
            DateTime doKdy = aktDoKdy.SelectedDate.Value;
            NewEntry(co, kolik, kdy, doKdy, urok);
        }

        private void futButton_Click(object sender, RoutedEventArgs e)
        {
            string co = futCo.Text;
            int kolik;
            try
            {
                kolik = int.Parse(futKolik.Text);
            }
            catch
            {
                MessageBox.Show("POUZE ČÍSLA !!");
                return;
            }
            NewEntry(co, kolik, DateTime.MinValue, DateTime.MinValue, -1);
        }

        private void statsDate_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateCurrentTotal();
        }

        public void UpdateGraph(string title ,int[] values)
        {
            if (values.Length != 12) return;
            var col = new SeriesCollection
            {
                new ColumnSeries
                {
                    Title = title,
                    Values = new ChartValues<int>(values)
                }
            };
            grafSpodek.Labels = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12" };
            graf.Series = col;
        }
    }
}
