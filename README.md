# Sistema de Hospedagem

Este reposit\u00f3rio cont\u00e9m um exemplo de aplica\u00e7\u00e3o console para controle de reservas de hotel. O c\u00f3digo foi desenvolvido como parte de um **Desafio de C\#** na Digital Innovation One (DIO), durante o bootcamp **End to End Engineering**, realizado em parceria com a **Wex**.

## Sobre o projeto

A aplica\u00e7\u00e3o simula a administra\u00e7\u00e3o b\u00e1sica de um hotel e permite:

- Listar su\u00edt\u00e9s dispon\u00edveis;
- Cadastrar h\u00f3spedes e criar uma reserva informando datas de check\u2011in e check\u2011out;
- Cancelar reservas ativas.

O menu principal \u00e9 interativo e executado diretamente no terminal. Todas as informa\u00e7\u00f5es s\u00e3o manipuladas em mem\u00f3ria, servindo como demonstra\u00e7\u00e3o de orienta\u00e7\u00e3o a objetos com C#.

## Estrutura

- `Program.cs` \– ponto de entrada da aplica\u00e7\u00e3o e menu principal.
- `Models/` \– classes que representam as entidades do sistema, como `Hospede`, `Suite`, `Reserva` e `Hotel`.
- `sistemaHospedagem.csproj` \– arquivo de projeto .NET direcionado ao framework `net9.0`.

## Como executar

\u00c9 necess\u00e1rio possuir o SDK do [.NET](https://dotnet.microsoft.com/) em uma vers\u00e3o compat\u00edvel (9.0 ou superior). Com o SDK instalado, execute:

```bash
dotnet run
```

O comando acima compila e inicia o aplicativo de linha de comando.

## Licen\u00e7a

Este projeto \u00e9 disponibilizado sem licen\u00e7a espec\u00edfica, apenas para fins de estudo.

