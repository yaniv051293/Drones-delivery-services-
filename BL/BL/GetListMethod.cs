
using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using BlApi;
using System.Runtime.CompilerServices;

namespace BO
{
    sealed internal partial class BL : IBL
    {
        //*********** Print list methods*************

        /// <summary>
        /// Get the Stations List
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetStationsList()
        {
            return data.GetStationsList()
                .Select(baseStation =>
                    new StationToList
                    (baseStation.Id,
                      baseStation.Name,
                      data.AvailableChargingSlots(baseStation.Id),
                      baseStation.ChargeSlots - data.AvailableChargingSlots(baseStation.Id)));
        }
        /// <summary>
        /// Get the Drones List
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetDronesList()
        {
            return droneList.Where(d=>d.Deleted==false);
        }

        /// <summary>
        /// Get the Customers List
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<CustomerToList> GetCustomersList()
        {
            List<CustomerToList> customers = new List<CustomerToList>();
            foreach (var c in data.GetCustomersList())
            {
                CustomerToList myCustomer = new CustomerToList(c.Id, c.Name, c.Phone);
                UpdatesCustomerParcels(ref myCustomer);
                customers.Add(myCustomer);
            }
            return customers;
        }

        /// <summary>
        /// Updates Customer Parcels
        /// </summary>
        /// <param name="myCustomer"></param>
        private void UpdatesCustomerParcels(ref CustomerToList myCustomer)
        {
            int countSendProvide = 0, countSendUnprovide = 0, countRecieved = 0, countNotRecieved = 0;
            foreach (ParcelData p in data.GetParcelsList())
            {
                if (p.SenderId == myCustomer.Id)
                    if (p.Delivered != null)// if the parcel provided
                        countSendProvide++;
                    else                             //the parcel not provided yet
                        countSendUnprovide++;

                if (p.TargetId == myCustomer.Id)
                    if (p.Delivered != null)// if the parcel provided
                        countRecieved++;
                    else                              //the parcel not provided yet
                        countNotRecieved++;
            }
            myCustomer.ParcelsProvided = countSendProvide;
            myCustomer.ParcelsNotProvided = countSendUnprovide;
            myCustomer.ParcelsRecieved = countRecieved;
            myCustomer.ParcelsNotRecieved = countNotRecieved;
        }
        /// <summary>
        /// Get the Parcels List
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetParcelsList()
        {
            return data.GetParcelsList()
                    .Select(parcel =>
                        new ParcelToList
                        (parcel.Id, parcel.SenderId, parcel.TargetId,
                           (WeightCategories)parcel.Wieght, (Priorities)parcel.Priority, GetParcelStatus(parcel.Id)));
            }
        /// <summary>
        /// Get the Parcel Status
        /// </summary>
        /// <param name="parcelId"></param>
        /// <returns></returns>
        private ParcelStatus GetParcelStatus(int parcelId)
        {
            if (data.GetParcel(parcelId).Scheduled == null)//if Scheduled time==0,
                return ParcelStatus.defined;                             // the parcel doesn't link to drone
            if (data.GetParcel(parcelId).PickedUp == null) //if Scheduled time!=0 & picked up time==0
                return ParcelStatus.associated;                          // the parcel link to drone but not picked-up yet
            if (data.GetParcel(parcelId).Delivered == null)//if Scheduled time!=0 & picked-up time!=0 & delivered time ==0
                return ParcelStatus.collected;                           // the parcel picked-up but not arrive to the target yet
            return ParcelStatus.provided;       //if all times!=0 the parcel provided
        }

        /// <summary>
        /// Get the Free Parcel List
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetFreeParcelList()
        {
            List<ParcelToList> parcels = new List<ParcelToList>();
            foreach (var p in data.GetSpecificParcels((ParcelData p) => p.DroneId == null))
            {
                parcels.Add(new ParcelToList(p.Id, p.SenderId, p.TargetId, (WeightCategories)p.Wieght, (Priorities)p.Priority, ParcelStatus.defined));
            }
            return parcels;
        }

        /// <summary>
        /// Get the Available Stations List
        /// </summary>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetAvailableStationsList()
        {
            return data.GetSpecificStations((StationData s) => data.AvailableChargingSlots(s.Id) > 0)
           .Select(baseStation =>
                  new StationToList
                 (baseStation.Id,
                  baseStation.Name,
                  data.AvailableChargingSlots(baseStation.Id),
                  baseStation.ChargeSlots - data.AvailableChargingSlots(baseStation.Id)));
        }

        /// <summary>
        /// Get the Specific Stations
        /// </summary>
        /// <param name="availbleSlots"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<StationToList> GetSpecificStations(int availbleSlots)//(StationData s) => data.AvailableChargingSlots(s) > 0
        {
            return data.GetSpecificStations(s=> data.AvailableChargingSlots(s.Id)== availbleSlots)
                            .Select(baseStation =>
                                new StationToList
                                (baseStation.Id,
                                  baseStation.Name,
                                  availbleSlots,
                                  baseStation.ChargeSlots - availbleSlots));
        }

        /// <summary>
        /// Get the Specific Parcels
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<ParcelToList> GetSpecificParcels(Priorities? priority, WeightCategories? weight)
        {
            if (priority != null && weight == null)
            {
                return data.GetSpecificParcels(p => p.Priority == (DO.Priorities)priority)
        .Select(parcel =>
            new ParcelToList
            (parcel.Id, parcel.SenderId, parcel.TargetId,
               (WeightCategories)parcel.Wieght, (Priorities)parcel.Priority, GetParcelStatus(parcel.Id)));
            }
            if (priority == null && weight != null)
            {
                return data.GetSpecificParcels(p => p.Wieght == (DO.WeightCategories)weight)
        .Select(parcel =>
            new ParcelToList
            (parcel.Id, parcel.SenderId, parcel.TargetId,
               (WeightCategories)parcel.Wieght, (Priorities)parcel.Priority, GetParcelStatus(parcel.Id)));
            }
            throw new Exception();
        }

        /// <summary>
        /// Get Specific Drones with status and weight
        /// </summary>
        /// <param name="status"></param>
        /// <param name="weight"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public IEnumerable<DroneToList> GetSpecificDrones(DroneStatus? status, WeightCategories? weight)
        {
            if( status != null&& weight == null)//sort by status
            {
                return droneList.Where(d => d.Deleted == false && d.Status==status);
            }
            if (weight!= null && status == null)//sort by wieght
            {
                return droneList.Where(d => d.Deleted == false&& d.MaxWeight==weight);
            }
            return droneList.Where(d => d.Deleted == false);
        }
    }
}
