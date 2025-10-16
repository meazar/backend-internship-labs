using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace VehicleRentalAndServices.Service.IService
{
    public interface IVehicle
    {
        
        void RentVehicle(int id);
        void ReturnVehicle(int id);
        void SendForService(int id);
        void CompleteService(int id);
        void DisplayAvailableVehicles();        
        void DisplayserviceVehicles();
    }
}
