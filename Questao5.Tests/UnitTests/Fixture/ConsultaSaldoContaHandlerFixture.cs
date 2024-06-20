using Questao5.Application.Commands.Requests;
using Questao5.Application.Queries.Responses;

namespace Questao5.Tests.UnitTests.Fixture
{
    public class ConsultaSaldoContaHandlerFixture : IDisposable
    {
        public SaldoContaCorrente GerarSaldoContaValida()
            => new SaldoContaCorrente
            {
                NumeroContaCorrente = "321",
                NomeTitular = "Fulano de Tal",
                SaldoAtual = 10
            };

        public SaldoContaCorrenteRequest GerarSaldoContaCorrenteRequestValida()
            => new SaldoContaCorrenteRequest
            {
                IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9"
            };

        public SaldoContaCorrenteRequest GerarSaldoContaInvalida()
            => new SaldoContaCorrenteRequest
            {
                IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C1"
            };

        public SaldoContaCorrenteRequest GerarSaldoContaInativa()
            => new SaldoContaCorrenteRequest
            {
                IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9"
            };

        

        public void Dispose()
        {
        }
    }
}
