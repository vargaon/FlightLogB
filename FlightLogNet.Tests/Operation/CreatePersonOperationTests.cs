namespace FlightLogNet.Tests.Operation
{
    using FlightLogNet.Operation;
    using FlightLogNet.Repositories.Interfaces;
    using Integration;
    using Models;
    using Moq;
    using Xunit;

    public class CreatePersonOperationTests
    {
        private readonly MockRepository mockRepository;

        private readonly Mock<IPersonRepository> mockPersonRepository;
        private readonly Mock<IClubUserDatabase> mockClubUserDatabase;

        public CreatePersonOperationTests()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockPersonRepository = this.mockRepository.Create<IPersonRepository>();
            this.mockClubUserDatabase = this.mockRepository.Create<IClubUserDatabase>();
        }

        private CreatePersonOperation CreateCreatePersonOperation()
        {
            return new CreatePersonOperation(
                this.mockPersonRepository.Object,
                this.mockClubUserDatabase.Object);
        }

        [Fact]
        public void Execute_ShouldReturnNull()
        {
            // Arrange
            var createPersonOperation = this.CreateCreatePersonOperation();

            // Act
            var result = createPersonOperation.Execute(null);

            // Assert
            Assert.Null(result);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void Execute_ShouldCreateGuest()
        {
            // Arrange
            var createPersonOperation = this.CreateCreatePersonOperation();
            PersonModel personModel = new PersonModel
            {
                Address = new AddressModel { City = "NY", PostalCode = "456", Street = "2nd Ev", Country = "USA" },
                FirstName = "John",
                LastName = "Smith"
            };
            this.mockPersonRepository.Setup(repository => repository.AddGuestPerson(personModel)).Returns(10);

            // Act
            var result = createPersonOperation.Execute(personModel);

            // Assert
            Assert.True(result > 0);
            this.mockRepository.VerifyAll();
        }

        [Fact]
        public void Execute_ShouldCreateNewClubMember()
        {
            // Arrange
            var createPersonOperation = CreateCreatePersonOperation();
            PersonModel clubUser = new PersonModel
            {
                FirstName = "Jan",
                LastName = "Novák",
                MemberId = 333
            };

            long id = clubUser.MemberId;

            // When it tries to find out if the person is in the reposiory already, it should not be there
            mockPersonRepository.Setup(repository => repository.TryGetPerson(clubUser, out id)).Returns(false);
            // When it tries to find out if the person is in the club user database, it should be there
            mockClubUserDatabase.Setup(db => db.TryGetClubUser(id, out clubUser)).Returns(true);
            // When it then adds the person to the repository, it should return the expected id
            mockPersonRepository.Setup(repository => repository.CreateClubMember(clubUser)).Returns(id);

            // Act
            var result = createPersonOperation.Execute(clubUser);
            // Assert
            Assert.Equal(id, result);
            mockRepository.VerifyAll();
        }
    }
}
