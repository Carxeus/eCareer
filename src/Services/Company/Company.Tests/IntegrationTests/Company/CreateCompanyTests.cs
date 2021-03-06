using System;
using System.Threading;
using System.Threading.Tasks;
using Bogus;
using Career.Domain.BusinessRule;
using Career.Repositories;
using Company.Application.Company.Commands.CreateCompany;
using Company.Domain.Repositories;
using Company.Domain.Rules.Company;
using Company.Tests.Helpers;
using Microsoft.Extensions.Logging;
using NSubstitute;
using Xunit;

namespace Company.Tests.IntegrationTests.Company
{
    public class CreateCompanyTests
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICompanyRepository _companyRepository;
        private readonly ILogger<CreateCompanyHandler> _logger;

        public CreateCompanyTests()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _companyRepository = Substitute.For<ICompanyRepository>();
            _logger = Substitute.For<ILogger<CreateCompanyHandler>>();
        }

        [Fact]
        public async Task CreateCompanyHandler_ShouldReturnCreatedCompanyId_WhenCompanyCreated()
        {
            // Arrange
            var command = GetCommand();
            var commandHandler = new CreateCompanyHandler(_unitOfWork, _companyRepository, _logger);

            // Act
            Guid companyId = await commandHandler.Handle(command, CancellationToken.None);
            
            // Assert
            Assert.NotEqual(Guid.Empty, companyId);
            await _unitOfWork.Received().SaveChangesAsync();
        }
        
        [Fact]
        public async Task CreateCompanyHandler_ShouldBeLogInformation_WhenCompanyCreated()
        {
            // Arrange
            var command = GetCommand();
            var commandHandler = new CreateCompanyHandler(_unitOfWork, _companyRepository, _logger);

            // Act
            await commandHandler.Handle(command, CancellationToken.None);
            
            // Assert
            _logger.ReceivedWithAnyArgs().LogInformation(string.Empty);
        }
        
        [Fact]
        public async Task CreateCompanyHandler_ThrowBusinessRuleValidationException_For_TaxNumberMustBeUniqueRule_WhenTaxNumberExists()
        {
            // Arrange
            var command = GetCommand();
            var commandHandler = new CreateCompanyHandler(_unitOfWork, _companyRepository, _logger);
            
            _companyRepository.IsTaxNumberExistsAsync(command.TaxNumber, command.CountryId).Returns(true);

            // Act
            var actualException = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => commandHandler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal(typeof(TaxNumberMustBeUniqueRule), actualException.BrokenRule.GetType());
        }
        
        [Fact]
        public async Task CreateCompanyHandler_ThrowBusinessRuleValidationException_For_EmailAddressMustBeUniqueRule_WhenEmailAlreadyRegistered()
        {
            // Arrange
            var command = GetCommand();
            var commandHandler = new CreateCompanyHandler(_unitOfWork, _companyRepository, _logger);
            
            _companyRepository.IsCompanyEmailExists(command.Email).Returns(true);

            // Act
            var actualException = await Assert.ThrowsAsync<BusinessRuleValidationException>(() => commandHandler.Handle(command, CancellationToken.None));

            // Assert
            Assert.Equal(typeof(EmailAddressMustBeUniqueRule), actualException.BrokenRule.GetType());
        }
        
        private CreateCompanyCommand GetCommand()
        {
            var commandFaker = new Faker<CreateCompanyCommand>()
                .Rules((faker,command) =>
                {
                    command.CountryId = faker.Random.Guid().ToString();
                    command.CityId = faker.Random.Guid().ToString();
                    command.SectorId = faker.Random.Guid().ToString();
                    command.Name = faker.Company.CompanyName();
                    command.Address = faker.Address.FullAddress();
                    command.Email = faker.Internet.Email();
                    command.Phone = faker.Phone.PhoneNumber();
                    command.TaxNumber = faker.Company.TaxNumber();
                    command.TaxOffice = faker.Address.City();
                });

            return commandFaker.Generate();
        }
    }
}