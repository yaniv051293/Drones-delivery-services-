using System;

namespace DO
{
    public struct ParcelData
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int TargetId { get; set; }
        public WeightCategories Wieght { get; set; }
        public Priorities Priority { get; set; }
        public DateTime Requested { get; set; }
        public int? DroneId { get; set; }
        public DateTime? Scheduled { get; set; }
        public DateTime? PickedUp { get; set; }
        public DateTime? Delivered { get; set; }
        public bool Deleted { get; set; }

        public ParcelData(int id = -1, int senderId = -1, int targetId = -1, WeightCategories wieght = WeightCategories.Lightweight
                , Priorities priority = Priorities.Regular, int? droneId = null,
                DateTime? scheduled = null, DateTime? pickedUp = null, DateTime? delivered = null)
        {
            Id = id; SenderId = senderId; TargetId = targetId;
            Wieght = wieght; Priority = priority; DroneId = droneId;
            Requested = DateTime.Now; Scheduled = scheduled; PickedUp = pickedUp; Delivered = delivered; Deleted = false;
        }
        public ParcelData(ParcelData p)
        { this = p; }
        public override string ToString()
        {
            return "Id: " + Id + ".  Sendetld: " + SenderId + ".  Targetld: " + TargetId + ".  Wieght: " + Wieght + "  Priority: " + Priority
               + "\nRequested: " + Requested.ToString() + "  Droneld: " + DroneId
               + "\nScheduled: " + Scheduled.ToString()
               + "\nPickedUp: " + PickedUp.ToString() + "\nDelivered: " + Delivered.ToString() + "\n";
        }
    }

}

