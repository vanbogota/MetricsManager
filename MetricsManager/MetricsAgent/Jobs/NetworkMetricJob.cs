﻿using MetricsAgent.Interfaces;
using MetricsAgent.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace MetricsAgent.Jobs
{
    public class NetworkMetricJob : IJob
    {
        private readonly INetworkMetricsRepository _repository;
        private readonly PerformanceCounter _hddCounter;
        public NetworkMetricJob(INetworkMetricsRepository repository)
        {
            _repository = repository;
            _hddCounter = new PerformanceCounter("NetworkAdapter", "Bytes Total/sec");
        }

        public Task Execute(IJobExecutionContext context)
        {
            var networkSpeed = Convert.ToInt32(_hddCounter.NextValue());
            var time = TimeSpan.FromSeconds(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
            _repository.Create(new NetworkMetric { Time = time, Value = networkSpeed });
            return Task.CompletedTask;
        }
    }
}
