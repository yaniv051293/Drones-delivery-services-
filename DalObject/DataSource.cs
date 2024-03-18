using System;
using DO;
using System.Text;
using System.Collections.Generic;
using Dal;
using DalApi;
namespace Dal
{
    internal class DataSource
    {
        internal class Config
        {
            public static double availableBatteryUse = 0.06;
            public static double lightBatteryUse = 0.09;
            public static double mediumBatteryUse = 0.12;
            public static double heavyBatteryUse = 0.15;
            public static double chargeRate = 8000;
            public static int parcelId = 1;
        }
        internal static List<DroneData> DronesList = new List<DroneData>();
        internal static List<StationData> BaseStationsList = new List<StationData>();
        internal static List<CustomerData> CustomersList = new List<CustomerData>();
        internal static List<ParcelData> ParcelsList = new List<ParcelData>();

        internal static List<DroneCharge> DroneCharges = new List<DroneCharge>();

        static Random random = new Random();
        public static void Initialize()
        {
            List<int> droneIDlist = new List<int>();
            List<int> costumerIDlist = new List<int>();

            // ********Initialize BaseStation ****************
            for (int i = 0; i < 5; i++)
            {
                int id = i + 1;//random.Next(1, 1000);
                string name = "Station" + (i+1);
                double longitude = 35.18 + random.NextDouble() / 10;//The Longitude of isreal
                double lattitude = 31.71 + random.NextDouble() / 10;
                int chargeSlots = random.Next(2, 6);
                StationData mystation = new StationData(id, name, longitude, lattitude, chargeSlots);
                BaseStationsList.Add(mystation);
            }

            // ********Initialize DronesList ****************
            for (int i = 0; i < 5; i++)
            {
                int id = random.Next(1, 1000);
                WeightCategories maxWeight = (WeightCategories)random.Next(0, 3);
                if (maxWeight == WeightCategories.heavy)
                    droneIDlist.Add(id);
                string model = "Model" + i;
                DroneData myDrone = new DroneData(id, model, maxWeight);
                DronesList.Add(myDrone);
            }

            // ********Initialize Costumer****************
            for (int i = 0; i < 10; i++)
            {
                int id = random.Next(10000000, 999999999);
                costumerIDlist.Add(id);
                string phone = "052" + random.Next(1000000, 10000000).ToString();
                string name = "David" + i;
                double longitude = random.NextDouble() * (35.0 - 30.0) + 30;//The Longitude of israel
                double lattitude = random.NextDouble() + 32;                //The Lattitude of israel
                CustomerData mycostumer = new CustomerData(id, name, phone, longitude, lattitude);
                CustomersList.Add(mycostumer);
            }


            // ********Initialize Parcel****************
            for (int i = 0; i < 10; i++)
            {
                int id = Config.parcelId++;
                int targetld, sendetld = costumerIDlist[i];
                int? droneId;
                if (i != 9)
                    targetld = costumerIDlist[i + 1];
                else
                    targetld = costumerIDlist[0];
                WeightCategories wieght = (WeightCategories)random.Next(0, 3);
                Priorities priority = (Priorities)random.Next(0, 3);
                DateTime? scheduled;
                if (droneIDlist.Count != 0)
                {
                    droneId = droneIDlist[0]; droneIDlist.RemoveAt(0);
                    scheduled = DateTime.Now;                          //time of scheduled parcel to drone
                }
                else
                {
                    droneId = null;
                    scheduled = null;
                }
                ParcelData myparcel = new ParcelData(id, sendetld, targetld, wieght, priority, droneId, scheduled);
                ParcelsList.Add(myparcel);
            }
            string CustomerPath = @"CustomerXml.xml";
            string DronePath = @"DroneXml.xml";
            string StationPath = @"StationXml.xml";
            string DroneChargePath = @"DroneChargeXml.xml";
            string ParcelPath = @"ParcelXml.xml";

              XMLTools.SaveListToXMLSerializer(DronesList, DronePath);
              XMLTools.SaveListToXMLSerializer(CustomersList, CustomerPath);
              XMLTools.SaveListToXMLSerializer(ParcelsList, ParcelPath);
             XMLTools.SaveListToXMLSerializer(DroneCharges, DroneChargePath);
              XMLTools.SaveListToXMLSerializer(BaseStationsList, StationPath);


        }
    }
}

