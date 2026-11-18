# Portfólio Backend .NET/C# — 3 projetos para Desenvolvedor Júnior

> **Autor:** Mateus de Andrade | Foco em C#/.NET, engenharia de software, APIs REST, banco de dados e integração de IA.
>
> Projetos demonstrativos **independentes** — não usam informações internas de empresas ou sistemas corporativos. Podem ser publicados em três repositórios GitHub separados.

## Projetos

| Projeto | Principal aprendizado | Tecnologias |
|---|---|---|
| **01 TaskFlow** | CRUD, autenticação JWT, autorização por proprietário, hash de senhas, paginação | .NET 10, ASP.NET Core, EF Core, SQLite, JWT, xUnit |
| **02 StockPilot** | POO, regras de negócio, estoque, SQL, concorrência otimista, transações | .NET 10, EF Core, LINQ, SQLite, xUnit |
| **03 FieldOps AI** | Injeção de dependência, API externa, JSON Schema, tratamento de falhas, IA opcional | .NET 10, HttpClient, OpenAI Responses API, SQLite, xUnit |

### O que as três demonstram
- **POO**: classes, objetos, métodos, construtores, encapsulamento, enums e invariantes de domínio.
- **Camadas reais (projetos `.csproj`)**: `Core` (domínio e aplicação) → `Infrastructure` (persistência/adapters) → `Api` (HTTP). O `Core` não importa EF Core nem ASP.NET Core.
- **SOLID/clean code**: DI, interfaces, responsabilidade única, validação, nomes expressivos e DTOs.
- **APIs REST**: rotas HTTP, códigos de status, input validation, Problem Details e OpenAPI JSON.
- **Banco de dados**: SQLite para rodar local sem instalar servidor; queries EF Core, restrições e índice.
- **Testes**: xUnit, fakes de repositório e testes de regras de domínio.
- **Automação**: workflow GitHub Actions, build e teste por projeto.
- **Docker**: Dockerfile multistage em cada projeto (com contexto da raiz do repositório).

Veja também [`docs/MATRIZ_COMPETENCIAS.md`](docs/MATRIZ_COMPETENCIAS.md) e [`docs/EXEMPLOS_SQL.sql`](docs/EXEMPLOS_SQL.sql). Cada projeto inclui sua própria solução `.sln` para abrir no Visual Studio.

## Requisitos
- SDK **.NET 10** (LTS) instalado: https://dotnet.microsoft.com/download/dotnet/10.0
- VS Code + extensão C# Dev Kit, Visual Studio 2026 ou JetBrains Rider.
- Opcional: REST Client do VS Code, Postman, Docker Desktop.
- Internet para restaurar pacotes NuGet na primeira execução.

## Como começar (Windows/PowerShell)
Abra o terminal **na raiz** onde está este `README.md`.

```powershell
# Testar ambiente
dotnet --version

# TaskFlow — configure primeiro a chave JWT apenas no seu terminal
$env:Jwt__Key = [Convert]::ToBase64String([Security.Cryptography.RandomNumberGenerator]::GetBytes(48))
dotnet run --project 01-TaskFlow/TaskFlow.Api

# Em outro terminal, rode StockPilot
dotnet run --project 02-StockPilot/StockPilot.Api

# Em outro terminal, rode FieldOps AI (sem chave externa)
dotnet run --project 03-FieldOpsAI/FieldOpsAI.Api
```

**Endereços:** TaskFlow `http://localhost:5101`, StockPilot `http://localhost:5102`, FieldOps AI `http://localhost:5103`. Em desenvolvimento, cada API publica um documento `GET /openapi/v1.json`. Para testar chamadas, abra o arquivo `requests.http` de cada projeto. Não use os três em uma mesma porta.

### Testes automatizados
```powershell
dotnet test 01-TaskFlow/TaskFlow.Tests/TaskFlow.Tests.csproj
dotnet test 02-StockPilot/StockPilot.Tests/StockPilot.Tests.csproj
dotnet test 03-FieldOpsAI/FieldOpsAI.Tests/FieldOpsAI.Tests.csproj
```

### Containers
```powershell
docker build -f 02-StockPilot/Dockerfile -t stockpilot .
docker run --rm -p 5102:8080 -e ASPNETCORE_ENVIRONMENT=Development stockpilot
```
Para usar SQLite de forma persistente no Docker, configure um volume e `ConnectionStrings__Database`. Para o TaskFlow, passe `Jwt__Key` no container por secret/variável de ambiente (jamais no Dockerfile).

## Roteiro recomendado de estudo
1. **TaskFlow** — leia `Core/Domain`, `Core/Services`, `Core/Ports`, `Infrastructure` e `Controllers`; compreenda cada responsabilidade e as rotas JWT.
2. **StockPilot** — explique por que o saldo não pode ficar negativo; use Postman para tentar uma saída inválida e observar `422`.
3. **FieldOps AI** — teste modo sem chave, conecte opcionalmente uma API real, compare `triageSource` e explique o fallback.
4. Escreva novos testes, faça commits pequenos, abra um Pull Request consigo mesmo e documente mudanças.

### Atenção ao grau de validação
A entrega contém **código-fonte completo**, testes definidos e exemplos de execução. O ambiente usado para gerar o material **não possui o SDK .NET nem acesso ao NuGet**, portanto não foi possível executar `dotnet restore`, `dotnet build` ou `dotnet test` aqui. A primeira validação de compilação deve ocorrer no seu PC ou no GitHub Actions. Isso também é indicado em `docs/VALIDACAO.md`.

## Visão de portfólio / currículo
- TaskFlow: "Desenvolvimento de API REST com ASP.NET Core, JWT, autenticação, autorização por usuário, EF Core e testes xUnit."
- StockPilot: "API de gestão de materiais com regras de estoque, persistência relacional, histórico de movimentações e concorrência otimista."
- FieldOps AI: "API de triagem com integração HTTP para IA generativa, retornos estruturados, fallback por regras e revisão humana."

Use como **projetos pessoais**, não como experiência profissional formal. Adapte os requisitos, implemente evoluções próprias e tenha segurança ao explicar cada decisão em entrevistas.
