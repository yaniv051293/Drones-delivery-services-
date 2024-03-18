using System.Collections.Generic;
namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public Location location { get; set; }
        public List<ParcelInCustomer> FromCustomer;
        public List<ParcelInCustomer> ToCustomer;

        public Customer(int id, string name, string phone, Location l)
        {
            Id = id; Name = name; Phone = phone; ; location = l;
            FromCustomer = new List<ParcelInCustomer>(); ToCustomer = new List<ParcelInCustomer>();
        }


        public override string ToString()
        {
            return "\nId:  " + Id + "\n\nName:  " + Name + "\n\nPhone:  " + Phone + "\n\nLocation:\n " + location;// + "\n"
               
        }
    }
}