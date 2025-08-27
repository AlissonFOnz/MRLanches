# Instruções de Uso - MRLanches Users API

## Configuração Inicial

### 1. Banco de Dados PostgreSQL
- Execute o comando: `docker-compose up -d` para subir o PostgreSQL
- Ou configure sua própria instância PostgreSQL e atualize a connection string no `appsettings.json`

### 2. Configuração da API
- Atualize a connection string no `appsettings.json` com suas credenciais do PostgreSQL
- Atualize a chave secreta JWT no `appsettings.json` (deve ter pelo menos 32 caracteres)

### 3. Executar a API
```bash
dotnet restore
dotnet build
dotnet run
```

## Endpoints da API

### Autenticação
- **POST** `/api/auth/login` - Login para obter token JWT

### Usuários (requer autenticação)
- **POST** `/api/users` - Criar usuário (apenas ADM)
- **PUT** `/api/users/{id}` - Atualizar usuário (apenas ADM)
- **DELETE** `/api/users/{id}` - Deletar usuário (apenas ADM)
- **GET** `/api/users/{id}` - Buscar usuário por ID
- **GET** `/api/users` - Listar todos os usuários (apenas ADM)

## Usuários do Sistema

### ADM (Administrador)
- Username: `ADM`
- Senha: `1234567`
- Acesso: Total a todos os endpoints

### MR (Sistema MR)
- Username: `MR`
- Senha: `1234567`
- Acesso: Apenas a usuários comuns (User)

## Exemplo de Uso

### 1. Login como ADM
```bash
curl -X POST "https://localhost:7001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"username": "ADM", "password": "1234567"}'
```

### 2. Criar usuário (com token ADM)
```bash
curl -X POST "https://localhost:7001/api/users" \
  -H "Authorization: Bearer SEU_TOKEN_AQUI" \
  -H "Content-Type: application/json" \
  -d '{"username": "usuario1", "password": "1234567"}'
```

## Documentação Swagger
Acesse `https://localhost:7001/swagger` para ver a documentação interativa da API.

## Observações
- Todas as senhas devem ter exatamente 7 números
- Usuários criados são sempre do tipo "User"
- Tokens JWT expiram em 24 horas por padrão
- A API valida automaticamente se o usuário já existe antes de criar
