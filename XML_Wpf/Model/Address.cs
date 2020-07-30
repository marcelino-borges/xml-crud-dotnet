using System.Collections.Generic;

namespace XML_Wpf.Model
{
    class Address
    {
        private string street;
        private string number;
        private string neighborhood;
        private string city;
        private string state;
        private string country;

        public Address(string street, string number, string neighborhood, string city, string state, string country)
        {
            this.Street = street;
            this.Number = number;
            this.Neighborhood = neighborhood;
            this.City = city;
            this.State = state;
            this.Country = country;
        }

        public Address()
        {
        }

        public string Street { get => street; set => street = value; }
        public string Number { get => number; set => number = value; }
        public string Neighborhood { get => neighborhood; set => neighborhood = value; }
        public string City { get => city; set => city = value; }
        public string State { get => state; set => state = value; }
        public string Country { get => country; set => country = value; }

        public override string ToString()
        {
            return "Address [ " +
                "Street: " + street +
                ", Number: " + number +
                ", Neighborhood: " + neighborhood +
                ", City: " + city +
                ", State: " + state +
                ", Country: " + country +
                " ]";
        }

        public override bool Equals(object obj)
        {
            return obj is Address address &&
                   Street == address.Street &&
                   Number == address.Number &&
                   Neighborhood == address.Neighborhood &&
                   City == address.City &&
                   State == address.State &&
                   Country == address.Country;
        }

        public override int GetHashCode()
        {
            int hashCode = -1412203064;
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(Street);
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(Number);
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(Neighborhood);
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(City);
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(State);
            hashCode *= -1521134295 + EqualityComparer<string>.Default.GetHashCode(Country);
            return hashCode;
        }
    }
}
