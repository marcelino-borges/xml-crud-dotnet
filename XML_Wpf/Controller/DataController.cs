using System.Collections.Generic;
using System.Windows.Documents;
using System.Xml;
using XML_Wpf.Model;

namespace XML_Wpf.Controller
{
    class DataController
    {
        XmlDocument doc;

        public DataController() { }

        /// <summary>
        /// Instantiates a new Xml document
        /// </summary>
        /// <returns>New Xml document</returns>
        public XmlDocument CreateXMLDocument()
        {
            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            this.doc = doc;
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
                #region obsolete
                /*foreach (XmlNode att in attributes)
                {

                    if (att.Name.ToLower().Equals("id"))
                    {
                        user.Id = int.Parse(att.InnerText);
                    }
                    if (att.Name.ToLower().Equals("name"))
                    {
                        user.Name = att.InnerText;
                    }
                    if (att.Name.ToLower().Equals("lastname"))
                    {
                        user.LastName = att.InnerText;
                    }
                    if (att.Name.ToLower().Equals("email"))
                    {
                        user.Email = att.InnerText;
                    }
                    if (att.Name.ToLower().Equals("phone"))
                    {
                        user.Phone = att.InnerText;
                    }
                }*/
                #endregion
                users.Add(MountUserObjectFromXmlNode(item));
            }
            return users;
        }

        /// <summary>
        /// Returns the User found with the passed id number
        /// </summary>
        /// <param name="id">Id corresponding to the Searched user</param>
        /// <returns>New User with the passed id</returns>
        public User FindUserById(int id)
        {
            XmlNodeList list = doc.GetElementsByTagName("user");

            foreach (XmlNode item in list)
            { 
                if (int.Parse(item["id"].InnerText) == id)
                {
                    return MountUserObjectFromXmlNode(item);
                }
            }
            return null;
        }

        /// <summary>
        /// Finds an address from a given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Address object</returns>
        public Address FindAddressFromUserId(int id)
        {
            Address address = new Address();

            XmlNodeList list = doc.GetElementsByTagName("address");

            foreach (XmlNode item in list)
            {
                if (id == int.Parse(item.Attributes.GetNamedItem("userid").Value))
                {
                    address.Street = item["street"].InnerText;
                    address.Number = item["number"].InnerText;
                    address.Neighborhood = item["neighborhood"].InnerText;
                    address.City = item["city"].InnerText;
                    address.State = item["state"].InnerText;
                    address.Country= item["country"].InnerText;
                    return address;
                }
            }
            return null;
        }

        public void InsertUser()
        {

        }

        public void InsertAddress()
        {

        }

        public void DeleteUser()
        {

        }

        public void DeleteAddress()
        {

        }

        /// <summary>
        /// Update an existing user
        /// </summary>
        /// <param name="userId">Updating User's id</param>
        /// <param name="updatedUser">Updated User object</param>
        public void UpdateUser(int userId, User updatedUser)
        {
            XmlNodeList usersNode = doc.GetElementsByTagName("user");

            foreach (XmlNode item in usersNode)
            {
                if (int.Parse(item["id"].InnerText) == userId)
                {
                    item["name"].InnerText = updatedUser.Name;
                    item["lastname"].InnerText = updatedUser.LastName;
                    item["email"].InnerText = updatedUser.Email;
                    item["phone"].InnerText = updatedUser.Phone;
                    item["name"].InnerText = updatedUser.Name;
                }
            }

            XmlNodeList adressesNode = doc.GetElementsByTagName("address");

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

        }

        /// <summary>
        /// Mounts an user object from a XML user node
        /// </summary>
        /// <param name="node">XMLNode as an user</param>
        /// <returns>New user from the XML node</returns>
        private User MountUserObjectFromXmlNode(XmlNode node)
        {
            User user = new User();
            user.Id = int.Parse(node["id"].InnerText);
            user.Name = node["name"].InnerText;
            user.LastName = node["lastname"].InnerText;
            user.Email = node["email"].InnerText;
            user.Phone = node["phone"].InnerText;
            user.Address = FindAddressFromUserId(user.Id);
            return user;
        }
    }
}
