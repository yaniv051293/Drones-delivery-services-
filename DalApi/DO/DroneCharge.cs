using System;
namespace DO
{
    public struct DroneCharge
    {
        public int Droneld { get; set; }
        public int Stationld { get; set; }
        public DateTime TimeSet { get; set; }
        public DroneCharge(int droneld, int stationld) { Droneld = droneld; Stationld = stationld; TimeSet = DateTime.Now; }
        public override string ToString()
        {
            return "Droneld: " + Droneld + " Stationld: " + Stationld + "\n";
        }
    }
}


