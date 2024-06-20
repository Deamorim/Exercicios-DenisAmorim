using Questao5.Application.Queries.Responses;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {
        void Setup();
        Task<bool> ContaCorrenteExisteAsync(string idContaCorrente);
        Task<bool> ContaCorrenteAtivaAsync(string idContaCorrente);
        Task<Guid> InserirMovimentoAsync(string idContaCorrente, string tipoMovimento, decimal valor);
        Task<SaldoContaCorrente> ObterSaldoContaCorrenteAsync(string idContaCorrente);
    }
}