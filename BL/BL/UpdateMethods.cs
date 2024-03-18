
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

        //********update method*****
        /// <summary>
        ///  1-Update of drone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id, string model)
        {
            CheckInt(id, "id drone");
            try
            {
                int index = GetDroneIndex(id);
                droneList[index].Model = model;
                DroneData D = data.GetDrone(id);
                D.Model = model;
                data.UpdateDrone(D);
            }
            catch (Exception e)
            { throw new ExistIdException($"There is no Drone id:{id} ", e); }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id,double battery,Location location, DroneStatus status,int? parcelId)
        {
            CheckInt(id, "id drone");
            try
            {
                int index = GetDroneIndex(id);
                droneList[index].Battery = battery;
                droneList[index].CurrentLocation = location;
                droneList[index].Status = status;
                droneList[index].ParcelId = parcelId;
            }
            catch (Exception e)
            { throw new ExistIdException($"There is no Drone id:{id} ", e); }
        }

        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateDrone(int id, double battery, Location location)
        {
            CheckInt(id, "id drone");
            try
            {
                int index = GetDroneIndex(id);
                droneList[index].Battery = battery;
                droneList[index].CurrentLocation = location;
            }
            catch (Exception e)
            { throw new ExistIdException($"There is no Drone id:{id} ", e); }
        }

        /// <summary>
        /// 2-update of station details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="charge"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateBaseStation(int id, string name, int charge)
        {
            CheckInt(id, " base station"); CheckInt(charge, "charge");
            try
            {
                StationData S = data.GetBaseStation(id);
                S.Name = name;
                S.ChargeSlots = charge;
                data.UpdateBaseStation(S);

            }
            catch (Exception e)
            { throw new ExistIdException($"There is no base station id:{id} ", e); }
        }

        /// <summary>
        /// 3-update of customer details
        /// </summary>
        /// <param name="id"></param>
        /// <param name="phone"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateCustomer(int id, string name, string phone)
        {
            try
            {

                CustomerData c = data.GetCustomer(id);
                c.Name = name;
                c.Phone = phone;
                data.UpdateCustomer(c);

            }
            catch (Exception e)
            { throw new ExistIdException($"Thare is no customer id to update:{id} ", e); }
        }


        /// <summary>
        /// 4- update setting of drone to charging in station
        /// </summary>
        /// <param name="droneId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void SetDroneToCharge(int droneId)
        {
            int index = GetDroneIndex(droneId);// find the index in droneslist

            if (droneList[index].Deleted == true)
                throw new DeletedException("The drone isn't exit\n");
            if (droneList[index].Status != DroneStatus.available)
                throw new AvailableException("The drone isn't available\n");

            StationData chargeStation = GetStationToCharge(droneList[index]);
            checkMinosOne(chargeStation.Id, "There isn't any station that the drone can set to her\n");//check if we find availble station to charge


            // update the details at drone list
            Location stationLocation = GetStationLocation(chargeStation.Id);
            droneList[index].Battery -= CalculateBattery(Distance(droneList[index].CurrentLocation, stationLocation), DroneStatus.available);
            droneList[index].CurrentLocation = stationLocation;
            droneList[index].Status = DroneStatus.maintenance;

            // update the details of this charging in dataSource
            data.AddDroneCharge(new DroneCharge(droneId, chargeStation.Id));

        }

        /// <summary>
        /// 5- update Release Drone From Charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="hoursCharging"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void ReleaseDroneFromCharge(int droneId)
        {
            // CheckInt(droneId, "Drone-Id");
            int index = GetDroneIndex(droneId);
            if (droneList[index].Status != DroneStatus.maintenance)
                throw new ChargeException("The drone isn't in charging\n");
            if (droneList[index].Deleted == true)
                throw new DeletedException("The drone was deleted");
            lock (data)
            {
                double hoursCharging = data.UpdateChargeTime(droneId);
                data.RemoveDroneCharge(droneId);
                double newBattery = droneList[index].Battery + hoursCharging * batteryUse[4];
                droneList[index].Battery = newBattery < 100 ? newBattery : 100;
                droneList[index].Status = DroneStatus.available;
            }
        }
        /// <summary>
        /// 6-link parcel to availabe drone
        /// </summary>
        /// <param name="parcelId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void LinkParcelToDrone(int DroneId)
        {
            int droneIndex = GetDroneIndex(DroneId); // get the drone
                                                     // CheckInt(droneIndex, "Drone-id:");
            if (droneList[droneIndex].Deleted == true)
                throw new DeletedException("The drone was deleted");
            if (droneList[droneIndex].Status != DroneStatus.available)       // check if the drone available
                throw new BusyException("The drone is busy");

            ParcelData myParcel = new ParcelData(FindFreeParcel(droneList[droneIndex]));
            checkMinosOne(myParcel.Id, "There is no parcel that this drone can carry");  // if there no parcel the drone can carry             

            //update drone data
            droneList[droneIndex].Status = DroneStatus.delivery;
            droneList[droneIndex].ParcelId = myParcel.Id;

            //update parcel data
            myParcel.DroneId = DroneId;
            myParcel.Scheduled = DateTime.Now;
            data.UpdateParcel(myParcel);

        }
        /// <summary>
        /// 7-update that drone collect the parcel
        /// </summary>
        /// <param name="parcelId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void CollectParcel(int droneId)
        {
            int index = GetDroneIndex(droneId);
            checkIsNull(droneList[index].ParcelId, "the drone not link to any parcel");

            ParcelData myParcel = new ParcelData(data.GetParcel((int)droneList[index].ParcelId));
            if (myParcel.PickedUp != null)
                throw new CollectedException("the parcel already collected");

            //update detail in parcel.
            myParcel.PickedUp = DateTime.Now;
            try
            {
                lock (data)
                {
                    data.UpdateParcel(myParcel);

                    //update details in droneList.
                    Location senderLocation = new Location(GetCustomerLocation(myParcel.SenderId));
                    droneList[index].Battery -= CalculateBattery(Distance(droneList[index].CurrentLocation, senderLocation), DroneStatus.available);
                    droneList[index].CurrentLocation = senderLocation;
                }
            }
            catch (Exception e) { throw new ExistIdException($"The Parcel id not exist:{myParcel.Id}", e); }
        }

        /// <summary>
        ///  8-update that the delivery reached the Customer
        /// </summary>
        /// <param name="parcelId"></param>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public void UpdateParcelProvided(int droneId)
        {
            int index = GetDroneIndex(droneId);
            checkIsNull(droneList[index].ParcelId, "The drone not link to any parcel");

            lock (data)
            {
                ParcelData myParcel = data.GetParcel((int)droneList[index].ParcelId);
                if (myParcel.PickedUp == null)
                    throw new CollectedException("the parcel was not collected yet");
                if (myParcel.Delivered != null)
                    throw new ProvidedException("the parcel already provided");

                //update detail in parcel.
                myParcel.Delivered = DateTime.Now;

                try
                {
                    data.UpdateParcel(myParcel);

                    //update detail in drone.
                    Location targetLocation = GetCustomerLocation(myParcel.TargetId);
                    droneList[index].Battery -= CalculateBattery(Distance(droneList[index].CurrentLocation, targetLocation),
                                                                 DroneStatus.delivery, myParcel.Wieght);
                    droneList[index].CurrentLocation = targetLocation;
                    droneList[index].Status = DroneStatus.available;
                    droneList[index].ParcelId = null;
                }
                catch (Exception e) { throw new ExistIdException($"The Parcel id not exist:{myParcel.Id}", e); }
            }
        }

        public void TurnOnSimulator( int Id, Action updateDrone, Func<bool> stop)
        {
           Simulator simulator = new Simulator(this, Id, updateDrone, stop); 
        }

        /// <summary>
        /// gets station available for drone to charge
        /// </summary>
        /// <param name="myDrone"></param>
        /// <returns></returns>
        private StationData GetStationToCharge(DroneToList myDrone)
        {
            lock (data)
            {
                StationData stationCharge = new StationData();
                double distanceOfCloseStation = 40070;

                foreach (StationData station in data.GetSpecificStations(s => data.AvailableChargingSlots(s.Id) > 0)) // run on all the available stations
                {// find the closest one which the drone has enough battery to arrive it
                    double thisDistance = Distance(GetStationLocation(station.Id), myDrone.CurrentLocation);
                    double batteryNeed = thisDistance * batteryUse[0];// calcuate the battery need to fly to this station
                    if ((thisDistance < distanceOfCloseStation) && (batteryNeed <= myDrone.Battery))
                    { // if the station closer and there is enough battery to fly to the station, save it
                        distanceOfCloseStation = thisDistance;
                        stationCharge = station;
                    }
                }
                return stationCharge;
            }
        }
        private int GetDroneIndex(int DroneId)
        {
            int index = droneList.FindIndex(d => d.Id == DroneId);
            return index == -1 ? throw new BadIdException(DroneId, $"The drone id:{DroneId} does'nt exist") : index;
        }

        //help methods to linkParcel

        /// <summary>
        /// find free parcel that the drone can link to (due to- 1.barttery 2.weight 3.pirority 4.distance)
        /// </summary>
        /// <param name="myDrone"></param>
        /// <returns></returns>
        private ParcelData FindFreeParcel(DroneToList myDrone)
        {
            lock (data)
            {
                ParcelData freeParcel = new ParcelData(-1);
                double distanceOfClosestParcel = 40070;
                DO.WeightCategories bigWeight = DO.WeightCategories.Lightweight;
                foreach (ParcelData parcel in data.GetSpecificParcels((ParcelData p) => p.DroneId == null))//run on free parcels
                {
                    if ((WeightCategories)parcel.Wieght <= myDrone.MaxWeight // check if the drone can carry parcel weight
                        && IsEnoughBattery(myDrone, parcel) // check if the have enough battery for this delivery
                         && data.GetCustomer(parcel.SenderId).Deleted == false
                         && data.GetCustomer(parcel.TargetId).Deleted == false) // check if the sender and target not deleted
                    {
                        double thisDis = Distance(GetCustomerLocation(parcel.SenderId), myDrone.CurrentLocation);
                        if (parcel.Priority > freeParcel.Priority) //if this parcel is more urgent choose it
                        {
                            bigWeight = parcel.Wieght;
                            distanceOfClosestParcel = thisDis;
                            freeParcel = parcel;
                        }
                        else
                        if ((parcel.Priority == freeParcel.Priority) && (parcel.Wieght > bigWeight))
                        {    //if this parcel has same priorty but this parcel is heavier
                            bigWeight = parcel.Wieght;
                            distanceOfClosestParcel = thisDis;
                            freeParcel = parcel;
                        }
                        else
                        if ((parcel.Priority == freeParcel.Priority) && (parcel.Wieght == bigWeight) && (thisDis < distanceOfClosestParcel))
                        {      //if this parcel has same priorty and wieght but closer to the drone choose it
                            bigWeight = parcel.Wieght;
                            distanceOfClosestParcel = thisDis;
                            freeParcel = parcel;
                        }
                    }
                }
                return freeParcel;
            }
        }
        /// <summary>
        /// return true if the drone has enough battery for fly to target and back to station 
        /// </summary>
        /// <param name="myDrone"></param>
        /// <param name="myParcel"></param>
        /// <returns></returns>
        private bool IsEnoughBattery(DroneToList myDrone, ParcelData myParcel)
        {
            Location senderLocation = new Location(GetCustomerLocation(myParcel.SenderId));
            Location targetLocation = new Location(GetCustomerLocation(myParcel.TargetId));
            double batteryNeed = CalculateBattery(Distance(myDrone.CurrentLocation, senderLocation),
                                                  DroneStatus.available);// Calculate Battery need to fly to the sender
            batteryNeed += CalculateBattery(Distance(senderLocation, targetLocation),
                                           DroneStatus.delivery, myParcel.Wieght);//+Calculate Battery need to fly to target
            batteryNeed += CalculateBattery(Distance(targetLocation, CloseStation(targetLocation)),
                                           DroneStatus.available);//+Calculate Battery need to fly to close station
            if (batteryNeed <myDrone.Battery)
                return true;
            return false;
        }
    }
}