using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
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
    /// Interaction logic for ParcelsListWindow.xaml
    /// </summary>
    public partial class ParcelsListWindow : Window
    {
        
        IBL intefaceParcel;
        public ParcelsListWindow(IBL parcels)
        {
            InitializeComponent();
            intefaceParcel = parcels;


            ParcelListView.ItemsSource = intefaceParcel.GetParcelsList();
            Priorities.ItemsSource = Enum.GetValues(typeof(Priorities));
            MaxWeight.ItemsSource= Enum.GetValues(typeof(WeightCategories));


            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Sendetld");
            view.GroupDescriptions.Add(groupDescription);



        }

        private void Priorities_Selction(object sender, SelectionChangedEventArgs e)
        {
            ParcelListView.ItemsSource = intefaceParcel.GetSpecificParcels((Priorities)Priorities.SelectedItem,null);
        }

        private void MaxWeight_Selction(object sender, SelectionChangedEventArgs e)
        {
            ParcelListView.ItemsSource = intefaceParcel.GetSpecificParcels(null,(WeightCategories)MaxWeight.SelectedItem);
        }

        private void Refrash_Click(object sender, RoutedEventArgs e)
        {
            ParcelListView.ItemsSource = intefaceParcel.GetParcelsList();
        }
        private void ParcelsListView_Click(object sender, MouseButtonEventArgs e)
        {
            if (ParcelListView.SelectedItem != null)
            {
                ParcelToList p = (ParcelToList)ParcelListView.SelectedItem;
                new ParcelWindow(intefaceParcel,p.Id ).Show();
            }
        }
        private void Close_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(intefaceParcel).Show();
            Close();
        }

        private void Add_ParcelWin_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(intefaceParcel).Show(); ;
        }

        private void ParcelListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
