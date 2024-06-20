using MediatR;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Utils;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Handlers
{
    public class MovimentacaoContaHandler : IRequestHandler<MovimentacaoContaCommand, Result<Guid>>
    {
        private readonly IDatabaseBootstrap _databaseBootstrap;

        public MovimentacaoContaHandler(IDatabaseBootstrap databaseBootstrap)
        {
            _databaseBootstrap = databaseBootstrap;
        }

        public async Task<Result<Guid>> Handle(MovimentacaoContaCommand request, CancellationToken cancellationToken)
        {
            if (!await _databaseBootstrap.ContaCorrenteExisteAsync(request.IdContaCorrente))
                return Result<Guid>.Fail("Conta corrente não encontrada.", "INVALID_ACCOUNT");

            if (!await _databaseBootstrap.ContaCorrenteAtivaAsync(request.IdContaCorrente))
                return Result<Guid>.Fail("Conta corrente não está ativa.", "INACTIVE_ACCOUNT");

            if (request.Valor <= 0)
                return Result<Guid>.Fail("Valor da movimentação deve ser positivo.", "INVALID_VALUE");

            if (request.TipoMovimento != "C" && request.TipoMovimento != "D")
                return Result<Guid>.Fail("Tipo de movimento inválido.", "INVALID_TYPE");

            var idMovimento = await _databaseBootstrap.InserirMovimentoAsync(request.IdContaCorrente, request.TipoMovimento, request.Valor);

            return Result<Guid>.Successfully(idMovimento);
        }
    }
}
