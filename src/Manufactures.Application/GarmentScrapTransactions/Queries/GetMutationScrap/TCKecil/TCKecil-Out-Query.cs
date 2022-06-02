﻿using Infrastructure.Domain.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Application.GarmentScrapTransactions.Queries.GetMutationScrap.TCKecil
{
    public class TCKecil_Out_Query : IQuery<ScrapListViewModel>
    {
        public string token { get; private set; }
        public DateTime dateFrom { get; private set; }
        public DateTime dateTo { get; private set; }

        public TCKecil_Out_Query(DateTime dateFrom, DateTime dateTo, string token)
        {
            this.dateFrom = dateFrom;
            this.dateTo = dateTo;
            this.token = token;
        }
    }
}
