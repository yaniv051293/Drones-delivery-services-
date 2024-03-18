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
using System.ComponentModel;
using System.Threading;

namespace PL
{
    /// <summary>
    /// Interaction logic for Drone.xaml
    /// </summary>
    public partial class DroneWindow : Window
    {
        IBL b1;

        bool stop = false;

        BackgroundWorker worker;
       
        public event EventHandler<RoutedEventArgs> needToRefreshScreenEvent;//for udate the list
        private RoutedEventArgs info;

        public Drone myDrone { get; set; }
        
        public DroneWindow(IBL dronel) // display Add Window
        {
            InitializeComponent();
            b1 = dronel;
            IdStation.ItemsSource = b1.GetAvailableStationsList().Select(x => x.Id);
            
            MaxWieghtSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            ///show all Weight Categories in the combox
            DroneDetails.Visibility = Visibility.Hidden;
            ParcelDetails.Visibility = Visibility.Hidden;
            Simulator.Visibility= Visibility.Hidden;
        }

        public DroneWindow(IBL bl, int IdDrone) // display Update Window
        {
            InitializeComponent();
            b1 = bl;
            myDrone= b1.GetDrone(IdDrone);
            DataContext = myDrone;  //Text of all details of drone

            worker = new BackgroundWorker();// start worker
            worker.DoWork += Work_Start;
            worker.WorkerReportsProgress = true;
            worker.ProgressChanged += Work_Screen;

            worker.RunWorkerCompleted += Work_Completed;

            AddProprties.Visibility = Visibility.Hidden;//hidden all first constractor
            UpdateModel.Visibility = Visibility.Visible;
            VisableUpdateButtuns();
        }

        private void VisableUpdateButtuns()
        {
            switch (myDrone.Status) //The buttun window to show
            {
                case DroneStatus.available:
                    SetToCharge.Visibility = Visibility.Visible;
                    LinkToParcel.Visibility = Visibility.Visible;
                    break;

                case DroneStatus.maintenance:
                    ReleaseFromCharge.Visibility = Visibility.Visible;
                    break;

                case DroneStatus.delivery:
                    ParcelDetails.Visibility = Visibility.Visible;
                    BO.Parcel parcel1 = b1.GetParcel(myDrone.Parcel.Id);
                    if (parcel1.PickedUp == null)
                        CollectParcel.Visibility = Visibility.Visible;
                    else
                        ParcelProvided.Visibility = Visibility.Visible;
                    break;

            }
        }
        private void DissableUpdateButtuns()
        {
            SetToCharge.Visibility = Visibility.Hidden;
            ReleaseFromCharge.Visibility = Visibility.Hidden;
            LinkToParcel.Visibility = Visibility.Hidden;
            CollectParcel.Visibility = Visibility.Hidden;
            ParcelProvided.Visibility = Visibility.Hidden;
        }

        private void Work_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
           
        }

        private void Work_Screen(object sender, ProgressChangedEventArgs e)
        {
            myDrone = b1.GetDrone(myDrone.Id);
            DataContext = myDrone;
            needToRefreshScreenEvent?.Invoke(this, info);
        }


        private void Work_Start(object sender, DoWorkEventArgs e)
        {
            Action refresh = () => worker.ReportProgress(1);
            try { b1.TurnOnSimulator(myDrone.Id, refresh, () => stop); }
            catch(Exception str) 
            {
                MessageBox.Show(str.Message);
                stop = true;
            }
        }

        private void Simulator_Click(object sender, RoutedEventArgs e)
        {
            if (worker.IsBusy==false)
            {
                worker.RunWorkerAsync();
                stop = false;
                Button change = sender as Button;
                change.Content = "Stop Simolator";
                change.Background = Brushes.DarkRed;
                UpdateModel.Visibility = Visibility.Hidden;
                DissableUpdateButtuns();
            }
            else
            {
                Button change = sender as Button;
                change.Content = "Start Simolator";
                change.Background = Brushes.ForestGreen;
                UpdateModel.Visibility = Visibility.Visible;
                stop = true;
                myDrone = b1.GetDrone(myDrone.Id);
                DataContext = myDrone;

            }
            needToRefreshScreenEvent?.Invoke(this, info);
        }

        /// <summary>
        /// The click button of add a drone to the list
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddDrone_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x;//parm for using only
                int id = int.TryParse(Id.Text, out x) ? x : throw new Exception("The id must be numbers");
                string model = Model.Text;
                WeightCategories Wc_input = (WeightCategories)MaxWieghtSelector.SelectedItem;
                int idStation = int.TryParse(IdStation.Text, out x) ? x : throw new Exception("The id_Station must be numbers");
                Drone newDrone = new Drone(id, model, Wc_input);//create new drone
                b1.AddDrone(newDrone, idStation);
             
                //new DronesListWindow(b1).Show();
                Close();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(b1);//stay in the window
            }
        }

        /// <summary>
        /// Update the model buttun
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Update_Click(object sender, RoutedEventArgs e)
        {
            int x = 500, y = 500;
            string model = Interaction.InputBox
                    ("Plese enter new  model name: ", "model box", "", x, y);
            try
            {
                
                b1.UpdateDrone(myDrone.Id, model);//update the model
                new DronesListWindow(b1).Show();
                Close();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(b1, myDrone.Id);//stay in the window
            }
        }

        /// <summary>
        /// Button that set the drone to charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetToCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                b1.SetDroneToCharge(myDrone.Id);
                UpdateScreen();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(b1, myDrone.Id);  //stay in the window
            }
        }

        /// <summary>
        /// Button Collect Parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CollectParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                b1.CollectParcel(myDrone.Id);
                UpdateScreen();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(b1, myDrone.Id);//stay in the window
            }

        }

        /// <summary>
        /// Button that Provided parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParcelProvided_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                b1.UpdateParcelProvided(myDrone.Id);
                UpdateScreen();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(b1, myDrone.Id);//stay in the window
            }

        }

        /// <summary>
        /// Button do Release From Charge
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ReleaseFromCharge_Click(object sender, RoutedEventArgs e)
        {
            try
            {            
                b1.ReleaseDroneFromCharge(myDrone.Id);
                UpdateScreen();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(b1, myDrone.Id);//stay in the window
            }

        }

        /// <summary>
        /// Button that ling Drone to parcel
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LinkToParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                b1.LinkParcelToDrone(myDrone.Id);
                UpdateScreen();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new DroneWindow(b1, myDrone.Id);//stay in the window
            }

        }

        /// <summary>
        /// Button close window
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            new DronesListWindow(b1).Show();
            Close();
        }

        /// <summary>
        /// update the dron details in DroneWindow and DroneListWindow after click on update click
        /// </summary>
        private void UpdateScreen()
        {
            needToRefreshScreenEvent?.Invoke(this, info);
            myDrone = b1.GetDrone(myDrone.Id);
            DataContext = myDrone;
            DissableUpdateButtuns();
            VisableUpdateButtuns();
        }
    }
}
