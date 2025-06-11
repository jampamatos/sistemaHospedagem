using System;
using System.Collections.Generic;
using System.Globalization;
using SistemaHospedagem.Models;

namespace SistemaHospedagem
{
    internal class Program
    {
        private static readonly Hotel hotel = new();
        private static readonly CultureInfo PtBr = new("pt-BR");
        private const string DataFormat = "dd/MM/yyyy";

        private static void Main()
        {
            // 1) Configura cultura brasileira para parsing e formatação
            CultureInfo.DefaultThreadCurrentCulture = PtBr;
            CultureInfo.DefaultThreadCurrentUICulture  = PtBr;

            // 2) Carga inicial de suítes
            SeedSuites();

            // 3) Loop principal do menu
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Sistema de Hotelaria ===");
                Console.WriteLine("1) Listar suítes disponíveis");
                Console.WriteLine("2) Criar reserva");
                Console.WriteLine("3) Cancelar reserva");
                Console.WriteLine("4) Sair");
                Console.Write("Opção: ");

                var opcao = Console.ReadLine();
                if (opcao == "4") break;

                ProcessarOpcao(opcao);
                Console.WriteLine("\nPressione ENTER para voltar ao menu...");
                Console.ReadLine();
            }
        }

        private static void SeedSuites()
        {
            // Apenas como exemplo, podemos girar quantidades ou ler de configuração
            hotel.AdicionarSuite(new Suite(TipoSuite.Economica));
            hotel.AdicionarSuite(new Suite(TipoSuite.Standard));
            hotel.AdicionarSuite(new Suite(TipoSuite.Luxo));
            hotel.AdicionarSuite(new Suite(TipoSuite.Presidencial));
        }

        private static void ProcessarOpcao(string? opcao)
        {
            switch (opcao)
            {
                case "1":
                    ListarSuitesDisponiveis();
                    break;
                case "2":
                    CriarReserva();
                    break;
                case "3":
                    CancelarReserva();
                    break;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }
        }

        private static void ListarSuitesDisponiveis()
        {
            Console.Clear();
            var disponiveis = hotel.ListarSuitesDisponiveis();
            Console.WriteLine("---- Suítes Disponíveis ----");
            if (disponiveis.Count == 0)
            {
                Console.WriteLine("Nenhuma suíte disponível no momento.");
                return;
            }

            foreach (var s in disponiveis)
                Console.WriteLine(s);
        }

        private static void CriarReserva()
        {
            Console.Clear();
            Console.WriteLine("---- Criar Reserva ----");

            // 1) Leitura de hóspedes
            int qtd = ReadInt("Quantos hóspedes deseja cadastrar?", min: 1);
            var hospedes = new List<Hospede>(qtd);
            for (int i = 1; i <= qtd; i++)
            {
                Console.WriteLine($"\nHóspede #{i}");
                var nome      = ReadString("Nome");
                var sobrenome = ReadString("Sobrenome");
                var cpf       = ReadString("CPF");
                var tel       = ReadString("Telefone");

                try
                {
                    hospedes.Add(new Hospede(nome, sobrenome, cpf, tel));
                }
                catch (ArgumentException ex)
                {
                    Console.WriteLine($"Erro: {ex.Message}");
                    i--; // repete essa iteração
                }
            }

            // 2) Escolha de suíte
            var disponiveis = hotel.ListarSuitesDisponiveis();
            if (disponiveis.Count == 0)
            {
                Console.WriteLine("Não há suítes disponíveis.");
                return;
            }
            Console.WriteLine("\nSelecione a suíte:");
            for (int i = 0; i < disponiveis.Count; i++)
            {
                Console.WriteLine($"{i + 1}) {disponiveis[i]}");
            }
            int idx = ReadInt("Número da suíte", 1, disponiveis.Count) - 1;
            var suiteEscolhida = disponiveis[idx];

            // 3) Datas
            var checkIn  = ReadDate($"Data de Check-in ({DataFormat})");
            var checkOut = ReadDate($"Data de Check-out ({DataFormat})");

            // 4) Criação da reserva
            try
            {
                var reserva = hotel.CriarReserva(suiteEscolhida, checkIn.ToString(DataFormat), checkOut.ToString(DataFormat));
                reserva.CadastrarHospedes(hospedes);
                Console.WriteLine($"\nReserva criada! ID: {reserva.Id}");
                Console.WriteLine($"Total a pagar: {reserva.CalcularValorTotal():C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nFalha ao criar reserva: {ex.Message}");
            }
        }

        private static void CancelarReserva()
        {
            Console.Clear();
            Console.WriteLine("---- Cancelar Reserva ----");
            var reservas = hotel.ListarReservas();
            if (reservas.Count == 0)
            {
                Console.WriteLine("Nenhuma reserva ativa.");
                return;
            }

            for (int i = 0; i < reservas.Count; i++)
            {
                var r = reservas[i];
                Console.WriteLine($"{i + 1}. ID: {r.Id}, Suite: {r.Suite.Tipo}, Check-in: {r.DataCheckIn:dd/MM/yyyy}, Check-out: {r.DataCheckOut:dd/MM/yyyy}");
            }
            int idx = ReadInt("Número da reserva", 1, reservas.Count) - 1;
            var reserva = reservas[idx];

            try
            {
                hotel.CancelarReserva(reserva.Id);
                Console.WriteLine("\nReserva cancelada com sucesso!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nErro ao cancelar: {ex.Message}");
            }
        }

        // ————— Métodos de leitura/validação —————

        private static string ReadString(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var input = Console.ReadLine()?.Trim();
                if (!string.IsNullOrWhiteSpace(input))
                    return input;
                Console.WriteLine("Entrada não pode ser vazia.");
            }
        }

        private static int ReadInt(string prompt, int min = int.MinValue, int max = int.MaxValue)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                if (int.TryParse(Console.ReadLine(), out int val) && val >= min && val <= max)
                    return val;
                Console.WriteLine($"Digite um número válido{(min != int.MinValue ? $" entre {min} e {max}" : "")}.");
            }
        }

        private static DateTime ReadDate(string prompt)
        {
            while (true)
            {
                Console.Write($"{prompt}: ");
                var txt = Console.ReadLine();
                if (DateTime.TryParseExact(txt, DataFormat, PtBr, DateTimeStyles.None, out var dt))
                    return dt;
                Console.WriteLine($"Formato inválido. Use {DataFormat}.");
            }
        }
    }
}
