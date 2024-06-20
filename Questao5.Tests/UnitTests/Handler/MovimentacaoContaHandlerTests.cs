using NSubstitute;
using Questao5.Application.Handlers;
using Questao5.Infrastructure.Sqlite;
using Questao5.Tests.UnitTests.Fixture;

namespace Questao5.Tests.UnitTests.Handler
{
    [Trait("Handler", "MovimentacaoConta")]
    public class MovimentacaoContaHandlerTests : IClassFixture<MovimentacaoContaHandlerFixture>
    {
        private readonly IDatabaseBootstrap _databaseBootstrap;
        private readonly MovimentacaoContaHandler _handler;
        private readonly MovimentacaoContaHandlerFixture _movimentacaoContaHandlerFixture;

        public MovimentacaoContaHandlerTests(MovimentacaoContaHandlerFixture movimentacaoContaHandlerFixture)
        {
            _databaseBootstrap = Substitute.For<IDatabaseBootstrap>();
            _handler = new MovimentacaoContaHandler(_databaseBootstrap);
            _movimentacaoContaHandlerFixture = movimentacaoContaHandlerFixture;
        }

        [Fact(DisplayName = "Handle da conta corrente não encontrado")]
        public async Task Handle_ContaCorrenteNaoEncontrada_DeveRetornarFalha()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(false));

            // Act
            var command = _movimentacaoContaHandlerFixture.GerarContaCorrenteInvalida();
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Conta corrente não encontrada.", result.ErrorMessage);
            Assert.Equal("INVALID_ACCOUNT", result.ErrorCode);
        }

        [Fact(DisplayName = "Handle da conta corrente inativo")]
        public async Task Handle_ContaCorrenteInativa_DeveRetornarFalha()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(true));
            _databaseBootstrap.ContaCorrenteAtivaAsync(Arg.Any<string>()).Returns(Task.FromResult(false));

            // Act
            var command = _movimentacaoContaHandlerFixture.GerarContaCorrenteInativa();
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Conta corrente não está ativa.", result.ErrorMessage);
            Assert.Equal("INACTIVE_ACCOUNT", result.ErrorCode);
        }

        [Fact(DisplayName = "Handle da conta corrente com valor movimentação negativo")]
        public async Task Handle_ValorMovimentacaoNegativo_DeveRetornarFalha()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(true));
            _databaseBootstrap.ContaCorrenteAtivaAsync(Arg.Any<string>()).Returns(Task.FromResult(true));

            var command = _movimentacaoContaHandlerFixture.GerarContaCorrenteComValorNegativo();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Valor da movimentação deve ser positivo.", result.ErrorMessage);
            Assert.Equal("INVALID_VALUE", result.ErrorCode);
        }

        [Fact(DisplayName = "Handle da conta corrente com tipo movimentação inválido")]
        public async Task Handle_TipoMovimentoInvalido_DeveRetornarFalha()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(true));
            _databaseBootstrap.ContaCorrenteAtivaAsync(Arg.Any<string>()).Returns(Task.FromResult(true));

            var command = _movimentacaoContaHandlerFixture.GerarContaCorrenteComTipoMovimentacaoInvalido();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result.Success);
            Assert.Equal("Tipo de movimento inválido.", result.ErrorMessage);
            Assert.Equal("INVALID_TYPE", result.ErrorCode);
        }

        [Fact(DisplayName = "Handle da conta corrente movimentação com sucesso")]
        public async Task Handle_MovimentacaoBemSucedida_DeveRetornarSucesso()
        {
            // Arrange
            _databaseBootstrap.ContaCorrenteExisteAsync(Arg.Any<string>()).Returns(Task.FromResult(true));
            _databaseBootstrap.ContaCorrenteAtivaAsync(Arg.Any<string>()).Returns(Task.FromResult(true));
            _databaseBootstrap.InserirMovimentoAsync(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<decimal>())
                .Returns(Task.FromResult(Guid.NewGuid())); 

            var command = _movimentacaoContaHandlerFixture.GerarContaCorrenteValida();

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.Success);
            Assert.NotEqual(Guid.Empty, result.Data); 
        }
    }
}