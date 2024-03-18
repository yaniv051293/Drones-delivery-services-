

namespace BO
{
    public class DroneToList
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatus Status { get; set; }
        public Location CurrentLocation { get; set; }
        public int? ParcelId { get; set; }
        public bool Deleted { get; set; }

        public DroneToList(int id, string model, WeightCategories weight, double battery = 0,
            DroneStatus status = DroneStatus.available, Location currentLocation = null, int? parcelId = null)
        {
            Id = id; Model = model; MaxWeight = weight; Battery = battery;
            Status = status; CurrentLocation = currentLocation; ParcelId = parcelId; Deleted = false;
        }

        public override string ToString()
        {
            string str = "Id: " + Id + ".  Model: " + Model + ".  MaxWeight: " + MaxWeight + ".  Battery:" + Battery +
                 ". Location: " + CurrentLocation + " Status: " + Status;
            if (Status == DroneStatus.delivery)
                str += ". Parcel Id: " + ParcelId;
            return str;
        }
    }
}