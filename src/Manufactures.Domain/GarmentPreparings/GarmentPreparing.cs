﻿using Infrastructure.Domain;
using Manufactures.Domain.Events;
using Manufactures.Domain.GarmentPreparings.ReadModels;
using Manufactures.Domain.GarmentPreparings.ValueObjects;
using Moonlay;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentPreparings
{
    public class GarmentPreparing : AggregateRoot<GarmentPreparing, GarmentPreparingReadModel>
    {
        public int UENId { get; private set; }
        public string UENNo { get; private set; }
        public UnitDepartmentId UnitId { get; private set; }
        //public string UnitCode { get; internal set; }
        //public string UnitName { get; internal set; }
        public DateTimeOffset? ProcessDate { get; private set; }
        public string RONo { get; private set; }
        public string Article { get; private set; }
        public bool IsCuttingIn { get; private set; }

        public GarmentPreparing(Guid identity, int uenId, string uenNo, UnitDepartmentId unitId, DateTimeOffset? processDate, string roNo, string article, bool isCuttingIn) : base(identity)
        {
            this.MarkTransient();

            Identity = identity;
            UENId = uenId;
            UENNo = uenNo;
            UnitId = unitId;
            ProcessDate = processDate;
            RONo = roNo;
            Article = article;
            IsCuttingIn = isCuttingIn;

            ReadModel = new GarmentPreparingReadModel(Identity)
            {
                UENId = UENId,
                UENNo = UENNo,
                UnitId = UnitId.Value,
                ProcessDate = ProcessDate,
                RONo = RONo,
                Article = Article,
                IsCuttingIn = IsCuttingIn,
            };
            ReadModel.AddDomainEvent(new OnGarmentPreparingPlaced(this.Identity));
        }

        public GarmentPreparing(GarmentPreparingReadModel readModel) : base(readModel)
        {
            UENId = ReadModel.UENId;
            UENNo = ReadModel.UENNo;
            UnitId = new UnitDepartmentId(ReadModel.UnitId);
            ProcessDate = ReadModel.ProcessDate;
            RONo = ReadModel.RONo;
            Article = ReadModel.Article;
            IsCuttingIn = ReadModel.IsCuttingIn;
        }

        public void setUENId(int newUENId)
        {
            Validator.ThrowIfNull(() => newUENId);

            if (newUENId != UENId)
            {
                UENId = newUENId;
                ReadModel.UENId = newUENId;
            }
        }

        public void setUENNo(string newUENNo)
        {
            Validator.ThrowIfNullOrEmpty(() => newUENNo);

            if(newUENNo != UENNo)
            {
                UENNo = newUENNo;
                ReadModel.UENNo = newUENNo;
            }
        }

        public void SetUnitId (UnitDepartmentId newUnitId)
        {
            Validator.ThrowIfNull(() => newUnitId);

            if(newUnitId != UnitId)
            {
                UnitId = newUnitId;
                ReadModel.UnitId = newUnitId.Value;
            }
        }

        public void setProcessDate(DateTimeOffset? newProcessDate)
        {
            if (newProcessDate != ProcessDate)
            {
                ProcessDate = newProcessDate;
                ReadModel.ProcessDate = newProcessDate;

                MarkModified();
            }
        }

        public void setRONo(string newRONo)
        {
            Validator.ThrowIfNullOrEmpty(() => newRONo);

            if (newRONo != RONo)
            {
                RONo = newRONo;
                ReadModel.RONo = newRONo;
            }
        }

        public void setArticle(string newArticle)
        {
            Validator.ThrowIfNullOrEmpty(() => newArticle);

            if (newArticle != Article)
            {
                Article = newArticle;
                ReadModel.Article = newArticle;
            }
        }

        public void setIsCuttingIN(bool newIsCuttingIn)
        {
            if (newIsCuttingIn != IsCuttingIn)
            {
                IsCuttingIn = newIsCuttingIn;
                ReadModel.IsCuttingIn = newIsCuttingIn;
            }
        }

        public void SetModified()
        {
            MarkModified();
        }

        protected override GarmentPreparing GetEntity()
        {
            return this;
        }
    }
}