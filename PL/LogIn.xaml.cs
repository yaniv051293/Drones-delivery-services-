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

namespace PL
{

    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private IBL BL;

        public MainWindow NewWindow;
        private string adminUsername = "Manager";
        private string adminPassword = "12345";
        private string userPassword;
        public LogIn()
        {
            try
            {
                InitializeComponent();
                BL = BLFactory.GetBL();
                passwordBox.Password = string.Empty;
                this.DataContext = BL.GetCustomersList();
                customerList_comboBox.Visibility = Visibility.Hidden;
            }
            catch (Exception e) { MessageBox.Show(e.Message); }
        }

        private void LogIn_btn(object sender, RoutedEventArgs e)
        {
            if ((string)Title.Content == "Welcome Manager") //login to admin
            {
                if (username_box.Text == adminUsername && passwordBox.Password == adminPassword)
                {
                    NewWindow = new MainWindow(BL);
                    passwordBox.Password = string.Empty;
                    NewWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong input , please try again");

                    passwordBox.Password = string.Empty;
                }
            }
            else // login from user customer
            {
                BO.CustomerToList tempCustomer = (BO.CustomerToList)customerList_comboBox.SelectedItem;
                
                if (tempCustomer != null)
                {
                    userPassword = tempCustomer.Id.ToString(); //user customer password will be as the user id
                }
                if (passwordBox.Password == userPassword)
                {
                    //check if we already opend this window

                    NewWindow = new MainWindow(BL, tempCustomer.Id);

                    NewWindow.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong input , please try again");

                    passwordBox.Password = string.Empty;
                }
            }
        }
      

        private void ChangeTitle(object sender, RoutedEventArgs e)
        {
            if (changeUser_btn.Content.ToString() == "Switch To User")
            {
                Title.Content = "Welcome User Name";
                changeUser_btn.Content = "Switch To Manager";
                customerList_comboBox.Visibility = Visibility.Visible;
                username_box.Visibility = Visibility.Hidden;

            }
            else // switch to admin
            {
                Title.Content = "Welcome Manager";
                changeUser_btn.Content = "Switch To User";
                customerList_comboBox.Visibility = Visibility.Hidden;
                username_box.Visibility = Visibility.Visible;
            }
        }
    }

}

