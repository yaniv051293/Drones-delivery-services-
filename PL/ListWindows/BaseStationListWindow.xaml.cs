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
    /// Interaction logic for BaseStationListWindow.xaml
    /// </summary>
    public partial class BaseStationListWindow : Window
    {
        IBL bl;
        ObservableCollection<StationToList> updateList;
        public BaseStationListWindow(IBL b1)
        {
            InitializeComponent();
            bl = b1;
            updateList = new ObservableCollection<StationToList>(bl.GetStationsList());
            DataContext = updateList;
 
            var list = bl.GetStationsList().Select(s => s.AvailableSlots).Distinct().ToList();
            list.Sort();
            SlotsSelector.ItemsSource = list;
        }
        private void State_Selction(object sender, SelectionChangedEventArgs e)
        {
            BaseStationListView.ItemsSource = bl.GetSpecificStations((int)SlotsSelector.SelectedItem);
        }
        private void CloseStartWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(bl).Show();
            Close();
        }

        private void RefreshWindow_Click(object sender, RoutedEventArgs e)
        {
            BaseStationListView.ItemsSource = bl.GetStationsList();
        }
        private void StationListView_Click(object sender, MouseButtonEventArgs e)
        {
            if (BaseStationListView.SelectedItem != null)
            {
                StationToList s = (StationToList)BaseStationListView.SelectedItem;
                new BaseStationWindow(bl, s.Id).Show();
            }
        }
        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(bl).Show();
            Close();
        }

        /// <summary>
        /// delete the selected iteam form the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteClick(object sender, RoutedEventArgs e)
        {
            StationToList stationToList = (StationToList)BaseStationListView.SelectedItem;
            if (stationToList != null)
            {
                MessageBoxResult result = MessageBox.Show("Are sure you want to delete ?", "Delete box", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //asking if sure to delete
                if (result.ToString() == "Yes")
                {
                    bl.RemoveBaseStation(stationToList.Id);
                    updateList.Remove(stationToList);
                }
            }
        }
    }
}