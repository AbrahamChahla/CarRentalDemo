using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using Core.Common.Contracts;

namespace CarRental.Data.Contracts.Repository_Interfaces
{
    public interface IRentalRepository : IDataRepository<Rental>
    {
        IEnumerable<Rental> GetRentalHistoryByCar(int id);
        Rental GetCurrentRentalByCar(int id);
        IEnumerable<Rental> GetCurrentlyRentedCars();
        IEnumerable<Rental> GetRentalHistoryByAccount(int accountId);
        IEnumerable<CustomerRentalInfo> GetCurrentCustomerInfo();
    }
}
