using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands.Requests;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Model;
using Questao5.Application.Utils;

namespace Questao5.Infrastructure.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaCorrenteController : ControllerBase
    {
        private readonly ISender _mediator;

        public ContaCorrenteController(ISender mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Movimenta a conta corrente com o valor especificado.
        /// </summary>
        /// <param name="request">Dados da movimentação da conta corrente.</param>
        /// <returns>Id do movimento gerado.</returns>
        [HttpPost("MovimentarConta")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> MovimentarConta([FromBody] MovimentacaoContaRequest request)
        {
            var command = new MovimentacaoContaCommand
            {
                RequisicaoId = request.RequisicaoId,
                IdContaCorrente = request.IdContaCorrente,
                Valor = request.Valor,
                TipoMovimento = request.TipoMovimento
            };

            var result = await _mediator.Send(command);

            return result.Success ? Ok(new { IdMovimento = result.Data }) : BadRequest(new ErrorResponse(result.ErrorMessage, result.ErrorCode));
        }

        /// <summary>
        /// Consulta o saldo atual da conta corrente.
        /// </summary>
        /// <param name="request">Dados da consulta de saldo.</param>
        /// <returns>Saldo atual da conta corrente.</returns>
        [HttpPost("ConsultarSaldo")]
        [ProducesResponseType(typeof(SaldoContaCorrenteResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConsultarSaldo([FromBody] SaldoContaCorrenteRequest request)
        {
            var result = await _mediator.Send(request);

            return result.Success ? Ok(result.Data) : BadRequest(new ErrorResponse(result.ErrorMessage, result.ErrorCode));
        }
    }
}