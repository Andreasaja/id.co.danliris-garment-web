﻿using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentAvalProducts.ReadModels;
using Manufactures.Domain.GarmentAvalProducts.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts
{
    public class GarmentAvalProduct : AggregateRoot<GarmentAvalProduct, GarmentAvalProductReadModel>
    {
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public DateTimeOffset? AvalDate { get; private set; }

        public GarmentAvalProduct(Guid identity, string roNo, string article, DateTimeOffset? avalDate) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            RONo = roNo;
            Article = article;
            AvalDate = avalDate;
            ReadModel = new GarmentAvalProductReadModel(Identity)
            {
                RONo = RONo,
                Article = Article,
                AvalDate = AvalDate
            };
            ReadModel.AddDomainEvent(new OnGarmentAvalProductPlaced(this.Identity));
        }

        public GarmentAvalProduct(GarmentAvalProductReadModel readModel) : base(readModel)
        {
            RONo = readModel.RONo;
            Article = readModel.Article;
            AvalDate = readModel.AvalDate;
        }

        public void SetRONo(string newRONo)
        {
            Validator.ThrowIfNullOrEmpty(() => newRONo);
            if (newRONo != RONo)
            {
                RONo = newRONo;
                ReadModel.RONo = newRONo;
            }
        }

        public void SetArticle(string newArticle)
        {
            Validator.ThrowIfNullOrEmpty(() => newArticle);
            if (newArticle != Article)
            {
                Article = newArticle;
                ReadModel.Article = newArticle;
            }
        }

        public void SetAvalDate(DateTimeOffset? newAvalDate)
        {
            if (newAvalDate != AvalDate)
            {
                AvalDate = newAvalDate;
                ReadModel.AvalDate = newAvalDate;

                MarkModified();
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        protected override GarmentAvalProduct GetEntity()
        {
            return this;
        }
    }
}