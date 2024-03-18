using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Linq;
using DalApi;
using DO;
using System.Xml.Linq;

namespace Dal 
{

    sealed partial class DalXml : IDal
    {
        #region singelton
        static readonly DalXml instance = new DalXml();
        
        static DalXml() { }
        DalXml() 
        {

        }

        public static DalXml Instance { get => instance; }
        #endregion
        static int parcelIndex = 11;

        #region DS XML Files
        string CustomerPath = @"CustomerXml.xml";
        string DronePath = @"DroneXml.xml";
        string StationPath = @"StationXml.xml";
        string DroneChargePath = @"DroneChargeXml.xml";
        string ParcelPath = @"ParcelXml.xml";

        #endregion



        //*****Add Method*****
        /// <summary>
        /// add station to the xml file
        /// </summary>
        /// <param name="newStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddBaseStation(StationData newStation)
        {
            List<StationData> listStation = XMLTools.LoadListFromXMLSerializer<StationData>(StationPath);
            listStation.Add(newStation);
            XMLTools.SaveListToXMLSerializer(listStation, StationPath);
        }

        /// <summary>
        /// add drone to the xml file
        /// </summary>
        /// <param name="newDrone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddDrone(DroneData newDrone)
        {
            List<DroneData> listDrone = XMLTools.LoadListFromXMLSerializer<DroneData>(DronePath);
            listDrone.Add(newDrone);
            XMLTools.SaveListToXMLSerializer(listDrone, DronePath);

        }
        /// <summary>
        /// add customer to the xml file
        /// </summary>
        /// <param name="newCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddCustomer(CustomerData newCustomer)
        {
            List<CustomerData> listCustomer = XMLTools.LoadListFromXMLSerializer<CustomerData>(CustomerPath);
            listCustomer.Add(newCustomer);
            XMLTools.SaveListToXMLSerializer(listCustomer, CustomerPath);

        }

        /// <summary>
        /// add parcel to the xml file
        /// </summary>
        /// <param name="newParcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddParcel(ParcelData newParcel)
        {
            List<ParcelData> listParcel = XMLTools.LoadListFromXMLSerializer<ParcelData>(ParcelPath);
            newParcel.Id = parcelIndex++;
            listParcel.Add(newParcel);
            XMLTools.SaveListToXMLSerializer<ParcelData>(listParcel, ParcelPath);
        }

        /// <summary>
        /// add drone charge to the xml file
        /// </summary>
        /// <param name="newCharge"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddDroneCharge(DroneCharge newCharge)
        {
            XElement DroneChargingRootElem = XMLTools.LoadListFromXMLElement(DroneChargePath);

            XElement dr = (from d in DroneChargingRootElem.Elements()
                           where int.Parse(d.Element("ID").Value) == newCharge.Droneld
                           select d).FirstOrDefault();

            if (dr != null)
                throw new BadIdException(newCharge.Droneld, "Duplicate DroneCharge ID");

            XElement droneElem = new XElement("Drone", new XElement("ID", newCharge.Droneld),
                                 new XElement("Name", newCharge.Stationld));

            DroneChargingRootElem.Add(droneElem);

            XMLTools.SaveListToXMLElement(DroneChargingRootElem, DroneChargePath);
        }

        

        //*****Remove Method****
        /// <summary>
        /// remove station from xml file
        /// </summary>
        /// <param name="idStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal .RemoveBaseStation(int idStation)
        {
            List<StationData> stationsList = XMLTools.LoadListFromXMLSerializer<StationData>(StationPath);

            int index = stationsList.FindIndex(bs => bs.Id == idStation);
            if (index == -1)
                throw new BadIdException(idStation, $"The BaseStationt id not exist:{idStation}");
            StationData s = stationsList[index];
            s.Deleted = true;
            stationsList[index] = s;

            XMLTools.SaveListToXMLSerializer(stationsList, StationPath);
        }

