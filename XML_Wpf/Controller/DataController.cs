using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using XML_Wpf.Model;

namespace XML_Wpf.Controller
{
    class DataController
    {
        private XmlDocument _doc;
        private List<User> _users;
        public static readonly string _DATA_FILE_NAME = @"\Database\Data.xml";
        public static readonly string _DATA_FILE_PATH = Path.Combine(Environment.CurrentDirectory.Replace(@"\bin\Debug","") + @"\" + _DATA_FILE_NAME);

        public XmlDocument Doc { get => _doc; set => _doc = value; }
        public List<User> Users { get => _users; set => _users = value; }

        public DataController() { }

        /// <summary>
        /// Instantiates a new Xml document
        /// </summary>
        /// <returns>New Xml document</returns>
        public XmlDocument CreateXMLDocument()
        {
            XmlDocument doc = new XmlDocument
            {
                PreserveWhitespace = true
            };
            this.Doc = doc;
            return doc;
        }

        /// <summary>
        /// Gets a list of Users from the XML
        /// </summary>
        /// <param name="doc">Xml document</param>
        /// <returns>List of Users</User></returns>
        public List<User> GetUsersInXml(XmlDocument doc)
        {
            List<User> users = new List<User>();
            XmlNodeList list = doc.GetElementsByTagName("user");

            foreach (XmlNode item in list)
            {
                users.Add(MountUserObjectFromXmlNode(item));
            }
            return users;
        }

        /// <summary>
        /// Returns the User found with the passed id number
        /// </summary>
        /// <param name="id">Id corresponding to the Searched user</param>
        /// <returns>New User with the passed id</returns>
        public User GetUserById(int id)
        {
            XmlNodeList list = Doc.GetElementsByTagName("user");

            foreach (XmlNode userNode in list)
            { 
                if (int.Parse(userNode["id"].InnerText) == id)
                {
                    return MountUserObjectFromXmlNode(userNode);
                }
            }
            return null;
        }

        /// <summary>
        /// Finds an address from a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Address object</returns>
        public Address GetAddressFromUserId(int id)
        {
            Address address = new Address();

            XmlNodeList list = Doc.GetElementsByTagName("address");

            foreach (XmlNode addressNode in list)
            {
                if (id == int.Parse(addressNode.Attributes.GetNamedItem("userid").Value))
                {
                    address.Street = addressNode["street"].InnerText;
                    address.Number = addressNode["number"].InnerText;
                    address.Neighborhood = addressNode["neighborhood"].InnerText;
                    address.City = addressNode["city"].InnerText;
                    address.State = addressNode["state"].InnerText;
                    address.Country= addressNode["country"].InnerText;
                    return address;
                }
            }
            return null;
        }

        /// <summary>
        /// Inserts a new user
        /// </summary>
        /// <param name="user">User to be inserted</param>
        /// <returns>XmlDocument</returns>
        public XmlDocument InsertUser(User user)
        {
            //User object properties
            XmlElement usersElement = (XmlElement)Doc.GetElementsByTagName("users")[0];
            usersElement.InnerXml = usersElement.InnerXml.Replace(usersElement.InnerXml, usersElement.InnerXml + "\t");
            XmlElement newUserElement = Doc.CreateElement("user");

            XmlElement idElement = Doc.CreateElement("id");
            idElement.InnerText = user.Id.ToString();
            XmlElement nameElement = Doc.CreateElement("name");
            nameElement.InnerText = user.Name;
            XmlElement lastNameElement = Doc.CreateElement("lastname");
            lastNameElement.InnerText = user.LastName;
            XmlElement emailElement = Doc.CreateElement("email");
            emailElement.InnerText = user.Email;
            XmlElement phoneElement = Doc.CreateElement("phone");
            phoneElement.InnerText = user.Phone;

            newUserElement.AppendChild(idElement);

            //Formatting XML
            newUserElement.InnerXml = newUserElement.InnerXml.Replace(idElement.OuterXml,
                "\n\t\t" + idElement.OuterXml +
                "\n\t\t" + nameElement.OuterXml + 
                "\n\t\t" + lastNameElement.OuterXml + 
                "\n\t\t" + emailElement.OuterXml +
                "\n\t\t" + phoneElement.OuterXml + "\n\t"
                );

            //Appending the user element to the userS element
            usersElement.AppendChild(newUserElement);

            usersElement.InnerXml = usersElement.InnerXml.Replace(usersElement.InnerXml, usersElement.InnerXml + "\n");

            InsertAddress(user.Id, user.Address);

            SaveDatabaseDocument();

            return Doc;
        }

        /// <summary>
        /// Inserts a new user address
        /// </summary>
        /// <param name="userId">Id of the user to whom the address belongs</param>
        /// <param name="address">The address to be inserted</param>
        private void InsertAddress(int userId, Address address)
        {
            //Address object properties
            XmlElement addressesElement = (XmlElement)Doc.GetElementsByTagName("addresses")[0];
            addressesElement.InnerXml = addressesElement.InnerXml.Replace(addressesElement.InnerXml, addressesElement.InnerXml + "\t");

            XmlElement newAddressElement = Doc.CreateElement("address");

            XmlAttribute userIdAttribute = Doc.CreateAttribute("userid");
            userIdAttribute.Value = userId.ToString();
            XmlElement streetElement = Doc.CreateElement("street");
            streetElement.InnerText = address.Street;
            XmlElement numberElement = Doc.CreateElement("number");
            numberElement.InnerText = address.Number;
            XmlElement neighborhoodElement = Doc.CreateElement("neighborhood");
            neighborhoodElement.InnerText = address.Neighborhood;
            XmlElement cityElement = Doc.CreateElement("city");
            cityElement.InnerText = address.City;
            XmlElement stateElement = Doc.CreateElement("state");
            stateElement.InnerText = address.State;
            XmlElement countryElement = Doc.CreateElement("country");
            countryElement.InnerText = address.Country;

            //Properties and attributes
            newAddressElement.Attributes.Append(userIdAttribute);
            newAddressElement.AppendChild(streetElement);

            newAddressElement.InnerXml = newAddressElement.InnerXml.Replace(streetElement.OuterXml,
                "\n\t\t" + streetElement.OuterXml +
                "\n\t\t" + numberElement.OuterXml +
                "\n\t\t" + neighborhoodElement.OuterXml +
                "\n\t\t" + cityElement.OuterXml +
                "\n\t\t" + stateElement.OuterXml +
                "\n\t\t" + countryElement.OuterXml + "\n\t"
                );

            addressesElement.AppendChild(newAddressElement);
            addressesElement.InnerXml = addressesElement.InnerXml.Replace(addressesElement.InnerXml, addressesElement.InnerXml + "\n");
        }

        /// <summary>
        /// Deletes an user from his id
        /// </summary>
        /// <param name="userId">Id of the user to be deleted</param>
        /// <returns>XmlDocument</returns>
        public XmlDocument DeleteUserFromId(int userId)
        {
            XmlNodeList list = Doc.GetElementsByTagName("user");
            XmlNode usersNode = Doc.GetElementsByTagName("users")[0];

            foreach (XmlNode userNode in list)
            {
                //Saving the previous user node
                XmlNode prevNode = userNode.PreviousSibling;

                if (int.Parse(userNode["id"].InnerText) == userId)
                {
                    Debug.WriteLine("usersNode.Name: " + usersNode.Name);
                    //Deleting user
                    usersNode.RemoveChild(userNode);
                    //Deleting any whitespace node before the deleted user node
                    RemoveWhitespaceNode(prevNode);

                    DeleteAddressFromUserId(userId);

                    SaveDatabaseDocument();
                }
            }

            return Doc;
        }

        /// <summary>
        /// Deletes an address from it's user id
        /// </summary>
        /// <param name="userId">Id of the user to whom this address belongs</param>
        private void DeleteAddressFromUserId(int userId)
        {
            XmlNodeList addressList = Doc.GetElementsByTagName("address");
            XmlNode addressesNode = Doc.GetElementsByTagName("addresses")[0];

            foreach (XmlNode addressNode in addressList)
            {
                if (userId == int.Parse(addressNode.Attributes.GetNamedItem("userid").Value))
                {
                    //Saving the previous address node
                    XmlNode prevNode = addressNode.PreviousSibling;

                    //Deleting address
                    Doc.DocumentElement.RemoveChild(addressNode);

                    //Deleting any whitespace node before the deleted address node
                    RemoveWhitespaceNode(prevNode);

                    SaveDatabaseDocument();
                }
            }
        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="userId">Updating User's id</param>
        /// <param name="updatedUser">Updated User object</param>
        public void UpdateUser(int userId, User updatedUser)
        {
            XmlNodeList usersNode = Doc.GetElementsByTagName("user");
            int index = _users.FindIndex(user => user.Id == updatedUser.Id);
            _users[index] = updatedUser;

            foreach (XmlNode item in usersNode)
            {
                if (int.Parse(item["id"].InnerText) == userId)
                {
                    XmlElement user = (XmlElement)item;
                    Debug.WriteLine("User being edited on XML: " + user["name"].InnerText);
                    user["name"].InnerText = updatedUser.Name;
                    user["lastname"].InnerText = updatedUser.LastName;
                    user["email"].InnerText = updatedUser.Email;
                    user["phone"].InnerText = updatedUser.Phone;
                    user["name"].InnerText = updatedUser.Name;
                    Debug.WriteLine("User after edited on XML: " + user["name"].InnerText);
                }
            }

            XmlNodeList adressesNode = Doc.GetElementsByTagName("address");

            foreach (XmlNode item in adressesNode)
            {
                if (userId == int.Parse(item.Attributes.GetNamedItem("userid").Value))
                {
                    item["street"].InnerText = updatedUser.Address.Street;
                    item["number"].InnerText = updatedUser.Address.Number;
                    item["neighborhood"].InnerText = updatedUser.Address.Neighborhood;
                    item["city"].InnerText = updatedUser.Address.City;
                    item["state"].InnerText = updatedUser.Address.State;
                    item["country"].InnerText = updatedUser.Address.Country;
                }
            }
            SaveDatabaseDocument();
        }

        /// <summary>
        /// Mounts an user object from a XML user node
        /// </summary>
        /// <param name="node">XMLNode as an user</param>
        /// <returns>New user from the XML node</returns>
        private User MountUserObjectFromXmlNode(XmlNode node)
        {
            User user = new User
            {
                Id = int.Parse(node["id"].InnerText),
                Name = node["name"].InnerText,
                LastName = node["lastname"].InnerText,
                Email = node["email"].InnerText,
                Phone = node["phone"].InnerText
            };
            user.Address = GetAddressFromUserId(user.Id);
            return user;
        }

        private void RemoveWhitespaceNode(XmlNode node)
        {
            if (node.NodeType == XmlNodeType.Whitespace || node.NodeType == XmlNodeType.SignificantWhitespace)
            {
                node.OwnerDocument.DocumentElement.RemoveChild(node);
            }
        }

        private void SaveDatabaseDocument()
        {
            Doc.Save(_DATA_FILE_PATH);
        }
    }
}
