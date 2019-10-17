﻿using Moonlay.Domain;
using System;
using System.Collections.Generic;
using Manufactures.Domain.Shared.ValueObjects;
using System.Text;

namespace Manufactures.Domain.GarmentSewingOuts.ValueObjects
{
    public class GarmentSewingOutItemValueObject : ValueObject
    {
        public Guid SewingOutId { get;  set; }
        public Guid SewingInId { get;  set; }
        public Guid SewingInItemId { get;  set; }
        public Product Product { get;  set; }
        public string DesignColor { get;  set; }
        public SizeValueObject Size { get;  set; }
        public double Quantity { get;  set; }
        public int UomId { get;  set; }
        public string UomUnit { get;  set; }
        public string Color { get;  set; }
        public List<GarmentSewingOutDetailValueObject> Details { get; set; }
        public GarmentSewingOutItemValueObject()
        {
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            throw new NotImplementedException();
        }
    }
}
