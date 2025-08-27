# Testes Unitários - MRLanches.Users

Este diretório contém os testes unitários dos controllers da API MRLanches.Users.

## Estrutura dos Testes

Os testes estão organizados em arquivos separados por controller:
- `AuthControllerTests.cs`: Testes do controller de autenticação
- `UsersControllerTests.cs`: Testes do controller de usuários (COBERTURA COMPLETA de todos os cenários dos endpoints)
- `TempAuthControllerTests.cs`: Testes do controller temporário de exemplo

## Cobertura dos Testes

O arquivo `UsersControllerTests.cs` cobre todos os cenários relevantes dos endpoints de usuário, incluindo:
- Criação de usuário (sucesso, erro de negócio, erro interno)
- Atualização de usuário (sucesso, erro de negócio, erro interno)
- Exclusão de usuário (sucesso, não encontrado, erro interno)
- Busca por ID (ADM, MR, usuário comum, não encontrado, forbidden, erro interno)
- Listagem de usuários (sucesso, erro interno)

Caso precise criar novos testes ou ajustar regras de negócio, utilize este arquivo como referência para garantir a padronização e cobertura.

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
