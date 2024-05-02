namespace FlightLogNet.Operation
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using AutoMapper;
    using CsvHelper;
    using CsvHelper.Configuration;
    using CsvHelper.Configuration.Attributes;
    using CsvHelper.TypeConversion;
    using FlightLogNet.Models;
    using Repositories.Interfaces;

    public class GetExportToCsvOperation(IFlightRepository flightRepository, IMapper mapper)
    {
        internal record CsvExportFlightModel
        {
            [Index(0), Name("Datum"), Format("dd. MM. yyyy")]
            public DateTime? Date { get; set; }
            [Index(1), Name("Typ")]
            public string AirplaneType { get; set; }
            [Index(2), Name("Typ")]
            public string AirplaneImmatriculation { get; set; }
            [Index(3), Name("Osádka")]
            public string PilotLastName { get; set; }
            [Index(4), Name("Úkol")]
            public string Task { get; set; }
            [Index(5), Name("Start")]
            public TimeSpan TakeoffTime { get; set; }
            [Index(6), Name("Přístání")]
            public TimeSpan? LandingTime { get; set; }
            [Index(7), Name("Doba letu")]
            public TimeSpan? FlightSpan { get; set; }
        }
        private sealed class TimeSpanConverter : DefaultTypeConverter
        {
            public override string ConvertToString(object value, IWriterRow row, MemberMapData memberMapData)
            {
                if (value == null)
                    return "";
                var timeSpan = (TimeSpan)value;
                return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
        }
        public byte[] Execute()
        {
            // flatten the list and leave out empty items
            var flightModels = flightRepository
                .GetReport()
                .SelectMany<ReportModel, FlightModel>(item => [item.Towplane, item.Glider])
                .Where(item => item != null);

            var rows = mapper.Map<CsvExportFlightModel[]>(flightModels);

            using var stream = new MemoryStream();
            using (var streamWriter = new StreamWriter(stream, Encoding.UTF8))
            {
                using var writer = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);
                writer.Context.TypeConverterCache.AddConverter<TimeSpan>(new TimeSpanConverter());
                writer.WriteRecords(rows);
            }
            return stream.ToArray();
        }
    }
}
