using Questao5.Application.Commands.Requests;

namespace Questao5.Tests.UnitTests.Fixture
{
    public class MovimentacaoContaHandlerFixture : IDisposable
    {
        public MovimentacaoContaCommand GerarContaCorrenteValida()
            => new MovimentacaoContaCommand 
            { 
                RequisicaoId = "1", 
                IdContaCorrente = "FA99D033-7067-ED11-96C6-7C5DFA4A16C9", 
                TipoMovimento = "D", 
                Valor = 10 
            };

        public MovimentacaoContaCommand GerarContaCorrenteInvalida()
           => new MovimentacaoContaCommand
           {
               RequisicaoId = "1",
               IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C4",
               TipoMovimento = "D",
               Valor = 10
           };

        public MovimentacaoContaCommand GerarContaCorrenteComValorNegativo()
          => new MovimentacaoContaCommand
          {
              RequisicaoId = "1",
              IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
              TipoMovimento = "D",
              Valor = -10
          };

        public MovimentacaoContaCommand GerarContaCorrenteComTipoMovimentacaoInvalido()
         => new MovimentacaoContaCommand
         {
             RequisicaoId = "1",
             IdContaCorrente = "B6BAFC09-6967-ED11-A567-055DFA4A16C9",
             TipoMovimento = "F",
             Valor = 10
         };

        public MovimentacaoContaCommand GerarContaCorrenteInativa()
         => new MovimentacaoContaCommand
         {
             RequisicaoId = "1",
             IdContaCorrente = "F475F943-7067-ED11-A06B-7E5DFA4A16C9",
             TipoMovimento = "F",
             Valor = 10
         };

        public void Dispose()
        {
        }
    }
}
