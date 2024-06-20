using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Utils;

namespace Questao5.Application.Commands.Requests
{
    public class SaldoContaCorrenteRequest : IRequest<Result<SaldoContaCorrenteResponse>>
    {
        public string IdContaCorrente { get; set; }
    }
}
