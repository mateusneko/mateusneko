# StockPilot — Gestão de estoque e materiais

**Objetivo:** aplicar orientação a objetos, regras de negócio, C# `checked`, LINQ, ORM EF Core, SQL, testes, transações e controle de concorrência em uma situação realista.

## Arquitetura
- `StockPilot.Core`: entidades `Product` e `StockMovement`, invariantes, contratos, interfaces e `InventoryService`.
- `StockPilot.Infrastructure`: SQLite, contexto EF Core, consultas LINQ, unicidade do SKU e concorrência otimista.
- `StockPilot.Api`: endpoints REST e OpenAPI.
- `StockPilot.Tests`: testes para saldo, estoque mínimo, duplicidade e movimentações.

## Executar
```powershell
dotnet restore 02-StockPilot/StockPilot.Api/StockPilot.Api.csproj
dotnet run --project 02-StockPilot/StockPilot.Api
dotnet test 02-StockPilot/StockPilot.Tests/StockPilot.Tests.csproj
```
API `http://localhost:5102`, documento `/openapi/v1.json` em Development e exemplos em `requests.http`.

## Endpoints
- `POST /api/products` cadastra material.
- `GET /api/products` lista materiais.
- `GET /api/products/{id}` consulta saldo.
- `GET /api/products/low-stock` retorna itens com `Stock <= MinimumStock`.
- `POST /api/products/{id}/movements` registra entrada/saída (com justificativa obrigatória).
- `GET /api/products/{id}/movements` retorna as 200 últimas movimentações.

### Regras de negócio
- SKU único normalizado em maiúsculas, nome obrigatório e preço em **centavos** para evitar arredondamentos monetários em `double`.
- Saldo nunca negativo; quantidades devem ser positivas.
- Produto e movimentação são gravados pelo EF Core em uma mesma operação transacional de `SaveChanges`.
- A coluna `Version` é um *concurrency token*; alterações simultâneas conflitantes geram HTTP 409, evitando sobrescritas silenciosas.
- `EnsureCreated` permite iniciar facilmente com SQLite. Para evolução, adicionar migrations, índices e paginação de grandes conjuntos.

**Atenção:** API didática **sem autenticação**. Não a exponha publicamente nem utilize dados reais de uma empresa. Para produção, acrescentar autenticação, autorização e auditoria.

**Evoluções para portfólio:** JWT com papéis, importação CSV, testes com SQLite in-memory, dashboard Blazor, PostgreSQL/SQL Server.

Abra `StockPilot.sln` no Visual Studio para navegar pelos quatro projetos (Core, Infrastructure, Api, Tests).
