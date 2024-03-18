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
namespace PL
{
    /// <summary>
    /// Interaction logic for ParcelWindow.xaml
    /// </summary>
    public partial class ParcelWindow : Window
    {
        IBL bl;
        bool isCustomerInterface = false;
        public Parcel myParcel { get; set; }
        /// <summary>
        /// costructor for add Parcel
        /// </summary>
        /// <param name="intefaceParcel"></param>
        public ParcelWindow(IBL intefaceParcel)
        {
            InitializeComponent();
            bl = intefaceParcel;
            ParcelDetails.Visibility = Visibility.Hidden;

            TargetId.ItemsSource = bl.GetCustomersList().Select(x => x.Id);
            PrioritySelctor.ItemsSource = Enum.GetValues(typeof(Priorities));
            MaxWieghtSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));
            
        }
        /// <summary>
        /// costructor for add Parcel
        /// </summary>
        /// <param name="intefaceParcel"></param>
        public ParcelWindow(IBL bl1,Customer sender)
        {
            InitializeComponent();
            bl = bl1;
            isCustomerInterface = true;

            IdSender.Text = sender.Id.ToString();
            IdSender.IsReadOnly=true;

            TargetId.ItemsSource = bl.GetCustomersList().Select(x => x.Id);
            PrioritySelctor.ItemsSource = Enum.GetValues(typeof(Priorities));
            MaxWieghtSelector.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            ParcelDetails.Visibility = Visibility.Hidden;
            CollectionButtun.Visibility= Visibility.Hidden;
        }
        public ParcelWindow(IBL intefaceParcel,int parcelId)
        {
            InitializeComponent();
            bl = intefaceParcel;
            myParcel = bl.GetParcel(parcelId);
            DataContext = this;  //Text of all details of drone
            AddProprties.Visibility = Visibility.Hidden;
        }

        private void AddParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int x;//parm for using only
                int sendId = int.TryParse(IdSender.Text, out x) ? x : throw new Exception("The id must be numbers");
                int targetId = int.TryParse(TargetId.Text, out x) ? x : throw new Exception("The id must be numbers");
                WeightCategories weight = (WeightCategories)MaxWieghtSelector.SelectedItem;
                Priorities priority = (Priorities)PrioritySelctor.SelectedItem;
                Parcel newParcel = new Parcel(0, new CustomerInParcel(sendId), new CustomerInParcel(targetId), weight, priority);
                bl.AddParcel(newParcel);
                if (!isCustomerInterface)
                    new ParcelsListWindow(bl).Show();
                Close();
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
                new ParcelWindow(bl);//stay in the window
            }
        }

        public DroneInParcel s { get; set; }
        private void DroneWindows(object sender, MouseButtonEventArgs e)
        {
          s = (sender as Button).DataContext as DroneInParcel;
          if(s!=null)
            new DroneWindow(bl, s.Id).Show();
        }

        private void Show_Click(object sender, RoutedEventArgs e)
        {
            CustomerInParcel s = (sender as Button).DataContext as CustomerInParcel;
            if (s != null)
                new CustomerWindow(bl, s.Id,true).Show();
        }

        private void CollectParcel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (myParcel.HisDrone != null)
                {
                    int id = myParcel.Id;
                    bl.CollectParcel(myParcel.HisDrone.Id);
                    myParcel = bl.GetParcel(id);
                    DataContext = this;
                }
            }
            catch (Exception str)
            {
                MessageBox.Show(str.Message);
            }
        }
    }
}
