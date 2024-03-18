
namespace BO
{
    public class ParcelInDelivery
    {
        public int Id { get; set; }
        public bool AtWay { get; set; }
        public WeightCategories Wieght { get; set; }
        public Priorities Priority { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public Location SenderLocation { get; set; }
        public Location targetLocation { get; set; }
        public double Distance { get; set; }

        public ParcelInDelivery(int id, WeightCategories wieght, Priorities priority)
        {
            Id = id; ; Wieght = wieght; Priority = priority;
        }

        public override string ToString()
        {
            string str = "\nId: " + Id + "\n\nWieght: " + Wieght + "\n\nPriority: " + Priority+
          "\n\nSender: " + Sender + "\nlocation: " + SenderLocation +
          "\n\nTarget: " + Target + "\nlocation: " + targetLocation +
          "\n\nDistance: " + Distance.ToString("0.##") ;
            if (AtWay)
                str = str + "\n\nParcel status: The parcel at way.";
            else
                str = str + "\n\nParcel status: The parcel isn't deliver yet.\n";
            return str;
        }
    }
}