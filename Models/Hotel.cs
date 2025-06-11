namespace SistemaHospedagem.Models
{
    public class Hotel
    {
        // Propriedades
        private readonly List<Suite> _suites = [];
        private readonly List<Reserva> _reservas = [];

        // Método para adicionar uma suíte ao hotel
        public void AdicionarSuite(Suite suite)
        {
            if (suite is null) throw new ArgumentNullException(nameof(suite), "Suite não pode ser nula.");
            _suites.Add(suite);
        }

        // Método para listar todas as suítes disponíveis
        public IReadOnlyList<Suite> ListarSuitesDisponiveis() => [.. _suites.Where(s => s.EstaDisponivel())];

        // Método para criar uma reserva
        public Reserva CriarReserva(Suite suite, string checkIn, string checkOut)
        {
            if (!_suites.Contains(suite)) throw new ArgumentException("Suite não encontrada no hotel.", nameof(suite));

            var reserva = new Reserva(suite, checkIn, checkOut);
            _reservas.Add(reserva);
            return reserva;
        }

        // Método para cancelar uma reserva
        public void CancelarReserva(Guid reservaId)
        {
            var reserva = _reservas.FirstOrDefault(r => r.Id == reservaId) ?? throw new KeyNotFoundException("Reserva não encontrada.");
            reserva.Suite.MarcarComoDisponivel();
            _reservas.Remove(reserva);
        }

        // Método para listar todas as reservas
        public IReadOnlyList<Reserva> ListarReservas() => [.. _reservas];
    }
}