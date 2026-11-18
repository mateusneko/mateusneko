-- Exemplos didáticos de consultas para SQLite (bancos criados em cada API).
-- Execute em cliente SQLite após criar dados de demonstração.

-- TaskFlow: tarefas agrupadas por status de usuário
SELECT a.Name, t.Status, COUNT(*) AS Total
FROM Accounts AS a
JOIN Tasks AS t ON t.OwnerId = a.Id
GROUP BY a.Id, t.Status
ORDER BY a.Name, t.Status;

-- StockPilot: estoque abaixo ou no mínimo, com valor expresso em centavos
SELECT Sku, Name, Stock, MinimumStock, (Stock * UnitPriceCents) AS ValueCents
FROM Products
WHERE Stock <= MinimumStock
ORDER BY Name;

-- StockPilot: total de entradas/saídas por produto
SELECT p.Sku, m.Type, SUM(m.Quantity) AS TotalQuantity
FROM Products AS p
JOIN Movements AS m ON m.ProductId = p.Id
GROUP BY p.Sku, m.Type
ORDER BY p.Sku, m.Type;

-- FieldOps AI: número de classificações por origem
SELECT TriageSource, Status, COUNT(*) AS Total
FROM Incidents
GROUP BY TriageSource, Status
ORDER BY Total DESC;
