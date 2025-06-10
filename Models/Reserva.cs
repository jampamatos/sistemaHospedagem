using System;
using System.Collections.Generic;
namespace SistemaHospedagem.Models
{
    public class Reserva(Suite suite, DateTime dataCheckIn, DateTime dataCheckOut)
    {
        public Guid Id { get; } = Guid.NewGuid();
        public List<Hospede> Hospedes { get; } = [];
        public Suite Suite { get; set; } = suite ?? throw new ArgumentNullException(nameof(suite));
        public DateTime DataCheckIn { get; set; } = dataCheckIn;
        public DateTime DataCheckOut { get; set; } = dataCheckOut;

        private int DiasReservados => (DataCheckOut - DataCheckIn).Days;

        public void CadastrarSuite(Suite suite)
        {
            // Validações
            if (suite == null)
                throw new ArgumentNullException(nameof(suite), "Suite não pode ser nula.");

            if (!suite.EstaDisponivel())
                throw new InvalidOperationException("Suite não está disponível.");

            Suite = suite;
            Suite.MarcarComoIndisponivel();
        }

        public void CadastrarHospede(List<Hospede> hospedes)
        {
            // Validações
            if (hospedes == null || hospedes.Count == 0)
                throw new ArgumentException("Lista de hóspedes não pode ser nula ou vazia.", nameof(hospedes));

            if (Hospedes.Count + hospedes.Count > Suite.Capacidade)
                throw new InvalidOperationException($"Capacidade máxima da suíte é {Suite.Capacidade} hóspedes.");

            // Adiciona os hóspedes
            foreach (var hospede in hospedes)
            {
                Hospedes.Add(hospede);
            }
        }

        public int ObterQuantidadeHospedes()
        {
            return Hospedes.Count;
        }

        public decimal CalcularValorTotal()
        {
            if (DiasReservados <= 0)
                throw new InvalidOperationException("Período de reserva inválido.");

            return Suite.PrecoDiaria * DiasReservados;
        }
    }
}