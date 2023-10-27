﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ExtCore.Data.Abstractions;
using Infrastructure.Domain.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Commands;
using Manufactures.Domain.GarmentSubcon.ServiceSubconSewings.Repositories;
using Manufactures.Domain.LogHistory;
using Manufactures.Domain.LogHistory.Repositories;

namespace Manufactures.Application.GarmentSubcon.GarmentServiceSubconSewings.CommandHandlers
{
    public class RemoveGarmentServiceSubconSewingCommandHandler : ICommandHandler<RemoveGarmentServiceSubconSewingCommand, GarmentServiceSubconSewing>
    {
        private readonly IStorage _storage;
        private readonly IGarmentServiceSubconSewingRepository _garmentServiceSubconSewingRepository;
        private readonly IGarmentServiceSubconSewingItemRepository _garmentServiceSubconSewingItemRepository;
        private readonly IGarmentServiceSubconSewingDetailRepository _garmentServiceSubconSewingDetailRepository;
        private readonly ILogHistoryRepository _logHistoryRepository;
        public RemoveGarmentServiceSubconSewingCommandHandler(IStorage storage)
        {
            _storage = storage;
            _garmentServiceSubconSewingRepository = storage.GetRepository<IGarmentServiceSubconSewingRepository>();
            _garmentServiceSubconSewingItemRepository = storage.GetRepository<IGarmentServiceSubconSewingItemRepository>();
            _garmentServiceSubconSewingDetailRepository = storage.GetRepository<IGarmentServiceSubconSewingDetailRepository>();
            _logHistoryRepository = storage.GetRepository<ILogHistoryRepository>();
        }

        public async Task<GarmentServiceSubconSewing> Handle(RemoveGarmentServiceSubconSewingCommand request, CancellationToken cancellationToken)
        {
            var serviceSubconSewing = _garmentServiceSubconSewingRepository.Query.Where(o => o.Identity == request.Identity).Select(o => new GarmentServiceSubconSewing(o)).Single();

            Dictionary<Guid, double> sewInItemToBeUpdated = new Dictionary<Guid, double>();

            _garmentServiceSubconSewingItemRepository.Find(o => o.ServiceSubconSewingId == serviceSubconSewing.Identity).ForEach(async serviceSubconSewingItem =>
            {
                //if (!serviceSubconSewing.IsDifferentSize)
                //{
                //    if (sewInItemToBeUpdated.ContainsKey(serviceSubconSewingItem.SewingInItemId))
                //    {
                //        sewInItemToBeUpdated[serviceSubconSewingItem.SewingInItemId] += serviceSubconSewingItem.Quantity;
                //    }
                //    else
                //    {
                //        sewInItemToBeUpdated.Add(serviceSubconSewingItem.SewingInItemId, serviceSubconSewingItem.Quantity);
                //    }
                //}
                _garmentServiceSubconSewingDetailRepository.Find(i => i.ServiceSubconSewingItemId == serviceSubconSewingItem.Identity).ForEach(async subconDetail =>
                {
                    subconDetail.Remove();
                    await _garmentServiceSubconSewingDetailRepository.Update(subconDetail);
                });
                serviceSubconSewingItem.Remove();
                await _garmentServiceSubconSewingItemRepository.Update(serviceSubconSewingItem);
            });

            serviceSubconSewing.Remove();
            await _garmentServiceSubconSewingRepository.Update(serviceSubconSewing);

            //Add Log History
            LogHistory logHistory = new LogHistory(new Guid(), "PRODUKSI", "Delete Packing List Subcon - Jasa Garment Wash - " + serviceSubconSewing.ServiceSubconSewingNo, DateTime.Now);
            await _logHistoryRepository.Update(logHistory);

            _storage.Save();

            return serviceSubconSewing;
        }
    }
}
