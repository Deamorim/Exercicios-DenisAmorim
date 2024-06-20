using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Sqlite;
using Questao5.Tests.UnitTests.Fixture;

namespace Questao5.Tests.UnitTests.Handler
{
    [Trait("Handler", "ConsultaSaldoConta")]
    public class ConsultaSaldoContaHandlerTests : IClassFixture<ConsultaSaldoContaHandlerFixture>
    {
        private readonly IDatabaseBootstrap _databaseBootstrap;
        private readonly ConsultaSaldoContaHandler _handler;
        private readonly ConsultaSaldoContaHandlerFixture _consultaSaldoContaHandlerFixture;

        public ConsultaSaldoContaHandlerTests(ConsultaSaldoContaHandlerFixture consultaSaldoContaHandlerFixture)
        {
            _databaseBootstrap = Substitute.For<IDatabaseBootstrap>();
            _handler = new ConsultaSaldoContaHandler(_databaseBootstrap);
            _consultaSaldoContaHandlerFixture = consultaSaldoContaHandlerFixture;
        }

        [Fact(DisplayName = "Handle consulta saldo de conta corrente não encontrado")]
        public async Task Handle_ConsultaSaldo_ContaCorrenteNaoEncontrada_DeveRetornarFalha()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(false));

            // Act
            var request = _consultaSaldoContaHandlerFixture.GerarSaldoContaInvalida();
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Conta corrente não encontrada.", result.ErrorMessage);
            Assert.Equal("INVALID_ACCOUNT", result.ErrorCode);
        }

        [Fact(DisplayName = "Handle consulta saldo de conta corrente inativa")]
        public async Task Handle_ConsultaSaldo_ContaCorrenteInativa_DeveRetornarFalha()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(true));
            _databaseBootstrap.ContaCorrenteAtivaAsync(Arg.Any<string>()).Returns(Task.FromResult(false));

            // Act
            var request = _consultaSaldoContaHandlerFixture.GerarSaldoContaInativa();
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Conta corrente não está ativa.", result.ErrorMessage);
            Assert.Equal("INACTIVE_ACCOUNT", result.ErrorCode);
        }

        [Fact(DisplayName = "Handle consulta saldo de conta corrente com sucesso")]
        public async Task Handle_ConsultaSaldo_ContaCorrenteValida_DeveRetornarSucesso()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(true));
            _databaseBootstrap.ContaCorrenteAtivaAsync(Arg.Any<string>()).Returns(Task.FromResult(true));

            var saldo = _consultaSaldoContaHandlerFixture.GerarSaldoContaValida();

            _databaseBootstrap.ObterSaldoContaCorrenteAsync(Arg.Any<string>()).Returns(Task.FromResult(saldo));

            // Act
            var request = _consultaSaldoContaHandlerFixture.GerarSaldoContaCorrenteRequestValida();
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotNull(result.Data);
            Assert.Equal(saldo.NumeroContaCorrente, result.Data.NumeroContaCorrente);
            Assert.Equal(saldo.NomeTitular, result.Data.NomeTitular);
            Assert.Equal(saldo.SaldoAtual, result.Data.SaldoAtual);
        }
    }
}