        /// <summary>
        /// remove drone from xml file
        /// </summary>
        /// <param name="idDrone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.RemoveDrone(int idDrone)
        {
            List<DroneData> listDrone = XMLTools.LoadListFromXMLSerializer<DroneData>(DronePath);

            int index = listDrone.FindIndex(d => d.Id == idDrone);
            if (index == -1)
                throw new BadIdException(idDrone, $"The DronesList id not found:{idDrone}");
            DroneData drone = listDrone[index];
            drone.Deleted = true;
            listDrone[index] = drone;

            XMLTools.SaveListToXMLSerializer(listDrone, DronePath);
        }

        /// <summary>
        /// remove customer from xml file
        /// </summary>
        /// <param name="idCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.RemoveCustomer(int idCustomer)
        {
            List<CustomerData> customersList = XMLTools.LoadListFromXMLSerializer<CustomerData>(CustomerPath);

            int index = customersList.FindIndex(d => d.Id == idCustomer);
            if (index == -1)
                throw new BadIdException(idCustomer, $"The Customer id not exist:{idCustomer}");
            CustomerData c = customersList[index];
            c.Deleted = true;
            customersList[index] = c;
            XMLTools.SaveListToXMLSerializer(customersList, CustomerPath);
        }

        /// <summary>
        /// remove parcel from xml file
        /// </summary>
        /// <param name="idParcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.RemoveParcel(int idParcel)
        {
            List<ParcelData> parcelsList = XMLTools.LoadListFromXMLSerializer<ParcelData>(ParcelPath);

            int index = parcelsList.FindIndex(p => p.Id == idParcel);
            if (index == -1)
                throw new BadIdException(idParcel, $"The Parcel id not exist:{idParcel}");
            ParcelData p = parcelsList[index];
            p.Deleted = true;
            parcelsList[index] = p;

            XMLTools.SaveListToXMLSerializer(parcelsList, ParcelPath);
        }



        /// <summary>
        /// remove drone charge from xml file
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void RemoveDroneCharge(int droneId)
        {
            List<DroneCharge> listDrone = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            if (!listDrone.Exists(d => d.Droneld == droneId))
                throw new BadIdException(droneId, $"The drone isn't in charge {droneId}");
            listDrone.RemoveAll(d => d.Droneld == droneId);

            XMLTools.SaveListToXMLSerializer(listDrone, DroneChargePath);
            return;
        }


        double IDal.UpdateChargeTime(int droneId)
        {
            List<DroneCharge> listDrone = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            double chargeTime;
            int index = listDrone.FindIndex(c => c.Droneld == droneId);
            if (index == -1)
                throw new BadIdException(droneId, $"The DronesList id not found:{droneId}");
            DroneCharge charge = listDrone[index];
            DateTime now = DateTime.Now;
            chargeTime = (now - charge.TimeSet).TotalHours;// the time the drone was is charge until now

            charge.TimeSet = now; // update the time of charge to now
            listDrone[index] = charge;
            XMLTools.SaveListToXMLSerializer(listDrone, DroneChargePath);
            return chargeTime;
        }


        //******Update Method*****
        /// <summary>
        /// update a new station to xml file
        /// </summary>
        /// <param name="newStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateBaseStation(StationData newStation)
        {
            List<StationData> listStation = XMLTools.LoadListFromXMLSerializer<StationData>(StationPath);

            StationData sta = listStation.Find(p => p.Id == newStation.Id);

            if (sta.Id != 0)
            {
                listStation.Remove(sta);
                listStation.Add(newStation);
            }
            else
                throw new Exception($"bad StationId:{newStation.Id}");
            //newStation.Id,$"bad StationId:{newStation.Id}"

            XMLTools.SaveListToXMLSerializer(listStation, StationPath);

        }

        /// <summary>
        /// update a new drone to xml file
        /// </summary>
        /// <param name="newDrone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateDrone(DroneData newDrone)
        {
            List<DroneData> listDrone = XMLTools.LoadListFromXMLSerializer<DroneData>(DronePath);

            int index = listDrone.FindIndex(d => d.Id == newDrone.Id);
            if (index == -1)
                throw new BadIdException(newDrone.Id, $"The DronesList id not found:{newDrone.Id}");
            listDrone[index] = newDrone;

            XMLTools.SaveListToXMLSerializer(listDrone, DronePath);
        }

