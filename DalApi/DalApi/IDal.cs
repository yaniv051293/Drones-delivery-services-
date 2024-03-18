using DO;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
namespace DalApi
{
    public interface IDal
    {

        //******* Add methods********
        public void AddBaseStation(StationData newStation);
        public void AddDrone(DroneData newDrone);
        public void AddCustomer(CustomerData newCustomer);
        public void AddParcel(ParcelData newParcel);
        public void AddDroneCharge(DroneCharge newCharge);

        //********delete methods ******
        public void RemoveBaseStation(int idStation);
        public void RemoveDrone(int idDrone);
        public void RemoveCustomer(int idCustomer);
        public void RemoveParcel(int idParcel);
        public void RemoveDroneCharge(int droneId);

        //******* update methods********
        public void UpdateBaseStation(StationData newStation);
        public void UpdateDrone(DroneData newDrone);
        public void UpdateCustomer(CustomerData newCustomer);
        public void UpdateParcel(ParcelData newParcel);


        // ********Get methds**********
        public StationData GetBaseStation(int stationId);
        public DroneData GetDrone(int droneId);
        public CustomerData GetCustomer(int customerId);
        public ParcelData GetParcel(int parcelId);

        //*******get list ******
        IEnumerable<StationData> GetStationsList();
        IEnumerable<DroneData> GetDronesList();
        IEnumerable<CustomerData> GetCustomersList();
        IEnumerable<ParcelData> GetParcelsList();

        delegate bool ParcelCondition(ParcelData p);
        IEnumerable<ParcelData> GetSpecificParcels(ParcelCondition cond);

        delegate bool StationCondition(StationData s);
        IEnumerable<StationData> GetSpecificStations(StationCondition cond);

        delegate bool CustomerCondition(CustomerData c);
        IEnumerable<CustomerData> GetSpecificCustomers(CustomerCondition cond);

        delegate bool DroneCondition(DroneData d);
        IEnumerable<DroneData> GetSpecificDrones(DroneCondition cond);
        IEnumerable<DroneCharge> GetDroneChargesList();

        //******Nother Method*****
        int AvailableChargingSlots(int StationId);
        public double[] GetBatteryUse();

         double UpdateChargeTime(int droneId);
    }
}