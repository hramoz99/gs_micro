using Moq;
using MongoDB.Driver;
using web_app_domain;
using web_app_repository;
using Xunit;
using System.Threading.Tasks;

namespace web_app_tests
{
    public class ConsumoRepositoryTests
    {
        private readonly ConsumoRepository _consumoRepository;
        private readonly Mock<IMongoCollection<Consumo>> _mockCollection;

        public ConsumoRepositoryTests()
        {
            _mockCollection = new Mock<IMongoCollection<Consumo>>();

            var mongoClientMock = new Mock<IMongoClient>();
            var mongoDatabaseMock = new Mock<IMongoDatabase>();
            mongoDatabaseMock.Setup(db => db.GetCollection<Consumo>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(_mockCollection.Object);

            _consumoRepository = new ConsumoRepository(mongoClientMock.Object, mongoDatabaseMock.Object);
        }

        [Fact]
        public async Task SalvarConsumo_DeveInserirConsumoComSucesso()
        {
            var consumo = new Consumo
            {
                titulo = "Teste",
                quantidade_consumo = 10,
                data_registro_consumo = "2024-11-21"
            };

            await _consumoRepository.SalvarConsumo(consumo);

            _mockCollection.Verify(c => c.InsertOneAsync(consumo, It.IsAny<InsertOneOptions>(), default), Times.Once);
        }
    }
}
