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
using BlApi;
using BO;
using System.Collections.ObjectModel;
namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerListWindow.xaml
    /// </summary>
    public partial class CustomerListWindow : Window
    {
        IBL bl;
        ObservableCollection<CustomerToList> updateList = new ObservableCollection<CustomerToList>();
        public CustomerListWindow(IBL b1)
        {
            InitializeComponent();
            bl = b1;
            b1.GetCustomersList().ToList().ForEach(i => updateList.Add(i));
            DataContext = updateList;
            CustomerListView.ItemsSource = b1.GetCustomersList();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl).Show();
            Close();
        }

        private void CustomerListView_Click(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListView.SelectedItem != null)
            {
                CustomerToList c = (CustomerToList)CustomerListView.SelectedItem;
                new CustomerWindow(bl,c.Id,true ).Show();
            }
        }

        private void deleteCustomer(object sender, RoutedEventArgs e)
        {
            CustomerToList customerToList = (CustomerToList)CustomerListView.SelectedItem;
            if (customerToList != null)
            {
                MessageBoxResult result = MessageBox.Show("Are sure you want to delete ?", "Delete box", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //asking if sure to delete
                if (result.ToString() == "Yes")
                {
                    bl.RemoveCustomer(customerToList.Id);
                    updateList.Remove(customerToList);
                }
            }
        }
        private void CloseStartWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(bl).Show();
            Close();
        }
        private void RefreshWindow_Click(object sender, RoutedEventArgs e)
        {
            CustomerListView.ItemsSource = bl.GetCustomersList();
        }
    }
}
