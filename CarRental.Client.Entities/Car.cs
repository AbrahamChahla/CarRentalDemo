﻿using Core.Common.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace CarRental.Client.Entities
{
    public class Car : ObjectBase
    {
        #region Properties
        private int _CarId;
        private string _Description;
        private string _Color;
        private int _Year;
        private decimal _RentalPrice;
        private bool _CurrentlyRented;

        public int CarId
        {
            get
            {
                return _CarId;
            }
            set
            {
                if (_CarId != value)
                {
                    _CarId = value;

                    OnPropertyChanged(() => CarId);
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

        #endregion


        class CardValidator : AbstractValidator<Car>
        {
            public CardValidator()
            {
                RuleFor(val => val.Description).NotEmpty();
                RuleFor(val => val.Color).NotEmpty();
                RuleFor(val => val.RentalPrice).GreaterThan(0);
                RuleFor(val => val.Year).GreaterThan(2000).LessThanOrEqualTo(DateTime.Now.Year);
            }
        }

        public override IValidator GetValidator()
        {
            return new CardValidator();
        }
        
    }
}
