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
using BlApi;
using BO;
namespace PL
{
    /// <summary>
    /// Interaction logic for BaseStationUC.xaml
    /// </summary>
    public partial class BaseStationUC : UserControl
    {
        
       static IBL baseStation;
        public BaseStationUC()
        {
            InitializeComponent();
            BaseStationListView.ItemsSource = baseStation.GetStationsList();
            List<int> slots = new List<int>();
            var list = baseStation.GetStationsList().Select(s => s.AvailableSlots).Distinct().ToList();
            list.Sort();
            SlotsSelector.ItemsSource = list; 
        }

        public BaseStationUC(IBL baseStations)
        {
            
            baseStation = baseStations;
            
        }
        private void State_Selction(object sender, SelectionChangedEventArgs e)
        {
            BaseStationListView.ItemsSource = baseStation.GetSpecificStations((int)SlotsSelector.SelectedItem);
        }
        private void CloseStartWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(baseStation).Show();

        }

        private void RefreshWindow_Click(object sender, RoutedEventArgs e)
        {
            BaseStationListView.ItemsSource = baseStation.GetStationsList();
        }

        private void AddStation_Click(object sender, RoutedEventArgs e)
        {
            new BaseStationWindow(baseStation).Show();

        }
    }
}

