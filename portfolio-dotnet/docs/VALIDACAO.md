# Checklist de validação antes de publicar

## O que foi verificado nesta entrega
- Criação de 3 projetos distintos, com camadas e código-fonte.
- Presença de arquivos `.csproj`, DTOs, entidades, services, repositórios, controllers, testes, configurações e documentação.
- Sintaxe XML/JSON dos artefatos de configuração (verificação automatizada local).
- Estrutura de diretórios, roteiros de requisição e workflow GitHub Actions.

## O que **não** foi validado neste ambiente
- Compilação C#, restauração de pacotes NuGet e execução real dos testes (SDK .NET indisponível).
- Respostas reais de um modelo OpenAI (chave/API não fornecidas).
- Comportamento de concorrência sob carga elevada ou integração com bancos de produção.

## Faça no computador
```powershell
dotnet --info
dotnet restore 01-TaskFlow/TaskFlow.Api/TaskFlow.Api.csproj
dotnet build 01-TaskFlow/TaskFlow.Api/TaskFlow.Api.csproj
dotnet test 01-TaskFlow/TaskFlow.Tests/TaskFlow.Tests.csproj
dotnet build 02-StockPilot/StockPilot.Api/StockPilot.Api.csproj
dotnet test 02-StockPilot/StockPilot.Tests/StockPilot.Tests.csproj
dotnet build 03-FieldOpsAI/FieldOpsAI.Api/FieldOpsAI.Api.csproj
dotnet test 03-FieldOpsAI/FieldOpsAI.Tests/FieldOpsAI.Tests.csproj
```

Depois confira `GET /health`, `GET /openapi/v1.json` e cada fluxo de `requests.http`. Em produção, substitua `EnsureCreated` por migrações do EF Core, configure HTTPS, controle de acesso e gerenciamento de segredos.
