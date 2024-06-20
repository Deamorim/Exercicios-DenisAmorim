namespace Questao5.Application.Model
{
    public class MovimentacaoContaRequest
    {
        public string RequisicaoId { get; set; }
        public string IdContaCorrente { get; set; }
        public decimal Valor { get; set; }
        public string TipoMovimento { get; set; } // 'C' para crédito, 'D' para débito
    }
}
