using System;
using BO;
using BlApi;

namespace ConsoleUI_BL
{
    class Program
    {
        /// <summary>
        /// The main that run all Function of BL
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {

            IBL checkIbl = BLFactory.GetBL();//Check interface BL
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
                        Add(ref checkIbl); break;
                    case "2":
                        Update(ref checkIbl); break;
                    case "3":
                        Display(checkIbl); break;
                    case "4":
                        ShowList(checkIbl); break;
                    case "5":
                        RemoveList(checkIbl); break;
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
        /// <summary>
        /// Display the object of BL
        /// </summary>
        /// <param name="checkIbl"></param>
        private static void Display(IBL checkIbl)

        {
            bool flag = true;// stop when input to stop display object
            while (flag)
            {
                Console.WriteLine(
                      "to display base-station press 1\n" +
                      "to display drone press 2\n" +
                      "to display costumer press 3\n" +
                      "to display parcel press 4\n" +
                      "to return to main menu press 5\n");

                string choose = Console.ReadLine();//1-5 input
                switch (choose)
                {
                    case "1":
                        {
                            int id;// base station id
                            do { Console.WriteLine("Enter id"); }
                            while(!int.TryParse(Console.ReadLine(),out id));
                            Console.WriteLine("base-station: \n");
                            try {
                                Console.WriteLine(checkIbl.GetBaseStation(id).ToString());
                                flag = false;// return to the main
                            }
                            catch(Exception e)
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
                            int id;// drone id
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("drone:\n");

                            try {
                                Console.WriteLine(checkIbl.GetDrone(id).ToString());
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {

                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "3":
                        {
                            int id;//customer id
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("costumer:\n");
                            try { 
                                Console.WriteLine(checkIbl.GetCustomer(id).ToString());
                                flag = false;//return to main
                            }
                            catch(Exception e)
                            {

                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "4":
                        {
                            int id;//parcel id
                            do { Console.WriteLine("Enter id"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("parcels:\n");
                            try { 
                                Console.WriteLine(checkIbl.GetParcel(id).ToString());
                                flag = false;//return to main
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
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
        /// <summary>
        /// remove object from lists BL
        /// </summary>
        /// <param name="checkIbl"></param>
        private static void RemoveList(IBL checkIbl)
        {
            bool flag = true;//flag for stop remove action
            while (flag)
            {
                Console.WriteLine(
                   "to remove  base-station press 1\n" +
                   "to remove  drones press 2\n" +
                   "to remove  costumers press 3\n" +
                   "to remove  parcels press 4\n" +
                    "to return to main menu press 5\n");

            string choose = Console.ReadLine();//between 1-5 action
            switch (choose)
            {
                case "1":
                    {
                        int id;//base station id
                        do { Console.WriteLine("Enter id"); }
                        while (!int.TryParse(Console.ReadLine(), out id));
                        try
                        {
                            checkIbl.RemoveBaseStation(id);
                            flag = false;//return to main
                            }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("do you want to  try add agian ?(yes or no)");
                            string ans = Console.ReadLine();
                            if (ans != "yes")
                                flag = false;//return to main
                            }
                        break;
                    }

                case "2":
                    {
                        int id;//drone id
                        do { Console.WriteLine("Enter id"); }
                        while (!int.TryParse(Console.ReadLine(), out id));

                        try
                        {
                            checkIbl.RemoveDrone(id);
                            flag = false;//return to main
                            }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                            Console.WriteLine("do you want to  try add agian ?(yes or no)");
                            string ans = Console.ReadLine();
                            if (ans != "yes")
                                flag = false;//return to main
                            }
                        break;
                    }
                case "3":
                    {
                        int id;//customer id
                        do { Console.WriteLine("Enter id"); }
                        while (!int.TryParse(Console.ReadLine(), out id));
                        try
                        {
                            checkIbl.RemoveCustomer(id);
                            flag = false;//return to main
                            }
                        catch (Exception e)
                        {

                            Console.WriteLine(e.Message);
                            Console.WriteLine("do you want to  try add agian ?(yes or no)");
                            string ans = Console.ReadLine();
                            if (ans != "yes")
                                flag = false;//return to main
                            }
                        break;
                    }
                case "4":
                    {
                        int id;//parcel id
                        do { Console.WriteLine("Enter id"); }
                        while (!int.TryParse(Console.ReadLine(), out id));
                        try
                        {
                                checkIbl.RemoveParcel(id);
                            flag = false;//return to main
                            }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message);
                            Console.WriteLine("do you want to  try add agian ?(yes or no)");
                            string ans = Console.ReadLine();
                            if (ans != "yes")
                                flag = false;//return to main
                            }
                        break;
                    }
                case "5":
                    return;//return to main
                    default:
                    {
                        Console.WriteLine("ERROR input - try again\n");
                        break;
                    }
            }

        }

    }

    /// <summary>
    /// shows The all lists
    /// </summary>
    /// <param name="checkIbl"></param>
    private static void ShowList(IBL checkIbl)
        {
            Console.WriteLine(
                   "to display all base-station press 1\n" +
                   "to display all drones press 2\n" +
                   "to display all costumers press 3\n" +
                   "to display all parcels press 4\n" +
                   "to display all parcel that does't have a drone press 5\n" +
                   "to display all base-station with charging position press 6\n"+
                    "to return to main menu press 7\n");

            string choose = Console.ReadLine();//between 1-7
            switch (choose)
            {
                case "1":
                    {
                        Console.WriteLine("base-stations: \n");
                        foreach (StationToList mystation in checkIbl.GetStationsList())
                            Console.WriteLine(mystation.ToString());
                        break;
                    }

                case "2":
                    {
                        Console.WriteLine("drones:\n");
                        foreach (DroneToList mydrone in checkIbl.GetDronesList())
                            Console.WriteLine(mydrone.ToString());
                        break;
                    }
                case "3":
                    {
                        Console.WriteLine("costumers:\n");
                        foreach (CustomerToList myCostumer in checkIbl.GetCustomersList())
                            Console.WriteLine(myCostumer.ToString());
                        break;
                    }
                case "4":
                    {
                        Console.WriteLine("parcels:\n");
                        foreach (ParcelToList myparcel in checkIbl.GetParcelsList())
                            Console.WriteLine(myparcel.ToString());
                        break;
                    }
                case "5":
                    {
                        Console.WriteLine("All parcels that not link to drone:\n");
                        foreach (ParcelToList myparcel in checkIbl.GetFreeParcelList())
                            Console.WriteLine(myparcel.ToString());
                        break;
                    }
                case "6":
                    {
                        Console.WriteLine("All Available Stations");
                        foreach (StationToList mystation in checkIbl.GetAvailableStationsList())
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
        /// <summary>
        /// check update function of BL
        /// </summary>
        /// <param name="checkIbl"></param>
        private static void Update(ref IBL checkIbl)
        {
            bool flag = true;//For stop update display
            while (flag)
            {
                Console.WriteLine(
                    "to update drone details press 1\n" +
                    "to update base-station press 2\n" +
                    "to update costumer press 3\n" +
                    "to set drone to charging press 4\n" +
                    "to realse drone from charging press 5\n" +
                    "to link parcel to drone press 6\n" +
                    "to get parcel with drone press 7\n" +
                    "to Delivery of parcel by drone press 8\n"+
                    "to return to main menu press 9\n");
                string inputForEnd = Console.ReadLine();//between 1-9
                switch (inputForEnd)
                {
                    case "1":
                        {
                            try { 
                            //update of drone
                            int id;//drone id
                            do { Console.WriteLine("input the id of the drone"); }
                            while (!int.TryParse(Console.ReadLine(), out id));
                            Console.WriteLine("input the name of the model");
                            string model = Console.ReadLine();//input model of drone
                            checkIbl.UpdateDrone(id, model);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }

                    case "2":
                        {
                            try
                            {
                                //Input of the drone
                                int id, countCharge;//base station id , and count of drone in charge
                                string name;//name of the Base station
                                do { Console.WriteLine("input the id of the base-station\n"); }
                                while (!int.TryParse(Console.ReadLine(), out id));
                                Console.WriteLine("input The name of station\n");
                                name = Console.ReadLine();//input
                                do { Console.WriteLine(" input the number of charche basesttion"); }
                                while (!int.TryParse(Console.ReadLine(), out countCharge)); ;
                                checkIbl.UpdateBaseStation(id, name, countCharge);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "3":
                        {
                            try { 
                            //Input of the costumer
                                int id;//id customer
                                do { Console.WriteLine("input the id of the costumer"); }
                                while (!int.TryParse(Console.ReadLine(), out id)) ;
                                 Console.WriteLine("input the new name of the costumer");
                                string name=Console.ReadLine() ;//input
                                Console.WriteLine("Enter the number of cellphone");
                                string phone = Console.ReadLine();
                               checkIbl.UpdateCustomer(id,name, phone);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "4":
                        {
                            try { 
                            //Input of parcel
                            int droneId;//drone id for set to charge
                            do { Console.WriteLine("Enter the id of the drone you want to charge"); }
                            while (!int.TryParse(Console.ReadLine(), out droneId));
                            checkIbl.SetDroneToCharge(droneId);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "5":
                        {
                            try { 
                            double hours;//hours in charge
                            int droneId;
                            do { Console.WriteLine("Enter the id of the drone you want to realse from charge"); }
                            while (!int.TryParse(Console.ReadLine(), out droneId));
                            do { Console.WriteLine("Enter the time the drone was in charging (in hours)"); }
                            while (!double.TryParse(Console.ReadLine(), out hours));
                            checkIbl.ReleaseDroneFromCharge(droneId);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "6":
                        {
                            try
                            {
                                int droneId;
                            do { Console.WriteLine("Enter the id of drone to want to use"); }
                            while (!int.TryParse(Console.ReadLine(), out droneId));
                            checkIbl.LinkParcelToDrone(droneId);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "7":
                        {
                            try 
                            { 
                            int droneId;
                            do { Console.WriteLine("Enter the id of drone to want to update that collect parcel:"); }
                            while (!int.TryParse(Console.ReadLine(), out droneId));
                            checkIbl.CollectParcel(droneId);
                             Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "8":
                        {
                            try {
                                int droneId;
                                do { Console.WriteLine("Enter the id of drone to want to update that arrive"); }
                                while (!int.TryParse(Console.ReadLine(), out droneId));
                                checkIbl.UpdateParcelProvided(droneId);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "9":
                        return;//return to main
                    default:
                        Console.WriteLine("ERROR input try again");
                        break;
                }
            }

        }
    /// <summary>
    /// check all add function BL
    /// </summary>
    /// <param name="checkIbl"></param>
        private static void Add(ref IBL checkIbl)
        {
            bool flag = true;//display add function BL
            while (flag)
            {
                Console.WriteLine(
                                  "to add basestation press 1\n" +
                                  "to add dorne press 2\n" +
                                  "to add costumer press 3\n" +
                                  "to add parcel press 4\n" +
                                  "to return to main menu press 5\n");
                string inputForEnd = Console.ReadLine();//between 1-5
                switch (inputForEnd)
                {
                    case "1":
                        {
                        //Input of basestation
                        try
                        {
                                checkIbl.AddBaseStation(GetStation());
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                        catch (Exception e)
                        { 
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to  try add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }    
                        break;
                        }

                    case "2":
                        {
                            //Input of the drone
                            try
                            {
                                int idS;//id station
                                do { Console.WriteLine("input id station"); }
                                while (!int.TryParse(Console.ReadLine(), out idS));
                                checkIbl.AddDrone(GetDrone(),idS);
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch(Exception e) 
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to add agian ?(yes or no):");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "3":
                        {
                            //Input of the costumer
                            try
                            {
                                checkIbl.AddCustomer(GetCustomer());
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
                            }
                            break;
                        }
                    case "4":
                        {
                            try { 
                            checkIbl.AddParcel(GetParcel());
                                Console.WriteLine(" success !");
                                flag = false;//return to main
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message);
                                Console.WriteLine("do you want to add agian ?(yes or no)");
                                string ans = Console.ReadLine();
                                if (ans != "yes")
                                    flag = false;//return to main
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
        /// <summary>
        /// get detilas of base station
        /// </summary>
        /// <returns></returns>
        private static Station GetStation()
        {

            int stationId, chargeSlots;// station id and num of charge slots
            string stationName;// the name of base station
            double longitude, lattitude;// for the loction
            do { Console.WriteLine("Enter staition Id:"); }
            while (!int.TryParse(Console.ReadLine(), out stationId));
            Console.WriteLine("Enter staition name:");
            stationName = Console.ReadLine();
            do
            { Console.WriteLine("Enter longitude number:"); }
            while (!double.TryParse(Console.ReadLine(), out longitude));
            do
            { Console.WriteLine("Enter lattitude number:"); }
            while (!double.TryParse(Console.ReadLine(), out lattitude));
            Location newloc = new Location(lattitude, longitude);
            do
            { Console.WriteLine("Enter chargeSlots"); }
            while (!int.TryParse(Console.ReadLine(), out chargeSlots));
            Station newStation = new Station(stationId, stationName, newloc, chargeSlots);

            return newStation;
        }
        /// <summary>
        /// get detials of the drone
        /// </summary>
        /// <returns></returns>
        static Drone GetDrone()
        {
            int  droneId;//id of drone 
            string droneModel;//model drone
            do { Console.WriteLine("Enter drone Id:"); }
            while(!int.TryParse(Console.ReadLine(), out droneId)) ;
            Console.WriteLine("Enter drone model");
            droneModel = Console.ReadLine();
            WeightCategories weight = GetWeightCategories();
            Drone newDrone = new Drone(droneId, droneModel, weight);
            return newDrone;
        }

        /// <summary>
        /// recieve data from user and return Costumer object 
        /// </summary>
        /// <returns></returns>
        static Customer GetCustomer()
        {
            int customerId;//customer id
            double lattitude, longitude;//loction 
            do { Console.WriteLine("Enter costumer Id:"); }
            while (!int.TryParse(Console.ReadLine(), out customerId));
            Console.WriteLine("Enter costomer name:");
            string costumerName = Console.ReadLine();
            Console.WriteLine("Enter phone number:");
            string phone = Console.ReadLine();
            do { Console.WriteLine("Enter lattitude:"); }
            while (!double.TryParse(Console.ReadLine(), out lattitude));
            do{ Console.WriteLine("Enter longitude:"); }
             while(!double.TryParse(Console.ReadLine(), out longitude));
            Location newLoc = new Location(lattitude, longitude);
            Customer newCustomer = new Customer(customerId, costumerName, phone,newLoc);
            return newCustomer;
        }
        /// <summary>
        /// get detilas of parcel
        /// </summary>
        /// <returns></returns>
        private static Parcel GetParcel()
        {
            int sendId, targetId;//id of sender and target
            do { Console.WriteLine("Enter send's ld:"); }
            while(!int.TryParse(Console.ReadLine(), out sendId));
            do { Console.WriteLine("Enter target's id:"); }
             while(!int.TryParse(Console.ReadLine(), out targetId));
            WeightCategories weight = GetWeightCategories();
            Priorities priority = GetPriorities();
            Parcel newParcel = new Parcel(0, new CustomerInParcel(sendId), new CustomerInParcel(targetId), weight, priority);
            return newParcel;
        }
        /// <summary>
        /// choose of priorities
        /// </summary>
        /// <returns></returns>
        static Priorities GetPriorities() {

            Console.WriteLine(
                    "Enter Priorities\n" +
                    "press 0 for Emergency\n " +
                    "press 1 for Regular\n " +
                    "press 2 for Fast\n "
                    );
            Priorities choose = 0;//between 0-1
            int w;//jast for use
            bool flag = true;//for eroor input
            while (flag)
            {
                choose = (Priorities)(int.TryParse(Console.ReadLine(),out w)?w:-1);
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
        /// <summary>
        /// choose of Weight Categories
        /// </summary>
        /// <returns></returns>
        static WeightCategories GetWeightCategories()
        {
            Console.WriteLine(
                    "Enter weight:\n" +
                    "press 0 for Lightweight\n "+
                    "press 1 for medium\n " +
                    "press 2 for heavy\n "
                    );
            WeightCategories choose=0;//between 0-2
            bool flag= true;//for error input
            while (flag)
            {
                int w;//jast for use
                choose = (WeightCategories)(int.TryParse(Console.ReadLine(), out w) ? w : -1);
                switch (choose)
                {
                    case 0: flag = false; break;
                    case (WeightCategories) 1: flag = false; break;
                    case (WeightCategories) 2: flag = false; break;
                    default: Console.WriteLine("worng input try again"); break;
                }
            }
            return choose;
        }
    }
}


