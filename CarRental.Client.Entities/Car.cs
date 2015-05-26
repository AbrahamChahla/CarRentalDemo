using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRental.Client.Entities
{
    public class Car : ObjectBase
    {
        private int _CardId;
        private string _Description;
        private string _Color;
        private int _Year;
        private decimal _RentalPrice;
        private bool _CurrentlyRented;

        public int CardId
        {
            get
            {
                return _CardId;
            }
            set
            {
                if (_CardId != value)
                {
                    _CardId = value;

                    OnPropertyChanged(() => CardId);
                }
            }
        }
        public string Description
        {
            get { return _Description; }
            set
            {
                if (_Description != value)
                {
                    _Description = value;
                    OnPropertyChanged(() => Description);
                }

            }
        }
        public string Color
        {
            get { return _Color; }
            set
            {
                if (_Color != value)
                {
                    _Color = value;
                    OnPropertyChanged(() => Color);
                }
            }
        }
        public int Year
        {
            get { return _Year; }
            set
            {

                if (_Year != value)
                {
                    _Year = value;
                    OnPropertyChanged(() => Year);
                }
            }
        }
        public decimal RentalPrice
        {
            get { return _RentalPrice; }
            set
            {
                if (_RentalPrice != value)
                {
                    _RentalPrice = value;
                    OnPropertyChanged(() => RentalPrice);
                }

            }
        }

        public bool CurrentlyRented
        {
            get
            {
                return _CurrentlyRented;

            }
            set
            {
                if (_CurrentlyRented != value)
                {
                    _CurrentlyRented = value;
                    OnPropertyChanged(() => CurrentlyRented);
                }
            }
        }


    }
}
