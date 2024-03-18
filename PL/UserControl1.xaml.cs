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
using System.Collections.ObjectModel;
using BO;
using BlApi;
namespace PL
{
 
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        IBL drones;
        ObservableCollection<DroneToList> updateList = new ObservableCollection<DroneToList>();
        public UserControl1(IBL b1)
        {
            InitializeComponent();

            drones = b1;

            b1.GetDronesList().ToList().ForEach(i => updateList.Add(i));
            //copy list b1 to updatelist

            DataContext = updateList;

            StatusSelector.ItemsSource = Enum.GetValues(typeof(DroneStatus));
            MaxWightSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
        }

      

        /// <summary>
        /// Change the list order by the status
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void State_Selction(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = drones.GetSpecificDrones(x => x.Status == (DroneStatus)StatusSelector.SelectedItem);

        }
        /// <summary>
        /// open droneWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowDroneWindow_Click(object sender, RoutedEventArgs e)
        {
            new DroneWindow(drones).Show();
        }
        /// <summary>
        /// Change the list order by the wight 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Wight_Selction(object sender, SelectionChangedEventArgs e)
        {
            DronesListView.ItemsSource = drones.GetSpecificDrones(x => x.MaxWeight == (WeightCategories)MaxWightSelector.SelectedItem);
        }

        /// <summary>
        /// Open drone window with 2 sender parmeter drones and the select item.
        /// And close the window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DronesListView_Click(object sender, MouseButtonEventArgs e)
        {
            new DroneWindow(drones, (DroneToList)DronesListView.SelectedItem).Show();
           

        }
        /// <summary>
        /// Close the window .
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CloseStartWindow_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            
        }

        /// <summary>
        /// Refresh, and show all the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshWindow_Click(object sender, RoutedEventArgs e)
        {
            DronesListView.ItemsSource = drones.GetDronesList();
        }

        /// <summary>
        /// delete the selected iteam form the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Trach_Click(object sender, RoutedEventArgs e)
        {
            DroneToList droneToList = (DroneToList)DronesListView.SelectedItem;
            if (droneToList != null)
            {
                MessageBoxResult result = MessageBox.Show("Are sure you want to delete ?", "Delete box", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                //asking if sure to delete
                if (result.ToString() == "Yes")
                {
                    drones.RemoveDrone(droneToList.Id);
                    updateList.Remove(droneToList);


                }
            }
        }







    }
}
