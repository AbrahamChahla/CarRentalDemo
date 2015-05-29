using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Rental : ObjectBase
    {
        private int _RentalId;

        public int RentalId
        {
            get { return _RentalId; }
            set
            {
                _RentalId = value;
                OnPropertyChanged(() => RentalId);
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
        private int _CardId;

        public int CardId
        {
            get { return _CardId; }
            set
            {
                _CardId = value;
                OnPropertyChanged(() => CardId);
            }
        }
        private DateTime _DateRented;

        public DateTime DateRented
        {
            get { return _DateRented; }
            set
            {
                _DateRented = value;
                OnPropertyChanged(() => DateRented);
            }
        }
        private DateTime _DateDue;

        public DateTime DateDue
        {
            get { return _DateDue; }
            set
            {
                _DateDue = value;
                OnPropertyChanged(() => DateDue);
            }
        }
        private DateTime? _DateReturned;

        public DateTime? DateReturned
        {
            get { return _DateReturned; }
            set
            {
                _DateReturned = value;
                OnPropertyChanged(() => DateReturned);
            }
        }

    }
}
