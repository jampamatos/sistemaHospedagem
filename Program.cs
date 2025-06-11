using SistemaHospedagem.Models;

class Program
{
    static Hotel hotel = new Hotel();

    static void Main()
    {
        // Carga inicial de suítes
        hotel.AdicionarSuite(new Suite(TipoSuite.Economica));
        hotel.AdicionarSuite(new Suite(TipoSuite.Economica));
        hotel.AdicionarSuite(new Suite(TipoSuite.Economica));
        hotel.AdicionarSuite(new Suite(TipoSuite.Economica));
        hotel.AdicionarSuite(new Suite(TipoSuite.Standard));
        hotel.AdicionarSuite(new Suite(TipoSuite.Standard));
        hotel.AdicionarSuite(new Suite(TipoSuite.Standard));
        hotel.AdicionarSuite(new Suite(TipoSuite.Luxo));
        hotel.AdicionarSuite(new Suite(TipoSuite.Luxo));
        hotel.AdicionarSuite(new Suite(TipoSuite.Presidencial));

        // Menu de opções
        while (true)
        {
            Console.Clear();
            Console.WriteLine("Sistema de Hotelaria");
            Console.WriteLine("1. Listar suítes disponíveis");
            Console.WriteLine("2. Criar reserva");
            Console.WriteLine("3. Cancelar reserva");
            Console.WriteLine("4. Sair");
            Console.Write("Escolha uma opção: ");

            var opcao = Console.ReadLine();

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
                case "4":
                    return;
                default:
                    Console.WriteLine("Opção inválida. Tente novamente.");
                    break;
            }

            Console.WriteLine("\nTecle ENTER para voltar ao menu...");
            Console.ReadKey();
        }

        static void ListarSuitesDisponiveis()
        {
            Console.Clear();
            Console.WriteLine("Suítes Disponíveis:");
            var suitesDisponiveis = hotel.ListarSuitesDisponiveis();
            if (suitesDisponiveis.Count == 0)
            {
                Console.WriteLine("Nenhuma suíte disponível no momento.");
            }
            else
            {
                foreach (var suite in suitesDisponiveis)
                {
                    Console.WriteLine($"ID: {suite.Id}, Tipo: {suite.Tipo}, Capacidade: {suite.Capacidade}, Preço Diária: {suite.PrecoDiaria:C}");
                }
            }
        }

        static void CriarReserva()
        {
            Console.Clear();
            Console.WriteLine("Quantos hóspedes deseja cadastrar?");
            var quantidade = Console.ReadLine();
            if (!int.TryParse(quantidade, out int qtd) || qtd <= 0)
            {
                Console.WriteLine("Quantidade inválida. Deve ser um número maior que zero.");
                return;
            }

            var hospedes = new List<Hospede>();
            for (int i = 0; i < qtd; i++)
            {
                Console.WriteLine($"Hóspede {i + 1}:");
                Console.Write("Nome: ");
                var nome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(nome))
                {
                    Console.WriteLine("Nome não pode ser vazio.");
                    i--; // Repetir a iteração para o hóspede atual
                    continue;
                }
                Console.Write("Sobrenome: ");
                var sobrenome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(sobrenome))
                {
                    Console.WriteLine("Sobrenome não pode ser vazio.");
                    i--; // Repetir a iteração para o hóspede atual
                    continue;
                }
                Console.Write("CPF: ");
                var cpf = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(cpf))
                {
                    Console.WriteLine("CPF não pode ser vazio.");
                    i--; // Repetir a iteração para o hóspede atual
                    continue;
                }
                Console.Write("Telefone: ");
                var telefone = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(telefone))
                {
                    Console.WriteLine("Telefone não pode ser vazio.");
                    i--; // Repetir a iteração para o hóspede atual
                    continue;
                }

                try
                {
                    var hospede = new Hospede(nome, sobrenome, cpf, telefone);
                    hospedes.Add(hospede);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao cadastrar hóspede: {ex.Message}");
                    i--; // Repetir a iteração para o hóspede atual
                }
            }

            Console.WriteLine("Selecione uma suíte para reserva:");
            var suitesDisponiveis = hotel.ListarSuitesDisponiveis();
            if (suitesDisponiveis.Count == 0)
            {
                Console.WriteLine("Nenhuma suíte disponível para reserva.");
                return;
            }
            for (int i = 0; i < suitesDisponiveis.Count; i++)
            {
                var suite = suitesDisponiveis[i];
                Console.WriteLine($"{i + 1}. ID: {suite.Id}, Tipo: {suite.Tipo}, Capacidade: {suite.Capacidade}, Preço Diária: {suite.PrecoDiaria:C}");
            }

            Console.Write("Escolha uma suíte pelo número: ");
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int suiteIndex) || suiteIndex < 1 || suiteIndex > suitesDisponiveis.Count)
            {
                Console.WriteLine("Número de suíte inválido.");
                return;
            }

            var suiteEscolhida = suitesDisponiveis[suiteIndex - 1];

            // Solicitar datas de check-in e check-out
            Console.Write("Data de Check-in (dd/MM/yyyy): ");
            var checkIn = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(checkIn))
            {
                Console.WriteLine("Data de check-in não pode ser vazia.");
                return;
            }
            Console.Write("Data de Check-out (dd/MM/yyyy): ");
            var checkOut = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(checkOut))
            {
                Console.WriteLine("Data de check-out não pode ser vazia.");
                return;
            }
            try
            {
                var reserva = hotel.CriarReserva(suiteEscolhida, checkIn, checkOut);
                reserva.CadastrarHospedes(hospedes);
                Console.WriteLine($"Reserva criada com sucesso! ID: {reserva.Id}, Total: {reserva.CalcularValorTotal():C}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao criar reserva: {ex.Message}");
            }
        }

        static void CancelarReserva()
        {
            Console.Clear();
            Console.WriteLine("Selecione uma reserva para cancelar:");
            var reservas = hotel.ListarReservas();
            if (reservas.Count == 0)
            {
                Console.WriteLine("Nenhuma reserva encontrada.");
                return;
            }
            for (int i = 0; i < reservas.Count; i++)
            {
                var reserva = reservas[i];
                Console.WriteLine($"{i + 1}. ID: {reserva.Id}, Suite: {reserva.Suite.Tipo}, Check-in: {reserva.DataCheckIn:dd/MM/yyyy}, Check-out: {reserva.DataCheckOut:dd/MM/yyyy}");
            }
            var input = Console.ReadLine();
            if (!int.TryParse(input, out int reservaIndex) || reservaIndex < 1 || reservaIndex > reservas.Count)
            {
                Console.WriteLine("Número de reserva inválido.");
                return;
            }
            var reservaEscolhida = reservas[reservaIndex - 1];
            try
            {
                hotel.CancelarReserva(reservaEscolhida.Id);
                Console.WriteLine("Reserva cancelada com sucesso.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao cancelar reserva: {ex.Message}");
            }
        }
    }
}