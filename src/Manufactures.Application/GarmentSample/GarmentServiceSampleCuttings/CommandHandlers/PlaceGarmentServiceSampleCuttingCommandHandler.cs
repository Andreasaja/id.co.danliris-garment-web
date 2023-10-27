﻿using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.Shared.ValueObjects;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Commands;
using Manufactures.Domain.GarmentSample.ServiceSampleCuttings.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentCuttingIns;
using Manufactures.Domain.GarmentPreparings.Repositories;
using Manufactures.Domain.GarmentSample.SampleCuttingIns.Repositories;
using Manufactures.Domain.GarmentSample.SamplePreparings.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;

namespace Manufactures.Application.GarmentSample.GarmentServiceSampleCuttings.CommandHandlers
{
    public class PlaceGarmentServiceSampleCuttingCommandHandler : ICommandHandler<PlaceGarmentServiceSampleCuttingCommand, GarmentServiceSampleCutting>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSampleCuttingRepository _garmentServiceSampleCuttingRepository;
        private readonly IGarmentServiceSampleCuttingItemRepository _garmentServiceSampleCuttingItemRepository;
        private readonly IGarmentServiceSampleCuttingDetailRepository _garmentServiceSampleCuttingDetailRepository;
        private readonly IGarmentServiceSampleCuttingSizeRepository _garmentServiceSampleCuttingSizeRepository;
        private readonly IGarmentSampleCuttingInRepository _garmentCuttingInRepository;
        private readonly IGarmentSampleCuttingInItemRepository _garmentCuttingInItemRepository;
        private readonly IGarmentSampleCuttingInDetailRepository _garmentCuttingInDetailRepository;
        private readonly IGarmentSamplePreparingRepository _garmentPreparingRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public PlaceGarmentServiceSampleCuttingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSampleCuttingRepository = storage.GetRepository<IGarmentServiceSampleCuttingRepository>();
            _garmentServiceSampleCuttingItemRepository = storage.GetRepository<IGarmentServiceSampleCuttingItemRepository>();
            _garmentServiceSampleCuttingDetailRepository= storage.GetRepository<IGarmentServiceSampleCuttingDetailRepository>();
            _garmentServiceSampleCuttingSizeRepository = storage.GetRepository<IGarmentServiceSampleCuttingSizeRepository>();
            _garmentCuttingInRepository = storage.GetRepository<IGarmentSampleCuttingInRepository>();
            _garmentCuttingInItemRepository = storage.GetRepository<IGarmentSampleCuttingInItemRepository>();
            _garmentCuttingInDetailRepository = storage.GetRepository<IGarmentSampleCuttingInDetailRepository>();
            _garmentPreparingRepository = storage.GetRepository<IGarmentSamplePreparingRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentServiceSampleCutting> Handle(PlaceGarmentServiceSampleCuttingCommand request, CancellationToken cancellationToken)
        {
            request.Items = request.Items.Where(item => item.Details.Where(detail => detail.IsSave).Count() > 0).ToList();
            var collectRoNo = _garmentPreparingRepository.RoChecking(request.Items.Select(x => x.RONo), request.Buyer.Code);
            if (!collectRoNo)
                throw new Exception("RoNo tidak sesuai dengan data pembeli");

            GarmentServiceSampleCutting garmentServiceSampleCutting = new GarmentServiceSampleCutting(
                Guid.NewGuid(),
                GenerateSampleNo(request),
                request.SubconType,
                new UnitDepartmentId(request.Unit.Id),
                request.Unit.Code,
                request.Unit.Name,
                request.SubconDate.GetValueOrDefault(),
                request.IsUsed,
                new BuyerId(request.Buyer.Id),
                request.Buyer.Code,
                request.Buyer.Name,
                new UomId(request.Uom.Id),
                request.Uom.Unit,
                request.QtyPacking,
                request.NettWeight,
                request.GrossWeight,
                request.Remark
            );
            foreach (var item in request.Items)
            {
                GarmentServiceSampleCuttingItem garmentServiceSampleCuttingItem = new GarmentServiceSampleCuttingItem(
                    Guid.NewGuid(),
                    garmentServiceSampleCutting.Identity,
                    item.RONo,
                    item.Article,
                    new GarmentComodityId(item.Comodity.Id),
                    item.Comodity.Code,
                    item.Comodity.Name
                );

                List<GarmentServiceSampleCuttingSize> cuttingInDetails = new List<GarmentServiceSampleCuttingSize>();
                var cuttingIn = _garmentCuttingInRepository.Query.Where(x => x.RONo == item.RONo).OrderBy(a => a.CreatedDate).ToList();

                #region Old Query Calculating From Cut In (13/09/23)
                //foreach (var cutIn in cuttingIn)
                //{
                //    var cuttingInItems = _garmentCuttingInItemRepository.Query.Where(x => x.CutInId == cutIn.Identity).OrderBy(a => a.CreatedDate).ToList();
                //    foreach (var cutInItem in cuttingInItems)
                //    {
                //        var cutInDetails = _garmentCuttingInDetailRepository.Query.Where(x => x.CutInItemId == cutInItem.Identity).OrderBy(a => a.CreatedDate).ToList();

                //        foreach (var cutInDetail in cutInDetails)
                //        {
                //            var subconCuttingSizes = _garmentServiceSampleCuttingSizeRepository.Query.Where(o => o.CuttingInDetailId == cutInDetail.Identity).ToList();
                //            if (subconCuttingSizes != null)
                //            {
                //                double qty = (double)cutInDetail.CuttingInQuantity;
                //                foreach (var subconCuttingDetail in subconCuttingSizes)
                //                {
                //                    qty -= subconCuttingDetail.Quantity;
                //                }
                //                if (qty > 0)
                //                {
                //                    cuttingInDetails.Add(new GarmentServiceSampleCuttingSize
                //                    (
                //                        new Guid(),
                //                        new SizeId(1),
                //                        "",
                //                        qty,
                //                        new UomId(1),
                //                        "",
                //                        cutInDetail.DesignColor,
                //                        item.Id,
                //                        cutIn.Identity,
                //                        cutInDetail.Identity,
                //                        new ProductId(cutInDetail.ProductId),
                //                        cutInDetail.ProductCode,
                //                        cutInDetail.ProductName
                //                    ));
                //                }

                //            }
                //        }
                //    }
                //}

                //foreach (var detail in item.Details)
                //{
                //    if (detail.IsSave)
                //    {
                //        GarmentServiceSampleCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSampleCuttingDetail(
                //                    Guid.NewGuid(),
                //                    garmentServiceSubconCuttingItem.Identity,
                //                    detail.DesignColor,
                //                    detail.Quantity
                //                );
                //        var cutInDetail = cuttingInDetails.Where(y => y.Color == detail.DesignColor).ToList();
                //        foreach (var size in detail.Sizes)
                //        {
                //            var qty = size.Quantity;
                //            foreach (var d in cutInDetail)
                //            {
                //                if (d.Quantity > 0)
                //                {
                //                    var qtyRemains = d.Quantity - qty;
                //                    if (qtyRemains >= 0)
                //                    {
                //                        GarmentServiceSampleCuttingSize garmentServiceSubconCuttingSize = new GarmentServiceSampleCuttingSize(
                //                            Guid.NewGuid(),
                //                            new SizeId(size.Size.Id),
                //                            size.Size.Size,
                //                            qty,
                //                            new UomId(size.Uom.Id),
                //                            size.Uom.Unit,
                //                            size.Color,
                //                            garmentServiceSubconCuttingDetail.Identity,
                //                            d.CuttingInId,
                //                            d.CuttingInDetailId,
                //                            d.ProductId,
                //                            d.ProductCode,
                //                            d.ProductName
                //                        );
                //                        await _garmentServiceSampleCuttingSizeRepository.Update(garmentServiceSubconCuttingSize);
                //                        d.SetQuantity(qtyRemains);
                //                        break;
                //                    }
                //                    else if (qtyRemains < 0)
                //                    {
                //                        qty -= d.Quantity;
                //                        GarmentServiceSampleCuttingSize garmentServiceSubconCuttingSize = new GarmentServiceSampleCuttingSize(
                //                            Guid.NewGuid(),
                //                            new SizeId(size.Size.Id),
                //                            size.Size.Size,
                //                            d.Quantity,
                //                            new UomId(size.Uom.Id),
                //                            size.Uom.Unit,
                //                            size.Color,
                //                            garmentServiceSubconCuttingDetail.Identity,
                //                            d.CuttingInId,
                //                            d.CuttingInDetailId,
                //                            d.ProductId,
                //                            d.ProductCode,
                //                            d.ProductName
                //                        );
                //                        await _garmentServiceSampleCuttingSizeRepository.Update(garmentServiceSubconCuttingSize);
                //                        d.SetQuantity(qtyRemains);
                //                    }
                //                }

                //            }
                //        }
                //        await _garmentServiceSubconCuttingDetailRepository.Update(garmentServiceSubconCuttingDetail);
                //    }
                //}
                #endregion

                #region New Query Ignore Calculating From Cut In (13/09/23)

                foreach (var cutIn in cuttingIn)
                {
                    var cuttingInItems = _garmentCuttingInItemRepository.Query.Where(x => x.CutInId == cutIn.Identity).OrderBy(a => a.CreatedDate).ToList();
                    foreach (var cutInItem in cuttingInItems)
                    {
                        var cutInDetails = _garmentCuttingInDetailRepository.Query.Where(x => x.CutInItemId == cutInItem.Identity).OrderBy(a => a.CreatedDate).ToList();

                        foreach (var cutInDetail in cutInDetails)
                        {
                            var subconCuttingSizes = _garmentServiceSampleCuttingSizeRepository.Query.Where(o => o.CuttingInDetailId == cutInDetail.Identity).ToList();
                            if (subconCuttingSizes != null)
                            {
                                cuttingInDetails.Add(new GarmentServiceSampleCuttingSize
                                (
                                    new Guid(),
                                    new SizeId(1),
                                    "",
                                    cutInDetail.CuttingInQuantity,
                                    new UomId(1),
                                    "",
                                    cutInDetail.DesignColor,
                                    item.Id,
                                    cutIn.Identity,
                                    cutInDetail.Identity,
                                    new ProductId(cutInDetail.ProductId),
                                    cutInDetail.ProductCode,
                                    cutInDetail.ProductName
                                ));
                            }
                        }
                    }
                }

                foreach (var detail in item.Details)
                {
                    if (detail.IsSave)
                    {
                        GarmentServiceSampleCuttingDetail garmentServiceSubconCuttingDetail = new GarmentServiceSampleCuttingDetail(
                                    Guid.NewGuid(),
                                    garmentServiceSampleCuttingItem.Identity,
                                    detail.DesignColor,
                                    detail.Quantity
                                );
                        var cutInDetail = cuttingInDetails.FirstOrDefault(y => y.Color == detail.DesignColor);
                        foreach (var size in detail.Sizes)
                        {
                            {
                                GarmentServiceSampleCuttingSize garmentServiceSubconCuttingSize = new GarmentServiceSampleCuttingSize(
                                    Guid.NewGuid(),
                                    new SizeId(size.Size.Id),
                                    size.Size.Size,
                                    size.Quantity,
                                    new UomId(size.Uom.Id),
                                    size.Uom.Unit,
                                    size.Color,
                                    garmentServiceSubconCuttingDetail.Identity,
                                    cutInDetail.CuttingInId,
                                    cutInDetail.CuttingInDetailId,
                                    cutInDetail.ProductId,
                                    cutInDetail.ProductCode,
                                    cutInDetail.ProductName
                                );
                                await _garmentServiceSampleCuttingSizeRepository.Update(garmentServiceSubconCuttingSize);

                            }
                        }
                        await _garmentServiceSampleCuttingDetailRepository.Update(garmentServiceSubconCuttingDetail);
                    }
                }
                #endregion

                await _garmentServiceSampleCuttingItemRepository.Update(garmentServiceSampleCuttingItem);
            }


            await _garmentServiceSampleCuttingRepository.Update(garmentServiceSampleCutting);


            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Create Packing List Subcon Sample - Jasa Komponen - " + garmentServiceSampleCutting.SampleNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);


            _storage.Save();

            return garmentServiceSampleCutting;
        }

        private string GenerateSampleNo(PlaceGarmentServiceSampleCuttingCommand request)
        {
            var now = DateTime.Now;
            var year = now.ToString("yy");
            var month = now.ToString("MM");
            var code = request.SubconType == "BORDIR" ? "BR" : request.SubconType == "PRINT" ? "PR" : request.SubconType == "OTHERS" ? "OT" : "PL";

            var prefix = $"SJC{code}{year}{month}";

            var lastSampleNo = _garmentServiceSampleCuttingRepository.Query.Where(w => w.SampleNo.StartsWith(prefix))
                .OrderByDescending(o => o.SampleNo)
                .Select(s => int.Parse(s.SampleNo.Substring(9, 4)))
                .FirstOrDefault();
            var CutInNo = $"{prefix}{(lastSampleNo + 1).ToString("D4")}" + "-S";

            return CutInNo;
        }
    }

    
}