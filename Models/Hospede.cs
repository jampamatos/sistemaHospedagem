using System.Text.RegularExpressions;

namespace SistemaHospedagem.Models
{
    public class Hospede
    {
        private static int _contador = 0;

        public int Id { get; }
        public string Nome { get; set; }
        public string Sobrenome { get; set; }

        private string _cpf = "";
        public string Cpf
        {
            get => _cpf;
            set
            {
                if (!TryValidarCpf(value, out var formatado))
                    throw new ArgumentException("CPF inválido.", nameof(value));
                _cpf = formatado;
            }
        }

        private string _telefone = "";
        public string Telefone
        {
            get => _telefone;
            set
            {
                if (string.IsNullOrWhiteSpace(value) || !_telefoneRegex.IsMatch(value))
                    throw new ArgumentException("Telefone inválido.", nameof(value));
                _telefone = value;
            }
        }

        public Hospede(string nome, string sobrenome, string cpf, string telefone)
        {
            Id = ++_contador;
            Nome = nome ?? throw new ArgumentNullException(nameof(nome));
            Sobrenome = sobrenome ?? throw new ArgumentNullException(nameof(sobrenome));
            Cpf = cpf;           // já valida e formata
            Telefone = telefone; // já valida
        }

        // Regex para extrair dígitos e para validar o telefone
        private static readonly Regex _naoDigitos     = new(@"[^\d]", RegexOptions.Compiled);
        private static readonly Regex _telefoneRegex   = new(@"^(\(\d{2}\)\d{4}-\d{4}|\d{10})$", RegexOptions.Compiled);

        private static bool TryValidarCpf(string? cpf, out string formatado)
        {
            formatado = "";
            if (string.IsNullOrWhiteSpace(cpf)) return false;

            // remove tudo que não é dígito
            var digitos = _naoDigitos.Replace(cpf, "");
            if (digitos.Length != 11 || new string(digitos[0], 11) == digitos) return false;

            // valida dígitos verificadores
            if (!ValidarDigitosCpf(digitos)) return false;

            // formata em 000.000.000-00
            formatado = ulong.Parse(digitos)
                            .ToString("000\\.000\\.000\\-00");
            return true;
        }

        private static bool ValidarDigitosCpf(string d)
        {
            int somar(int até, int pesoInicial)
            {
                var soma = 0;
                for (int i = 0; i < até; i++)
                    soma += (d[i] - '0') * (pesoInicial - i);
                return soma;
            }

            // 1º dígito
            var resto1 = somar(9, 10) % 11;
            var dig1   = resto1 < 2 ? 0 : 11 - resto1;
            if (dig1 != (d[9] - '0')) return false;

            // 2º dígito
            var resto2 = somar(10, 11) % 11;
            var dig2   = resto2 < 2 ? 0 : 11 - resto2;
            return dig2 == (d[10] - '0');
        }
    }
}
