using System.Collections.Generic;

namespace XML_Wpf.Model
{
    class User
    {
        private int id;
        private string name;
        private string lastName;
        private string email;
        private string phone;
        private Address address;

        public User(string name, string lastName, string email, string phone)
        {
            Name = name;
            LastName = lastName;
            Email = email;
            Phone = phone;
        }

        public User() { }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string LastName { get => lastName; set => lastName = value; }
        public string Email { get => email; set => email = value; }
        public string Phone { get => phone; set => phone = value; }
        internal Address Address { get => address; set => address = value; }

        public override bool Equals(object obj)
        {
            return obj is User user &&
                   Id == user.Id &&
                   Name == user.Name &&
                   LastName == user.LastName &&
                   Email == user.Email;
        }



        public override string ToString()
        {
            return "User [ " +
                "ID: " + id +
                ", Name: " + name +
                ", LastName: " + lastName+
                ", Email: " + email +
                ", Phone: " + phone +
                " ]";
        }

        public override int GetHashCode()
        {
            int hashCode = 1124078070;
            hashCode *= -1521134295 + Id.GetHashCode();
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(Name);
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(LastName);
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(Email);
            return hashCode;
        }
    }
}
