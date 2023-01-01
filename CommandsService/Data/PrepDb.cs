﻿using CommandsService.Models;
using CommandsService.SyncDataServices.Grpc;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace CommandsService.Data
{
    public class PrepDb
    {
        public static void PrepPopulation(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var grpcClient = serviceScope.ServiceProvider.GetService<IPlatformDataClient>();
                var platforms = grpcClient.ReturnAllPlatforms();

                SeedData(serviceScope.ServiceProvider.GetService<ICommandRepo>(), platforms);
            }
        }

        private static void SeedData(ICommandRepo repository, IEnumerable<Platform> platforms)
        {
            Console.WriteLine("Seeding new platforms...");
            foreach (var platform in platforms)
            {
                if (!repository.ExternalPlatformExists(platform.ExternalID))
                {
                    repository.CreatePlatform(platform);
                }
                repository.SaveChanges();
            }
        }
    }
}
