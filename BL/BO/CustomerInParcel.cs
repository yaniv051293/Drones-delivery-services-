
namespace BO
{
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public CustomerInParcel(int id, string name = "")
        {
            Id = id; Name = name;
        }
        public override string ToString()
        {
            return "Id: " + Id + " Name: " + Name + " ";
        }
    }
}