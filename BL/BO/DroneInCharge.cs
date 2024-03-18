
namespace BO
{
    public class DroneInCharge
    {
        public int Id { get; set; }
        public double Battery { get; set; }

        public DroneInCharge(int id, double battery)
        { Id = id; Battery = battery; }
        public override string ToString()
        {
            return "Id: " + Id + ". Battery: " + Battery + "\n";
        }
    }
}