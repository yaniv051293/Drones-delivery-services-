using DalApi;
using DO;
using System;
using BlApi;
using System.Runtime.CompilerServices;



namespace BO
{
    internal partial class BL : IBL
    {
        //******* Add methods********

        /// <summary>
        /// get station  it into the Data 
        /// </summary>
        /// <param name="newStation"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.AddBaseStation(Station newStation)
        {
            CheckInt(newStation.Id, "id");
            CheckInt(newStation.AvailableSlots, "Available-Slots");
            Checklen(newStation.StationLocation.Longitude, "Longitude");
            Checklen(newStation.StationLocation.Lattitude, "Lattitude");

            StationData forSend = new StationData(newStation.Id, newStation.Name
            , newStation.StationLocation.Longitude, newStation.StationLocation.Lattitude, newStation.AvailableSlots);
            try
            {
                data.AddBaseStation(forSend);
            }
            catch (BadIdException e)
            { throw new ExistIdException($"The base-station id :{newStation.Id} alredy exist", e); }
        }

        /// <summary>
        /// get drone class and set it into the list and create drone struct  
        /// </summary>
        /// <param name = "newDrone" ></ param >
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.AddDrone(Drone newDrone, int stationID)
        {
            CheckInt(newDrone.Id, "ID station"); CheckInt(newDrone.Id, "id drone");
            DroneToList myDrone = new DroneToList(newDrone.Id, newDrone.Model, newDrone.MaxWeight);
            myDrone.Battery = random.Next(0, 21);
            myDrone.Status = DroneStatus.maintenance;
            myDrone.ParcelId = null;
            myDrone.CurrentLocation = GetStationLocation(stationID);
            DroneData dForSend = new DroneData(newDrone.Id, newDrone.Model, (DO.WeightCategories)newDrone.MaxWeight);
            try
            {
                data.AddDrone(dForSend);
                droneList.Add(myDrone);
                data.AddDroneCharge(new DroneCharge(myDrone.Id, stationID));
            }
            catch (BadIdException e) { throw new ExistIdException($"The drone id :{newDrone.Id} alredy exist", e); }
        }

        /// <summary>
        /// get customer struct and set it into the array 
        /// </summary>
        /// <param name = "newCustomer" ></ param >
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.AddCustomer(Customer newCustomer)
        {
            CheckInt(newCustomer.Id, "ID");

            CustomerData cForSend = new CustomerData(newCustomer.Id, newCustomer.Name, newCustomer.Phone,
                newCustomer.location.Longitude, newCustomer.location.Lattitude);
            try
            { data.AddCustomer(cForSend); }
            catch (BadIdException e) { throw new ExistIdException($"The customer id alredy exist:{newCustomer.Id}", e); }

        }

        /// <summary>
        /// get parcel struct and set it into the array 
        /// </summary>
        /// <param name = "newParcel" ></ param >
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.AddParcel(Parcel newParcel)
        {
            CheckInt(newParcel.Sender.Id, "Sender-Id"); CheckInt(newParcel.Target.Id, "Target-Id");
            ParcelData cForSend = new ParcelData(newParcel.Id, newParcel.Sender.Id, newParcel.Target.Id,
               (DO.WeightCategories)newParcel.Wieght, (DO.Priorities)newParcel.Priority);

            try
            { data.AddParcel(cForSend); }
            catch (BadIdException e) { throw new ExistIdException($"The Parcel id alredy exist:{newParcel.Id}", e); }
        }
    }
}


