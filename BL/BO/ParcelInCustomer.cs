
namespace BO
{
    public class ParcelInCustomer
    {
        public int Id { get; set; }
        public WeightCategories Wieght { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus Status { get; set; }
        public CustomerInParcel Customer { get; set; }
        public ParcelInCustomer(int id, WeightCategories wieght, Priorities priority, ParcelStatus s, CustomerInParcel customer)
        {
            Id = id; Wieght = wieght; Priority = priority; Status = s; Customer = customer;
        }

        public override string ToString()
        {
            return "parcelId: " + Id + ".  Wieght: " + Wieght + "  Priority: " + Priority +
              "parcel status:" + Status + " Costumer:" + Customer + "\n";
        }


    }
}