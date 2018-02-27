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
        }

        private void NewEntry(string name, int value, DateTime date)
        {
            db.Insert<Item>(new Item() { Name = name, Price = value, Date = date });
            UpdateLists();
        }

        private void aktButton_Click(object sender, RoutedEventArgs e)
        {
            string co = aktCo.Text;
            int kolik;
            try
            {
                kolik = int.Parse(aktKolik.Text);
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
            DateTime kdy = aktKdy.SelectedDate.Value;
            NewEntry(co, kolik, kdy);
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
            NewEntry(co, kolik, DateTime.MinValue);
        }
    }
}
