using System.Collections.Generic;

namespace DO
{
    public struct CustomerData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public bool Deleted { get; set; }

        public CustomerData(int id, string name, string phone, double longitude, double lattitude)
        { Id = id; Name = name; Phone = phone; Longitude = longitude; Lattitude = lattitude; Deleted = false; }
        //public CustomerData(CustomerData c)
        //{ Id = c.Id; Name = c.Name; Phone = c.Phone; Longitude = c.Longitude; Lattitude = c.Lattitude; deleled = c.deleled; }
        public CustomerData(int id, CustomerData c)
        { this = c; }

        

        public override string ToString()
        {
            return "Id: " + Id + " Name:" + Name + " Phone: " + Phone +
                " Longitude: " + Longitude + " Lattitude: " + Lattitude + "\n";
        }
    }
}

