using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using layoutlovers.MultiTenancy.HostDashboard.Dto;

namespace layoutlovers.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}