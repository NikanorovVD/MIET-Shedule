﻿using DataLayer;
using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ServiceLayer.Configuration;


namespace ServiceLayer.Services
{
    public class DatabaseInitService : BackgroundService
    {
        private readonly AppDbContext _dbContext;
        private readonly SheduleSettings _sheduleSettings;
        public DatabaseInitService(IServiceScopeFactory serviceScopeFactory)
        {
            var serviceProvider = serviceScopeFactory.CreateScope().ServiceProvider;
            _dbContext = serviceProvider.GetRequiredService<AppDbContext>();
            _sheduleSettings = serviceProvider.GetRequiredService<IOptions<SheduleSettings>>().Value;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _dbContext.Database.MigrateAsync(cancellationToken: stoppingToken);
            await SeedDataAsync(stoppingToken);
        }

        private async Task SeedDataAsync(CancellationToken cancellationToken)
        {
            if (!_dbContext.TimePairs.Any())
            {
                IEnumerable<TimePair> timePairs = _sheduleSettings.Times.Select(s => new TimePair()
                {
                    Id = s.Order,
                    Start = s.Start,
                    End = s.End,
                });
                await _dbContext.AddRangeAsync(timePairs, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