        /// <summary>
        /// update a new customer to xml file
        /// </summary>
        /// <param name="newCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateCustomer(CustomerData newCustomer)
        {
            List<CustomerData> listCustomer = XMLTools.LoadListFromXMLSerializer<CustomerData>(CustomerPath);

            CustomerData customer = listCustomer.Find(p => p.Id == newCustomer.Id);

            if (customer.Id != 0)
            {
                listCustomer.Remove(customer);
                listCustomer.Add(newCustomer);
            }
            else
                throw new Exception($"bad Customer Id:{newCustomer.Id}");
            //newStation.Id,$"bad StationId:{newStation.Id}"

            XMLTools.SaveListToXMLSerializer(listCustomer, CustomerPath);
        }

        /// <summary>
        /// update a new parcel to xml file
        /// </summary>
        /// <param name="newParcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateParcel(ParcelData newParcel)
        {

            List<ParcelData> listParcel = XMLTools.LoadListFromXMLSerializer<ParcelData>(ParcelPath);
            ParcelData parcel = listParcel.Find(p => p.Id == newParcel.Id);
            if (parcel.Id != 0)
            {
                listParcel.Remove(parcel);
                listParcel.Add(newParcel);
            }
            else
                throw new Exception($"bad parcel Id:{newParcel.Id}");
            //newStation.Id,$"bad StationId:{newStation.Id}"

            XMLTools.SaveListToXMLSerializer(listParcel, ParcelPath);
        }


        //*********Get Method********


        /// <summary>
        /// get Base station from xml file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]

        StationData IDal.GetBaseStation(int id)
        {
            List<StationData> listStation = XMLTools.LoadListFromXMLSerializer<StationData>(StationPath);
            StationData s = listStation.Find(s => s.Id == id && s.Deleted == false);
            if (s.Id == -1)
                throw new BadIdException(id, $"The Base-Station id:{id} not exist");
            return s;
        }

        /// <summary>
        /// Get drone from xml file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        DroneData IDal.GetDrone(int id)
{
            List<DroneData> listdrone = XMLTools.LoadListFromXMLSerializer<DroneData>(DronePath);
            DroneData d = listdrone.Find(s => s.Id == id && s.Deleted == false);
            if (d.Id == 0)
                throw new BadIdException(id, $"The Drone id:{id} not exist");
            return d;
        }

        /// <summary>
        /// Get customer from xml file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        CustomerData IDal.GetCustomer(int id)
{
            List<CustomerData> listCustomer = XMLTools.LoadListFromXMLSerializer<CustomerData>(CustomerPath);
            CustomerData c = listCustomer.Find(s => s.Id == id && s.Deleted == false);
            if (c.Id == 0)
                throw new BadIdException(id, $"The Customer id:{id} not exist");
            return c;
        }

        /// <summary>
        /// Get parcel from xml file
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        ParcelData IDal.GetParcel(int id)
{
            List<ParcelData> listParcel = XMLTools.LoadListFromXMLSerializer<ParcelData>(ParcelPath);
            ParcelData p = listParcel.Find(s => s.Id == id && s.Deleted == false);
            if (p.Id == 0)
                throw new BadIdException(id, $"The Parcel id:{id} not exist");
            return p;
        }

        //**********get List**********

        /// <summary>
        /// Get station list from xml file
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<StationData> IDal.GetStationsList()
        {
            List<StationData> listStation = XMLTools.LoadListFromXMLSerializer<StationData>(StationPath);
            return listStation.Where(s => s.Deleted == false).ToList();

        }

        /// <summary>
        /// Get drone list from xml file
        /// </summary>
        /// <returns></returns>

        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<DroneData> IDal.GetDronesList()
        {

            List<DroneData> listDrones = XMLTools.LoadListFromXMLSerializer<DroneData>(DronePath);
            return  listDrones.Where(s => s.Deleted == false).ToList();
        }

