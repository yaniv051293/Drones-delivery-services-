using System;

namespace DO
{
    public struct StationData
    {

        public int Id { get; set; }
        public string Name { get; set; } 
        public double Longitude { get; set; }
        public double Lattitude { get; set; }
        public int ChargeSlots { get; set; }
        public bool Deleted { get; set; }

        public StationData(int id = 0, string name = "", double longitude =0, double lattitude = 0, int chargeSlots =0)
    { Id = id; Name = name; Longitude = longitude; Lattitude = lattitude; ChargeSlots = chargeSlots; Deleted = false; }

        public StationData(StationData s)
        { this = s; }

        public static string DDtoDMS(double? coordinate, char longOrLan)
        {
            if (coordinate != null)
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
            return "0";
        }

        public override string ToString()
        {
            return "id: " + Id + " Name: " + Name + " Longitude: " + DDtoDMS(Longitude, 'G') +
                    " Latitude: " + DDtoDMS(Lattitude, 'T')  + " ChargeSlots: " + ChargeSlots + "\n";
        }
    }
}

