
namespace BO
{
    public class CustomerToList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public int ParcelsProvided { get; set; }
        public int ParcelsNotProvided { get; set; }
        public int ParcelsRecieved { get; set; }
        public int ParcelsNotRecieved { get; set; }
        public CustomerToList(int id, string name, string phone, int parcelsProvided = 0, int parcelsNotProvided = 0,
                                                                 int parcelsRecieved = 0, int parcelsNotRecieved = 0)
        {
            Id = id; Name = name; Phone = phone; ParcelsProvided = parcelsProvided; ParcelsNotProvided = parcelsNotProvided;
            ParcelsRecieved = parcelsRecieved; ParcelsNotRecieved = parcelsNotRecieved;
        }
        public override string ToString()
        {
            return "Id: " + Id + " Name:" + Name + " Phone: " + Phone +
                "\nParcels sent from customer- Provided:" + ParcelsProvided + ". Not Provided:" + ParcelsNotProvided
              + "\nParcels delivered to customer- Recieved:" + ParcelsRecieved + ". Not Recieved:" + ParcelsNotRecieved + "\n";
        }
    }
}