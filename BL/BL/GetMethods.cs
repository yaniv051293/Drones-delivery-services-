
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
        // ********Print methds**********


        [MethodImpl(MethodImplOptions.Synchronized)]
        public Station GetBaseStation(int stationId)
        {
            CheckInt(stationId, "station-Id");
            try
            {
                StationData s = data.GetBaseStation(stationId);
                int availableSlotes = data.AvailableChargingSlots(s.Id);
                int usedSlotes = (int)(s.ChargeSlots - availableSlotes);
                Station newStation = new Station(s.Id, s.Name,
                    new Location(s.Lattitude, s.Longitude), availableSlotes, usedSlotes);
                UpdateDronesinCharge(newStation);
                return newStation;
            }
            catch (Exception e)
            {
                throw new ExistIdException($"The id:{stationId} of station does'nt exist", e);
            }
        }



        [MethodImpl(MethodImplOptions.Synchronized)]
        public Drone GetDrone(int droneId)
        {
            DroneToList d = droneList[GetDroneIndex(droneId)];
            Drone newDrone = new Drone(d.Id, d.Model, d.MaxWeight, d.Battery, d.CurrentLocation, d.Status);
            if (d.ParcelId != null)
                newDrone.Parcel = CreateParcelDeliver((int)d.ParcelId);
            return newDrone;
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public Customer GetCustomer(int id)
        {
            CheckInt(id, "Customer");
            try
            {
                CustomerData c = data.GetCustomer(id);
                Customer newCustomer = new Customer(c.Id, c.Name, c.Phone, new Location(c.Lattitude, c.Longitude));
                UpdateParcelFromCustomer(newCustomer);
                UpdateParcelToCustomer(newCustomer);
                return newCustomer;
            }
            catch (Exception e)
            {
                throw new ExistIdException($"The customer id:{id} doe'nt exist", e);
            }
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        public Parcel GetParcel(int id)
        {
            try
            {
                ParcelData p = data.GetParcel(id);
                Parcel newParcel = new Parcel(p.Id, CreateCostumerInParcel(p.SenderId), CreateCostumerInParcel(p.TargetId),
                                              (WeightCategories)p.Wieght, (Priorities)p.Priority
                                             , p.Requested, p.Scheduled, p.PickedUp, p.Delivered);
                if (p.DroneId != null&& p.Delivered==null)
                    newParcel.HisDrone = CreateDroneInParcel((int)p.DroneId);
                return newParcel;
            }
            catch (Exception e)
            {
                throw new ExistIdException($"The parcel id:{id} doe'nt exist", e);
            }
        }


        /// <summary>
        /// fill the list of drones that charge at this station
        /// </summary>
        /// <param name="station"></param>
        private void UpdateDronesinCharge(Station station)
        {
            station.dronesinCharge.Clear();
            foreach (var charge in data.GetDroneChargesList())
                if (charge.Stationld == station.Id)
                    station.AddDroneInCharge(new DroneInCharge(charge.Droneld,
                                                               droneList[GetDroneIndex(charge.Droneld)].Battery));
        }

        /// <summary>
        /// create and return ParcelInDelivery object
        /// </summary>
        /// <param name="p"></param>
        /// <param name="mydrone"></param>
        /// <returns></returns>
        private ParcelInDelivery CreateParcelDeliver(int parcelId)
        {
            try
            {
                ParcelData p = data.GetParcel(parcelId);
                ParcelInDelivery myParcel = new ParcelInDelivery(p.Id, (WeightCategories)p.Wieght, (Priorities)p.Priority);
                myParcel.Sender = CreateCostumerInParcel(p.SenderId);       //details of sender costumer
                myParcel.Target = CreateCostumerInParcel(p.TargetId);       //details of reicve costumer
                myParcel.SenderLocation = GetCustomerLocation(p.SenderId);     //location of sender costumer
                myParcel.targetLocation = GetCustomerLocation(p.TargetId);    //location of reicve costumer
                myParcel.Distance = Distance(myParcel.SenderLocation, myParcel.targetLocation); //distance between sender to reciever

                if (p.PickedUp == null)
                    myParcel.AtWay = false;         // boolian property- Is the parcel on the way?
                else
                    myParcel.AtWay = true;
                return myParcel;
            }
            catch (Exception e) { throw new ExistIdException($"The parcel:{parcelId} does'nt exist ", e); }
        }

        /// <summary>
        /// create and return CostumerInParcel object
        /// </summary>
        /// <param name="costumerId"></param>
        /// <returns></returns>
        private CustomerInParcel CreateCostumerInParcel(int costumerId)
        {
            return new CustomerInParcel(costumerId, data.GetCustomer(costumerId).Name);
        }
        private void UpdateParcelFromCustomer(Customer c)
        {
            foreach (ParcelData p in data.GetParcelsList())
            {
                if (p.SenderId == c.Id)
                    c.FromCustomer.Add(CreateParcelInCustomer(p, CreateCostumerInParcel(p.TargetId)));
            }
        }
        private void UpdateParcelToCustomer(Customer c)
        {
            foreach (ParcelData p in data.GetParcelsList())
            {
                if (p.TargetId == c.Id)
                    c.ToCustomer.Add(CreateParcelInCustomer(p, CreateCostumerInParcel(p.SenderId)));
            }
        }
        private ParcelInCustomer CreateParcelInCustomer(ParcelData p,CustomerInParcel c)
        {
            return new ParcelInCustomer(p.Id, (WeightCategories)p.Wieght, (Priorities)p.Priority, GetParcelStatus(p.Id),c);
        }
        private DroneInParcel CreateDroneInParcel(int droneId)
        {
            int index = GetDroneIndex(droneId);
            return new DroneInParcel(droneId, droneList[index].Battery, droneList[index].CurrentLocation);
        }
    }
}
