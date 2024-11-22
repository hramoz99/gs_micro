using web_app_domain;

namespace web_app_repository
{
    public interface IConsumoRepository
    {
        Task<IEnumerable<Consumo>> ListarConsumos();
        Task SalvarConsumo(Consumo usuario);
        Task AtualizarConsumo(Consumo usuario);
        Task RemoverConsumo(int id);
    }
}