
namespace BO
{
    public class StationToList
    {
        public int Id { get; set; }//id 
        public string Name { get; set; }//
        public int AvailableSlots { get; set; }
        public int UsedSlots { get; set; }
        public StationToList(int id, string name, int aSlots, int uSlots)
        { Id = id; Name = name; AvailableSlots = aSlots; UsedSlots = uSlots; }

        public override string ToString()
        {
            return "Id: " + Id + ". Name: " + Name +
                ". Available Slots: " + AvailableSlots + " Used Slots:" + UsedSlots + "\n";
        }
    }
}