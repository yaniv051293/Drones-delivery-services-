using BO;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;


namespace BlApi
{
    public static class BLFactory
    {
        public static IBL GetBL()
        {
            return BL.Instance;
        }
    }

    public interface IBL
    {
        //******* Add methods********
        void AddBaseStation(Station newStation);
        void AddDrone(Drone newDrone, int stationID);
        void AddCustomer(Customer newCustomer);
        void AddParcel(Parcel newParcel);
        //******* delete methods********
        void RemoveBaseStation(int StationId);
        void RemoveDrone(int DroneId);
        void RemoveCustomer(int CustomerId);
        void RemoveParcel(int ParcelId);

        //******* update methods********
        void UpdateDrone(int id, string model);
        void UpdateBaseStation(int id, string model, int Charche);
        void UpdateCustomer(int id, string name, string phone);
        void LinkParcelToDrone(int droneId);
        void CollectParcel(int droneId);
        void UpdateParcelProvided(int droneId);
        void SetDroneToCharge(int droneId);
        void ReleaseDroneFromCharge(int droneId);
        void TurnOnSimulator (int Id, Action updateDrone, Func<bool> stop);

        // ********Print methds**********
        Station GetBaseStation(int id);
        Drone GetDrone(int id);
        Customer GetCustomer(int id);
        Parcel GetParcel(int id);

        //*********** return list *************
        IEnumerable<StationToList> GetStationsList();
        IEnumerable<DroneToList> GetDronesList();
        IEnumerable<CustomerToList> GetCustomersList();
        IEnumerable<ParcelToList> GetParcelsList();
        IEnumerable<ParcelToList> GetFreeParcelList();
        IEnumerable<StationToList> GetAvailableStationsList();

        IEnumerable<DroneToList> GetSpecificDrones(DroneStatus? status, WeightCategories? weight);

        IEnumerable<StationToList> GetSpecificStations(int availableSlots);

        IEnumerable<ParcelToList> GetSpecificParcels(Priorities? priority, WeightCategories? weight);

    }
}