using MediatR;
using Questao5.Application.Utils;
using Questao5.Infrastructure.Sqlite;

namespace Questao5.Application.Commands.Requests
{
    public class MovimentacaoContaCommand : IRequest<Result<Guid>>
    {
        public string RequisicaoId { get; set; }
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; }
    }
}
