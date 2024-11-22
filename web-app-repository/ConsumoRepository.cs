using MongoDB.Driver;
using web_app_domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace web_app_repository
{
    public class ConsumoRepository : IConsumoRepository
    {
        private readonly IMongoCollection<Consumo> _consumosCollection;

        // Construtor para produção
        public ConsumoRepository()
        {
            string connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("global3sir");
            _consumosCollection = database.GetCollection<Consumo>("consumos");
        }

        // Construtor para injeção de dependências, útil para testes
        public ConsumoRepository(IMongoClient mongoClient, IMongoDatabase mongoDatabase)
        {
            _consumosCollection = mongoDatabase.GetCollection<Consumo>("consumos");
        }

        public async Task<IEnumerable<Consumo>> ListarConsumos()
        {
            var consumos = await _consumosCollection.Find(_ => true).ToListAsync();
            return consumos;
        }

        public async Task SalvarConsumo(Consumo consumo)
        {
            await _consumosCollection.InsertOneAsync(consumo);
        }

        public async Task AtualizarConsumo(Consumo consumo)
        {
            var filter = Builders<Consumo>.Filter.Eq(c => c.Id, consumo.Id);
            var update = Builders<Consumo>.Update
                .Set(c => c.titulo, consumo.titulo)
                .Set(c => c.quantidade_consumo, consumo.quantidade_consumo)
                .Set(c => c.data_registro_consumo, consumo.data_registro_consumo);

            await _consumosCollection.UpdateOneAsync(filter, update);
        }

        public async Task RemoverConsumo(int id)
        {
            var filter = Builders<Consumo>.Filter.Eq(c => c.Id, id);
            await _consumosCollection.DeleteOneAsync(filter);
        }
    }
}
