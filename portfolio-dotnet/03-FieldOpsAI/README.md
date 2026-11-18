# FieldOps AI — Triagem de ocorrências (IA opcional)

**Objetivo:** mostrar arquitetura backend, integração HTTP externa, JSON estruturado, injeção de dependência, tratamento de falhas, persistência e revisão humana em cenário inspirado no setor elétrico.

## Arquitetura
- `FieldOpsAI.Core`: entidades, contratos, interfaces de infraestrutura e fluxos de negócios.
- `FieldOpsAI.Infrastructure`: Entity Framework + SQLite, classificador por regras e integração opcional com a API de IA (Responses API).
- `FieldOpsAI.Api`: endpoints REST e configuração por ambiente.
- `FieldOpsAI.Tests`: testes de regras determinísticas e ciclo de revisão.

## Executar
```powershell
dotnet restore 03-FieldOpsAI/FieldOpsAI.Api/FieldOpsAI.Api.csproj
dotnet run --project 03-FieldOpsAI/FieldOpsAI.Api
dotnet test 03-FieldOpsAI/FieldOpsAI.Tests/FieldOpsAI.Tests.csproj
```
API em `http://localhost:5103` e OpenAPI no modo Development em `/openapi/v1.json`. Exemplos em `requests.http`.

### Dois modos de classificação
1. **Sem chave**: executa `RuleBasedTriageEngine`, um classificador de **regras fixas**, sem modelo generativo. `triageSource: "rules-demo"`.
2. **Com chave da API** (opcional): antes de executar, no PowerShell:
   ```powershell
   $env:OpenAI__ApiKey = 'sua-chave-valida'
   $env:OpenAI__Model = 'gpt-4.1-mini'
   ```
   Executa `OpenAiTriageEngine` via HTTPS + JSON Schema. `triageSource: "openai"`.
   Falha remota, JSON inesperado ou timeout geram fallback por regras (`triageSource: "rules-fallback"`).

O modo de IA exige conta, chave válida e pode gerar **custos**. Não é obrigatório para rodar o projeto. Nunca comite a chave ou dados confidenciais.

## Endpoints
- `POST /api/incidents` cadastra ocorrência **fictícia**.
- `GET /api/incidents` lista até 100 ocorrências.
- `GET /api/incidents/{id}` consulta ocorrência.
- `POST /api/incidents/{id}/triage` classifica a ocorrência.
- `POST /api/incidents/{id}/review` marca revisão humana após triagem.

## Segurança e limitações
- Código **didático**, não é sistema operacional de resposta a emergências.
- **Não utilize descrições reais de ocorrências, nomes de colaboradores, segredos corporativos ou informações sensíveis** com a API externa sem autorização explícita da organização.
- Texto enviado à IA é dado não confiável; o prompt restringe instruções, mas **não garante** resistência a prompt injection.
- Classificações por IA e regras não devem orientar intervenções em rede energizada nem substituir procedimentos técnicos, protocolos de emergência, profissionais habilitados e revisão humana.
- API demonstrativa sem login. Não exponha em rede pública.
- Para produção, adicionar JWT, RBAC, auditoria, rate limiting, métricas, sanitização de dados e testes de integração.

**Boas práticas demonstradas:** `HttpClientFactory`, injeção por interface `ITriageEngine`, JSON estruturado, configuração segura por variáveis de ambiente, fallback explícito, ciclo de aprovação humana e testes unitários.

Abra `FieldOpsAI.sln` no Visual Studio para navegar pelos quatro projetos (Core, Infrastructure, Api, Tests).
