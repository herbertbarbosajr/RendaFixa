namespace RendaFixa.Domain.Entities
{
    public class ContaInvestidor
    {
        public ContaInvestidor(decimal saldo)
        {
            Saldo = saldo;
        }

        public int Id { get; set; }
        public decimal Saldo { get; set; }

        public void DebitarSaldo(decimal valor)
        {
            if (valor > Saldo)
                throw new InvalidOperationException("Saldo insuficiente.");

            Saldo -= valor;
        }
    }
}