        /// <summary>
        /// Get customer list from xml file
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<CustomerData> IDal.GetCustomersList()
        {

            List<CustomerData> listCustomer = XMLTools.LoadListFromXMLSerializer<CustomerData>(CustomerPath);
            return listCustomer.Where(c => c.Deleted == false).ToList();
        }

        /// <summary>
        /// Get parcel list from xml file
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<ParcelData> IDal.GetParcelsList()
        {
            List<ParcelData> listParcel = XMLTools.LoadListFromXMLSerializer<ParcelData>(ParcelPath);

            return listParcel.Where(p => p.Deleted == false).ToList();
        }

        /// <summary>
        /// Get Drone charge list from xml file 
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<DroneCharge> IDal.GetDroneChargesList()
        {
            List<DroneCharge> listDroneCharge = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);

            return from droneCharge in listDroneCharge select droneCharge;
        }

        //**********Get specific Method ********
        /// <summary>
        /// Get Specific Parcels from xml file 
        /// </summary>
        /// <param name="meetCondition"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<ParcelData> IDal.GetSpecificParcels(IDal.ParcelCondition meetCondition)
        {
            List<ParcelData> listParcel = XMLTools.LoadListFromXMLSerializer<ParcelData>(ParcelPath);
            var Parcel = listParcel
                                .Where(parcel => meetCondition(parcel) && parcel.Deleted == false).ToList();
            return Parcel;
        }

        /// <summary>
        /// Get Specific Stations from xml file 
        /// </summary>
        /// <param name="meetCondition"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<StationData> IDal.GetSpecificStations(IDal.StationCondition meetCondition)
        {
            List<StationData> listStation = XMLTools.LoadListFromXMLSerializer<StationData>(StationPath);
            var station = listStation
                                .Where(station => meetCondition(station) && station.Deleted == false).ToList();
            return station;

        }
        /// <summary>
        /// Get Specific Customers from xml file 
        /// </summary>
        /// <param name="meetCondition"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<CustomerData> IDal.GetSpecificCustomers(IDal.CustomerCondition meetCondition)
        {
            List<CustomerData> listCustomer = XMLTools.LoadListFromXMLSerializer<CustomerData>(CustomerPath);
            var customer = listCustomer
                                .Where(customer => meetCondition(customer) && customer.Deleted == false).ToList();
            return customer;
        }

        /// <summary>
        /// Get Specific drones from xml file 
        /// </summary>
        /// <param name="meetCondition"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<DroneData> IDal.GetSpecificDrones(IDal.DroneCondition meetCondition)
        {
            List<DroneData> listdrone = XMLTools.LoadListFromXMLSerializer<DroneData>(DronePath);
            var drone = listdrone
                                .Where(drone => meetCondition(drone) && drone.Deleted == false).ToList();
            return drone;
        }

       


        //***************** 2 Method*********************
        
        /// <summary>
        /// return Available ChargingSlots from xml file
        /// </summary>
        /// <param name="stationID"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        int IDal.AvailableChargingSlots(int stationID)
        {
            List<StationData> listStation = XMLTools.LoadListFromXMLSerializer<StationData>(StationPath); // g
            StationData s = listStation.Find(s => s.Id == stationID && s.Deleted == false);
            if (s.Id == -1)
                throw new BadIdException(stationID, $"The Base-Station id:{stationID} not exist");

            List<DroneCharge> droneCharges = XMLTools.LoadListFromXMLSerializer<DroneCharge>(DroneChargePath);
            int UsedSlots = 0; // check how much times this station appear at DroneCharges list
            foreach (DroneCharge charge in droneCharges)
            {
                if (stationID == charge.Stationld)
                    UsedSlots++;
            }

            return s.ChargeSlots - UsedSlots;
        }
        

        /// <summary>
        /// Return array of battery use
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetBatteryUse()
        { 
              
            //like in DalObject

            return new[]{
                0.06, 0.09,
                0.12, 0.15,
                10000 };
            //available,light,medium, heavy,loading 
        }

    }
}
