
namespace DO
{
    public struct DroneData
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public bool Deleted { get; set; }
        public DroneData(int id, string model, WeightCategories weight)
        {
            Id = id; Model = model; MaxWeight = weight; Deleted = false;
        }
        public override string ToString()
        {
            return "Id: " + Id + ". Model: " + Model + ". MaxWeight: " + MaxWeight + "\n";
        }
    }

}

