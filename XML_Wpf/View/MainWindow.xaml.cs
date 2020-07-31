using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
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
        readonly DataController _controller;
        static readonly string _BUTTON_REGISTER_TEXT = "CADASTRAR";
        static readonly string _BUTTON_UPDATE_TEXT = "ATUALIZAR";

        public MainWindow()
        {
            InitializeComponent();
            _controller = new DataController();
            _controller.CreateXMLDocument();
            LoadXmlDocument();
        }

        #region ButtonEvents

        /// <summary>
        /// Event of button click from the Register button
        /// </summary>
        private void ButtonRegisterUpdate_Click(object sender, RoutedEventArgs e)
        {
            string registerBtnName = buttonRegister.Content.ToString();

            if (registerBtnName.Equals(_BUTTON_UPDATE_TEXT))
            {
                //UPDATE
                try
                {
                    User updatedUser = CreateUserObjectFromForm();
                    _controller.UpdateUser(updatedUser.Id, updatedUser);

                    FillUsersListBox();
                    //_controller.Users = _controller.GetUsersInXml(_controller.Doc);
                }
                catch (FormatException ex)
                {
                    Debug.WriteLine(ex.Message);
                }
                finally
                {
                    ClearForm();
                }
            } else if(registerBtnName.Equals(_BUTTON_REGISTER_TEXT))
            {
                //CREATE
                if(CheckAllFieldsAreFilled())
                {
                    _controller.InsertUser(CreateUserObjectFromForm());
                    LoadXmlDocument();
                    FillUsersListBox();
                    ClearForm();

                }
            }
        }

        /// <summary>
        /// Event of button click from the Clear button
        /// </summary>
        private void ButtonClear_Click(object sender, RoutedEventArgs e)
        {            
            ClearForm();
        }

        /// <summary>
        /// Event of button click from the Search Id button
        /// </summary>
        private void ButtonSearchId_Click(object sender, RoutedEventArgs e)
        {            
            if (!string.IsNullOrEmpty(id_field.Text)) {
                try
                {
                    int userId = int.Parse(id_field.Text);

                    for (int i = 0; i < lbUsers.Items.Count; i++)
                    {
                        int idOnList = int.Parse(lbUsers.Items[i].ToString().Split(' ')[0].Replace(".", ""));

                        if (userId == idOnList)
                        {
                            Debug.WriteLine(idOnList);
                            lbUsers.SelectedItem = lbUsers.Items[i];
                            break;
                        }
                    }
                } catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Event of button click from the Edit button
        /// </summary>
        private void ButtonEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (lbUsers.SelectedItem != null)
            {
                string fullName = lbUsers.SelectedItem.ToString();
                if (!String.IsNullOrEmpty(fullName))
                {
                    try
                    {
                        int id = int.Parse(fullName.Split(' ')[0].Replace(".", ""));
                        FillFormWithUserData(_controller.GetUserById(id));
                        buttonRegister.Content = _BUTTON_UPDATE_TEXT;
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Event of button click from the Delete button
        /// </summary>
        private void ButtonDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (lbUsers.SelectedItem != null)
            {
                string fullName = lbUsers.SelectedItem.ToString();
                if (!String.IsNullOrEmpty(fullName))
                {
                    try
                    {
                        int id = int.Parse(fullName.Split(' ')[0].Replace(".", ""));
                        _controller.DeleteUserFromId(id);
                        LoadXmlDocument();
                        FillUsersListBox();
                        ClearForm();
                    }
                    catch (ArgumentException ex)
                    {
                        Debug.WriteLine(ex.Message);
                    }
                }
            }
        }

        /// <summary>
        /// Event of KeyUp from idField
        /// </summary>
        private void IdField_OnKeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (string.IsNullOrEmpty(id_field.Text))
                buttonSearchId.IsEnabled = false;
            else
                buttonSearchId.IsEnabled = true;
        }

        #endregion

        #region CustomMethods

        /// <summary>
        /// Loads an XML file from the default file path
        /// </summary>
        private void LoadXmlDocument()
        {
            try
            {
                _controller.Doc.Load(DataController._DATA_FILE_PATH);
                _controller.Users = _controller.GetUsersInXml(_controller.Doc);
                FillUsersListBox();
            }
            catch (FileNotFoundException ex)
            {
                Debug.WriteLine("Exception:\n" + ex.Message);
            }
        }

        /// <summary>
        /// Creates an User object from the window form
        /// </summary>
        /// <returns>User object</returns>
        private User CreateUserObjectFromForm()
        {
            Address address = new Address
            {
                Street = street_field.Text,
                Number = number_field.Text,
                Neighborhood = neighborhood_field.Text,
                City = city_field.Text,
                State = state_field.Text,
                Country = country_field.Text
            };

            User user = new User
            {
                Id = int.Parse(id_field.Text),
                Name = name_field.Text,
                LastName = lastName_field.Text,
                Email = email_field.Text,
                Phone = phone_field.Text,
                Address = address
            };
            return user;
        }

        /// <summary>
        /// Checks if all the form fields were filled
        /// </summary>
        /// <returns>Whether the fields are filled or not</returns>
        private bool CheckAllFieldsAreFilled()
        {
            foreach (TextBox tb in grid.Children.OfType<TextBox>())
            {
                if (string.IsNullOrEmpty(tb.Text))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Clears all the fields in the form
        /// </summary>
        private void ClearForm()
        {
            foreach (TextBox tb in grid.Children.OfType<TextBox>())
            {
                //Debug.WriteLine(tb.Name);
                tb.Text = "";
            }
            buttonRegister.Content = _BUTTON_REGISTER_TEXT;
        }

        /// <summary>
        /// Loads all the users from database to the list box in the window
        /// </summary>
        private void FillUsersListBox()
        {
            foreach (User user in _controller.Users)
            {
                lbUsers.Items.Add(user.Id + ". " + user.Name + " " + user.LastName);
            }
        }

        /// <summary>
        /// Fills all fields of the form with an user's personal data
        /// </summary>
        /// <param name="user">User object</param>
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
