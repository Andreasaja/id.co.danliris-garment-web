﻿using Infrastructure.Domain.Queries;
using Infrastructure.External.DanLirisClient.Microservice.HttpClientService;
using System;
using ExtCore.Data.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Manufactures.Domain.GarmentCuttingOuts.Repositories;
using Manufactures.Domain.GarmentLoadings.Repositories;
using Manufactures.Domain.GarmentSewingOuts.Repositories;
using Manufactures.Domain.GarmentFinishingOuts.Repositories;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using Infrastructure.External.DanLirisClient.Microservice.MasterResult;
using Newtonsoft.Json;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.CostCalculationGarmentDataProductionReport;
using Infrastructure.External.DanLirisClient.Microservice;
using System.Linq;
using static Infrastructure.External.DanLirisClient.Microservice.MasterResult.HOrderDataProductionReport;
using Manufactures.Domain.GarmentCuttingIns.Repositories;
using Manufactures.Domain.GarmentAvalComponents.Repositories;
using Manufactures.Domain.GarmentAdjustments.Repositories;
using Manufactures.Domain.GarmentSewingIns.Repositories;
using Manufactures.Domain.GarmentFinishingIns.Repositories;
using Manufactures.Domain.GarmentExpenditureGoods.Repositories;
using Manufactures.Domain.GarmentExpenditureGoodReturns.Repositories;
using System.IO;
using System.Data;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Manufactures.Domain.GarmentSewingDOs.Repositories;
using Manufactures.Domain.GarmentComodityPrices.Repositories;

namespace Manufactures.Application.GarmentMonitoringProductionStockFlows.Queries
{
	public class GetXlsMonitoringProductionStockFlowQueryHandler : IQueryHandler<GetXlsMonitoringProductionStockFlowQuery, MemoryStream>
	{
		protected readonly IHttpClientService _http;
		private readonly IStorage _storage;
		private readonly IGarmentCuttingOutRepository garmentCuttingOutRepository;
		private readonly IGarmentCuttingOutItemRepository garmentCuttingOutItemRepository;
		private readonly IGarmentCuttingOutDetailRepository garmentCuttingOutDetailRepository;
		private readonly IGarmentCuttingInRepository garmentCuttingInRepository;
		private readonly IGarmentCuttingInItemRepository garmentCuttingInItemRepository;
		private readonly IGarmentCuttingInDetailRepository garmentCuttingInDetailRepository;
		private readonly IGarmentLoadingRepository garmentLoadingRepository;
		private readonly IGarmentLoadingItemRepository garmentLoadingItemRepository;
		private readonly IGarmentSewingInRepository garmentSewingInRepository;
		private readonly IGarmentSewingInItemRepository garmentSewingInItemRepository;
		private readonly IGarmentAvalComponentRepository garmentAvalComponentRepository;
		private readonly IGarmentAvalComponentItemRepository garmentAvalComponentItemRepository;
		private readonly IGarmentAdjustmentRepository garmentAdjustmentRepository;
		private readonly IGarmentAdjustmentItemRepository garmentAdjustmentItemRepository;
		private readonly IGarmentSewingOutRepository garmentSewingOutRepository;
		private readonly IGarmentSewingOutItemRepository garmentSewingOutItemRepository;
		private readonly IGarmentFinishingOutRepository garmentFinishingOutRepository;
		private readonly IGarmentFinishingOutItemRepository garmentFinishingOutItemRepository;
		private readonly IGarmentFinishingInRepository garmentFinishingInRepository;
		private readonly IGarmentFinishingInItemRepository garmentFinishingInItemRepository;
		private readonly IGarmentExpenditureGoodRepository garmentExpenditureGoodRepository;
		private readonly IGarmentExpenditureGoodItemRepository garmentExpenditureGoodItemRepository;
		private readonly IGarmentExpenditureGoodReturnRepository garmentExpenditureGoodReturnRepository;
		private readonly IGarmentExpenditureGoodReturnItemRepository garmentExpenditureGoodReturnItemRepository;
		private readonly IGarmentSewingDORepository garmentSewingDORepository;
		private readonly IGarmentSewingDOItemRepository garmentSewingDOItemRepository;
		private readonly IGarmentComodityPriceRepository garmentComodityPriceRepository;
		public GetXlsMonitoringProductionStockFlowQueryHandler(IStorage storage, IServiceProvider serviceProvider)
		{
			_storage = storage;
			garmentCuttingOutRepository = storage.GetRepository<IGarmentCuttingOutRepository>();
			garmentCuttingOutItemRepository = storage.GetRepository<IGarmentCuttingOutItemRepository>();
			garmentCuttingOutDetailRepository = storage.GetRepository<IGarmentCuttingOutDetailRepository>();
			garmentCuttingInRepository = storage.GetRepository<IGarmentCuttingInRepository>();
			garmentCuttingInItemRepository = storage.GetRepository<IGarmentCuttingInItemRepository>();
			garmentCuttingInDetailRepository = storage.GetRepository<IGarmentCuttingInDetailRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentSewingInRepository = storage.GetRepository<IGarmentSewingInRepository>();
			garmentSewingInItemRepository = storage.GetRepository<IGarmentSewingInItemRepository>();
			garmentAvalComponentRepository = storage.GetRepository<IGarmentAvalComponentRepository>();
			garmentAvalComponentItemRepository = storage.GetRepository<IGarmentAvalComponentItemRepository>();
			garmentLoadingRepository = storage.GetRepository<IGarmentLoadingRepository>();
			garmentLoadingItemRepository = storage.GetRepository<IGarmentLoadingItemRepository>();
			garmentAdjustmentRepository = storage.GetRepository<IGarmentAdjustmentRepository>();
			garmentAdjustmentItemRepository = storage.GetRepository<IGarmentAdjustmentItemRepository>();
			garmentSewingOutRepository = storage.GetRepository<IGarmentSewingOutRepository>();
			garmentSewingOutItemRepository = storage.GetRepository<IGarmentSewingOutItemRepository>();
			garmentFinishingOutRepository = storage.GetRepository<IGarmentFinishingOutRepository>();
			garmentFinishingOutItemRepository = storage.GetRepository<IGarmentFinishingOutItemRepository>();
			garmentFinishingInRepository = storage.GetRepository<IGarmentFinishingInRepository>();
			garmentFinishingInItemRepository = storage.GetRepository<IGarmentFinishingInItemRepository>();
			garmentExpenditureGoodRepository = storage.GetRepository<IGarmentExpenditureGoodRepository>();
			garmentExpenditureGoodItemRepository = storage.GetRepository<IGarmentExpenditureGoodItemRepository>();
			garmentExpenditureGoodReturnRepository = storage.GetRepository<IGarmentExpenditureGoodReturnRepository>();
			garmentExpenditureGoodReturnItemRepository = storage.GetRepository<IGarmentExpenditureGoodReturnItemRepository>();
			garmentSewingDORepository = storage.GetRepository<IGarmentSewingDORepository>();
			garmentSewingDOItemRepository = storage.GetRepository<IGarmentSewingDOItemRepository>();
			garmentComodityPriceRepository = storage.GetRepository<IGarmentComodityPriceRepository>();
			_http = serviceProvider.GetService<IHttpClientService>();
		}


