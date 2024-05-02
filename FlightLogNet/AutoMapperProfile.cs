namespace FlightLogNet
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;
    using FlightLogNet.Operation;
    using Integration;
    using Models;
    using Repositories.Entities;

    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            this.CreateMap<Address, AddressModel>();
            this.CreateMapAirplanes();

            this.CreateMap<Flight, FlightModel>();
            this.CreateMap<FlightStart, ReportModel>();
            this.CreateMap<Person, PersonModel>();
            this.CreateMap<ClubUser, PersonModel>()
                .ForMember(dest => dest.Address, opt => opt.Ignore());
            this.CreateMap<FlightModel, GetExportToCsvOperation.CsvExportFlightModel>()
                .ForMember(dest => dest.FlightSpan, opt => opt.MapFrom(flightModel => flightModel.LandingTime != null
                    ? flightModel.LandingTime - flightModel.TakeoffTime
                    : null
                ))
                .ForMember(dest => dest.TakeoffTime, opt => opt.MapFrom(flightModel => flightModel.TakeoffTime.TimeOfDay))
                .ForMember(dest => dest.LandingTime, opt => opt.MapFrom<TimeSpan?>(flightModel => flightModel.LandingTime != null
                    ? flightModel.LandingTime.Value.TimeOfDay
                    : null
                    ))
                .ForMember(dest => dest.Date, opt => opt.MapFrom<DateTime?>(flightModel => flightModel.Task == "VLEK" ? null : flightModel.TakeoffTime.Date));
        }

        private void CreateMapAirplanes()
        {
            Expression<Func<Airplane, string>> immatriculationMap = airplane => airplane.ClubAirplane != null
                ? airplane.ClubAirplane.Immatriculation
                : airplane.GuestAirplaneImmatriculation;

            Expression<Func<Airplane, string>> typeMap = airplane => airplane.ClubAirplane != null
                ? airplane.ClubAirplane.AirplaneType.Type
                : airplane.GuestAirplaneType;

            this.CreateMap<Airplane, AirplaneModel>()
                .ForMember(dest => dest.Immatriculation, opt => opt.MapFrom(immatriculationMap))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(typeMap));

            this.CreateMap<ClubAirplane, AirplaneModel>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(airplane => airplane.AirplaneType.Type));
        }
    }
}
