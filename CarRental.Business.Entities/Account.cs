using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Core.Common.Contracts;
using Core.Common.Core;

namespace CarRental.Business.Entities
{
    [DataContract]
    public class Account : EntityBase, IIdentifiableEntity
    {
        public int AccountId { get; set; }

        public int EntityId
        {
            get
            {
                return AccountId;
            }
            set { AccountId = value; }
        }
    }
}
