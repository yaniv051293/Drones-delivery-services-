using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using System.Collections.ObjectModel;
using System.Windows;


namespace BO
{
    sealed internal partial class BL : IBL
    {
        static readonly IBL instance = new BL();
        public static IBL Instance { get => instance; }

        internal IDal data = DalFactory.GetDal();
        public static double[] batteryUse;
        private Random random = new Random();
        private List<DroneToList> droneList;

         BL()
        {
            try 
            { 
            batteryUse = data.GetBatteryUse();
            droneList = new List<DroneToList>();
            initializeDrones();
            }
            catch(Exception e) 
            { 
                throw new Exception(e.Message, e);
            }
        }

        /// <summary>
        /// ///Check error to throw
        /// </summary>
        /// <param name="intval"></param>
        /// <param name="e"></param>
        private void CheckInt(int intval, string error)
        {
            if (intval < 0)
                throw new NagatineIDException(intval, $"worng {error} input ");
        }
        /// <summary>
        /// check if the num between 30-36
        /// </summary>
        /// <param name="len"></param>
        /// <param name="e"></param>
        private void Checklen(double len, string e)
        {
            if (len < 30 || len > 36)//Exist in earch
                throw new LongLattException(len, $"worng {e} input ");
        }
        /// <summary>
        /// check if is null
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mes"></param>
        private void checkIsNull(int? id, string mes)
        {
            if (id == null)
                throw new MinosOneException((int)id, mes);
        }
        /// <summary>
        /// Check if eqal to -1
        /// </summary>
        /// <param name="id"></param>
        /// <param name="mes"></param>
        private void checkMinosOne(int id, string mes)
        {
            if (id == -1)
                throw new MinosOneException(id, mes);
        }

        /// <summary>
        /// fill the droneList with all the data from dataSource
        /// </summary>
        private void initializeDrones()
        {
            foreach (var d in data.GetDronesList())
            {
                DroneToList myDrone = new DroneToList(d.Id, d.Model, (WeightCategories)d.MaxWeight);
                //intialize the proprties in order:  Status ,ParcelId, CurrentLocation,Battery ,
                ParcelData hisParcel = FindParcelOfDrone(d.Id);// find parcel that link to the drone 
                if (hisParcel.DroneId == myDrone.Id) //if the drone link to parcel in delivery
                {
                    myDrone.Status = DroneStatus.delivery;
                    myDrone.ParcelId = hisParcel.Id;
                }
                else //if the drone doen't link to parcel in delivery
                {
                    myDrone.Status = (DroneStatus)random.Next(0, 2);
                }
                myDrone.CurrentLocation = InitializeDroneLocation(myDrone, hisParcel);
                myDrone.Battery = InitializeDroneBattery(myDrone, hisParcel);
                droneList.Add(myDrone);
            }
        }

        /// <summary>
        /// return battery of drone in initialze
        /// </summary>
        /// <param name="myDrone"></param>
        /// <param name="hisParcel"></param>
        /// <returns></returns>
        private int InitializeDroneBattery(DroneToList myDrone, ParcelData hisParcel)
        {
            switch (myDrone.Status)
            {
                case (DroneStatus)0: // if the drone available random battery between 100% to the minimom battery need to  arrive close station 
                    {
                        double distance = Distance(myDrone.CurrentLocation, CloseStation(myDrone.CurrentLocation));
                        int minBarttery = (int) CalculateBattery(distance,
                                                                myDrone.Status,
                                                                (DO.WeightCategories)myDrone.MaxWeight);
                        return random.Next(minBarttery, 101);
                    }
                case (DroneStatus)1: // if the drone in charging random battery between 0 to 20
                    return random.Next(0, 21);
                case (DroneStatus)2: // if the drone linked to parcel 
                    {
                        CustomerData sender = data.GetCustomer(hisParcel.SenderId);
                        Location senderLocation = new (sender.Lattitude, sender.Longitude);

                        CustomerData target = data.GetCustomer(hisParcel.TargetId);
                        Location targetLocation = new(target.Lattitude, target.Longitude);
                        
                        double minimomBattery = CalculateBattery(Distance(myDrone.CurrentLocation, senderLocation), DroneStatus.available); // the battery need for fly to the sender
                        minimomBattery += CalculateBattery(Distance(senderLocation, targetLocation),
                                                         DroneStatus.delivery, hisParcel.Wieght);// +the battery need for fly from sender to the target
                        minimomBattery += CalculateBattery(Distance(targetLocation, CloseStation(targetLocation)),
                                                                           DroneStatus.available);// +the battery need for fly from target to  close station
                        return minimomBattery > 100 ? 100 : random.Next((int)minimomBattery, 101);
                    }
                default:
                    throw new Exception();
            }
        }

