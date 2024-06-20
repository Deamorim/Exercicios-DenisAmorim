using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Utils;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers
{
    public class ConsultaSaldoContaHandler : IRequestHandler<SaldoContaCorrenteRequest, Result<SaldoContaCorrenteResponse>>
    {
        private readonly IDatabaseBootstrap _databaseBootstrap;

        public ConsultaSaldoContaHandler(IDatabaseBootstrap databaseBootstrap)
        {
            _databaseBootstrap = databaseBootstrap;
        }

        public async Task<Result<SaldoContaCorrenteResponse>> Handle(SaldoContaCorrenteRequest request, CancellationToken cancellationToken)
        {
            if (!await _databaseBootstrap.ContaCorrenteExisteAsync(request.IdContaCorrente))
                return Result<SaldoContaCorrenteResponse>.Fail("Conta corrente não encontrada.", "INVALID_ACCOUNT");

            if (!await _databaseBootstrap.ContaCorrenteAtivaAsync(request.IdContaCorrente))
                return Result<SaldoContaCorrenteResponse>.Fail("Conta corrente não está ativa.", "INACTIVE_ACCOUNT");

            var saldo = await _databaseBootstrap.ObterSaldoContaCorrenteAsync(request.IdContaCorrente);

            var response = new SaldoContaCorrenteResponse
            {
                NumeroContaCorrente = saldo.NumeroContaCorrente,
                NomeTitular = saldo.NomeTitular,
                DataHoraConsulta = DateTime.UtcNow,
                SaldoAtual = saldo.SaldoAtual
            };

            return Result<SaldoContaCorrenteResponse>.Successfully(response);
        }
    }
}
