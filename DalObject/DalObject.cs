using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;
using DalApi;
using System.Runtime.CompilerServices;

namespace Dal
{

    internal sealed partial class DalObject : IDal
    {
        static readonly DalObject instance = new DalObject();
        public static DalObject Instance { get => instance; }

        DalObject()
        {
            DataSource.Initialize();
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public double[] GetBatteryUse()
        {
            return new[]{
                DataSource.Config.availableBatteryUse, DataSource.Config.lightBatteryUse,
                DataSource.Config.mediumBatteryUse, DataSource.Config.heavyBatteryUse,
                DataSource.Config.chargeRate };
        }
        //******* Add methods********
        /// <summary>
        /// get station struct and set it into the list 
        /// </summary>
        /// <param name="newStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddBaseStation(StationData newStation)
        {
            if (DataSource.BaseStationsList.Exists(bs => bs.Id == newStation.Id))
                throw new BadIdException(newStation.Id, $"The station already exist:{newStation.Id}");
            DataSource.BaseStationsList.Add(newStation);
        }

        /// <summary>
        /// get drone struct and set it into the list 
        /// </summary>
        /// <param name="newDrone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddDrone(DroneData newDrone)
        {
            if (DataSource.DronesList.Exists(dr => dr.Id == newDrone.Id))
                throw new BadIdException(newDrone.Id, $"The Drone already exist: {newDrone.Id}");
            DataSource.DronesList.Add(newDrone);
        }

        /// <summary>
        /// get Costumer struct and set it into the list 
        /// </summary>
        /// <param name="newCustomer"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        
        void IDal.AddCustomer(CustomerData newCustomer)
        {
            if (DataSource.CustomersList.Exists(dr => dr.Id == newCustomer.Id))
                throw new BadIdException(newCustomer.Id, $"The Customer already exist: {newCustomer.Id}");
            DataSource.CustomersList.Add(newCustomer);
        }

        /// <summary>
        /// get parcel struct and set it into the list 
        /// </summary>
        /// <param name="newParcel"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddParcel(ParcelData newParcel)
        {
            newParcel.Id = DataSource.Config.parcelId++;
            DataSource.ParcelsList.Add(newParcel);
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.AddDroneCharge(DroneCharge newCharge)
        {
            DataSource.DroneCharges.Add(newCharge);
        }

    }
    internal sealed partial class DalObject : IDal
    {
        // *********remove methods***********
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.RemoveBaseStation(int idStation)
        {
            int index = DataSource.BaseStationsList.FindIndex(bs => bs.Id == idStation);
            if (index == -1)
                throw new BadIdException(idStation, $"The BaseStationt id not exist:{idStation}");
            StationData s = DataSource.BaseStationsList[index];
            s.Deleted = true;
            DataSource.BaseStationsList[index] = s;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.RemoveDrone(int idDrone)
        {
            int index = DataSource.DronesList.FindIndex(d => d.Id == idDrone);
            if (index == -1)
                throw new BadIdException(idDrone, $"The Drone id not exist:{idDrone}");
            DroneData d = DataSource.DronesList[index];
            d.Deleted = true;
            DataSource.DronesList[index] = d;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.RemoveCustomer(int idCustomer)
        {
            int index = DataSource.CustomersList.FindIndex(d => d.Id == idCustomer);
            if (index == -1)
                throw new BadIdException(idCustomer, $"The Customer id not exist:{idCustomer}");
            CustomerData c = DataSource.CustomersList[index];
            c.Deleted = true;
            DataSource.CustomersList[index] = c;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.RemoveParcel(int idParcel)
        {
            int index = DataSource.ParcelsList.FindIndex(p => p.Id == idParcel);
            if (index == -1)
                throw new BadIdException(idParcel, $"The Parcel id not exist:{idParcel}");
            ParcelData p = DataSource.ParcelsList[index];
            p.Deleted = true;
            DataSource.ParcelsList[index] = p;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        double IDal.UpdateChargeTime(int droneId)
        {
            double chargeTime;
            int index = DataSource.DroneCharges.FindIndex(c => c.Droneld == droneId);
            if (index == -1)
                throw new BadIdException(droneId, $"The DronesList id not found:{droneId}");
            
            DroneCharge charge = DataSource.DroneCharges[index];
            DateTime now = DateTime.Now;
           
            chargeTime= (now-charge.TimeSet).TotalHours;// the time the drone was is charge until now
            
            charge.TimeSet = now; // update the time of charge to now
            DataSource.DroneCharges[index] = charge;
            return chargeTime;
        }
        void IDal.RemoveDroneCharge(int droneId)
        {
            if (!DataSource.DroneCharges.Exists(d => d.Droneld== droneId))
                throw new BadIdException(droneId, $"The drone isn't in charge {droneId}");

            DataSource.DroneCharges.RemoveAll(d => d.Droneld == droneId);
            return;
        }
    }
    internal sealed partial class DalObject : IDal
    {
        // *********Update methods***********
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateBaseStation(StationData newStation)
        {
            int index = DataSource.BaseStationsList.FindIndex(bs => bs.Id == newStation.Id);
            if (index == -1)
                throw new BadIdException(newStation.Id, $"The BaseStationt id not exist:{newStation.Id}");
            DataSource.BaseStationsList[index] = newStation;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateDrone(DroneData newDrone)
        {
            int index = DataSource.DronesList.FindIndex(d => d.Id == newDrone.Id);
            if (index == -1)
                throw new BadIdException(newDrone.Id, $"The DronesList id not found:{newDrone.Id}");
            DataSource.DronesList[index] = newDrone;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateCustomer(CustomerData newCustomer)
        {
            int index = DataSource.CustomersList.FindIndex(c => c.Id == newCustomer.Id);
            if (index == -1)
                throw new BadIdException(newCustomer.Id, $"The Customer id not exist:{newCustomer.Id}");
            DataSource.CustomersList[index] = newCustomer;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IDal.UpdateParcel(ParcelData newParcel)
        {
            int index = DataSource.ParcelsList.FindIndex(p => p.Id == newParcel.Id);
            if (index == -1)
                throw new BadIdException(newParcel.Id, $"The Parcel id not exist:{newParcel.Id}");
            DataSource.ParcelsList[index] = newParcel;
        }
    }
    internal sealed partial class DalObject : IDal
    {
        // ********Get methods**********
        [MethodImpl(MethodImplOptions.Synchronized)]
        StationData IDal.GetBaseStation(int id)
        {
            StationData s = DataSource.BaseStationsList.Find(s => s.Id == id && s.Deleted == false);
            if (s.Id == 0)
            {
                throw new BadIdException(id, $"The Base-Station id:{id} not exist");
            }
            return s;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        DroneData IDal.GetDrone(int id)
        {
            DroneData d = DataSource.DronesList.Find(d => d.Id == id && d.Deleted == false);
            if (d.Id == 0)
            {

                throw new BadIdException(d.Id, $"The drone id:{d.Id} not exist");
            }
            return d;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        CustomerData IDal.GetCustomer(int id)
        {
            CustomerData c = DataSource.CustomersList.Find(c => c.Id == id && c.Deleted == false);
            if (c.Id == 0)
                throw new BadIdException(id, $"The Costumer id:{id} not exist");
            return c;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        ParcelData IDal.GetParcel(int id)
        {
            ParcelData p = DataSource.ParcelsList.Find(p => p.Id == id && p.Deleted == false);
            if (p.Id == 0)
                throw new BadIdException(id, $"The Parcel id:{id} not exist");
            return p;
        }
    }
    internal sealed partial class DalObject : IDal
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<StationData> IDal.GetStationsList()
        {
            var stations = DataSource.BaseStationsList.Where(s => s.Deleted == false).ToList();
            return stations;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<DroneData> IDal.GetDronesList()
        {
            var drones = DataSource.DronesList.Where(d => d.Deleted == false).ToList();
            return drones;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<CustomerData> IDal.GetCustomersList()
        {
            var customers = DataSource.CustomersList.Where(c => c.Deleted == false).ToList();
            return customers;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<ParcelData> IDal.GetParcelsList()
        {
            var parcels = DataSource.ParcelsList.Where(p => p.Deleted == false).ToList();
            return parcels;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<ParcelData> IDal.GetSpecificParcels(IDal.ParcelCondition meetCondition)
        {
            var parcels = DataSource.ParcelsList
           .Where(parcel => meetCondition(parcel) && parcel.Deleted == false).ToList();
            return parcels;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<StationData> IDal.GetSpecificStations(IDal.StationCondition meetCondition)
        {
            var stations = DataSource.BaseStationsList
           .Where(station => meetCondition(station) && station.Deleted == false).ToList();
            return stations;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<CustomerData> IDal.GetSpecificCustomers(IDal.CustomerCondition meetCondition)
        {
            var customers = DataSource.CustomersList
           .Where(customer => meetCondition(customer) && customer.Deleted == false).ToList();
            return customers;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<DroneData> IDal.GetSpecificDrones(IDal.DroneCondition meetCondition)
        {
            var drones = DataSource.DronesList
           .Where(drone => meetCondition(drone) && drone.Deleted == false).ToList();
            return drones;
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        IEnumerable<DroneCharge> IDal.GetDroneChargesList()
        {
            var charges = DataSource.DroneCharges.ToList();
            return charges;
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        int IDal.AvailableChargingSlots(int stationID)
        {
            int UsedSlots = 0; // check how much times this station appear at DroneCharges list
            foreach (DroneCharge charge in DataSource.DroneCharges)
            {
                if (stationID== charge.Stationld)
                    UsedSlots++;
            }
            return DataSource.BaseStationsList.Find(s=>s.Id==stationID).ChargeSlots - UsedSlots;
        }
    }
}