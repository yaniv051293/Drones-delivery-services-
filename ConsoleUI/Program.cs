using System;
using DO;
using DalApi;

namespace ConsoleUI
{
    public class Program
    {
        static void Main(string[] args)
        {
            IDal data = DalFactory.GetDal();
            Console.WriteLine("wellcome :)\n" +
                        "for adding options press 1 \n" +
                        "for update options press 2\n" +
                        "for view options press 3\n" +
                        "for view list of data options press 4\n" +
                        "for remove options press 5\n" +
                        "to end all press 6\n");

            string choose = Console.ReadLine();
            while (choose != "6")
            {
                switch (choose)
                {
                    case "1":
                        Add(ref data); break;
                    case "2":
                        Update(ref data); break;
                    case "3":
                        Display(data); break;
                    case "4":
                        ShowList(data); break;
                    case "5":
                        Remove(data); break;
                    default:
                        Console.WriteLine("EROOR Input try again\n");
                        break;
                }
                Console.WriteLine("\nfor adding options press 1 \n" +
                                    "for updade options press 2\n" +
                                    "for view options press 3\n" +
                                    "for view list of data options press 4\n" +
                                    "for remove options press 5\n" +
                         "to end all press 6\n");
                choose = Console.ReadLine();
            }
        }
        private static void Add(ref IDal data)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine(
                                  "to add basestation press 1\n" +
                                  "to add dorne press 2\n" +
                                  "to add costumer press 3\n" +
                                  "to add parcel press 4\n" +
                                  "to return to main menu press 5\n");
                string inputForEnd = Console.ReadLine();
                switch (inputForEnd)
                {
                    case "1":
                        {
                            //Input of basestation
                            try
                            {
                                data.AddBaseStation(GetStation());
                                Console.WriteLine(" success !");
                                flag = false;//break the while
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }

                    case "2":
                        {
                            //Input of the drone
                            try
                            {
                                data.AddDrone(GetDrone());
                                Console.WriteLine(" success !");
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to add agian ?(yes or no):");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "3":
                        {
                            //Input of the costumer
                            try
                            {
                                data.AddCustomer(GetCustomer());
                                Console.WriteLine(" success !");
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "4":
                        {
                            try
                            {
                                data.AddParcel(GetParcel(0));
                                Console.WriteLine(" success !");
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to add again ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "5":
                        return;
                    default:
                        Console.WriteLine("ERROR input ");
                        break;
                }
            }
        }
        private static ParcelData GetParcel(int id)
        {
            int sendId, targetId;
            do { Console.WriteLine("Enter send's ld:"); }
            while (!int.TryParse(Console.ReadLine(), out sendId));
            do { Console.WriteLine("Enter target's id:"); }
            while (!int.TryParse(Console.ReadLine(), out targetId));
            WeightCategories weight = GetWeightCategories();
            Priorities priority = GetPriorities();
            ParcelData newParcel = new ParcelData(id, sendId,targetId, weight, priority);
            return newParcel;
        }
        static Priorities GetPriorities()
        {

            Console.WriteLine(
                    "Enter Priorities\n" +
                    "press 0 for Emergency\n " +
                    "press 1 for Regular\n " +
                    "press 2 for Fast\n "
                    );
            Priorities choose = 0;
            int w;
            bool flag = true;
            while (flag)
            {
                choose = (Priorities)(int.TryParse(Console.ReadLine(), out w) ? w : -1);
                switch (choose)
                {
                    case 0: flag = false; break;
                    case (Priorities)1: flag = false; break;
                    case (Priorities)2: flag = false; break;
                    default: Console.WriteLine("worng input try again"); break;
                }
            }
            return choose;
        }
        private static void Update(ref IDal data)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine(
                    "to update drone details press 1\n" +
                    "to update base-station press 2\n" +
                    "to update costumer press 3\n" +
                    "to update parcel press 4\n" +
                    "to return to main menu press 5\n");
                string inputForEnd = Console.ReadLine();
                switch (inputForEnd)
                {
                    case "1":
                        {
                            try
                            {
                                //update of drone
                                data.UpdateDrone(GetDrone());
                                Console.WriteLine(" success !");
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }

                    case "2":
                        {
                            try
                            {
                                data.UpdateBaseStation(GetStation());
                                Console.WriteLine(" success !");
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "3":
                        {
                            try
                            {
                                data.UpdateCustomer(GetCustomer());
                                Console.WriteLine(" success !");
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "4":
                        {
                            try
                            {
                                //Input of parcel
                                int parcelId;
                                do { Console.WriteLine("Enter the id of the parcel you want to update"); }
                                while (!int.TryParse(Console.ReadLine(), out parcelId));
                                data.UpdateParcel(GetParcel(parcelId));
                                Console.WriteLine(" success !");
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "5":
                        return;
                    default:
                        Console.WriteLine("ERROR input try again");
                        break;
                }
            }
        }
        private static void Display(IDal data)

        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine(
                      "to display base-station press 1\n" +
                      "to display drone press 2\n" +
                      "to display costumer press 3\n" +
                      "to display parcel press 4\n" +
                      "to return to main menu press 5\n");

                string choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("base-station: \n");
                            try
                            {
                                Console.WriteLine(data.GetBaseStation(id).ToString());
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }

                    case "2":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("drone:\n");

                            try
                            {
                                Console.WriteLine(data.GetDrone(id).ToString());
                                flag = false;
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "3":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("costumer:\n");
                            try
                            {
                                Console.WriteLine(data.GetCustomer(id).ToString());
                                flag = false;
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "4":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("parcels:\n");
                            try
                            {
                                Console.WriteLine(data.GetParcel(id).ToString());
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "5":
                        return;
                    default:
                        {
                            Console.WriteLine("ERROR input - try again\n");
                            break;
                        }
                }
            }
        }
        private static void ShowList(IDal data)
        {
            Console.WriteLine(
                   "to display all base-station press 1\n" +
                   "to display all drones press 2\n" +
                   "to display all customers press 3\n" +
                   "to display all parcels press 4\n" +
                   "to display all parcel that does't have a drone press 5\n" +
                   "to display all base-station with charging position press 6\n" +
                    "to return to main menu press 7\n");

            string choose = Console.ReadLine();
            switch (choose)
            {
                case "1":
                    {
                        Console.WriteLine("base-stations: \n");
                        foreach (StationData mystation in data.GetStationsList())
                            Console.WriteLine(mystation.ToString());
                        break;
                    }

                case "2":
                    {
                        Console.WriteLine("drones:\n");
                        foreach (DroneData mydrone in data.GetDronesList())
                            Console.WriteLine(mydrone.ToString());
                        break;
                    }
                case "3":
                    {
                        Console.WriteLine("costumers:\n");
                        foreach (CustomerData myCostumer in data.GetCustomersList())
                            Console.WriteLine(myCostumer.ToString());
                        break;
                    }
                case "4":
                    {
                        Console.WriteLine("parcels:\n");
                        foreach (ParcelData myparcel in data.GetParcelsList())
                            Console.WriteLine(myparcel.ToString());
                        break;
                    }
                case "5":
                    {
                        Console.WriteLine("All parcels that not link to drone:\n");
                        foreach (ParcelData myparcel in data.GetSpecificParcels(p=>p.DroneId==null))
                            Console.WriteLine(myparcel.ToString());
                        break;
                    }
                case "6":
                    {
                        Console.WriteLine("All Available Stations");
                        foreach (StationData mystation in data.GetSpecificStations(s=> data.AvailableChargingSlots(s.Id) > 0))
                            Console.WriteLine(mystation.ToString());
                        break;
                    }
                case "7":
                    return;
                default:
                    Console.WriteLine("ERROR input ");
                    break;
            }
        }

        private static void Remove(IDal data)
        {
            bool flag = true;
            while (flag)
            {
                Console.WriteLine(
                   "to remove  base-station press 1\n" +
                   "to remove  drones press 2\n" +
                   "to remove  costumers press 3\n" +
                   "to remove  parcels press 4\n" +
                    "to return to main menu press 5\n");

                string choose = Console.ReadLine();
                switch (choose)
                {
                    case "1":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            try
                            {
                                data.RemoveBaseStation(id);
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }

                    case "2":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));

                            try
                            {
                                data.RemoveDrone(id);
                                flag = false;
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "3":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            try
                            {
                                data.RemoveCustomer(id);
                                flag = false;
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "4":
                        {
                            int id;
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            try
                            {
                                data.RemoveParcel(id);
                                flag = false;
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;
                            }
                            break;
                        }
                    case "5":
                        return;
                    default:
                        {
                            Console.WriteLine("ERROR input - try again\n");
                            break;
                        }
                }

            }

        }
        private static StationData GetStation()
        {

            int stationId, chargeSlots;
            string stationName;
            double longitude, lattitude;
            do { Console.WriteLine("Enter staition Id:"); }
            while (!int.TryParse(Console.ReadLine(), out stationId));
            Console.WriteLine("Enter staition name:"); 
            stationName=Console.ReadLine();
            do
            { Console.WriteLine("Enter longitude number:"); }
            while (!double.TryParse(Console.ReadLine(), out longitude));
            do
            { Console.WriteLine("Enter lattitude number:"); }
            while (!double.TryParse(Console.ReadLine(), out lattitude));
            do
            { Console.WriteLine("Enter chargeSlots"); }
            while (!int.TryParse(Console.ReadLine(), out chargeSlots));
            StationData newStation = new StationData(stationId, stationName, longitude, lattitude, chargeSlots);

            return newStation;
        }
        static DroneData GetDrone()
        {
            int droneId;
            string droneModel;
            do { Console.WriteLine("Enter drone Id:"); }
            while (!int.TryParse(Console.ReadLine(), out droneId));
            Console.WriteLine("Enter drone model");
            droneModel = Console.ReadLine();
            WeightCategories weight = GetWeightCategories();
            DroneData newDrone = new DroneData(droneId, droneModel, weight);
            return newDrone;
        }
        static WeightCategories GetWeightCategories()
        {
            Console.WriteLine(
                    "Enter weight:\n" +
                    "press 0 for Lightweight\n " +
                    "press 1 for medium\n " +
                    "press 2 for heavy\n "
                    );
            WeightCategories choose = 0;
            bool flag = true;
            while (flag)
            {
                int w;
                choose = (WeightCategories)(int.TryParse(Console.ReadLine(), out w) ? w : -1);
                switch (choose)
                {
                    case 0: flag = false; break;
                    case (WeightCategories)1: flag = false; break;
                    case (WeightCategories)2: flag = false; break;
                    default: Console.WriteLine("worng input try again"); break;
                }
            }
            return choose;
        }
        /// <summary>
        /// recieve data from user and return Costumer object 
        /// </summary>
        /// <returns></returns>
        static CustomerData GetCustomer()
        {
            int customerId;
            double lattitude, longitude;
            do { Console.WriteLine("Enter costumer Id:"); }
            while (!int.TryParse(Console.ReadLine(), out customerId));
            Console.WriteLine("Enter costomer name:");
            string costumerName = Console.ReadLine();
            Console.WriteLine("Enter phone number:");
            string phone = Console.ReadLine();
            do { Console.WriteLine("Enter lattitude:"); }
            while (!double.TryParse(Console.ReadLine(), out lattitude));
            do { Console.WriteLine("Enter longitude:"); }
            while (!double.TryParse(Console.ReadLine(), out longitude));
            CustomerData newCustomer = new CustomerData(customerId, costumerName, phone, lattitude, longitude);
            return newCustomer;
        }
            }
       
    }

