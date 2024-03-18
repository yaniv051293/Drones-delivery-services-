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
using System.Collections.ObjectModel;

namespace PL
{
    /// <summary>
    /// Interaction logic for Parcel.xaml
    /// </summary>
    public partial class ParcelPL : UserControl
    {
        static IBL intefaceParcel;
        static ObservableCollection<ParcelToList> updateList = new ObservableCollection<ParcelToList>();

        public ParcelPL()
        {
            InitializeComponent();
            DataContext = updateList;
            ParcelListView.ItemsSource = intefaceParcel.GetFreeParcelList();

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ParcelListView.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Sendetld");
            view.GroupDescriptions.Add(groupDescription);



            Priorities.ItemsSource = Enum.GetValues(typeof(Priorities));
            MaxWeight.ItemsSource = Enum.GetValues(typeof(WeightCategories));

            



        }

        public ParcelPL(IBL parcels)
        {
            intefaceParcel = parcels;
            intefaceParcel.GetParcelsList().ToList().ForEach(i => updateList.Add(i));
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
            ParcelListView.ItemsSource = intefaceParcel.GetFreeParcelList();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow(intefaceParcel).Show();
       
        }

        private void Add_ParcelWin_Click(object sender, RoutedEventArgs e)
        {
            new ParcelWindow(intefaceParcel).Show(); 
        }

    }
}
