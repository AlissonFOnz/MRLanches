# Testes Unitários - MRLanches.Users

Este diretório contém os testes unitários dos controllers da API MRLanches.Users.

## Estrutura dos Testes

Os testes estão organizados em arquivos separados por controller:
- `AuthControllerTests.cs`: Testes do controller de autenticação
- `UsersControllerTests.cs`: Testes do controller de usuários
- `TempAuthControllerTests.cs`: Testes do controller temporário de exemplo

## Como Executar os Testes

1. Certifique-se de estar na raiz do projeto ou no diretório `MRLanches.Users`.
2. Execute o comando abaixo no terminal:

```
dotnet test MRLanches.Users.Tests/MRLanches.Users.Tests.csproj
```

Ou, se estiver na pasta do projeto de teste:

```
dotnet test
```

O resultado mostrará o status de todos os testes unitários.

## Observações
- Os testes utilizam xUnit e Moq para simular dependências e garantir independência do banco de dados.
- Para adicionar novos testes, crie novos métodos nos arquivos correspondentes ou adicione novos arquivos para outros controllers.
- Não é necessário rodar a aplicação ou o banco de dados para executar os testes unitários.
