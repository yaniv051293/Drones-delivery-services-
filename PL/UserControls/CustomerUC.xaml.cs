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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BO;
using BlApi;
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerUC.xaml
    /// </summary>
    public partial class CustomerUC : UserControl
    {
        ObservableCollection<CustomerToList> updateList = new ObservableCollection<CustomerToList>();

        static IBL interfaceBL;
        public CustomerUC()
        {
            InitializeComponent();
            CustomerListView.ItemsSource = interfaceBL.GetCustomersList();
        }

        
        public CustomerUC(IBL customers)
        {
            InitializeComponent();
            interfaceBL = customers;
            interfaceBL.GetCustomersList().ToList().ForEach(i => updateList.Add(i));
            DataContext = updateList;
            CustomerListView.ItemsSource = interfaceBL.GetCustomersList();

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(interfaceBL).Show();
          
        }

        private void CustomerListView_Click(object sender, MouseButtonEventArgs e)
        {
            if (CustomerListView.SelectedItem != null)
            {
                CustomerToList c=(CustomerToList)CustomerListView.SelectedItem;
                new CustomerWindow(interfaceBL, c.Id,true).Show();
               
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
                    interfaceBL.RemoveCustomer(customerToList.Id);
                    updateList.Remove(customerToList);
                }
            }
        }
        private void CloseStartWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(interfaceBL).Show();
            
        }
        private void RefreshWindow_Click(object sender, RoutedEventArgs e)
        {
            CustomerListView.ItemsSource = interfaceBL.GetCustomersList();
        }
    }
}
