using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Account : ObjectBase
    {

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
        private string _LoginEmail;

        public string LoginEmail
        {
            get { return _LoginEmail; }
            set
            {
                _LoginEmail = value;
                OnPropertyChanged(() => _LoginEmail);
            }
        }
        private string _FirstName;

        public string FirstName
        {
            get { return _FirstName; }
            set
            {
                _FirstName = value;
                OnPropertyChanged(() => FirstName);
            }
        }
        private string _LastName;

        public string LastName
        {
            get { return _LastName; }
            set
            {
                _LastName = value;
                OnPropertyChanged(() => LastName);
            }
        }
        private string _Address;

        public string Address
        {
            get { return _Address; }
            set
            {
                _Address = value;
                OnPropertyChanged(() => Address);
            }
        }
        private string _City;

        public string City
        {
            get { return _City; }
            set
            {
                _City = value;
                OnPropertyChanged(() => City);
            }
        }
        private string _State;

        public string State
        {
            get { return _State; }
            set
            {
                _State = value;
                OnPropertyChanged(() => State);
            }
        }

        private string _ZipCode;

        public string ZipCode
        {
            get { return _ZipCode; }
            set
            {
                _ZipCode = value;
                OnPropertyChanged(() => ZipCode);
            }
        }

        private string _CreditCard;

        public string CreditCard
        {
            get { return _CreditCard; }
            set
            {
                _CreditCard = value;
                OnPropertyChanged(() => CreditCard);
            }
        }
        private string _ExpDate;

        public string ExpDate
        {
            get { return _ExpDate; }
            set
            {
                _ExpDate = value;
                OnPropertyChanged(() => ExpDate);
            }
        }

    }
}
