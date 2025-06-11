# Sistema de Hospedagem

Este repositório contém um exemplo de aplicação console para controle de reservas de hotel. O ódigo foi desenvolvido como parte de um **Desafio de C\#** na [Digital Innovation One (DIO)](https://www.dio.me/), durante o bootcamp **End to End Engineering**, realizado em parceria com a **Wex**.

## Sobre o projeto

A aplicação simula a adminitração básica de um hotel e permite:

- Listar suites disponíveis;
- Cadastrar hóspedes e criar uma reserva informando datas de check-in e check-out;
- Cancelar reservas ativas.

O menu principal é interativo e executado diretamente no terminal. Todas as informações são manipuladas em memória, servindo como demonstração de orientação a objetos com C#.

## Estrutura

- `Program.cs` \– ponto de entrada da aplicação e menu principal.
- `Models/` \– classes que representam as entidades do sistema, como `Hospede`, `Suite`, `Reserva` e `Hotel`.
- `sistemaHospedagem.csproj` \– arquivo de projeto .NET direcionado ao framework `net9.0`.

## Como executar

É necessário possuir o SDK do [.NET](https://dotnet.microsoft.com/) em uma versão compatível (9.0 ou superior). Com o SDK instalado, execute:

```bash
dotnet run
```

O comando acima compila e inicia o aplicativo de linha de comando.

## Licença

Este projeto é disponibilizado sem licença específica, apenas para fins de estudo.