		class monitoringView
		{
			public string Ro { get; internal set; }
			public string BuyerCode { get; internal set; }
			public string Article { get; internal set; }
			public string Comodity { get; internal set; }
			public double QtyOrder { get; internal set; }
			public double BasicPrice { get; internal set; }
			public decimal Fare { get; internal set; }
			public double FC { get; internal set; }
			public double Hours { get; internal set; }
			public double BeginingBalanceCuttingQty { get; internal set; }
			public double BeginingBalanceCuttingPrice { get; internal set; }
			public double QtyCuttingIn { get; internal set; }
			public double PriceCuttingIn { get; internal set; }
			public double QtyCuttingOut { get; internal set; }
			public double PriceCuttingOut { get; internal set; }
			public double QtyCuttingTransfer { get; internal set; }
			public double PriceCuttingTransfer { get; internal set; }
			public double QtyCuttingsubkon { get; internal set; }
			public double PriceCuttingsubkon { get; internal set; }
			public double AvalCutting { get; internal set; }
			public double AvalCuttingPrice { get; internal set; }
			public double AvalSewing { get; internal set; }
			public double AvalSewingPrice { get; internal set; }
			public double EndBalancCuttingeQty { get; internal set; }
			public double EndBalancCuttingePrice { get; internal set; }
			public double BeginingBalanceLoadingQty { get; internal set; }
			public double BeginingBalanceLoadingPrice { get; internal set; }
			public double QtyLoadingIn { get; internal set; }
			public double PriceLoadingIn { get; internal set; }
			public double QtyLoading { get; internal set; }
			public double PriceLoading { get; internal set; }
			public double QtyLoadingAdjs { get; internal set; }
			public double PriceLoadingAdjs { get; internal set; }
			public double EndBalanceLoadingQty { get; internal set; }
			public double EndBalanceLoadingPrice { get; internal set; }
			public double BeginingBalanceSewingQty { get; internal set; }
			public double BeginingBalanceSewingPrice { get; internal set; }
			public double QtySewingIn { get; internal set; }
			public double PriceSewingIn { get; internal set; }
			public double QtySewingOut { get; internal set; }
			public double PriceSewingOut { get; internal set; }
			public double QtySewingInTransfer { get; internal set; }
			public double PriceSewingInTransfer { get; internal set; }
			public double WipSewingOut { get; internal set; }
			public double WipSewingOutPrice { get; internal set; }
			public double WipFinishingOut { get; internal set; }
			public double WipFinishingOutPrice { get; internal set; }
			public double QtySewingRetur { get; internal set; }
			public double PriceSewingRetur { get; internal set; }
			public double QtySewingAdj { get; internal set; }
			public double PriceSewingAdj { get; internal set; }
			public double EndBalanceSewingQty { get; internal set; }
			public double EndBalanceSewingPrice { get; internal set; }
			public double BeginingBalanceFinishingQty { get; internal set; }
			public double BeginingBalanceFinishingPrice { get; internal set; }
			public double FinishingInQty { get; internal set; }
			public double FinishingInPrice { get; internal set; }
			public double BeginingBalanceSubconQty { get; internal set; }
			public double BeginingBalanceSubconPrice { get; internal set; }
			public double SubconInQty { get; internal set; }
			public double SubconInPrice { get; internal set; }
			public double SubconOutQty { get; internal set; }
			public double SubconOutPrice { get; internal set; }
			public double EndBalanceSubconQty { get; internal set; }
			public double EndBalanceSubconPrice { get; internal set; }
			public double FinishingOutQty { get; internal set; }
			public double FinishingOutPrice { get; internal set; }
			public double FinishingInTransferQty { get; internal set; }
			public double FinishingInTransferPrice { get; internal set; }
			public double FinishingAdjQty { get; internal set; }
			public double FinishingAdjPrice { get; internal set; }
			public double FinishingReturQty { get; internal set; }
			public double FinishingReturPrice { get; internal set; }
			public double EndBalanceFinishingQty { get; internal set; }
			public double EndBalanceFinishingPrice { get; internal set; }
			public double BeginingBalanceExpenditureGood { get; internal set; }
			public double BeginingBalanceExpenditureGoodPrice { get; internal set; }
			public double FinishingTransferExpenditure { get; internal set; }
			public double FinishingTransferExpenditurePrice { get; internal set; }
			public double ExpenditureGoodRetur { get; internal set; }
			public double ExpenditureGoodReturPrice { get; internal set; }
			public double ExportQty { get; internal set; }
			public double ExportPrice { get; internal set; }
			public double OtherQty { get; internal set; }
			public double OtherPrice { get; internal set; }
			public double SampleQty { get; internal set; }
			public double SamplePrice { get; internal set; }
			public double ExpenditureGoodRemainingQty { get; internal set; }
			public double ExpenditureGoodRemainingPrice { get; internal set; }
			public double ExpenditureGoodAdj { get; internal set; }
			public double ExpenditureGoodAdjPrice { get; internal set; }
			public double EndBalanceExpenditureGood { get; internal set; }
			public double EndBalanceExpenditureGoodPrice { get; internal set; }

		}
		async Task<HOrderDataProductionReport> GetDataHOrder(List<string> ro, string token)
		{
			HOrderDataProductionReport hOrderDataProductionReport = new HOrderDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"local-merchandiser/horders/data-production-report-by-no/{listRO}";
			var httpResponse = await _http.GetAsync(costCalculationUri, token);

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();

				var listData = JsonConvert.DeserializeObject<List<HOrderViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.No == item);
					if (data != null)
					{
						hOrderDataProductionReport.data.Add(data);
					}
				}
			}

			return hOrderDataProductionReport;
		}

		public async Task<CostCalculationGarmentDataProductionReport> GetDataCostCal(List<string> ro, string token)
		{
			CostCalculationGarmentDataProductionReport costCalculationGarmentDataProductionReport = new CostCalculationGarmentDataProductionReport();

			var listRO = string.Join(",", ro.Distinct());
			var costCalculationUri = SalesDataSettings.Endpoint + $"cost-calculation-garments/data/{listRO}";
			var httpResponse = await _http.GetAsync(costCalculationUri, token);

			var freeRO = new List<string>();

			if (httpResponse.IsSuccessStatusCode)
			{
				var contentString = await httpResponse.Content.ReadAsStringAsync();
				Dictionary<string, object> content = JsonConvert.DeserializeObject<Dictionary<string, object>>(contentString);
				var dataString = content.GetValueOrDefault("data").ToString();
				var listData = JsonConvert.DeserializeObject<List<CostCalViewModel>>(dataString);

				foreach (var item in ro)
				{
					var data = listData.SingleOrDefault(s => s.ro == item);
					if (data != null)
					{
						costCalculationGarmentDataProductionReport.data.Add(data);
					}
					else
					{
						freeRO.Add(item);
					}
				}
			}

			HOrderDataProductionReport hOrderDataProductionReport = await GetDataHOrder(freeRO, token);

			Dictionary<string, string> comodities = new Dictionary<string, string>();
			if (hOrderDataProductionReport.data.Count > 0)
			{
				var comodityCodes = hOrderDataProductionReport.data.Select(s => s.Kode).Distinct().ToList();
				var filter = "{\"(" + string.Join(" || ", comodityCodes.Select(s => "Code==" + "\\\"" + s + "\\\"")) + ")\" : \"true\"}";

				var masterGarmentComodityUri = MasterDataSettings.Endpoint + $"master/garment-comodities?filter=" + filter;
				var garmentComodityResponse = _http.GetAsync(masterGarmentComodityUri).Result;
				var garmentComodityResult = new GarmentComodityResult();
				if (garmentComodityResponse.IsSuccessStatusCode)
				{
					garmentComodityResult = JsonConvert.DeserializeObject<GarmentComodityResult>(garmentComodityResponse.Content.ReadAsStringAsync().Result);
					//comodities = garmentComodityResult.data.ToDictionary(d => d.Code, d => d.Name);
					foreach (var comodity in garmentComodityResult.data)
					{
						comodities[comodity.Code] = comodity.Name;
					}
				}
			}

			foreach (var hOrder in hOrderDataProductionReport.data)
			{
				costCalculationGarmentDataProductionReport.data.Add(new CostCalViewModel
				{
					ro = hOrder.No,
					buyerCode = hOrder.Codeby,
					comodityName = comodities.GetValueOrDefault(hOrder.Kode),
					hours = (double)hOrder.Sh_Cut,
					qtyOrder = (double)hOrder.Qty
				});
			}

			return costCalculationGarmentDataProductionReport;
		}

		public async Task<MemoryStream> Handle(GetXlsMonitoringProductionStockFlowQuery request, CancellationToken cancellationToken)
		{
			DateTimeOffset dateFrom = new DateTimeOffset(request.dateFrom, new TimeSpan(7, 0, 0));
			DateTimeOffset dateTo = new DateTimeOffset(request.dateTo, new TimeSpan(7, 0, 0));

			var QueryRo = (from a in garmentCuttingInRepository.Query
						   join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
						   where a.UnitId == request.unit && a.CuttingInDate <= dateTo
						   select a.RONo).Distinct();
			List<string> _ro = new List<string>();
			foreach (var item in QueryRo)
			{
				_ro.Add(item);
			}
			CostCalculationGarmentDataProductionReport costCalculation = await GetDataCostCal(_ro, request.token);

			var sumFC = (from a in garmentCuttingInRepository.Query
						 join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
						 join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
						 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) &&
						 a.UnitId == request.unit && a.CuttingInDate <= dateTo
						 select new { a.FC, a.RONo, c.BasicPrice })
						 .GroupBy(x => new { x.RONo }, (key, group) => new
						 {
							 RO = key.RONo,
							 FC = group.Sum(s => s.FC),
							 BasicPrice = group.Sum(s => s.BasicPrice),
							 count = group.Count()
						 });
			var FC = from a in sumFC
					 select new { ro = a.RO, fc = a.FC / a.count, basicPrice = a.BasicPrice / a.count };
			var queryGroup = (from a in garmentCuttingOutRepository.Query
							  join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
							  join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
							  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SEWING" && a.UnitId == a.UnitFromId
							  select new { BasicPrice = (from aa in FC where aa.ro == a.RONo select aa.basicPrice).FirstOrDefault(), FareNew = (from aa in garmentComodityPriceRepository.Query where a.UnitId == aa.UnitId && a.ComodityId == aa.ComodityId && aa.Date > dateTo select aa.Price).FirstOrDefault(), Fare = (from aa in garmentComodityPriceRepository.Query where a.UnitId == aa.UnitId && a.ComodityId == aa.ComodityId && aa.IsValid == true select aa.Price).FirstOrDefault(), Ro = a.RONo, Article = a.Article, Comodity = a.ComodityName, BuyerCode = (from cost in costCalculation.data where cost.ro == a.RONo select cost.buyerCode).FirstOrDefault(), QtyOrder = (from cost in costCalculation.data where cost.ro == a.RONo select cost.qtyOrder).FirstOrDefault(), FC = (from cost in FC where cost.ro == a.RONo select cost.fc).FirstOrDefault(), Hours = (from cost in costCalculation.data where cost.ro == a.RONo select cost.hours).FirstOrDefault() }).Distinct();

			var QueryCuttingOut = (from a in garmentCuttingOutRepository.Query
								   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
								   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
								   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro))
								   && a.UnitId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SEWING" && a.UnitId == a.UnitFromId
								   select new monitoringView
								   {
									   QtyCuttingIn = 0,
									   PriceCuttingIn = 0,
									   QtySewingIn = 0,
									   PriceSewingIn = 0,
									   QtyCuttingTransfer = 0,
									   PriceCuttingTransfer = 0,
									   QtyCuttingsubkon = 0,
									   PriceCuttingsubkon = 0,
									   AvalCutting = 0,
									   AvalCuttingPrice = 0,
									   AvalSewing = 0,
									   AvalSewingPrice = 0,
									   QtyLoading = 0,
									   PriceLoading = 0,
									   QtyLoadingAdjs = 0,
									   PriceLoadingAdjs = 0,
									   QtySewingOut = 0,
									   PriceSewingOut = 0,
									   QtySewingAdj = 0,
									   PriceSewingAdj = 0,
									   WipSewingOut = 0,
									   WipSewingOutPrice = 0,
									   WipFinishingOut = 0,
									   WipFinishingOutPrice = 0,
									   QtySewingRetur = 0,
									   PriceSewingRetur = 0,
									   QtySewingInTransfer = 0,
									   PriceSewingInTransfer = 0,
									   FinishingInQty = 0,
									   FinishingInPrice = 0,
									   SubconInQty = 0,
									   SubconInPrice = 0,
									   FinishingAdjQty = 0,
									   FinishingAdjPrice = 0,
									   FinishingTransferExpenditure = 0,
									   FinishingTransferExpenditurePrice = 0,
									   FinishingInTransferQty = 0,
									   FinishingInTransferPrice = 0,
									   FinishingOutQty = 0,
									   FinishingOutPrice = 0,
									   FinishingReturQty = 0,
									   FinishingReturPrice = 0,
									   SubconOutQty = 0,
									   SubconOutPrice = 0,
									   BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
									   BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
									   BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
									   BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
									   Ro = a.RONo,
									   ExpenditureGoodRetur = 0,
									   ExpenditureGoodReturPrice = 0,
									   QtyCuttingOut = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
									   PriceCuttingOut = a.CuttingOutDate >= dateFrom ? c.Price : 0,
									   ExportQty = 0,
									   ExportPrice = 0,
									   SampleQty = 0,
									   SamplePrice = 0,
									   OtherQty = 0,
									   OtherPrice = 0,
								   });
			var QueryCuttingOutSubkon = (from a in garmentCuttingOutRepository.Query
										 join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
										 join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
										 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SUBKON"
										 select new monitoringView
										 {
											 QtyCuttingIn = 0,
											 PriceCuttingIn = 0,
											 QtySewingIn = 0,
											 PriceSewingIn = 0,
											 QtyCuttingOut = 0,
											 PriceCuttingOut = 0,
											 QtyCuttingTransfer = 0,
											 PriceCuttingTransfer = 0,
											 AvalCutting = 0,
											 AvalCuttingPrice = 0,
											 AvalSewing = 0,
											 AvalSewingPrice = 0,
											 QtyLoading = 0,
											 PriceLoading = 0,
											 QtyLoadingAdjs = 0,
											 PriceLoadingAdjs = 0,
											 QtySewingOut = 0,
											 PriceSewingOut = 0,
											 QtySewingAdj = 0,
											 PriceSewingAdj = 0,
											 WipSewingOut = 0,
											 WipSewingOutPrice = 0,
											 WipFinishingOut = 0,
											 WipFinishingOutPrice = 0,
											 QtySewingRetur = 0,
											 PriceSewingRetur = 0,
											 QtySewingInTransfer = 0,
											 PriceSewingInTransfer = 0,
											 FinishingInQty = 0,
											 FinishingInPrice = 0,
											 SubconInQty = 0,
											 SubconInPrice = 0,
											 FinishingAdjQty = 0,
											 FinishingAdjPrice = 0,
											 FinishingTransferExpenditure = 0,
											 FinishingTransferExpenditurePrice = 0,
											 FinishingInTransferQty = 0,
											 FinishingInTransferPrice = 0,
											 FinishingOutQty = 0,
											 FinishingOutPrice = 0,
											 FinishingReturQty = 0,
											 FinishingReturPrice = 0,
											 SubconOutQty = 0,
											 SubconOutPrice = 0,
											 BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
											 BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
											 BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
											 Ro = a.RONo,
											 BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
											 FC = (from cost in FC where cost.ro == a.RONo select cost.fc).FirstOrDefault(),
											 QtyCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
											 PriceCuttingsubkon = a.CuttingOutDate >= dateFrom ? c.Price : 0,
											 ExpenditureGoodRetur = 0,
											 ExpenditureGoodReturPrice = 0,
											 ExportQty = 0,
											 ExportPrice = 0,
											 SampleQty = 0,
											 SamplePrice = 0,
											 OtherQty = 0,
											 OtherPrice = 0
										 });
			var QueryCuttingOutTransfer = (from a in garmentCuttingOutRepository.Query
										   join b in garmentCuttingOutItemRepository.Query on a.Identity equals b.CutOutId
										   join c in garmentCuttingOutDetailRepository.Query on b.Identity equals c.CutOutItemId
										   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitFromId == request.unit && a.CuttingOutDate <= dateTo && a.CuttingOutType == "SEWING" && a.UnitId != a.UnitFromId
										   select new monitoringView
										   {
											   QtyCuttingIn = 0,
											   PriceCuttingIn = 0,
											   QtySewingIn = 0,
											   PriceSewingIn = 0,
											   QtyCuttingOut = 0,
											   PriceCuttingOut = 0,
											   QtyCuttingsubkon = 0,
											   PriceCuttingsubkon = 0,
											   AvalCutting = 0,
											   AvalCuttingPrice = 0,
											   AvalSewing = 0,
											   AvalSewingPrice = 0,
											   QtyLoading = 0,
											   PriceLoading = 0,
											   QtyLoadingAdjs = 0,
											   PriceLoadingAdjs = 0,
											   QtySewingOut = 0,
											   PriceSewingOut = 0,
											   QtySewingAdj = 0,
											   PriceSewingAdj = 0,
											   WipSewingOut = 0,
											   WipSewingOutPrice = 0,
											   WipFinishingOut = 0,
											   WipFinishingOutPrice = 0,
											   QtySewingRetur = 0,
											   PriceSewingRetur = 0,
											   QtySewingInTransfer = 0,
											   PriceSewingInTransfer = 0,
											   FinishingInQty = 0,
											   FinishingInPrice = 0,
											   SubconInQty = 0,
											   SubconInPrice = 0,
											   FinishingAdjQty = 0,
											   FinishingAdjPrice = 0,
											   FinishingTransferExpenditure = 0,
											   FinishingTransferExpenditurePrice = 0,
											   FinishingInTransferQty = 0,
											   FinishingInTransferPrice = 0,
											   FinishingOutQty = 0,
											   FinishingOutPrice = 0,
											   FinishingReturQty = 0,
											   FinishingReturPrice = 0,
											   SubconOutQty = 0,
											   SubconOutPrice = 0,
											   BeginingBalanceLoadingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
											   BeginingBalanceLoadingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
											   BeginingBalanceCuttingQty = a.CuttingOutDate < dateFrom ? -c.CuttingOutQuantity : 0,
											   BeginingBalanceCuttingPrice = a.CuttingOutDate < dateFrom ? -c.Price : 0,
											   Ro = a.RONo,
											   QtyCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.CuttingOutQuantity : 0,
											   PriceCuttingTransfer = a.CuttingOutDate >= dateFrom ? c.Price : 0,
											   ExpenditureGoodRetur = 0,
											   ExpenditureGoodReturPrice = 0,
											   ExportQty = 0,
											   ExportPrice = 0,
											   SampleQty = 0,
											   SamplePrice = 0,
											   OtherQty = 0,
											   OtherPrice = 0
										   });
			var QueryCuttingIn = (from a in garmentCuttingInRepository.Query
								  join b in garmentCuttingInItemRepository.Query on a.Identity equals b.CutInId
								  join c in garmentCuttingInDetailRepository.Query on b.Identity equals c.CutInItemId
								  where a.CuttingType != "Non Main Fabric" && (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.CuttingInDate <= dateTo
								  select new monitoringView
								  {
									  QtySewingIn = 0,
									  PriceSewingIn = 0,
									  QtyCuttingOut = 0,
									  PriceCuttingOut = 0,
									  QtyCuttingTransfer = 0,
									  PriceCuttingTransfer = 0,
									  QtyCuttingsubkon = 0,
									  PriceCuttingsubkon = 0,
									  AvalCutting = 0,
									  AvalCuttingPrice = 0,
									  AvalSewing = 0,
									  AvalSewingPrice = 0,
									  QtyLoading = 0,
									  PriceLoading = 0,
									  QtyLoadingAdjs = 0,
									  PriceLoadingAdjs = 0,
									  QtySewingOut = 0,
									  PriceSewingOut = 0,
									  QtySewingAdj = 0,
									  PriceSewingAdj = 0,
									  WipSewingOut = 0,
									  WipSewingOutPrice = 0,
									  WipFinishingOut = 0,
									  WipFinishingOutPrice = 0,
									  QtySewingRetur = 0,
									  PriceSewingRetur = 0,
									  QtySewingInTransfer = 0,
									  PriceSewingInTransfer = 0,
									  FinishingInQty = 0,
									  FinishingInPrice = 0,
									  SubconInQty = 0,
									  SubconInPrice = 0,
									  FinishingAdjQty = 0,
									  FinishingAdjPrice = 0,
									  FinishingTransferExpenditure = 0,
									  FinishingTransferExpenditurePrice = 0,
									  FinishingInTransferQty = 0,
									  FinishingInTransferPrice = 0,
									  FinishingOutQty = 0,
									  FinishingOutPrice = 0,
									  FinishingReturQty = 0,
									  FinishingReturPrice = 0,
									  SubconOutQty = 0,
									  SubconOutPrice = 0,
									  BeginingBalanceCuttingQty = a.CuttingInDate < dateFrom ? c.CuttingInQuantity : 0,
									  BeginingBalanceCuttingPrice = a.CuttingInDate < dateFrom ? c.Price : 0,
									  Ro = a.RONo,
									  QtyCuttingIn = a.CuttingInDate >= dateFrom ? c.CuttingInQuantity : 0,
									  PriceCuttingIn = a.CuttingInDate >= dateFrom ? c.Price : 0,
									  ExpenditureGoodRetur = 0,
									  ExpenditureGoodReturPrice = 0,
									  ExportQty = 0,
									  ExportPrice = 0,
									  SampleQty = 0,
									  SamplePrice = 0,
									  OtherQty = 0,
									  OtherPrice = 0
								  });

			var QueryAvalCompSewing = from a in garmentAvalComponentRepository.Query
									  join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
									  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.Date <= dateTo && a.AvalComponentType == "SEWING"
									  select new monitoringView
									  {
										  QtySewingIn = 0,
										  PriceSewingIn = 0,
										  QtyCuttingOut = 0,
										  PriceCuttingOut = 0,
										  QtyCuttingTransfer = 0,
										  PriceCuttingTransfer = 0,
										  QtyCuttingsubkon = 0,
										  PriceCuttingsubkon = 0,
										  AvalCutting = 0,
										  AvalCuttingPrice = 0,
										  QtyLoading = 0,
										  PriceLoading = 0,
										  QtyLoadingAdjs = 0,
										  PriceLoadingAdjs = 0,
										  QtySewingOut = 0,
										  PriceSewingOut = 0,
										  QtySewingAdj = 0,
										  PriceSewingAdj = 0,
										  WipSewingOut = 0,
										  WipSewingOutPrice = 0,
										  WipFinishingOut = 0,
										  WipFinishingOutPrice = 0,
										  QtySewingRetur = 0,
										  PriceSewingRetur = 0,
										  QtySewingInTransfer = 0,
										  PriceSewingInTransfer = 0,
										  FinishingInQty = 0,
										  FinishingInPrice = 0,
										  SubconInQty = 0,
										  SubconInPrice = 0,
										  FinishingAdjQty = 0,
										  FinishingAdjPrice = 0,
										  FinishingTransferExpenditure = 0,
										  FinishingTransferExpenditurePrice = 0,
										  FinishingInTransferQty = 0,
										  FinishingInTransferPrice = 0,
										  FinishingOutQty = 0,
										  FinishingOutPrice = 0,
										  FinishingReturQty = 0,
										  FinishingReturPrice = 0,
										  SubconOutQty = 0,
										  SubconOutPrice = 0,
										  BeginingBalanceCuttingQty = a.Date < dateFrom ? b.Quantity : 0,
										  BeginingBalanceCuttingPrice = a.Date < dateFrom ? Convert.ToDouble(b.Price) : 0,
										  Ro = a.RONo,
										  QtyCuttingIn = 0,
										  PriceCuttingIn = 0,
										  AvalSewing = a.Date >= dateFrom ? b.Quantity : 0,
										  AvalSewingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
										  ExpenditureGoodRetur = 0,
										  ExpenditureGoodReturPrice = 0,
										  ExportQty = 0,
										  ExportPrice = 0,
										  SampleQty = 0,
										  SamplePrice = 0,
										  OtherQty = 0,
										  OtherPrice = 0
									  };
			var QueryAvalCompCutting = from a in garmentAvalComponentRepository.Query
									   join b in garmentAvalComponentItemRepository.Query on a.Identity equals b.AvalComponentId
									   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.Date <= dateTo && a.AvalComponentType == "CUTTING"
									   select new monitoringView
									   {
										   QtyCuttingIn = 0,
										   PriceCuttingIn = 0,
										   QtySewingIn = 0,
										   PriceSewingIn = 0,
										   QtyCuttingOut = 0,
										   PriceCuttingOut = 0,
										   QtyCuttingTransfer = 0,
										   PriceCuttingTransfer = 0,
										   QtyCuttingsubkon = 0,
										   PriceCuttingsubkon = 0,
										   AvalSewing = 0,
										   AvalSewingPrice = 0,
										   QtyLoading = 0,
										   PriceLoading = 0,
										   QtyLoadingAdjs = 0,
										   PriceLoadingAdjs = 0,
										   QtySewingOut = 0,
										   PriceSewingOut = 0,
										   QtySewingAdj = 0,
										   PriceSewingAdj = 0,
										   WipSewingOut = 0,
										   WipSewingOutPrice = 0,
										   WipFinishingOut = 0,
										   WipFinishingOutPrice = 0,
										   QtySewingRetur = 0,
										   PriceSewingRetur = 0,
										   QtySewingInTransfer = 0,
										   PriceSewingInTransfer = 0,
										   FinishingInQty = 0,
										   FinishingInPrice = 0,
										   SubconInQty = 0,
										   SubconInPrice = 0,
										   FinishingAdjQty = 0,
										   FinishingAdjPrice = 0,
										   FinishingTransferExpenditure = 0,
										   FinishingTransferExpenditurePrice = 0,
										   FinishingInTransferQty = 0,
										   FinishingInTransferPrice = 0,
										   FinishingOutQty = 0,
										   FinishingOutPrice = 0,
										   FinishingReturQty = 0,
										   FinishingReturPrice = 0,
										   SubconOutQty = 0,
										   SubconOutPrice = 0,
										   BeginingBalanceCuttingQty = a.Date < dateFrom ? b.Quantity : 0,
										   BeginingBalanceCuttingPrice = a.Date < dateFrom ? Convert.ToDouble(b.Price) : 0,
										   Ro = a.RONo,
										   AvalCutting = a.Date >= dateFrom ? b.Quantity : 0,
										   AvalCuttingPrice = a.Date >= dateFrom ? Convert.ToDouble(b.Price) : 0,
										   ExpenditureGoodRetur = 0,
										   ExpenditureGoodReturPrice = 0,
										   ExportQty = 0,
										   ExportPrice = 0,
										   SampleQty = 0,
										   SamplePrice = 0,
										   OtherQty = 0,
										   OtherPrice = 0
									   };
			var QuerySewingDO = (from a in garmentSewingDORepository.Query
								 join b in garmentSewingDOItemRepository.Query on a.Identity equals b.SewingDOId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.SewingDODate <= dateTo
								 select new monitoringView
								 {
									 QtyCuttingIn = 0,
									 PriceCuttingIn = 0,
									 QtySewingIn = 0,
									 PriceSewingIn = 0,
									 QtyCuttingOut = 0,
									 PriceCuttingOut = 0,
									 QtyCuttingTransfer = 0,
									 PriceCuttingTransfer = 0,
									 QtyCuttingsubkon = 0,
									 PriceCuttingsubkon = 0,
									 AvalCutting = 0,
									 AvalCuttingPrice = 0,
									 AvalSewing = 0,
									 AvalSewingPrice = 0,
									 QtyLoading = 0,
									 PriceLoading = 0,
									 QtyLoadingAdjs = 0,
									 PriceLoadingAdjs = 0,
									 QtySewingOut = 0,
									 PriceSewingOut = 0,
									 QtySewingAdj = 0,
									 PriceSewingAdj = 0,
									 WipSewingOut = 0,
									 WipSewingOutPrice = 0,
									 WipFinishingOut = 0,
									 WipFinishingOutPrice = 0,
									 QtySewingRetur = 0,
									 PriceSewingRetur = 0,
									 QtySewingInTransfer = 0,
									 PriceSewingInTransfer = 0,
									 FinishingInQty = 0,
									 FinishingInPrice = 0,
									 SubconInQty = 0,
									 SubconInPrice = 0,
									 FinishingAdjQty = 0,
									 FinishingAdjPrice = 0,
									 FinishingTransferExpenditure = 0,
									 FinishingTransferExpenditurePrice = 0,
									 FinishingInTransferQty = 0,
									 FinishingInTransferPrice = 0,
									 FinishingOutQty = 0,
									 FinishingOutPrice = 0,
									 FinishingReturQty = 0,
									 FinishingReturPrice = 0,
									 SubconOutQty = 0,
									 SubconOutPrice = 0,
									 QtyLoadingIn = a.SewingDODate >= dateFrom ? b.Quantity : 0,
									 PriceLoadingIn = a.SewingDODate >= dateFrom ? b.Price : 0,
									 BeginingBalanceLoadingQty = (a.SewingDODate < dateFrom) ? b.Quantity : 0,
									 BeginingBalanceLoadingPrice = (a.SewingDODate < dateFrom) ? b.Price : 0,
									 Ro = a.RONo,
									 ExpenditureGoodRetur = 0,
									 ExpenditureGoodReturPrice = 0,
									 ExportQty = 0,
									 ExportPrice = 0,
									 SampleQty = 0,
									 SamplePrice = 0,
									 OtherQty = 0,
									 OtherPrice = 0
								 });

			var QueryLoading = from a in garmentLoadingRepository.Query
							   join b in garmentLoadingItemRepository.Query on a.Identity equals b.LoadingId
							   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.LoadingDate >= dateFrom && a.LoadingDate <= dateTo
							   select new monitoringView
							   {
								   QtyCuttingIn = 0,
								   PriceCuttingIn = 0,
								   QtySewingIn = 0,
								   PriceSewingIn = 0,
								   QtyCuttingOut = 0,
								   PriceCuttingOut = 0,
								   QtyCuttingTransfer = 0,
								   PriceCuttingTransfer = 0,
								   QtyCuttingsubkon = 0,
								   PriceCuttingsubkon = 0,
								   AvalCutting = 0,
								   AvalCuttingPrice = 0,
								   AvalSewing = 0,
								   AvalSewingPrice = 0,
								   QtyLoadingAdjs = 0,
								   PriceLoadingAdjs = 0,
								   QtySewingOut = 0,
								   PriceSewingOut = 0,
								   QtySewingAdj = 0,
								   PriceSewingAdj = 0,
								   WipSewingOut = 0,
								   WipSewingOutPrice = 0,
								   WipFinishingOut = 0,
								   WipFinishingOutPrice = 0,
								   QtySewingRetur = 0,
								   PriceSewingRetur = 0,
								   QtySewingInTransfer = 0,
								   PriceSewingInTransfer = 0,
								   FinishingInQty = 0,
								   FinishingInPrice = 0,
								   SubconInQty = 0,
								   SubconInPrice = 0,
								   FinishingAdjQty = 0,
								   FinishingAdjPrice = 0,
								   FinishingTransferExpenditure = 0,
								   FinishingTransferExpenditurePrice = 0,
								   FinishingInTransferQty = 0,
								   FinishingInTransferPrice = 0,
								   FinishingOutQty = 0,
								   FinishingOutPrice = 0,
								   FinishingReturQty = 0,
								   FinishingReturPrice = 0,
								   SubconOutQty = 0,
								   SubconOutPrice = 0,
								   BeginingBalanceSewingQty = a.LoadingDate < dateFrom ? b.Quantity : 0,
								   BeginingBalanceSewingPrice = a.LoadingDate < dateFrom ? b.Price : 0,
								   BeginingBalanceLoadingQty = a.LoadingDate < dateFrom ? -b.Quantity : 0,
								   BeginingBalanceLoadingPrice = a.LoadingDate < dateFrom ? -b.Price : 0,
								   Ro = a.RONo,
								   QtyLoading = a.LoadingDate >= dateFrom ? b.Quantity : 0,
								   PriceLoading = a.LoadingDate >= dateFrom ? b.Price : 0,
								   ExpenditureGoodRetur = 0,
								   ExpenditureGoodReturPrice = 0,
								   ExportQty = 0,
								   ExportPrice = 0,
								   SampleQty = 0,
								   SamplePrice = 0,
								   OtherQty = 0,
								   OtherPrice = 0
							   };
			var QueryLoadingAdj = from a in garmentAdjustmentRepository.Query
								  join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
								  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "LOADING"
								  select new monitoringView
								  {
									  QtyCuttingIn = 0,
									  PriceCuttingIn = 0,
									  QtySewingIn = 0,
									  PriceSewingIn = 0,
									  QtyCuttingOut = 0,
									  PriceCuttingOut = 0,
									  QtyCuttingTransfer = 0,
									  PriceCuttingTransfer = 0,
									  QtyCuttingsubkon = 0,
									  PriceCuttingsubkon = 0,
									  AvalCutting = 0,
									  AvalCuttingPrice = 0,
									  AvalSewing = 0,
									  AvalSewingPrice = 0,
									  QtyLoading = 0,
									  PriceLoading = 0,
									  QtySewingOut = 0,
									  PriceSewingOut = 0,
									  QtySewingAdj = 0,
									  PriceSewingAdj = 0,
									  WipSewingOut = 0,
									  WipSewingOutPrice = 0,
									  WipFinishingOut = 0,
									  WipFinishingOutPrice = 0,
									  QtySewingRetur = 0,
									  PriceSewingRetur = 0,
									  QtySewingInTransfer = 0,
									  PriceSewingInTransfer = 0,
									  FinishingInQty = 0,
									  FinishingInPrice = 0,
									  SubconInQty = 0,
									  SubconInPrice = 0,
									  FinishingAdjQty = 0,
									  FinishingAdjPrice = 0,
									  FinishingTransferExpenditure = 0,
									  FinishingTransferExpenditurePrice = 0,
									  FinishingInTransferQty = 0,
									  FinishingInTransferPrice = 0,
									  FinishingOutQty = 0,
									  FinishingOutPrice = 0,
									  FinishingReturQty = 0,
									  FinishingReturPrice = 0,
									  SubconOutQty = 0,
									  SubconOutPrice = 0,
									  BeginingBalanceLoadingQty = a.AdjustmentDate < dateFrom ? -b.Quantity : 0,
									  BeginingBalanceLoadingPrice = a.AdjustmentDate < dateFrom ? -b.Price : 0,
									  Ro = a.RONo,
									  QtyLoadingAdjs = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
									  PriceLoadingAdjs = a.AdjustmentDate >= dateFrom ? b.Price : 0,
									  ExpenditureGoodRetur = 0,
									  ExpenditureGoodReturPrice = 0,
									  ExportQty = 0,
									  ExportPrice = 0,
									  SampleQty = 0,
									  SamplePrice = 0,
									  OtherQty = 0,
									  OtherPrice = 0
								  };
			var QuerySewingIn = (from a in garmentSewingInRepository.Query
								 join b in garmentSewingInItemRepository.Query on a.Identity equals b.SewingInId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.SewingInDate <= dateTo
								 select new monitoringView
								 {
									 QtyCuttingIn = 0,
									 PriceCuttingIn = 0,
									 QtyCuttingOut = 0,
									 PriceCuttingOut = 0,
									 QtyCuttingTransfer = 0,
									 PriceCuttingTransfer = 0,
									 QtyCuttingsubkon = 0,
									 PriceCuttingsubkon = 0,
									 AvalCutting = 0,
									 AvalCuttingPrice = 0,
									 AvalSewing = 0,
									 AvalSewingPrice = 0,
									 QtyLoading = 0,
									 PriceLoading = 0,
									 QtyLoadingAdjs = 0,
									 PriceLoadingAdjs = 0,
									 QtySewingOut = 0,
									 PriceSewingOut = 0,
									 QtySewingAdj = 0,
									 PriceSewingAdj = 0,
									 WipSewingOut = 0,
									 WipSewingOutPrice = 0,
									 WipFinishingOut = 0,
									 WipFinishingOutPrice = 0,
									 QtySewingRetur = 0,
									 PriceSewingRetur = 0,
									 QtySewingInTransfer = 0,
									 PriceSewingInTransfer = 0,
									 FinishingInQty = 0,
									 FinishingInPrice = 0,
									 SubconInQty = 0,
									 SubconInPrice = 0,
									 FinishingAdjQty = 0,
									 FinishingAdjPrice = 0,
									 FinishingTransferExpenditure = 0,
									 FinishingTransferExpenditurePrice = 0,
									 FinishingInTransferQty = 0,
									 FinishingInTransferPrice = 0,
									 FinishingOutQty = 0,
									 FinishingOutPrice = 0,
									 FinishingReturQty = 0,
									 FinishingReturPrice = 0,
									 SubconOutQty = 0,
									 SubconOutPrice = 0,
									 BeginingBalanceSewingQty = (a.SewingInDate < dateFrom /*&& a.SewingFrom == "FINISHING"*/) ? -b.Quantity : 0,
									 BeginingBalanceSewingPrice = (a.SewingInDate < dateFrom /*&& a.SewingFrom == "FINISHING"*/) ? -b.Price : 0,
									 QtySewingIn = (a.SewingInDate >= dateFrom) ? b.Quantity : 0,
									 PriceSewingIn = (a.SewingInDate >= dateFrom) ? b.Price : 0,
									 Ro = a.RONo,
									 ExpenditureGoodRetur = 0,
									 ExpenditureGoodReturPrice = 0,
									 ExportQty = 0,
									 ExportPrice = 0,
									 SampleQty = 0,
									 SamplePrice = 0,
									 OtherQty = 0,
									 OtherPrice = 0
								 });
			var QuerySewingOut = (from a in garmentSewingOutRepository.Query
								  join b in garmentSewingOutItemRepository.Query on a.Identity equals b.SewingOutId
								  where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.SewingOutDate <= dateTo
								  select new monitoringView
								  {
									  QtyCuttingIn = 0,
									  PriceCuttingIn = 0,
									  QtySewingIn = 0,
									  PriceSewingIn = 0,
									  QtyCuttingOut = 0,
									  PriceCuttingOut = 0,
									  QtyCuttingTransfer = 0,
									  PriceCuttingTransfer = 0,
									  AvalCutting = 0,
									  AvalCuttingPrice = 0,
									  AvalSewing = 0,
									  AvalSewingPrice = 0,
									  QtyLoading = 0,
									  PriceLoading = 0,
									  QtyLoadingAdjs = 0,
									  PriceLoadingAdjs = 0,
									  QtySewingAdj = 0,
									  PriceSewingAdj = 0,
									  FinishingInQty = 0,
									  FinishingInPrice = 0,
									  SubconInQty = 0,
									  SubconInPrice = 0,
									  FinishingAdjQty = 0,
									  FinishingAdjPrice = 0,
									  FinishingOutQty = 0,
									  FinishingOutPrice = 0,
									  FinishingReturQty = 0,
									  FinishingReturPrice = 0,
									  SubconOutQty = 0,
									  SubconOutPrice = 0,
									  FinishingTransferExpenditure = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
									  FinishingTransferExpenditurePrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitId == request.unit) ? b.Price : 0,
									  FinishingInTransferQty = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
									  FinishingInTransferPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
									  BeginingBalanceFinishingQty = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
									  BeginingBalanceFinishingPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
									  BeginingBalanceSewingQty = ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Quantity : 0) + ((a.SewingOutDate < dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0),
									  BeginingBalanceSewingPrice = ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? -b.Price : 0) + ((a.SewingOutDate < dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0) - ((a.SewingOutDate < dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Price : 0),
									  QtySewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Quantity : 0,
									  PriceSewingRetur = (a.SewingOutDate >= dateFrom && a.SewingTo == "CUTTING" && a.UnitId == a.UnitToId && a.UnitId == request.unit) ? b.Price : 0,
									  QtySewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Quantity : 0,
									  PriceSewingInTransfer = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? b.Price : 0,
									  WipSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
									  WipSewingOutPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "SEWING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
									  WipFinishingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
									  WipFinishingOutPrice = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId != a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
									  QtySewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Quantity : 0,
									  PriceSewingOut = (a.SewingOutDate >= dateFrom && a.SewingTo == "FINISHING" && a.UnitToId == a.UnitId && a.UnitId == request.unit) ? b.Price : 0,
									  BeginingBalanceExpenditureGood = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Quantity : 0,
									  BeginingBalanceExpenditureGoodPrice = (a.SewingOutDate < dateFrom && a.SewingTo == "FINISHING" && a.UnitId != a.UnitToId && a.UnitToId == request.unit) ? -b.Price : 0,
									  Ro = a.RONo,
									  ExpenditureGoodRetur = 0,
									  ExpenditureGoodReturPrice = 0,
									  ExportQty = 0,
									  ExportPrice = 0,
									  SampleQty = 0,
									  SamplePrice = 0,
									  OtherQty = 0,
									  OtherPrice = 0
								  });

			var QuerySewingAdj = from a in garmentAdjustmentRepository.Query
								 join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
								 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "SEWING"
								 select new monitoringView
								 {
									 QtyCuttingIn = 0,
									 PriceCuttingIn = 0,
									 QtySewingIn = 0,
									 PriceSewingIn = 0,
									 QtyCuttingOut = 0,
									 PriceCuttingOut = 0,
									 QtyCuttingTransfer = 0,
									 PriceCuttingTransfer = 0,
									 QtyCuttingsubkon = 0,
									 PriceCuttingsubkon = 0,
									 AvalCutting = 0,
									 AvalCuttingPrice = 0,
									 AvalSewing = 0,
									 AvalSewingPrice = 0,
									 QtyLoading = 0,
									 PriceLoading = 0,
									 QtyLoadingAdjs = 0,
									 PriceLoadingAdjs = 0,
									 QtySewingOut = 0,
									 PriceSewingOut = 0,
									 WipSewingOut = 0,
									 WipSewingOutPrice = 0,
									 WipFinishingOut = 0,
									 WipFinishingOutPrice = 0,
									 QtySewingRetur = 0,
									 PriceSewingRetur = 0,
									 QtySewingInTransfer = 0,
									 PriceSewingInTransfer = 0,
									 FinishingInQty = 0,
									 FinishingInPrice = 0,
									 SubconInQty = 0,
									 SubconInPrice = 0,
									 FinishingAdjQty = 0,
									 FinishingAdjPrice = 0,
									 FinishingTransferExpenditure = 0,
									 FinishingTransferExpenditurePrice = 0,
									 FinishingInTransferQty = 0,
									 FinishingInTransferPrice = 0,
									 FinishingOutQty = 0,
									 FinishingOutPrice = 0,
									 FinishingReturQty = 0,
									 FinishingReturPrice = 0,
									 SubconOutQty = 0,
									 SubconOutPrice = 0,
									 BeginingBalanceSewingQty = a.AdjustmentDate < dateFrom ? -b.Quantity : 0,
									 BeginingBalanceSewingPrice = a.AdjustmentDate < dateFrom ? -b.Price : 0,
									 Ro = a.RONo,
									 QtySewingAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
									 PriceSewingAdj = a.AdjustmentDate >= dateFrom ? b.Price : 0,
									 ExpenditureGoodRetur = 0,
									 ExpenditureGoodReturPrice = 0,
									 ExportQty = 0,
									 ExportPrice = 0,
									 SampleQty = 0,
									 SamplePrice = 0,
									 OtherQty = 0,
									 OtherPrice = 0
								 };

			var QueryFinishingIn = (from a in garmentFinishingInRepository.Query
									join b in garmentFinishingInItemRepository.Query on a.Identity equals b.FinishingInId
									where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.FinishingInDate <= dateTo
									select new monitoringView
									{
										QtyCuttingIn = 0,
										PriceCuttingIn = 0,
										QtySewingIn = 0,
										PriceSewingIn = 0,
										QtyCuttingOut = 0,
										PriceCuttingOut = 0,
										QtyCuttingTransfer = 0,
										PriceCuttingTransfer = 0,
										QtyCuttingsubkon = 0,
										PriceCuttingsubkon = 0,
										AvalCutting = 0,
										AvalCuttingPrice = 0,
										AvalSewing = 0,
										AvalSewingPrice = 0,
										QtyLoading = 0,
										PriceLoading = 0,
										QtyLoadingAdjs = 0,
										PriceLoadingAdjs = 0,
										QtySewingOut = 0,
										PriceSewingOut = 0,
										QtySewingAdj = 0,
										PriceSewingAdj = 0,
										WipSewingOut = 0,
										WipSewingOutPrice = 0,
										WipFinishingOut = 0,
										WipFinishingOutPrice = 0,
										QtySewingRetur = 0,
										PriceSewingRetur = 0,
										QtySewingInTransfer = 0,
										PriceSewingInTransfer = 0,
										FinishingAdjQty = 0,
										FinishingAdjPrice = 0,
										FinishingTransferExpenditure = 0,
										FinishingTransferExpenditurePrice = 0,
										FinishingInTransferQty = 0,
										FinishingInTransferPrice = 0,
										FinishingOutQty = 0,
										FinishingOutPrice = 0,
										FinishingReturQty = 0,
										FinishingReturPrice = 0,
										SubconOutQty = 0,
										SubconOutPrice = 0,
										BeginingBalanceSubconQty = (a.FinishingInDate < dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
										BeginingBalanceSubconPrice = (a.FinishingInDate < dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
										BeginingBalanceFinishingQty = (a.FinishingInDate < dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
										BeginingBalanceFinishingPrice = (a.FinishingInDate < dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
										FinishingInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
										FinishingInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType != "PEMBELIAN") ? b.Price : 0,
										SubconInQty = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
										SubconInPrice = (a.FinishingInDate >= dateFrom && a.FinishingInType == "PEMBELIAN") ? b.Price : 0,
										Ro = a.RONo,
										ExpenditureGoodRetur = 0,
										ExpenditureGoodReturPrice = 0,
										ExportQty = 0,
										ExportPrice = 0,
										SampleQty = 0,
										SamplePrice = 0,
										OtherQty = 0,
										OtherPrice = 0
									});
			var QueryFinishingOut = (from a in garmentFinishingOutRepository.Query
									 join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									 join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
									 join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
									 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.FinishingOutDate <= dateTo && a.FinishingTo == "GUDANG JADI"
									 select new monitoringView
									 {
										 QtyCuttingIn = 0,
										 PriceCuttingIn = 0,
										 QtySewingIn = 0,
										 PriceSewingIn = 0,
										 QtyCuttingOut = 0,
										 PriceCuttingOut = 0,
										 QtyCuttingTransfer = 0,
										 PriceCuttingTransfer = 0,
										 QtyCuttingsubkon = 0,
										 PriceCuttingsubkon = 0,
										 AvalCutting = 0,
										 AvalCuttingPrice = 0,
										 AvalSewing = 0,
										 AvalSewingPrice = 0,
										 QtyLoading = 0,
										 PriceLoading = 0,
										 QtyLoadingAdjs = 0,
										 PriceLoadingAdjs = 0,
										 QtySewingOut = 0,
										 PriceSewingOut = 0,
										 QtySewingAdj = 0,
										 PriceSewingAdj = 0,
										 WipSewingOut = 0,
										 WipSewingOutPrice = 0,
										 WipFinishingOut = 0,
										 WipFinishingOutPrice = 0,
										 QtySewingRetur = 0,
										 PriceSewingRetur = 0,
										 QtySewingInTransfer = 0,
										 PriceSewingInTransfer = 0,
										 FinishingInQty = 0,
										 FinishingInPrice = 0,
										 SubconInQty = 0,
										 SubconInPrice = 0,
										 FinishingAdjQty = 0,
										 FinishingAdjPrice = 0,
										 FinishingTransferExpenditure = 0,
										 FinishingTransferExpenditurePrice = 0,
										 FinishingInTransferQty = 0,
										 FinishingInTransferPrice = 0,
										 FinishingReturQty = 0,
										 FinishingReturPrice = 0,
										 BeginingBalanceFinishingQty = (a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? -b.Quantity : 0,
										 BeginingBalanceFinishingPrice = (a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? -b.Price : 0,
										 BeginingBalanceExpenditureGood = ((a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0) + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0),
										 BeginingBalanceExpenditureGoodPrice = (a.FinishingOutDate < dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0 + ((a.FinishingOutDate < dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0),
										 FinishingOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Quantity : 0,
										 FinishingOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType != "PEMBELIAN") ? b.Price : 0,
										 SubconOutQty = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Quantity : 0,
										 SubconOutPrice = (a.FinishingOutDate >= dateFrom && d.FinishingInType == "PEMBELIAN") ? b.Price : 0,
										 Ro = a.RONo,
										 ExpenditureGoodRetur = 0,
										 ExpenditureGoodReturPrice = 0,
										 ExportQty = 0,
										 ExportPrice = 0,
										 SampleQty = 0,
										 SamplePrice = 0,
										 OtherQty = 0,
										 OtherPrice = 0

									 });

			var QueryFinishingAdj = from a in garmentAdjustmentRepository.Query
									join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
									where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "FINISHING"
									select new monitoringView
									{
										QtyCuttingIn = 0,
										PriceCuttingIn = 0,
										QtySewingIn = 0,
										PriceSewingIn = 0,
										QtyCuttingOut = 0,
										PriceCuttingOut = 0,
										QtyCuttingTransfer = 0,
										PriceCuttingTransfer = 0,
										QtyCuttingsubkon = 0,
										PriceCuttingsubkon = 0,
										AvalCutting = 0,
										AvalCuttingPrice = 0,
										AvalSewing = 0,
										AvalSewingPrice = 0,
										QtyLoading = 0,
										PriceLoading = 0,
										QtyLoadingAdjs = 0,
										PriceLoadingAdjs = 0,
										QtySewingOut = 0,
										PriceSewingOut = 0,
										QtySewingAdj = 0,
										PriceSewingAdj = 0,
										WipSewingOut = 0,
										WipSewingOutPrice = 0,
										WipFinishingOut = 0,
										WipFinishingOutPrice = 0,
										QtySewingRetur = 0,
										PriceSewingRetur = 0,
										QtySewingInTransfer = 0,
										PriceSewingInTransfer = 0,
										FinishingInQty = 0,
										FinishingInPrice = 0,
										SubconInQty = 0,
										SubconInPrice = 0,
										FinishingTransferExpenditure = 0,
										FinishingTransferExpenditurePrice = 0,
										FinishingInTransferQty = 0,
										FinishingInTransferPrice = 0,
										BeginingBalanceFinishingQty = a.AdjustmentDate >= dateFrom ? -b.Quantity : 0,
										BeginingBalanceFinishingPrice = a.AdjustmentDate >= dateFrom ? -b.Price : 0,
										FinishingAdjQty = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
										FinishingAdjPrice = a.AdjustmentDate >= dateFrom ? b.Price : 0,
										FinishingOutQty = 0,
										FinishingOutPrice = 0,
										FinishingReturQty = 0,
										FinishingReturPrice = 0,
										SubconOutQty = 0,
										SubconOutPrice = 0,
										Ro = a.RONo,
										ExpenditureGoodRetur = 0,
										ExpenditureGoodReturPrice = 0,
										ExportQty = 0,
										ExportPrice = 0,
										SampleQty = 0,
										SamplePrice = 0,
										OtherQty = 0,
										OtherPrice = 0
									};

			var QueryFinishingRetur = (from a in garmentFinishingOutRepository.Query
									   join b in garmentFinishingOutItemRepository.Query on a.Identity equals b.FinishingOutId
									   join c in garmentFinishingInItemRepository.Query on b.FinishingInItemId equals c.Identity
									   join d in garmentFinishingInRepository.Query on c.FinishingInId equals d.Identity
									   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.FinishingOutDate <= dateTo && a.FinishingTo == "SEWING"
									   select new monitoringView
									   {
										   QtyCuttingIn = 0,
										   PriceCuttingIn = 0,
										   QtySewingIn = 0,
										   PriceSewingIn = 0,
										   QtyCuttingOut = 0,
										   PriceCuttingOut = 0,
										   QtyCuttingTransfer = 0,
										   PriceCuttingTransfer = 0,
										   QtyCuttingsubkon = 0,
										   PriceCuttingsubkon = 0,
										   AvalCutting = 0,
										   AvalCuttingPrice = 0,
										   AvalSewing = 0,
										   AvalSewingPrice = 0,
										   QtyLoading = 0,
										   PriceLoading = 0,
										   QtyLoadingAdjs = 0,
										   PriceLoadingAdjs = 0,
										   QtySewingOut = 0,
										   PriceSewingOut = 0,
										   QtySewingAdj = 0,
										   PriceSewingAdj = 0,
										   WipSewingOut = 0,
										   WipSewingOutPrice = 0,
										   WipFinishingOut = 0,
										   WipFinishingOutPrice = 0,
										   QtySewingRetur = 0,
										   PriceSewingRetur = 0,
										   QtySewingInTransfer = 0,
										   PriceSewingInTransfer = 0,
										   FinishingInQty = 0,
										   FinishingInPrice = 0,
										   SubconInQty = 0,
										   SubconInPrice = 0,
										   FinishingAdjQty = 0,
										   FinishingAdjPrice = 0,
										   FinishingTransferExpenditure = 0,
										   FinishingTransferExpenditurePrice = 0,
										   FinishingInTransferQty = 0,
										   FinishingInTransferPrice = 0,
										   FinishingOutQty = 0,
										   FinishingOutPrice = 0,
										   SubconOutQty = 0,
										   SubconOutPrice = 0,
										   BeginingBalanceFinishingQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.UnitId == a.UnitToId) ? -b.Quantity : 0,
										   BeginingBalanceFinishingPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate < dateFrom && a.UnitId == a.UnitToId) ? -b.Price : 0,
										   FinishingReturQty = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitId == a.UnitToId) ? b.Quantity : 0,
										   FinishingReturPrice = (d.FinishingInType != "PEMBELIAN" && a.FinishingOutDate >= dateFrom && a.UnitId == a.UnitToId) ? b.Price : 0,
										   Ro = a.RONo,
										   ExpenditureGoodRetur = 0,
										   ExpenditureGoodReturPrice = 0,
										   ExportQty = 0,
										   ExportPrice = 0,
										   SampleQty = 0,
										   SamplePrice = 0,
										   OtherQty = 0,
										   OtherPrice = 0
									   });
			var QueryExpenditureGoods = (from a in garmentExpenditureGoodRepository.Query
										 join b in garmentExpenditureGoodItemRepository.Query on a.Identity equals b.ExpenditureGoodId
										 where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.ExpenditureDate <= dateTo
										 select new monitoringView
										 {
											 QtyCuttingIn = 0,
											 PriceCuttingIn = 0,
											 QtySewingIn = 0,
											 PriceSewingIn = 0,
											 QtyCuttingOut = 0,
											 PriceCuttingOut = 0,
											 QtyCuttingTransfer = 0,
											 PriceCuttingTransfer = 0,
											 QtyCuttingsubkon = 0,
											 PriceCuttingsubkon = 0,
											 AvalCutting = 0,
											 AvalCuttingPrice = 0,
											 AvalSewing = 0,
											 AvalSewingPrice = 0,
											 QtyLoading = 0,
											 PriceLoading = 0,
											 QtyLoadingAdjs = 0,
											 PriceLoadingAdjs = 0,
											 QtySewingOut = 0,
											 PriceSewingOut = 0,
											 QtySewingAdj = 0,
											 PriceSewingAdj = 0,
											 WipSewingOut = 0,
											 WipSewingOutPrice = 0,
											 WipFinishingOut = 0,
											 WipFinishingOutPrice = 0,
											 QtySewingRetur = 0,
											 PriceSewingRetur = 0,
											 QtySewingInTransfer = 0,
											 PriceSewingInTransfer = 0,
											 FinishingInQty = 0,
											 FinishingInPrice = 0,
											 SubconInQty = 0,
											 SubconInPrice = 0,
											 FinishingAdjQty = 0,
											 FinishingAdjPrice = 0,
											 FinishingTransferExpenditure = 0,
											 FinishingTransferExpenditurePrice = 0,
											 FinishingInTransferQty = 0,
											 FinishingInTransferPrice = 0,
											 FinishingOutQty = 0,
											 FinishingOutPrice = 0,
											 FinishingReturQty = 0,
											 FinishingReturPrice = 0,
											 SubconOutQty = 0,
											 SubconOutPrice = 0,
											 BeginingBalanceExpenditureGood = a.ExpenditureDate < dateFrom ? -b.Quantity : 0,
											 BeginingBalanceExpenditureGoodPrice = a.ExpenditureDate < dateFrom ? -b.Price : 0,
											 ExportQty = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Quantity : 0,
											 ExportPrice = (a.ExpenditureDate >= dateFrom && a.ExpenditureType == "EXPORT") ? b.Price : 0,
											 SampleQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SAMPLE" || (a.ExpenditureType == "LAIN-LAIN"))) ? b.Quantity : 0,
											 SamplePrice = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SAMPLE" || (a.ExpenditureType == "LAIN-LAIN"))) ? b.Price : 0,
											 OtherQty = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Quantity : 0,
											 OtherPrice = (a.ExpenditureDate >= dateFrom && (a.ExpenditureType == "SISA")) ? b.Price : 0,
											 Ro = a.RONo,
											 ExpenditureGoodRetur = 0,
											 ExpenditureGoodReturPrice = 0
										 });
			var QueryExpenditureGoodsAdj = from a in garmentAdjustmentRepository.Query
										   join b in garmentAdjustmentItemRepository.Query on a.Identity equals b.AdjustmentId
										   where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.AdjustmentDate <= dateTo && a.AdjustmentType == "BARANG JADI"
										   select new monitoringView
										   {
											   QtyCuttingIn = 0,
											   PriceCuttingIn = 0,
											   QtySewingIn = 0,
											   PriceSewingIn = 0,
											   QtyCuttingOut = 0,
											   PriceCuttingOut = 0,
											   QtyCuttingTransfer = 0,
											   PriceCuttingTransfer = 0,
											   QtyCuttingsubkon = 0,
											   PriceCuttingsubkon = 0,
											   AvalCutting = 0,
											   AvalCuttingPrice = 0,
											   AvalSewing = 0,
											   AvalSewingPrice = 0,
											   QtyLoading = 0,
											   PriceLoading = 0,
											   QtyLoadingAdjs = 0,
											   PriceLoadingAdjs = 0,
											   QtySewingOut = 0,
											   PriceSewingOut = 0,
											   QtySewingAdj = 0,
											   PriceSewingAdj = 0,
											   WipSewingOut = 0,
											   WipSewingOutPrice = 0,
											   WipFinishingOut = 0,
											   WipFinishingOutPrice = 0,
											   QtySewingRetur = 0,
											   PriceSewingRetur = 0,
											   QtySewingInTransfer = 0,
											   PriceSewingInTransfer = 0,
											   FinishingInQty = 0,
											   FinishingInPrice = 0,
											   SubconInQty = 0,
											   SubconInPrice = 0,
											   FinishingAdjQty = 0,
											   FinishingAdjPrice = 0,
											   FinishingTransferExpenditure = 0,
											   FinishingTransferExpenditurePrice = 0,
											   FinishingInTransferQty = 0,
											   FinishingInTransferPrice = 0,
											   FinishingOutQty = 0,
											   FinishingOutPrice = 0,
											   FinishingReturQty = 0,
											   FinishingReturPrice = 0,
											   SubconOutQty = 0,
											   SubconOutPrice = 0,
											   BeginingBalanceExpenditureGood = a.AdjustmentDate < dateFrom ? -b.Quantity : 0,
											   BeginingBalanceExpenditureGoodPrice = a.AdjustmentDate < dateFrom ? -b.Price : 0,
											   ExpenditureGoodAdj = a.AdjustmentDate >= dateFrom ? b.Quantity : 0,
											   ExpenditureGoodAdjPrice = a.AdjustmentDate >= dateFrom ? b.Price : 0,
											   Ro = a.RONo,
											   ExpenditureGoodRetur = 0,
											   ExpenditureGoodReturPrice = 0,
											   ExportQty = 0,
											   ExportPrice = 0,
											   SampleQty = 0,
											   SamplePrice = 0,
											   OtherQty = 0,
											   OtherPrice = 0
										   };
			var QueryExpenditureGoodRetur = from a in garmentExpenditureGoodReturnRepository.Query
											join b in garmentExpenditureGoodReturnItemRepository.Query on a.Identity equals b.ReturId
											where (request.ro == null || (request.ro != null && request.ro != "" && a.RONo == request.ro)) && a.UnitId == request.unit && a.ReturDate <= dateTo
											select new monitoringView
											{
												QtyCuttingIn = 0,
												PriceCuttingIn = 0,
												QtySewingIn = 0,
												PriceSewingIn = 0,
												QtyCuttingOut = 0,
												PriceCuttingOut = 0,
												QtyCuttingTransfer = 0,
												PriceCuttingTransfer = 0,
												QtyCuttingsubkon = 0,
												PriceCuttingsubkon = 0,
												AvalCutting = 0,
												AvalCuttingPrice = 0,
												AvalSewing = 0,
												AvalSewingPrice = 0,
												QtyLoading = 0,
												PriceLoading = 0,
												QtyLoadingAdjs = 0,
												PriceLoadingAdjs = 0,
												QtySewingOut = 0,
												PriceSewingOut = 0,
												QtySewingAdj = 0,
												PriceSewingAdj = 0,
												WipSewingOut = 0,
												WipSewingOutPrice = 0,
												WipFinishingOut = 0,
												WipFinishingOutPrice = 0,
												QtySewingRetur = 0,
												PriceSewingRetur = 0,
												QtySewingInTransfer = 0,
												PriceSewingInTransfer = 0,
												FinishingInQty = 0,
												FinishingInPrice = 0,
												SubconInQty = 0,
												SubconInPrice = 0,
												FinishingAdjQty = 0,
												FinishingAdjPrice = 0,
												FinishingTransferExpenditure = 0,
												FinishingTransferExpenditurePrice = 0,
												FinishingInTransferQty = 0,
												FinishingInTransferPrice = 0,
												FinishingOutQty = 0,
												FinishingOutPrice = 0,
												FinishingReturQty = 0,
												FinishingReturPrice = 0,
												SubconOutQty = 0,
												SubconOutPrice = 0,
												BeginingBalanceExpenditureGood = a.ReturDate < dateFrom ? b.Quantity : 0,
												BeginingBalanceExpenditureGoodPrice = a.ReturDate < dateFrom ? b.Price : 0,
												ExpenditureGoodRetur = a.ReturDate >= dateFrom ? b.Quantity : 0,
												ExpenditureGoodReturPrice = a.ReturDate >= dateFrom ? b.Price : 0,
												Ro = a.RONo,
												ExportQty = 0,
												ExportPrice = 0,
												SampleQty = 0,
												SamplePrice = 0,
												OtherQty = 0,
												OtherPrice = 0
											};


			var queryNow = QueryCuttingIn
				.Union(QueryCuttingOut)
				.Union(QueryCuttingOutSubkon)
				.Union(QueryCuttingOutTransfer)
				.Union(QueryAvalCompCutting)
				.Union(QueryAvalCompSewing)
				.Union(QuerySewingDO)
				.Union(QueryLoading)
				.Union(QueryLoadingAdj)
				.Union(QuerySewingIn)
				.Union(QuerySewingOut)
				.Union(QuerySewingAdj)
				.Union(QueryFinishingIn)
				.Union(QueryFinishingOut)
				.Union(QueryFinishingAdj)
				.Union(QueryFinishingRetur)
				.Union(QueryExpenditureGoods)
				.Union(QueryExpenditureGoodsAdj)
				.Union(QueryExpenditureGoodRetur)
				.AsEnumerable();

			var querySum = (from a in queryNow
							join b in queryGroup on a.Ro equals b.Ro
							select new
							{
								b.Article,
								b.BuyerCode,
								b.Comodity,
								b.Hours,
								b.FC,
								b.QtyOrder,
								b.BasicPrice,
								b.Fare,
								b.FareNew,
								a.Ro,
								a.BeginingBalanceCuttingQty,
								a.BeginingBalanceCuttingPrice,
								a.QtyCuttingIn,
								a.PriceCuttingIn,
								a.QtyCuttingOut,
								a.PriceCuttingOut,
								a.QtyCuttingTransfer,
								a.PriceCuttingTransfer,
								a.QtyCuttingsubkon,
								a.PriceCuttingsubkon,
								a.AvalCutting,
								a.AvalCuttingPrice,
								a.AvalSewing,
								a.AvalSewingPrice,
								a.BeginingBalanceLoadingQty,
								a.BeginingBalanceLoadingPrice,
								a.QtyLoadingIn,
								a.PriceLoadingIn,
								a.QtyLoading,
								a.PriceLoading,
								a.QtyLoadingAdjs,
								a.PriceLoadingAdjs,
								a.BeginingBalanceSewingQty,
								a.BeginingBalanceSewingPrice,
								a.QtySewingIn,
								a.PriceSewingIn,
								a.QtySewingOut,
								a.PriceSewingOut,
								a.QtySewingInTransfer,
								a.PriceSewingInTransfer,
								a.WipSewingOut,
								a.WipSewingOutPrice,
								a.WipFinishingOut,
								a.WipFinishingOutPrice,
								a.QtySewingRetur,
								a.PriceSewingRetur,
								a.QtySewingAdj,
								a.PriceSewingAdj,
								a.BeginingBalanceFinishingQty,
								a.BeginingBalanceFinishingPrice,
								a.FinishingInQty,
								a.FinishingInPrice,
								a.BeginingBalanceSubconQty,
								a.BeginingBalanceSubconPrice,
								a.SubconInQty,
								a.SubconInPrice,
								a.SubconOutQty,
								a.SubconOutPrice,
								a.FinishingOutQty,
								a.FinishingOutPrice,
								a.FinishingInTransferQty,
								a.FinishingInTransferPrice,
								a.FinishingAdjQty,
								a.FinishingAdjPrice,
								a.FinishingReturQty,
								a.FinishingReturPrice,
								a.BeginingBalanceExpenditureGood,
								a.BeginingBalanceExpenditureGoodPrice,
								a.ExpenditureGoodRetur,
								a.ExpenditureGoodReturPrice,
								a.ExportQty,
								a.ExportPrice,
								a.OtherQty,
								a.OtherPrice,
								a.SampleQty,
								a.SamplePrice,
								a.ExpenditureGoodAdj,
								a.ExpenditureGoodAdjPrice
							})
				.GroupBy(x => new { x.FareNew, x.Fare, x.BasicPrice, x.FC, x.Hours, x.BuyerCode, x.Ro, x.Article, x.Comodity, x.QtyOrder }, (key, group) => new
				{
					ro = key.Ro,
					article = key.Article,
					comodity = key.Comodity,
					qtyOrder = key.QtyOrder,
					buyer = key.BuyerCode,
					fc = key.FC,
					fare = key.Fare,
					farenew = key.FareNew,
					hours = key.Hours,
					basicprice = key.BasicPrice,
					qtycutting = group.Sum(s => s.QtyCuttingOut),
					priceCuttingOut = group.Sum(s => s.PriceCuttingOut),
					qtCuttingSubkon = group.Sum(s => s.QtyCuttingsubkon),
					priceCuttingSubkon = group.Sum(s => s.PriceCuttingsubkon),
					qtyCuttingTransfer = group.Sum(s => s.QtyCuttingTransfer),
					priceCuttingTransfer = group.Sum(s => s.PriceCuttingTransfer),
					qtyCuttingIn = group.Sum(s => s.QtyCuttingIn),
					priceCuttingIn = group.Sum(s => s.PriceCuttingIn),
					begining = group.Sum(s => s.BeginingBalanceCuttingQty),
					beginingcuttingPrice = group.Sum(s => s.BeginingBalanceCuttingPrice),
					qtyavalsew = group.Sum(s => s.AvalSewing),
					priceavalsew = group.Sum(s => s.AvalSewingPrice),
					qtyavalcut = group.Sum(s => s.AvalCutting),
					priceavalcut = group.Sum(s => s.AvalCuttingPrice),
					beginingloading = group.Sum(s => s.BeginingBalanceLoadingQty),
					beginingloadingPrice = group.Sum(s => s.BeginingBalanceLoadingPrice),
					qtyLoadingIn = group.Sum(s => s.QtyLoadingIn),
					priceLoadingIn = group.Sum(s => s.PriceLoadingIn),
					qtyloading = group.Sum(s => s.QtyLoading),
					priceloading = group.Sum(s => s.PriceLoading),
					qtyLoadingAdj = group.Sum(s => s.QtyLoadingAdjs),
					priceLoadingAdj = group.Sum(s => s.PriceLoadingAdjs),
					beginingSewing = group.Sum(s => s.BeginingBalanceSewingQty),
					beginingSewingPrice = group.Sum(s => s.BeginingBalanceSewingPrice),
					sewingIn = group.Sum(s => s.QtySewingIn),
					sewingInPrice = group.Sum(s => s.PriceSewingIn),
					sewingintransfer = group.Sum(s => s.QtySewingInTransfer),
					sewingintransferPrice = group.Sum(s => s.PriceSewingInTransfer),
					sewingout = group.Sum(s => s.QtySewingOut),
					sewingoutPrice = group.Sum(s => s.PriceSewingOut),
					sewingretur = group.Sum(s => s.QtySewingRetur),
					sewingreturPrice = group.Sum(s => s.PriceSewingRetur),
					wipsewing = group.Sum(s => s.WipSewingOut),
					wipsewingPrice = group.Sum(s => s.WipSewingOutPrice),
					wipfinishing = group.Sum(s => s.WipFinishingOut),
					wipfinishingPrice = group.Sum(s => s.WipFinishingOutPrice),
					sewingadj = group.Sum(s => s.QtySewingAdj),
					sewingadjPrice = group.Sum(s => s.PriceSewingAdj),
					finishingin = group.Sum(s => s.FinishingInQty),
					finishinginPrice = group.Sum(s => s.FinishingInPrice),
					finishingintransfer = group.Sum(s => s.FinishingInTransferQty),
					finishingintransferPrice = group.Sum(s => s.FinishingInTransferPrice),
					finishingadj = group.Sum(s => s.FinishingAdjQty),
					finishingadjPrice = group.Sum(s => s.FinishingAdjPrice),
					finishingout = group.Sum(s => s.FinishingOutQty),
					finishingoutPrice = group.Sum(s => s.FinishingOutPrice),
					finishinigretur = group.Sum(s => s.FinishingReturQty),
					finishinigreturPrice = group.Sum(s => s.FinishingReturPrice),
					beginingbalanceFinishing = group.Sum(s => s.BeginingBalanceFinishingQty),
					beginingbalanceFinishingPrice = group.Sum(s => s.BeginingBalanceFinishingPrice),
					beginingbalancesubcon = group.Sum(s => s.BeginingBalanceSubconQty),
					beginingbalancesubconPrice = group.Sum(s => s.BeginingBalanceSubconPrice),
					subconIn = group.Sum(s => s.SubconInQty),
					subconInPrice = group.Sum(s => s.SubconInPrice),
					subconout = group.Sum(s => s.SubconOutQty),
					subconoutPrice = group.Sum(s => s.SubconOutPrice),
					exportQty = group.Sum(s => s.ExportQty),
					exportPrice = group.Sum(s => s.ExportPrice),
					otherqty = group.Sum(s => s.OtherQty),
					otherprice = group.Sum(s => s.OtherPrice),
					sampleQty = group.Sum(s => s.SampleQty),
					samplePrice = group.Sum(s => s.SamplePrice),
					expendAdj = group.Sum(s => s.ExpenditureGoodAdj),
					expendAdjPrice = group.Sum(s => s.ExpenditureGoodAdjPrice),
					expendRetur = group.Sum(s => s.ExpenditureGoodRetur),
					expendReturPrice = group.Sum(s => s.ExpenditureGoodReturPrice),
					//finishinginqty =group.Sum(s=>s.FinishingInQty)
					beginingBalanceExpenditureGood = group.Sum(s => s.BeginingBalanceExpenditureGood),
					beginingBalanceExpenditureGoodPrice = group.Sum(s => s.BeginingBalanceExpenditureGoodPrice)



				});

			GarmentMonitoringProductionStockFlowListViewModel garmentMonitoringProductionFlow = new GarmentMonitoringProductionStockFlowListViewModel();
			List<GarmentMonitoringProductionStockFlowDto> monitoringDtos = new List<GarmentMonitoringProductionStockFlowDto>();

			foreach (var item in querySum)
			{
				GarmentMonitoringProductionStockFlowDto garmentMonitoringDto = new GarmentMonitoringProductionStockFlowDto()
				{
					Article = item.article,
					Ro = item.ro,
					QtyOrder = item.qtyOrder,
					FC = item.fc,
					Fare = item.fare,
					BuyerCode = item.buyer,
					Hours = item.hours,
					BasicPrice = item.basicprice,
					BeginingBalanceCuttingQty = item.begining,
					BeginingBalanceCuttingPrice = item.beginingcuttingPrice,
					QtyCuttingTransfer = item.qtyCuttingTransfer,
					PriceCuttingTransfer = item.priceCuttingTransfer,
					QtyCuttingsubkon = item.qtCuttingSubkon,
					PriceCuttingsubkon = item.priceCuttingSubkon,
					QtyCuttingIn = item.qtyCuttingIn,
					PriceCuttingIn = item.priceCuttingIn,
					QtyCuttingOut = item.qtycutting,
					PriceCuttingOut = item.priceCuttingOut,
					Comodity = item.comodity,
					AvalCutting = item.qtyavalcut,
					AvalCuttingPrice = item.priceavalcut,
					AvalSewing = item.qtyavalsew,
					AvalSewingPrice = item.priceavalsew,
					EndBalancCuttingeQty = item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon - item.qtyavalcut - item.qtyavalsew,
					EndBalancCuttingePrice = item.beginingcuttingPrice + item.priceCuttingIn - item.priceCuttingOut - item.priceCuttingTransfer - item.priceCuttingSubkon - item.priceavalcut - item.priceavalsew,
					BeginingBalanceLoadingQty = item.beginingloading,
					BeginingBalanceLoadingPrice = item.beginingloadingPrice,
					QtyLoadingIn = item.qtyLoadingIn,
					PriceLoadingIn = item.priceLoadingIn,
					QtyLoading = item.qtyloading,
					PriceLoading = item.priceloading,
					QtyLoadingAdjs = item.qtyLoadingAdj,
					PriceLoadingAdjs = item.priceLoadingAdj,
					EndBalanceLoadingQty = item.beginingloading + item.qtyLoadingIn - item.qtyloading - item.qtyLoadingAdj,
					EndBalanceLoadingPrice = item.beginingloadingPrice + item.priceLoadingIn - item.priceloading - item.priceLoadingAdj,
					BeginingBalanceSewingQty = item.beginingSewing,
					BeginingBalanceSewingPrice = item.beginingSewingPrice,
					QtySewingIn = item.sewingIn,
					PriceSewingIn = item.sewingInPrice,
					QtySewingOut = item.sewingout,
					PriceSewingOut = item.sewingoutPrice,
					QtySewingInTransfer = item.sewingintransfer,
					PriceSewingInTransfer = item.sewingintransferPrice,
					QtySewingRetur = item.sewingretur,
					PriceSewingRetur = item.sewingreturPrice,
					WipSewingOut = item.wipsewing,
					WipSewingOutPrice = item.wipsewingPrice,
					WipFinishingOut = item.wipfinishing,
					WipFinishingOutPrice = item.wipfinishingPrice,
					QtySewingAdj = item.sewingadj,
					PriceSewingAdj = item.sewingadjPrice,
					EndBalanceSewingQty = item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj,
					EndBalanceSewingPrice = item.beginingSewingPrice + item.sewingInPrice - item.sewingoutPrice + item.sewingintransferPrice - item.wipsewingPrice - item.wipfinishingPrice - item.sewingreturPrice - item.sewingadjPrice,
					BeginingBalanceFinishingQty = item.beginingbalanceFinishing,
					BeginingBalanceFinishingPrice = item.beginingbalanceFinishingPrice,
					FinishingInExpenditure = item.finishingout + item.subconout,
					FinishingInExpenditurepPrice = item.finishingoutPrice + item.subconoutPrice,
					FinishingInQty = item.finishingin,
					FinishingInPrice = item.finishinginPrice,
					FinishingOutQty = item.finishingout,
					FinishingOutPrice = item.finishingoutPrice,
					BeginingBalanceSubconQty = item.beginingbalancesubcon,
					BeginingBalanceSubconPrice = item.beginingbalancesubconPrice,
					SubconInQty = item.subconIn,
					SubconInPrice = item.subconoutPrice,
					SubconOutQty = item.subconout,
					SubconOutPrice = item.subconoutPrice,
					EndBalanceSubconQty = item.beginingbalancesubcon + item.subconIn - item.subconout,
					EndBalanceSubconPrice = item.beginingbalancesubconPrice + item.subconInPrice - item.subconoutPrice,
					FinishingInTransferQty = item.finishingintransfer,
					FinishingInTransferPrice = item.finishingintransferPrice,
					FinishingReturQty = item.finishinigretur,
					FinishingReturPrice = item.finishinigreturPrice,
					FinishingAdjQty = item.finishingadj,
					FinishingAdjPRice = item.finishingadjPrice,
					BeginingBalanceExpenditureGood = item.beginingBalanceExpenditureGood,
					BeginingBalanceExpenditureGoodPrice = item.beginingBalanceExpenditureGoodPrice,
					EndBalanceFinishingQty = item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur,
					EndBalanceFinishingPrice = item.beginingbalanceFinishingPrice + item.finishinginPrice + item.finishingintransferPrice - item.finishingoutPrice - item.finishingadjPrice - item.finishinigreturPrice,
					ExportQty = item.exportQty,
					ExportPrice = item.exportPrice,
					SampleQty = item.sampleQty,
					SamplePrice = item.samplePrice,
					OtherQty = item.otherqty,
					OtherPrice = item.otherprice,
					ExpenditureGoodAdj = item.expendAdj,
					ExpenditureGoodAdjPrice = item.expendAdjPrice,
					EndBalanceExpenditureGood = item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur - item.finishingintransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj,
					EndBalanceExpenditureGoodPrice = item.beginingBalanceExpenditureGoodPrice + item.finishingoutPrice + item.subconoutPrice + item.expendReturPrice - item.finishingintransferPrice - item.exportPrice - item.otherprice - item.samplePrice - item.expendAdjPrice,
					FareNew = item.farenew,
					CuttingNew = item.farenew * Convert.ToDecimal(item.begining + item.qtyCuttingIn - item.qtycutting - item.qtyCuttingTransfer - item.qtCuttingSubkon - item.qtyavalcut - item.qtyavalsew),
					LoadingNew = item.farenew * Convert.ToDecimal(item.beginingloading + item.qtyLoadingIn - item.qtyloading - item.qtyLoadingAdj),
					SewingNew = item.farenew * Convert.ToDecimal(item.beginingSewing + item.sewingIn - item.sewingout + item.sewingintransfer - item.wipsewing - item.wipfinishing - item.sewingretur - item.sewingadj),
					FinishingNew = item.farenew * Convert.ToDecimal(item.beginingbalanceFinishing + item.finishingin + item.finishingintransfer - item.finishingout - item.finishingadj - item.finishinigretur),
					ExpenditureNew = item.farenew * Convert.ToDecimal(item.beginingBalanceExpenditureGood + item.finishingout + item.subconout + item.expendRetur - item.finishingintransfer - item.exportQty - item.otherqty - item.sampleQty - item.expendAdj)


				};
				monitoringDtos.Add(garmentMonitoringDto);
			
		}
			garmentMonitoringProductionFlow.garmentMonitorings = monitoringDtos;
			var reportDataTable = new DataTable();
			
			if (request.type != "bookkeeping")
			{
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI9", DataType = typeof(string) });

				reportDataTable.Rows.Add("", "", "", "",
				"Saldo Awal WIP Cutting", "Cutting In (WIP Cutting)", "Cutting Out / HP(WIP Loading)", "Cutting Out Transfer", "Cutting Out Subkon", "Aval Komponen dari Cutting", "Aval Komponen dari Sewing", "Saldo Akhir WIP Cutting",
				"Saldo Awal Loading", "Loading In", "Loading Out (WIP Sewing)	", "Adjs Loading", "Saldo Akhir Loading",
				"Saldo Awal WIP Sewing", "Sewing In (WIP Sewing)", "Sewing Out (WIP Finishing)", "Sewing In Transfer", "Sewing Out Tranfer WIP Sewing", "Sewing Out Transfer WIP Finishing", "Retur ke Cutting", "Adjs Sewing", "Saldo Akhir WIP Sewing",
				"Saldo Awal WIP Finishing", "Finishing In (WIP Finishing)", "Saldo Awal WIP Subkon", "Subkon In", "Subkon Out", "Saldo Akhir WIP Subkon", "Finishing Out (WIP BJ)", "Finishing In Transfer", "Adjs Finishing", "Retur ke Sewing", "Saldo Akhir WIP Finishing",
				"Saldo Awal Barang jadi", "Barang Jadi In/ (WIP BJ)", "Finishing Transfer", "Penerimaan Lain-lain (Retur)", "Pengiriman Export", "Pengiriman Gudang Sisa", "Pengiriman Lain-lain/Sample", "Adjust Barang Jadi", "Saldo Akhir Barang Jadi"
				);
			}
			else
			{
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "RO", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "No Article", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Buyer", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Komoditi", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Jumlah Order", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FC", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HOURS", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "TARIF", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "HARGA BAHAN BAKU", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING513", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING614", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "CUTTING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "LOADING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "SEWING18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING19", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING20", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING21", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "FINISHING22", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI6", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI7", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI8", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI9", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI10", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI11", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI12", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI13", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI14", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI15", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI16", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI17", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "BARANG JADI18", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)2", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)3", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)4", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)5", DataType = typeof(string) });
				reportDataTable.Columns.Add(new DataColumn() { ColumnName = "Nilai Baru Komersil(SaldoAkhir)6", DataType = typeof(string) });

				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "",
									"Saldo Awal WIP Cutting","", "Cutting In (WIP Cutting)","", "Cutting Out / HP(WIP Loading)","", "Cutting Out Transfer","", "Cutting Out Subkon","", "Aval Komponen dari Cutting","", "Aval Komponen dari Sewing","", "Saldo Akhir WIP Cutting","",
									"Saldo Awal Loading","", "Loading In","", "Loading Out (WIP Sewing)	","", "Adjs Loading","", "Saldo Akhir Loading","",
									"Saldo Awal WIP Sewing","", "Sewing In (WIP Sewing)","", "Sewing Out (WIP Finishing)","", "Sewing In Transfer","", "Sewing Out Tranfer WIP Sewing","", "Sewing Out Transfer WIP Finishing","", "Retur ke Cutting","", "Adjs Sewing","", "Saldo Akhir WIP Sewing","",
									"Saldo Awal WIP Finishing","", "Finishing In (WIP Finishing)","", "Saldo Awal WIP Subkon","", "Subkon In","", "Subkon Out","", "Saldo Akhir WIP Subkon","", "Finishing Out (WIP BJ)","", "Finishing In Transfer","", "Adjs Finishing","", "Retur ke Sewing","", "Saldo Akhir WIP Finishing","",
									"Saldo Awal Barang jadi","", "Barang Jadi In/ (WIP BJ)","", "Finishing Transfer","", "Penerimaan Lain-lain (Retur)","", "Pengiriman Export","", "Pengiriman Gudang Sisa","", "Pengiriman Lain-lain/Sample","", "Adjust Barang Jadi","", "Saldo Akhir Barang Jadi","",
									"Tarif","Cutting","Loading","Sewing","Finishing","Barang Jadi"
									);
				reportDataTable.Rows.Add("", "", "", "", "", "", "", "", "",
					"Qty","Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga",
					"Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty", "Harga", "Qty","Harga",
					"","","","","",""
					);
			}
			int counter = 2;

			foreach (var report in garmentMonitoringProductionFlow.garmentMonitorings)
			{
				if (request.type != "bookkeeping")
				{
					reportDataTable.Rows.Add(report.Ro, report.Article, report.Comodity, report.QtyOrder,
					report.BeginingBalanceCuttingQty, report.QtyCuttingIn, report.QtyCuttingOut, report.QtyCuttingTransfer, report.QtyCuttingsubkon, report.AvalCutting, report.AvalSewing, report.EndBalancCuttingeQty,
					report.BeginingBalanceLoadingQty, report.QtyLoadingIn, report.QtyLoading, report.QtyLoadingAdjs, report.EndBalanceLoadingQty,
					report.BeginingBalanceSewingQty, report.QtySewingIn, report.QtySewingOut, report.QtySewingInTransfer, report.WipSewingOut, report.WipFinishingOut, report.QtySewingRetur, report.QtySewingAdj, report.EndBalanceSewingQty,
					report.BeginingBalanceFinishingQty, report.FinishingInQty, report.BeginingBalanceSubconQty, report.SubconInQty, report.SubconOutQty, report.EndBalanceSubconQty, report.FinishingOutQty, report.FinishingInTransferQty, report.FinishingAdjQty, report.FinishingReturQty, report.EndBalanceFinishingQty,
					report.BeginingBalanceExpenditureGood, report.FinishingInExpenditure, report.FinishingInTransferQty, report.ExpenditureGoodRetur, report.ExportQty, report.OtherQty, report.SampleQty, report.ExpenditureGoodAdj, report.EndBalanceExpenditureGood);
					counter++;
				}
				else
				{
					reportDataTable.Rows.Add(report.Ro, report.Article, report.BuyerCode, report.Comodity, report.QtyOrder, report.FC, report.Hours, report.Fare, report.BasicPrice,
						report.BeginingBalanceCuttingQty, report.BeginingBalanceCuttingPrice, report.QtyCuttingIn, report.PriceCuttingIn, report.QtyCuttingOut, report.PriceCuttingOut, report.QtyCuttingTransfer, report.PriceCuttingOut, report.QtyCuttingsubkon, report.PriceCuttingsubkon, report.AvalCutting, report.AvalCuttingPrice, report.AvalSewing, report.AvalSewingPrice, report.EndBalancCuttingeQty, report.EndBalancCuttingePrice,
						report.BeginingBalanceLoadingQty, report.BeginingBalanceLoadingPrice, report.QtyLoadingIn, report.PriceLoadingIn, report.QtyLoading, report.PriceLoading, report.QtyLoadingAdjs, report.PriceLoadingAdjs, report.EndBalanceLoadingQty, report.EndBalanceLoadingPrice,
						report.BeginingBalanceSewingQty, report.BeginingBalanceSewingPrice, report.QtySewingIn, report.PriceSewingIn, report.QtySewingOut, report.PriceSewingOut, report.QtySewingInTransfer, report.PriceSewingInTransfer, report.WipSewingOut, report.WipSewingOutPrice, report.WipFinishingOut, report.WipFinishingOutPrice, report.QtySewingRetur, report.PriceSewingRetur, report.QtySewingAdj, report.PriceSewingAdj, report.EndBalanceSewingQty, report.EndBalanceSewingPrice,
						report.BeginingBalanceFinishingQty, report.BeginingBalanceFinishingPrice, report.FinishingInQty, report.FinishingInPrice, report.BeginingBalanceSubconQty, report.BeginingBalanceSubconPrice, report.SubconInQty, report.SubconInPrice, report.SubconOutQty, report.SubconOutPrice, report.EndBalanceSubconQty, report.EndBalanceSubconPrice, report.FinishingOutQty, report.FinishingOutPrice, report.FinishingInTransferQty, report.FinishingInTransferPrice, report.FinishingAdjQty, report.FinishingAdjPRice, report.FinishingReturQty, report.FinishingReturPrice, report.EndBalanceFinishingQty, report.EndBalanceFinishingPrice,
						report.BeginingBalanceExpenditureGood, report.BeginingBalanceExpenditureGoodPrice, report.FinishingInExpenditure, report.FinishingInExpenditurepPrice, report.FinishingInTransferQty, report.FinishingInTransferPrice, report.ExpenditureGoodRetur, report.ExpenditureGoodReturPrice, report.ExportQty, report.ExportPrice, report.OtherQty, report.OtherPrice, report.SampleQty, report.SamplePrice, report.ExpenditureGoodAdj, report.ExpenditureGoodAdjPrice, report.EndBalanceExpenditureGood, report.EndBalanceExpenditureGoodPrice,
						report.FareNew, report.CuttingNew, report.LoadingNew, report.SewingNew, report.FinishingNew, report.ExpenditureNew
						);
					counter++;
				}
			}
			using (var package = new ExcelPackage())
			{
				var worksheet = package.Workbook.Worksheets.Add("Sheet 1");
				worksheet.Cells["A1"].LoadFromDataTable(reportDataTable, true);
				if (request.type != "bookkeeping")
				{
					worksheet.Cells["E" + 1 + ":L" + 1 + ""].Merge = true;
					worksheet.Cells["M" + 1 + ":Q" + 1 + ""].Merge = true;
					worksheet.Cells["R" + 1 + ":Z" + 1 + ""].Merge = true;
					worksheet.Cells["AA" + 1 + ":AK" + 1 + ""].Merge = true;
					worksheet.Cells["AL" + 1 + ":AT" + 1 + ""].Merge = true;
					worksheet.Cells["E" + 1 + ":L" + 1 + ""].Merge = true;
					worksheet.Cells["M" + 1 + ":Q" + 1 + ""].Merge = true;
					worksheet.Cells["R" + 1 + ":Z" + 1 + ""].Merge = true;
					worksheet.Cells["AA" + 1 + ":AK" + 1 + ""].Merge = true;
					worksheet.Cells["AL" + 1 + ":AT" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":AT" + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":AT" + 1 + ""].Style.Font.Bold = true;
					worksheet.Cells["E" + 3 + ":AT" + counter + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;
					//worksheet.Cells["E" + 3 + ":AT" + counter + ""].Style.Numberformat.Format = "#,##0.00";
				}
				else
				{
					worksheet.Cells["J" + 1 + ":Y" + 1 + ""].Merge = true;
					worksheet.Cells["Z" + 1 + ":AI" + 1 + ""].Merge = true;
					worksheet.Cells["AJ" + 1 + ":BA" + 1 + ""].Merge = true;
					worksheet.Cells["BB" + 1 + ":BW" + 1 + ""].Merge = true;
					worksheet.Cells["BX" + 1 + ":CO" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":A" + 3 + ""].Merge = true;
					worksheet.Cells["B" + 1 + ":B" + 3 + ""].Merge = true;
					worksheet.Cells["C" + 1 + ":C" + 3 + ""].Merge = true;
					worksheet.Cells["D" + 1 + ":D" + 3 + ""].Merge = true;
					worksheet.Cells["E" + 1 + ":E" + 3 + ""].Merge = true;
					worksheet.Cells["F" + 1 + ":F" + 3 + ""].Merge = true;
					worksheet.Cells["G" + 1 + ":G" + 3 + ""].Merge = true;
					worksheet.Cells["H" + 1 + ":H" + 3 + ""].Merge = true;
					worksheet.Cells["I" + 1 + ":I" + 3 + ""].Merge = true;
					worksheet.Cells["J" + 2 + ":K" + 2 + ""].Merge = true;
					worksheet.Cells["L" + 2 + ":M" + 2 + ""].Merge = true;
					worksheet.Cells["N" + 2 + ":O" + 2 + ""].Merge = true;
					worksheet.Cells["P" + 2 + ":Q" + 2 + ""].Merge = true;
					worksheet.Cells["R" + 2 + ":S" + 2 + ""].Merge = true;
					worksheet.Cells["T" + 2 + ":U" + 2 + ""].Merge = true;
					worksheet.Cells["V" + 2 + ":W" + 2 + ""].Merge = true;
					worksheet.Cells["X" + 2 + ":Y" + 2 + ""].Merge = true;
					worksheet.Cells["Z" + 2 + ":AA" + 2 + ""].Merge = true;
					worksheet.Cells["AB" + 2 + ":AC" + 2 + ""].Merge = true;
					worksheet.Cells["AD" + 2 + ":AE" + 2 + ""].Merge = true;
					worksheet.Cells["AF" + 2 + ":AG" + 2 + ""].Merge = true;
					worksheet.Cells["AH" + 2 + ":AI" + 2 + ""].Merge = true;
					worksheet.Cells["AJ" + 2 + ":AK" + 2 + ""].Merge = true;
					worksheet.Cells["AL" + 2 + ":AM" + 2 + ""].Merge = true;
					worksheet.Cells["AN" + 2 + ":AO" + 2 + ""].Merge = true;
					worksheet.Cells["AP" + 2 + ":AQ" + 2 + ""].Merge = true;
					worksheet.Cells["AR" + 2 + ":AS" + 2 + ""].Merge = true;
					worksheet.Cells["AT" + 2 + ":AU" + 2 + ""].Merge = true;
					worksheet.Cells["AV" + 2 + ":AW" + 2 + ""].Merge = true;
					worksheet.Cells["AX" + 2 + ":AY" + 2 + ""].Merge = true;
					worksheet.Cells["Az" + 2 + ":BA" + 2 + ""].Merge = true;
					worksheet.Cells["BB" + 2 + ":BC" + 2 + ""].Merge = true;
					worksheet.Cells["BD" + 2 + ":BE" + 2 + ""].Merge = true;
					worksheet.Cells["BF" + 2 + ":BG" + 2 + ""].Merge = true;
					worksheet.Cells["BH" + 2 + ":BI" + 2 + ""].Merge = true;
					worksheet.Cells["BJ" + 2 + ":BK" + 2 + ""].Merge = true;
					worksheet.Cells["BL" + 2 + ":BM" + 2 + ""].Merge = true;
					worksheet.Cells["BN" + 2 + ":BO" + 2 + ""].Merge = true;
					worksheet.Cells["BP" + 2 + ":BQ" + 2 + ""].Merge = true;
					worksheet.Cells["BR" + 2 + ":BS" + 2 + ""].Merge = true;
					worksheet.Cells["BT" + 2 + ":BU" + 2 + ""].Merge = true;
					worksheet.Cells["BV" + 2 + ":BW" + 2 + ""].Merge = true;
					worksheet.Cells["BX" + 2 + ":BY" + 2 + ""].Merge = true;
					worksheet.Cells["BZ" + 2 + ":CC" + 2 + ""].Merge = true;
					worksheet.Cells["CD" + 2 + ":CE" + 2 + ""].Merge = true;
					worksheet.Cells["CF" + 2 + ":CG" + 2 + ""].Merge = true;
					worksheet.Cells["CH" + 2 + ":CI" + 2 + ""].Merge = true;
					worksheet.Cells["CJ" + 2 + ":CK" + 2 + ""].Merge = true;
					worksheet.Cells["CL" + 2 + ":CM" + 2 + ""].Merge = true;
					worksheet.Cells["CN" + 2 + ":CO" + 2 + ""].Merge = true;
					worksheet.Cells["CP" + 1 + ":CU" + 1 + ""].Merge = true;
					worksheet.Cells["A" + 1 + ":CU" + 1 + ""].Style.Font.Bold = true;
					worksheet.Cells["A" + 1 + ":CU" + 1 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["A" + 1 + ":CU" + 1 + ""].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
					worksheet.Cells["E" + 3 + ":CU" + 3 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
					worksheet.Cells["E" + 4 + ":CU" + 4 + ""].Style.HorizontalAlignment = ExcelHorizontalAlignment.Right;

					//worksheet.Cells["E" + 4 + ":CU" + 4 + ""].Style.Numberformat.Format = "#,##0.00";
				}
				var stream = new MemoryStream();

				package.SaveAs(stream);

				return stream;
			}
		}
	}
}