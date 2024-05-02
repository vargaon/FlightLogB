﻿namespace FlightLogNet
{
    using Facades;
    using Integration;
    using Microsoft.Extensions.DependencyInjection;
    using Operation;
    using Repositories;
    using Repositories.Interfaces;

    internal static class InjectConfiguration
    {
        internal static void Initialization(IServiceCollection services, bool isInTestEnvironment = false)
        {
            services.AddScoped<IAirplaneRepository, AirplaneRepository>();
            services.AddScoped<IFlightRepository, FlightRepository>();
            services.AddScoped<IPersonRepository, PersonRepository>();

            services.AddScoped<TakeoffOperation, TakeoffOperation>();
            services.AddScoped<GetExportToCsvOperation, GetExportToCsvOperation>();
            services.AddScoped<LandOperation, LandOperation>();
            services.AddScoped<CreatePersonOperation, CreatePersonOperation>();

            services.AddScoped<AirplaneFacade, AirplaneFacade>();
            services.AddScoped<PersonFacade, PersonFacade>();
            services.AddScoped<FlightFacade, FlightFacade>();

            if (isInTestEnvironment)
                services.AddScoped<IClubUserDatabase, ClubUserDatabaseStub>();
            else
                services.AddScoped<IClubUserDatabase, ClubUserDatabase>();
        }
    }
}
