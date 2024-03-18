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
using BO;
using BlApi;

namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationWindow.xaml
    /// </summary>
    public partial class BaseStationWindow : Window
    {
        IBL bl;
        public Station myStation { get; set; }
        public BaseStationWindow(IBL b1)
        {
            InitializeComponent();
            bl = b1;
            int[] arr = new int[5] { 1, 2, 3, 4, 5 };
            slots.ItemsSource=arr ;
            AddProprties.Visibility = Visibility.Visible;
            StationDetails.Visibility = Visibility.Hidden;
            Drones.Visibility = Visibility.Hidden;
            DronesListView.Visibility = Visibility.Hidden;
            UpdateStation.Visibility = Visibility.Hidden;
        }
        public BaseStationWindow(IBL b1,int stationId)
        {
            InitializeComponent();
            bl = b1;
            myStation = bl.GetBaseStation(stationId);
            DataContext = myStation;
            DronesListView.ItemsSource = myStation.dronesinCharge;
        }
        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x;//parm for using only
                double z;
                int id = int.TryParse(Id.Text, out x) ? x : throw new Exception("The id must be numbers");
                string nameStation = name.Text;
                double longtitude= double.TryParse(longtitud.Text, out z) ? z : throw new Exception("The longtitude must be numbers");
                double latitude = double.TryParse(latitud.Text, out z) ? z : throw new Exception("The latitude must be numbers");
                int availableSlots= int.TryParse(slots.Text, out x) ? x : throw new Exception("The id must be numbers");
                Station newStation = new Station(id, nameStation, new Location(longtitude,latitude), availableSlots, 0 );//create new station
                bl.AddBaseStation(newStation);
                new BaseStationListWindow(bl).Show();
                Close();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(bl);//stay in the window
            }
        }
        private void DronesListView_Click(object sender, MouseButtonEventArgs e)
        {
            if (DronesListView.SelectedItem != null)
            {
                DroneInCharge d = (DroneInCharge)DronesListView.SelectedItem;
                new DroneWindow(bl, d.Id).Show();
            }
        }

        private void UpdateStation_Click(object sender, RoutedEventArgs e)
        {
            UpdatePanel.Visibility = Visibility.Visible;
            slotsUpdate.ItemsSource = new int[5] { 1, 2, 3, 4, 5 };
        }

        private void SaveUpdate_Click(object sender, RoutedEventArgs e)
        {
            int i = int.Parse(slotsUpdate.Text);
            bl.UpdateBaseStation(myStation.Id, nameUpdate.Text,i );
            myStation = bl.GetBaseStation(myStation.Id);
            DataContext = myStation;
            UpdatePanel.Visibility=Visibility.Hidden;
        }
    }
}
