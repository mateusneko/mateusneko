# Apresentação no GitHub e entrevista — desenvolvedor .NET júnior

## Como publicar
1. Crie três repositórios públicos separados: `taskflow-dotnet-api`, `stockpilot-inventory-api` e `fieldops-ai-api`.
2. Copie apenas a pasta correspondente para cada repositório. Mantenha o `.gitignore` e o README do projeto. Se preferir um monorepo, publique a pasta toda e use a CI incluída.
3. Abra os projetos no Visual Studio/VS Code; confirme build e testes localmente.
4. Crie commits semânticos: `feat: implementa CRUD de tarefas`, `test: cobre saldo negativo`, `docs: documenta endpoints`.
5. Faça 2–3 screenshots **reais** das requisições Postman/HTTP e das saídas de testes, depois de executar.
6. Não apresente scores, métricas, cobertura de testes ou deploys que ainda não mediu.

## O que saber explicar

**TaskFlow:** Qual a diferença entre autenticação e autorização? Como o JWT é assinado e validado? Por que hashear senha? Como impedir acesso às tarefas de outros usuários? Quais status HTTP retornam e por quê?

**StockPilot:** Onde ficam as regras de negócio? O que é a chave primária e o índice exclusivo de SKU? Por que não permitir estoque negativo? Para que serve o `Version` como token de concorrência? O que o EF Core grava numa transação?

**FieldOps AI:** Qual diferença entre regra determinística e IA generativa? O que acontece se API de IA retornar 429/500 ou JSON inválido? Qual o significado de `triageSource`? Por que dados corporativos não devem ser enviados sem autorização? Por que a revisão humana é necessária?

## Evoluções técnicas propostas
- Adicionar teste de integração com banco SQLite in-memory, sem mock, usando `WebApplicationFactory`.
- Criar migrations `dotnet ef migrations add InitialCreate` (retirar `EnsureCreated` antes de iniciar migrations em projeto existente).
- Implementar health checks e logging estruturado (Serilog/OpenTelemetry).
- Criar interfaces web em Blazor ou Angular.
- Adicionar autenticação no StockPilot/FieldOps AI, rate limiting e RBAC.
- Substituir o SQLite por SQL Server ou PostgreSQL quando testar deployment em Azure.

## Sugestão de descrição para currículo
**Projetos Pessoais — C#/.NET:** desenvolvimento de três APIs REST com ASP.NET Core, arquitetura em camadas, orientação a objetos, EF Core/SQLite, autenticação JWT, testes xUnit e integração opcional com IA generativa via Responses API. Implementação de regras de negócio, tratamento de erros, versionamento e automação CI/CD.
