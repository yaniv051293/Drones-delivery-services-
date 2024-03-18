using System.Collections.Generic;

namespace BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location StationLocation { get; set; }
        public int AvailableSlots { get; set; }
        public int BusySlots { get; set; } = 0;

        public List<DroneInCharge> dronesinCharge;
        public Station(int id = -1, string name = "", Location  l = null, int availableSlots =0, int BusyeSlots =0)
        {
            Id = id; Name = name; StationLocation = l; AvailableSlots = availableSlots; BusySlots = BusyeSlots;
            dronesinCharge = new List<DroneInCharge>();
        }
        public void AddDroneInCharge(DroneInCharge d)
        {
            dronesinCharge.Add(d);
        }

        public override string ToString()
        {
            string str = "id: " + Id + ". Name: " + Name + ". Location: " + StationLocation +
                    ".  Available Slots: " + AvailableSlots + ".  Slots in used:" + BusySlots + "\n";
            if (BusySlots != 0)
            {
                str += "Drones which in charge now: ";
                foreach (DroneInCharge d in dronesinCharge)
                {
                    str += d.ToString() + " ";
                }
            }
            return str;
        }
    }
}