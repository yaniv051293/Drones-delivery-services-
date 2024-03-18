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
using Microsoft.VisualBasic;
using BO;
using BlApi;
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for CustomerWindow.xaml
    /// </summary>
    public partial class CustomerWindow : Window
    {
        IBL bl;
        int id;
        public Customer myCustomer { get; set; }
        bool isManager;
        public CustomerWindow(IBL b1)
        {
            InitializeComponent();
            bl = b1;
            ParcelsListView.Visibility = Visibility.Hidden;
            ParcelsToListView.Visibility = Visibility.Hidden;
            customerDetails.Visibility = Visibility.Hidden;
            UpdateCustomer.Visibility = Visibility.Hidden;
            ParcelDetails.Visibility = Visibility.Hidden;
            ParcelDetails2.Visibility = Visibility.Hidden;
        }
        public CustomerWindow(IBL b1, int customerId,bool isManager)
        {
            InitializeComponent();
            bl = b1;
            id = customerId;

            myCustomer = bl.GetCustomer(customerId);
            DataContext = myCustomer;

            idT.Visibility = Visibility.Hidden;
            NameT.Visibility = Visibility.Hidden;
            phoneT.Visibility = Visibility.Hidden;
            LongtitudeT.Visibility = Visibility.Hidden;
            LatitudeT.Visibility = Visibility.Hidden;
            Id.Visibility = Visibility.Hidden;
            name.Visibility = Visibility.Hidden;
            phone.Visibility = Visibility.Hidden;
            Longtitude.Visibility = Visibility.Hidden;
            Latitude.Visibility = Visibility.Hidden;
            AddCustomer.Visibility = Visibility.Hidden;

            //CustomerDetails.Text += "\n" + myCustomer.ToString();
            ParcelsListView.ItemsSource = myCustomer.FromCustomer;
            ParcelsToListView.ItemsSource = myCustomer.ToCustomer;

            this.isManager = isManager;
        }

        private void AddCustomer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x;//parm for using only
                double z;
                int id = int.TryParse(Id.Text, out x) ? x : throw new Exception("The id must be numbers");
                double longtitude = double.TryParse(Longtitude.Text, out z) ? z : throw new Exception("The longtitude must be numbers");
                double latitude = double.TryParse(Latitude.Text, out z) ? z : throw new Exception("The latitude must be numbers");
                Customer newCustomer = new Customer(id,name.Text,phone.Text, new Location(latitude,longtitude));//create new drone
                bl.AddCustomer(newCustomer);

                new CustomerListWindow(bl).Show();
                Close();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new CustomerWindow(bl);//stay in the window
            }
        }

        private void UpdateCustomer_Click(object sender, RoutedEventArgs e)
        {
            UpdatePanel.Visibility = Visibility.Visible;
        }

        private void SaveUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bl.UpdateCustomer(myCustomer.Id, nameUpdate.Text,phoneUpdate.Text);//update the model
                myCustomer = bl.GetCustomer(id);
                DataContext = myCustomer;
                UpdatePanel.Visibility = Visibility.Hidden;
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                UpdatePanel.Visibility = Visibility.Hidden;
            }
        }
        private void ParcelsListView_Click(object sender, MouseButtonEventArgs e)
        {
            if (ParcelsListView.SelectedItem != null && isManager)
            {
                ParcelInCustomer p = (ParcelInCustomer)ParcelsListView.SelectedItem;
                new ParcelWindow(bl, p.Id).Show();
            }
        }
    }
}
