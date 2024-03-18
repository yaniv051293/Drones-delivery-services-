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
namespace PL
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IBL bl;
        private int customerId;

        public MainWindow(IBL blFromUser,int id)
        {
            try
            {
                InitializeComponent();
                bl = blFromUser;
                BtnCustomer();
                customerId = id;
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
            
        }
        public MainWindow(IBL blFromUser)
        {
            InitializeComponent();
            bl = blFromUser;
            BtnManger();
        }
        private void BtnCustomer()
        {
            manager.Visibility = Visibility.Hidden;
            Parcels.Visibility = Visibility.Hidden;
            Drones.Visibility = Visibility.Hidden;
            Customer.Visibility = Visibility.Hidden;
            BaseStation.Visibility = Visibility.Hidden;
        }
        private void BtnManger()
        {
            CustomerDetails.Visibility = Visibility.Hidden;
            AddParcel.Visibility = Visibility.Hidden;
        }
     

        private void ShowDronesButton_Click(object sender, RoutedEventArgs e)
        {
            new DronesListWindow(bl).Show();
            Close();
        }

        private void ShowParcelsButton_Click(object sender, RoutedEventArgs e)
        {
            new ParcelsListWindow(bl).Show();
            Close();
        }

        private void ShowCustomerButton_Click(object sender, RoutedEventArgs e)
        {
            new CustomerListWindow(bl).Show();
            Close();
        }

        private void ShowBaseStationButton_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationListWindow(bl).Show();
            Close();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            new LogIn().Show();
            Close();
        }

        private void ShowTabs_Click(object sender, RoutedEventArgs e)
        {
            new Tabs(bl).Show();
            Close();
        }

        private void CustomerInfo_Click(object sender, RoutedEventArgs e)
        {
            new CustomerWindow(bl, customerId,false).Show();
        }

        private void CustomerParcel_Click(object sender, RoutedEventArgs e)
        {
            Customer c = bl.GetCustomer(customerId);
            new ParcelWindow(bl, c).Show();
        }
    }
}