using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Reservation : ObjectBase
    {
        private int _ReservationId;

        public int ReservationId
        {
            get { return _ReservationId; }
            set
            {
                _ReservationId = value;
                OnPropertyChanged(() => ReservationId);
            }
        }
        private int _AccountId;

        public int AccountId
        {
            get { return _AccountId; }
            set
            {
                _AccountId = value;
                OnPropertyChanged(() => AccountId);
            }
        }
        private int _CarId;

        public int CarId
        {
            get { return _CarId; }
            set
            {
                _CarId = value;
                OnPropertyChanged(() => CarId);
            }
        }
        private DateTime _ReturnDate;

        public DateTime ReturnDate
        {
            get { return _ReturnDate; }
            set
            {
                _ReturnDate = value;
                OnPropertyChanged(() => ReturnDate);
            }
        }
        private DateTime _RentalDate;

        public DateTime RentalDate
        {
            get { return _RentalDate; }
            set
            {
                _RentalDate = value;
                OnPropertyChanged(() => RentalDate);
            }
        }


    }
}
