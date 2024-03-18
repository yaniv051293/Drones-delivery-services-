using System;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Target { get; set; }
        public WeightCategories Wieght { get; set; }
        public Priorities Priority { get; set; }
        public DroneInParcel HisDrone { get; set; }
        public DateTime? Requested { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public Parcel(int id, CustomerInParcel sender, CustomerInParcel target,
                      WeightCategories weight, Priorities priority,
                      DateTime? requested = null, DateTime? scheduled = null,
                      DateTime? pickedUp = null, DateTime? delivered = null,
                      DroneInParcel hisDrone = null)
        {
            Id = id; Sender = sender; Target = target;
            Wieght = weight; Priority = priority; HisDrone = hisDrone;
            Requested = requested; Scheduled = scheduled; PickedUp = pickedUp; Delivered = delivered;
        }

        public override string ToString()
        {
            string str = "Id: " + Id + ".\nWieght: " + Wieght + "  Priority: " + Priority +
                "\nSender: " + Sender.ToString() +
                "\nTarget: " + Target.ToString()
               + "\nRequested: " + Requested.ToString() + "   Scheduled: " + Scheduled.ToString()
               + "\nPickedUp: " + PickedUp.ToString() + "   Delivered: " + Delivered.ToString() + "\n";
            if (HisDrone != null)
                str += "Drone: " + HisDrone.ToString();
            return str;
        }
    }
}
