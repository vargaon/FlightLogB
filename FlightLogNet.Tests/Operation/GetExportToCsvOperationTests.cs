namespace FlightLogNet.Tests.Operation
{
    using System.IO;
    using System.Text;
    using FlightLogNet.Operation;
    using Microsoft.Extensions.Configuration;
    using Xunit;

    public class GetExportToCsvOperationTests(GetExportToCsvOperation getExportToCsvOperation, IConfiguration configuration)
    {
        private void RenewDatabase()
        {
            TestDatabaseGenerator.RenewDatabase(configuration);
        }
        [Fact]
        public void Execute_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            this.RenewDatabase();
            var expectedCsv = File.ReadAllBytes("export.csv");
            // Act
            var resultCsv = getExportToCsvOperation.Execute();
            // Assert
            Assert.Equal(
                Encoding.UTF8.GetString(expectedCsv),
                Encoding.UTF8.GetString(resultCsv)
                );
        }
    }
}
