using System;
using System.Collections.Generic;
using System.Globalization;
namespace SistemaHospedagem.Models
{
    // Tipos de suites disponíveis
    public enum TipoSuite
    {
        Economica,
        Standard,
        Luxo,
        Presidencial
    }

    public class Suite
    {
        // Mapas estaticos para validação de capacidade
        private static readonly IReadOnlyDictionary<TipoSuite, int> CapacidadeMax = new Dictionary<TipoSuite, int>
        {
            [TipoSuite.Economica] = 2,
            [TipoSuite.Standard] = 2,
            [TipoSuite.Luxo] = 4,
            [TipoSuite.Presidencial] = 6
        };

        // Mapa estático para preços por tipo de suíte
        private static readonly IReadOnlyDictionary<TipoSuite, decimal> PrecosPorTipo = new Dictionary<TipoSuite, decimal>
        {
            [TipoSuite.Economica] = 100m,
            [TipoSuite.Standard] = 150m,
            [TipoSuite.Luxo] = 300m,
            [TipoSuite.Presidencial] = 600m
        };

        private static readonly CultureInfo PtBr = new("pt-BR");

        // Propriedades
        public Guid Id { get; } = Guid.NewGuid();
        public TipoSuite Tipo { get; private set; }
        public int Capacidade { get; private set; }
        public decimal PrecoDiaria { get; private set; }
        private bool Disponivel { get; set; } = true;

        // Construtor
        public Suite(TipoSuite tipo, int capacidadeDesejada)
        {
            // Validações
            if (!CapacidadeMax.ContainsKey(tipo)) throw new ArgumentException("Tipo de suíte inválido.", nameof(tipo));

            var max = CapacidadeMax[tipo];
            if (capacidadeDesejada < 1 || capacidadeDesejada > max)
                throw new ArgumentOutOfRangeException(nameof(capacidadeDesejada), $"Capacidade deve ser entre 1 e {max} para o tipo {tipo}.");

            // Atribuições
            Tipo = tipo;
            Capacidade = capacidadeDesejada;
            PrecoDiaria = PrecosPorTipo[tipo];
        }

        // Método para marcar a suíte como indisponível ou disponível
        public void MarcarComoIndisponivel()
        {
            Disponivel = false;
        }

        public void MarcarComoDisponivel()
        {
            Disponivel = true;
        }

        // Métoodo para verificar se a suíte está disponível
        public bool EstaDisponivel()
        {
            return Disponivel;
        }

        // ToString override para exibir informações da suíte
        public override string ToString()
        {
            return $"{Tipo} - Capacidade: {Capacidade} - Preço: {PrecoDiaria.ToString("C", PtBr)} - Disponível: {Disponivel}";
        }
    }
}