        /// <summary>
        /// return the location of drone in initialize
        /// </summary>
        /// <param name="myDrone"></param>
        /// <param name="hisParcel"></param>
        /// <returns></returns>
        private Location InitializeDroneLocation(DroneToList myDrone, ParcelData hisParcel)
        {
            if (myDrone.Status == DroneStatus.available)
            {
                // intiliaze CurrentLocation between locations of customers who get parcel from this drone
                var targetIds = data.GetSpecificParcels(parcel => parcel.DroneId == myDrone.Id)
                                    .Select(parcel => parcel.TargetId).ToList();// list of all customer's id who get parcel by this drone 
                if (targetIds.Count != 0)
                    return GetCustomerLocation(targetIds[random.Next(targetIds.Count)]);
                else
                { //if there is no customer who get parcel by this drone, return random location of available station
                    var stationsIds = data.GetSpecificStations(station => data.AvailableChargingSlots(station.Id) > 0)
                                          .Select(station => station.Id).ToList();
                    return GetStationLocation(stationsIds[random.Next(stationsIds.Count)]);
                }
            }
            if (myDrone.Status == DroneStatus.maintenance)
            {
                var stationsIds = data.GetSpecificStations(station => data.AvailableChargingSlots(station.Id) > 0)
                                      .Select(station => station.Id).ToList();
                int stationId = (int)stationsIds[random.Next(stationsIds.Count())];
                data.AddDroneCharge(new DroneCharge(myDrone.Id, stationId));
                return GetStationLocation(stationId);
            }
            if (myDrone.Status == DroneStatus.delivery)
            {
                CustomerData customer = data.GetCustomer(hisParcel.SenderId);//get the location of the sender
                Location senderLocation = new(customer.Lattitude, customer.Longitude);
                if (hisParcel.PickedUp == null) //if the drone not picked up the parcel yet
                {
                    return CloseStation(senderLocation); //drone location is the closest station to the sender 
                }
                else //if the drone picked up the parcel but not arrive to customer yet
                {
                    return senderLocation; //drone location= sender location }
                }
            }
            return null;
        }

        /// <summary>
        ///find and return the parcel that linked to this drone (if doesnt exist return parcel with id=-1)
        /// </summary>
        /// <returns></returns>
        private ParcelData FindParcelOfDrone(int droneID)
        {    // find parcel that link to the drone 
            return data.GetSpecificParcels(p => (p.DroneId == droneID) && (p.Delivered == null)).FirstOrDefault();
        }

        /// <summary>
        /// get customer id and return location of customer
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        internal Location GetCustomerLocation(int id)
        {
            CustomerData c =data.GetCustomer(id);
            return new Location(c.Lattitude, c.Longitude);
        }

        /// <summary>
        /// get station id and return location of station
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private Location GetStationLocation(int id)
        {
            StationData s = data.GetBaseStation(id);
            return new Location(s.Lattitude, s.Longitude);
        }

        /// <summary>
        /// return the location of closest station to this location
        /// </summary>
        /// <param name="lattitude1"></param>
        /// <param name="longitude1"></param>
        /// <returns></returns>
        private Location CloseStation(Location location)
        {

            double closelong = 9999;
            double closeLat = 9999;
            double shortDis = 99999;
            foreach (StationData s in data.GetStationsList())
            {
                double thisDistance = Distance(location, new Location(s.Lattitude, s.Longitude));
                if (thisDistance < shortDis)
                {
                    shortDis = thisDistance;
                    closelong = (double)s.Longitude;
                    closeLat = (double)s.Lattitude;
                }
            }
            return new Location(closeLat, closelong);
        }

        /// <summary>
        /// return the distance between two locations
        /// </summary>
        /// <param name="loc1"></param>
        /// <param name="loc2"></param>
        /// <returns></returns>
        internal double Distance(Location loc1, Location loc2)
        {
            int R = 6371 * 1000; // metres
            double phi1 = (double)(loc1.Lattitude * Math.PI / 180); // φ, λ in radians
            double phi2 = (double) (loc2.Lattitude * Math.PI / 180);
            double deltaPhi = (double)(loc2.Lattitude - loc1.Lattitude) * Math.PI / 180;
            double deltaLambda = (double)(loc2.Longitude - loc1.Longitude) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
                       Math.Cos(phi1) * Math.Cos(phi2) *
                       Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double d = R * c / 1000; // in kilometres
            return d;

        }

        /// <summary>
        /// get two loactions and Calculate the battery need to fly between them (due to the weight of carry)
        /// </summary>
        /// <param name="myLocation"></param>
        /// <param name="destination"></param>
        /// <param name="weight"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        internal double CalculateBattery(double distance, DroneStatus status, DO.WeightCategories weight = 0)
        {
            if (status == DroneStatus.available)
            {
                return distance * batteryUse[0];
            }
            return weight switch
            {
                (DO.WeightCategories)0 => distance * batteryUse[1],
                (DO.WeightCategories)1 => distance * batteryUse[2],
                (DO.WeightCategories)2 => distance * batteryUse[3],
                _ => throw new Exception(),
            };
        }
    }
}