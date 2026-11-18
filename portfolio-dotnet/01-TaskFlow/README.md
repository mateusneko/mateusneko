# TaskFlow — API de tarefas com autenticação JWT

**Objetivo:** demonstrar fundamentos de C#, orientação a objetos, CRUD REST, autenticação/autorização, EF Core e testes.

## Arquitetura
- `TaskFlow.Core`: domínio, DTOs, interfaces (ports) e regras de negócio.
- `TaskFlow.Infrastructure`: Entity Framework Core, repositórios SQLite e hash seguro de senhas (ASP.NET Identity).
- `TaskFlow.Api`: controllers HTTP, JWT bearer, DI e documentação OpenAPI.
- `TaskFlow.Tests`: testes unitários de domínio e controle de acesso.

## Executar (PowerShell / Windows)
1. Instale o SDK **.NET 10**.
2. Defina uma chave local com pelo menos 32 bytes; não a commite:
   ```powershell
   $env:Jwt__Key = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
   ```
3. Na raiz do repositório:
   ```powershell
   dotnet restore 01-TaskFlow/TaskFlow.Api/TaskFlow.Api.csproj
   dotnet run --project 01-TaskFlow/TaskFlow.Api
   ```
4. API: `http://localhost:5101` (veja `Properties/launchSettings.json`). Documentação: `/openapi/v1.json` no modo Development.
5. Use `requests.http` (extensão REST Client do VS Code) ou Postman. Copie o `accessToken` retornado no login.
6. Testes:
   ```powershell
   dotnet test 01-TaskFlow/TaskFlow.Tests/TaskFlow.Tests.csproj
   ```

## Rotas
| Verbo | Rota | JWT |
|---|---|---|
| POST | `/api/auth/register` | Não |
| POST | `/api/auth/login` | Não |
| GET | `/api/tasks?status=Pending&page=1&pageSize=20` | Sim |
| GET | `/api/tasks/{id}` | Sim |
| POST | `/api/tasks` | Sim |
| PUT | `/api/tasks/{id}` | Sim |
| PATCH | `/api/tasks/{id}/status` | Sim |
| DELETE | `/api/tasks/{id}` | Sim |

**Segurança:** cada consulta/edição de tarefa valida o proprietário pelo `OwnerId`. A senha é armazenada por `PasswordHasher`, nunca em texto claro. Não reutilize JWTs de demonstração nem chaves em produção. `EnsureCreated` simplifica a demonstração; para produção, use migrations do EF Core, HTTPS, gerenciamento de segredos, validação operacional e políticas de senha/limitação de tentativas.

**Evoluções:** refresh token, perfis, migrations, testes de integração, rate limiting, observabilidade.

Abra `TaskFlow.sln` no Visual Studio para navegar pelos quatro projetos (Core, Infrastructure, Api, Tests).
