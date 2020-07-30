using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using XML_Wpf.Controller;
using XML_Wpf.Model;
using Path = System.IO.Path;

namespace XML_Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        XmlDocument _doc;
        List<User> _users;
        DataController _controller;
        static readonly string _DATA_FILE_NAME = "Data.xml";
        static readonly string _DATA_FILE_PATH = Path.Combine(Environment.CurrentDirectory + @"\" + _DATA_FILE_NAME);
        static readonly string _BUTTON_REGISTER_TEXT = "CADASTRAR";
        static readonly string _BUTTON_UPDATE_TEXT = "ATUALIZAR";

        public MainWindow()
        {
            InitializeComponent();
            _controller = new DataController();
            _doc = _controller.CreateXMLDocument();

            try
            {
                _doc.Load(_DATA_FILE_PATH);

                _users = _controller.GetUsersInXml(_doc);

                FillUsersListBox();

                //Console.WriteLine(doc.InnerXml);
            }
            catch (FileNotFoundException ex)
            {                
                Debug.WriteLine("--------------------------------------------------");
                Debug.WriteLine("Exception:\n" + ex.Message);
            }        
        }

        #region ButtonEvents

        private void buttonRegisterUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int userId = int.Parse(id_field.Text);
                _controller.UpdateUser(userId, _controller.FindUserById(userId));
            }
            catch(FormatException ex)
            {
                Debug.WriteLine(ex.Message);
            } finally
            {
                ClearForm();
            }            
        }

        private void buttonClear_Click(object sender, RoutedEventArgs e)
        {            
            ClearForm();
        }

        private void buttonSearchId_Click(object sender, RoutedEventArgs e)
        {            
            if (!string.IsNullOrEmpty(id_field.Text)) {
                int userId = int.Parse(id_field.Text);
                
                foreach (string item in lbUsers.Items.)
                {
                    int idOnList = int.Parse(item.Split(' ')[0].Replace(".", ""));

                }
            }
        }

        private void buttonEditUser_Click(object sender, RoutedEventArgs e)
        {
            string fullName = lbUsers.SelectedItem.ToString();
            if (!String.IsNullOrEmpty(fullName))
            {
                try
                {
                    int id = int.Parse(fullName.Split(' ')[0].Replace(".", ""));
                    FillFormWithUserData(_controller.FindUserById(id));
                    buttonRegister.Content = _BUTTON_UPDATE_TEXT;
                }
                catch (ArgumentException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        #endregion

        #region CustomMethods

        private bool CheckAllFieldsAreFilled()
        {
            foreach (TextBox tb in grid.Children.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(tb.Text))
                    return false;
            }
            return true;
        }

        private void ClearForm()
        {
            foreach (TextBox tb in grid.Children.OfType<TextBox>())
            {
                //Debug.WriteLine(tb.Name);
                tb.Text = "";
            }
            buttonRegister.Content = _BUTTON_REGISTER_TEXT;
        }
        private void FillUsersListBox()
        {
            foreach (User user in _users)
            {
                lbUsers.Items.Add(user.Id + ". " + user.Name + " " + user.LastName);
            }
        }

        private void FillFormWithUserData(User user)
        {
            //User personal data
            id_field.Text = user.Id.ToString();
            name_field.Text = user.Name;
            lastName_field.Text = user.LastName;
            email_field.Text = user.Email;
            phone_field.Text = user.Phone;
            //User address
            street_field.Text = user.Address.Street;
            number_field.Text = user.Address.Number;
            neighborhood_field.Text = user.Address.Neighborhood;
            city_field.Text = user.Address.City;
            state_field.Text = user.Address.State;
            country_field.Text = user.Address.Country;
        }

        #endregion
    }
}
