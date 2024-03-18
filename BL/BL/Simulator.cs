using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using System.Threading;
using BlApi;


namespace BO
{
    class Simulator
    {
        int DELAY = 1000;
        int SPEED = 20;// k"m per  6 seconds
        Double[] batteryUse = new double[5];
        public Simulator(BL bl, int id, Action updatePL, Func<bool> stop)
        {
            batteryUse = bl.data.GetBatteryUse();

            while (!stop())
            {
                Drone drone = bl.GetDrone(id);
                switch (drone.Status)
                {
                    case DroneStatus.delivery:
                        DO.ParcelData parcel = bl.data.GetParcel(drone.Parcel.Id);
                        if (parcel.PickedUp == null)
                        {
                            CollectParcel( drone, parcel);
                        }
                        else
                        {
                            if (parcel.Delivered == null)
                                UpdateParcelProvided( drone, parcel);
                        }
                        break;

                    case DroneStatus.available:
                        try//try to link to parcel
                        {
                            bl.LinkParcelToDrone(id);
                            updatePL();
                            Thread.Sleep(DELAY);
                        }
                        catch//can't link to parcel
                        {
                            if (drone.Battery < 100)
                            {
                                try { bl.SetDroneToCharge(id); }
                                catch// emernecy refill battery for stuck drone
                                {
                                    bl.UpdateDrone(drone.Id, drone.Battery + 20,drone.CurrentLocation);
                                    updatePL();
                                    throw new Exception("The drone stuck without battery and can't arrive to base station\n" +
                                                         "An emergency battery was sent to charge the drone 20 %");
                                }
                                updatePL();
                                Thread.Sleep(DELAY);
                            }
                            else  //the battery 100 % but still isn't found parcel to link
                                while (drone.Status == DroneStatus.available)
                                {
                                    try { bl.LinkParcelToDrone(id); }
                                    catch { Thread.Sleep(5000); }

                                }
                        }
                        break;

                    case DroneStatus.maintenance:
                        if (drone.Battery == 100)
                        {
                            bl.ReleaseDroneFromCharge(id);
                            updatePL();
                            Thread.Sleep(DELAY);
                        }
                        else
                        {
                            double newBattery = drone.Battery + bl.data.UpdateChargeTime(id) * batteryUse[4];
                            drone.Battery = newBattery <= 100 ? newBattery : 100;
                            bl.UpdateDrone(id, drone.Battery,drone.CurrentLocation);
                            updatePL();
                            Thread.Sleep(DELAY);
                        }
                        break;
                }
                updatePL();
            }

            void CollectParcel( Drone drone, DO.ParcelData parcel)
            {
                Location senderLocation = new Location(bl.GetCustomerLocation(parcel.SenderId));
                double distance = bl.Distance(drone.CurrentLocation, senderLocation);
                while (SPEED < distance)
                {
                    drone.Battery -= bl.CalculateBattery(SPEED, DroneStatus.available);
                    bl.UpdateDrone(id, drone.Battery, drone.CurrentLocation);
                    distance -= SPEED;
                    updatePL();
                    Thread.Sleep(DELAY);
                }

                //update the drone collect the parcel
                drone.Battery -= bl.CalculateBattery(distance, DroneStatus.available);
                bl.UpdateDrone(id, drone.Battery, senderLocation);

                //update detail in parcel.
                parcel.PickedUp = DateTime.Now;
                try { bl.data.UpdateParcel(parcel); }
                catch (Exception e) { throw new ExistIdException($"The Parcel id not exist:{parcel.Id}", e); }
                updatePL();
                Thread.Sleep(DELAY);
            }

            void UpdateParcelProvided( Drone drone, DO.ParcelData parcel)
            {
                //update detail in drone.
                Location targetLocation = bl.GetCustomerLocation(parcel.TargetId);
                double distance = bl.Distance(drone.CurrentLocation, targetLocation);
                while (SPEED < distance)
                {
                    drone.Battery -= bl.CalculateBattery(SPEED, DroneStatus.delivery, parcel.Wieght);
                    bl.UpdateDrone(id, drone.Battery, drone.CurrentLocation);
                    distance -= SPEED;
                    updatePL();
                    Thread.Sleep(DELAY);
                }

                drone.Battery -= bl.CalculateBattery(distance, DroneStatus.delivery, parcel.Wieght);
                bl.UpdateDrone(id, drone.Battery,targetLocation, DroneStatus.available,null);

                //update detail in parcel.
                parcel.Delivered = DateTime.Now;
                try { bl.data.UpdateParcel(parcel); }
                catch (Exception e) { throw new ExistIdException($"The Parcel id not exist:{parcel.Id}", e); }
               
                updatePL();
                Thread.Sleep(DELAY);
            }
        }
    }
}

