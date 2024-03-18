
namespace BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public int Sendetld { get; set; }
        public int Targetld { get; set; }
        public WeightCategories Wieght { get; set; }
        public Priorities Priority { get; set; }
        public ParcelStatus status { get; set; }
        public ParcelToList(int id, int sendetld, int targetld,
            WeightCategories wieght, Priorities priority, ParcelStatus s)
        {
            Id = id; Sendetld = sendetld; Targetld = targetld;
            Wieght = wieght; Priority = priority; status = s;
        }
        public override string ToString()
        {
            return "Id: " + Id + ".  Sendetld: " + Sendetld + ".  Targetld: " + Targetld +
                ".  Wieght: " + Wieght + "  Priority: " + Priority + " status:" + status + "\n";
        }
    }
}