
namespace BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; }
        public DroneStatus Status { get; set; }
        public ParcelInDelivery Parcel { get; set; }
        public Location CurrentLocation { get; set; } 

        public Drone(int id, string model, WeightCategories weight, double battery = 0,
                 Location currect = null, DroneStatus status = DroneStatus.available, ParcelInDelivery p = null)
        {
            Id = id; Model = model; MaxWeight = weight; Battery = battery;
            Status = status; Parcel = p; CurrentLocation = currect; 
        }

        public override string ToString()
        {
             string str = "Id:" + Id + "\n\nModel: " + Model + "\n\nMaxWeight: " + MaxWeight + "\n\nBattery: " + Battery.ToString("#.#") + "\n\nDrone Status: " + Status +
                 "\n\nCurrent Location:\n" + CurrentLocation.ToString();
            return str;
        }
    }





}