using BlApi;
using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace BO
{
    sealed internal partial class BL : IBL
    {
        //******* remove methods********
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.RemoveBaseStation(int StationId) 
        {
            try
            {
                data.RemoveBaseStation(StationId);
            }
            catch (BadIdException e) { throw new ExistIdException($"The Station id :{StationId} is not exist", e); }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.RemoveDrone(int droneId)
        {
            try
            {
                int index = GetDroneIndex(droneId);
                DroneToList d = droneList[index];

                if (d.Status == DroneStatus.maintenance)// if the drone in charge, release it
                    data.RemoveDroneCharge(droneId);
                if (d.Status == DroneStatus.delivery)
                {
                    ParcelData parcel = data.GetParcel((int)d.ParcelId);
                    if (parcel.PickedUp == null)
                    {
                        CollectParcel(droneId);
                        UpdateParcelProvided(droneId);
                    }
                    else
                        UpdateParcelProvided(droneId);
                }
                d.Deleted = true; // delete from the list at BL
                droneList[index] = d;

                data.RemoveDrone(droneId); // delete from Dal
            }
            catch (BadIdException e) { throw new ExistIdException($"The drone id :{droneId} is not exist", e); }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.RemoveCustomer(int CustomerId) 
        {
            try
            {
                data.RemoveCustomer(CustomerId);
            }
            catch (BadIdException e) { throw new ExistIdException($"The Customer id :{CustomerId} is not exist", e); }
        }
        [MethodImpl(MethodImplOptions.Synchronized)]
        void IBL.RemoveParcel(int ParcelId) 
        {
            try
            {
                if (droneList.Exists(d => d.ParcelId == ParcelId)) // if this parcel link to any drone 
                {
                    throw new LinkException ("cannot remove parcel.\n The parcel link to any drone");
                }
                data.RemoveParcel(ParcelId);
            }
            catch (BadIdException e) { throw new ExistIdException($"The Parcel id :{ParcelId} is not exist", e); }
        }
    }
}


