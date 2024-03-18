
namespace BO
{
    public class DroneInParcel
    {
        public int Id { get; set; }
        public double? Battery { get; set; }
        public Location CurrentLocation { get; set; }
        public DroneInParcel(int id , double? b = null, Location current = null)
        { Id = id; Battery = b; CurrentLocation = current; }
        public override string ToString()
        {
            return "Id: " + Id + ". Battery:" + Battery + " Current Location:" + CurrentLocation.ToString() + "\n";
        }
    }
}