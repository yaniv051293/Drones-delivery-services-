using System;
namespace BO
{
    public class Location
    {
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public Location(double la, double lo) { Lattitude = la; Longitude = lo; }
        public Location(Location loc) { Lattitude = loc.Lattitude; Longitude = loc.Longitude; }

        public static string DDtoDMS(double coordinate, char longOrLan)
        {

            // Set flag if number is negative
            bool neg = coordinate < 0d;

            // Work with a positive number
            coordinate = Math.Abs((double)coordinate);

            // Get d/m/s components
            double d = Math.Floor((double)coordinate);
            coordinate -= d;
            coordinate *= 60;
            double m = Math.Floor((double)coordinate);
            coordinate -= m;
            coordinate *= 60;
            double s = Math.Round((double)coordinate);

            // Create padding character
            char pad;
            char.TryParse("0", out pad);

            // Create d/m/s strings
            string dd = d.ToString();
            string mm = m.ToString().PadLeft(2, pad);
            string ss = s.ToString().PadLeft(2, pad);

            // Append d/m/s
            string dms = string.Format("{0}°{1}'{2}\"", dd, mm, ss);

            // Append compass heading
            switch (longOrLan)
            {
                case 'G':
                    dms += neg ? "W" : "E";
                    break;
                case 'T':
                    dms += neg ? "S" : "N";
                    break;
            }

            // Return formated string
            return dms;
        }

    
        public override string ToString()
        {
        return "Longitude-" + DDtoDMS(Longitude, 'G') + "\nLattitude-" + DDtoDMS(Lattitude, 'T'); }
    }
}
