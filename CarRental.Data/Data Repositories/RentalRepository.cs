using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarRental.Business.Entities;
using CarRental.Data;
using CarRental.Data.Contracts.Repository_Interfaces;

namespace CarRental.Data.Data_Repositories
{
    [Export(typeof(IRentalRepository))]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class RentalRepository : DataRepositoryBase<Rental>, IRentalRepository
    {
        protected override Rental AddEntity(CarRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.Add(entity);
        }

        protected override Rental UpdateEntity(CarRentalContext entityContext, Rental entity)
        {
            return entityContext.RentalSet.FirstOrDefault(e => e.RentalId == entity.RentalId);
        }

        protected override IEnumerable<Rental> GetEntities(CarRentalContext entityContext)
        {
            return entityContext.RentalSet.Select(e => e);
        }

        protected override Rental GetEntity(CarRentalContext entityContext, int id)
        {
            return entityContext.RentalSet.FirstOrDefault(e => e.RentalId == id);
        }

        public IEnumerable<Rental> GetRentalHistoryByCar(int id)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return entityContext.RentalSet
                    .Where(e => e.CarId == id)
                    .Select(e => e).ToFullyLoaded();

            }
        }

        public Rental GetCurrentRentalByCar(int id)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return entityContext.RentalSet
                    .Where(e => e.CarId == id && e.DateReturned == null)
                    .Select(e => e).FirstOrDefault();
            }
        }

        public IEnumerable<Rental> GetCurrentlyRentedCars()
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return entityContext.RentalSet
                    .Where(e => e.DateReturned == null)
                    .Select(e => e).ToFullyLoaded();
            }
        }

        public IEnumerable<Rental> GetRentalHistoryByAccount(int accountId)
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                return entityContext.RentalSet
                    .Where(e => e.AccountId == accountId)
                    .Select(e => e).ToFullyLoaded();
            }
        }

        public IEnumerable<CustomerRentalInfo> GetCurrentCustomerInfo()
        {
            using (CarRentalContext entityContext = new CarRentalContext())
            {
                var query = from r in entityContext.RentalSet
                            where r.DateReturned == null
                            join a in entityContext.AccountSet on r.AccountId equals a.AccountId
                            join c in entityContext.CarSet on r.CarId equals c.CarId
                            select new CustomerRentalInfo()
                            {
                                Customer = a,
                                Car = c,
                            }
            }
        }
    }
}
