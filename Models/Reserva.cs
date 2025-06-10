using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
namespace SistemaHospedagem.Models
{
    public class Reserva
    {
        private static int _contador = 0;
        public Guid Id { get; } = Guid.NewGuid();

        public Suite Suite { get; private set; }
        public List<Hospede> Hospedes { get; } = [];

        public DateTime DataCheckIn { get; private set; }
        public DateTime DataCheckOut { get; private set; }

        private static readonly CultureInfo PtBr = new("pt-BR");
        private static readonly string[] DateFormats = new[]
        {
            // com 24h
            "dd/MM/yyyy HH:mm",
            "dd/MM/yyyy H:mm",
            // com 12h
            "dd/MM/yyyy hh:mm tt",
            "dd/MM/yyyy h:mm tt",
            // só data
        };

        public Reserva(Suite suite, string checkIn, string checkOut)
        {
            if (suite == null) throw new ArgumentNullException(nameof(suite), "Suite não pode ser nula.");
            if (!suite.EstaDisponivel()) throw new InvalidOperationException("Suite não está disponível.");
            Suite = suite;
            Suite.MarcarComoIndisponivel();
            DataCheckIn = ParsePtBrDateTime(checkIn, nameof(checkIn));
            DataCheckOut = ParsePtBrDateTime(checkOut, nameof(checkOut));

            if (DataCheckOut <= DataCheckIn) throw new ArgumentException("Data de check-out deve ser posterior à data de check-in.", nameof(checkOut));

            _contador++;
        }

        public void CadastrarHospedes(IEnumerable<Hospede> hospedes)
        {
            var lista = hospedes as List<Hospede> ?? [.. hospedes];
            if (lista.Count == 0) throw new ArgumentException("Lista de hóspedes não pode ser vazia.", nameof(hospedes));
            if (Hospedes.Count + lista.Count > Suite.Capacidade) throw new InvalidOperationException($"Capacidade máxima da suíte é {Suite.Capacidade} hóspedes.");

            Hospedes.AddRange(lista);
        }

        public int ObterQuantidadeHospedes()
        {
            return Hospedes.Count;
        }

        public decimal CalcularValorTotal()
        {
            var dias = (DataCheckOut - DataCheckIn).Days;
            if (dias <= 0) throw new InvalidOperationException("A reserva deve ter pelo menos um dia de duração.");

            return dias * Suite.PrecoDiaria;
        }

        // Método para parsear datas no formato pt-BR
        private static DateTime ParsePtBrDateTime(string input, string paramName)
        {
            if (string.IsNullOrWhiteSpace(input)) throw new ArgumentException("Data não pode ser vazia.", paramName);

            if (DateTime.TryParseExact(input, DateFormats, PtBr, DateTimeStyles.None, out var dt)) return dt;

            throw new ArgumentException($"Data inválida: '{input}'. Formatos válidos: {string.Join(", ", DateFormats)}", paramName);
        }
    }
}
