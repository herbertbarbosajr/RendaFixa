namespace RendaFixa.Infrastructure.Events
{
    public record CompraRealizadaEvent(int ProdutoId, int Quantidade, decimal Total, DateTime Data);

}
