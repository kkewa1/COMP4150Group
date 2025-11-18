IF OBJECT_ID('dbo.trg_Orders_Ready_UpdateAnalytics', 'TR') IS NOT NULL
    DROP TRIGGER dbo.trg_Orders_Ready_UpdateAnalytics;
GO

CREATE TRIGGER dbo.trg_Orders_Ready_UpdateAnalytics
ON dbo.Orders
AFTER UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    WITH Ready AS (
        SELECT i.OrderID, i.TableID
        FROM inserted  AS i
        JOIN deleted   AS d ON d.OrderID = i.OrderID
        WHERE i.OrderStatus = 'Ready'
          AND ISNULL(d.OrderStatus,'') <> 'Ready'
    ),
    ItemAgg AS (
        SELECT 
            oi.ItemID,
            YEAR(GETDATE())            AS [Year],
            DATENAME(MONTH, GETDATE()) AS [Month],
            SUM(oi.Quantity)           AS TimesOrdered,
            SUM(oi.Quantity * it.Price)AS Revenue
        FROM Ready r
        JOIN dbo.OrderedItem oi ON oi.OrderID = r.OrderID
        JOIN dbo.Item        it ON it.ItemID  = oi.ItemID
        GROUP BY oi.ItemID
    )
    MERGE dbo.ItemAnalytics AS tgt
    USING ItemAgg AS src
      ON tgt.ItemID = src.ItemID
     AND tgt.[Year] = src.[Year]
     AND tgt.[Month] = src.[Month]
    WHEN MATCHED THEN
        UPDATE SET
            tgt.TimesOrdered = tgt.TimesOrdered + src.TimesOrdered,
            tgt.Revenue      = tgt.Revenue      + src.Revenue
    WHEN NOT MATCHED THEN
        INSERT (ItemID, [Year], [Month], TimesOrdered, Revenue)
        VALUES (src.ItemID, src.[Year], src.[Month], src.TimesOrdered, src.Revenue);

    WITH Ready AS (
        SELECT i.OrderID, i.TableID
        FROM inserted  AS i
        JOIN deleted   AS d ON d.OrderID = i.OrderID
        WHERE i.OrderStatus = 'Ready'
          AND ISNULL(d.OrderStatus,'') <> 'Ready'
    ),
    TableAgg AS (
        SELECT
            r.TableID,
            YEAR(GETDATE())            AS [Year],
            DATENAME(MONTH, GETDATE()) AS [Month],
            COUNT(DISTINCT r.OrderID)  AS TimesUsed,
            SUM(oi.Quantity * it.Price)AS Revenue
        FROM Ready r
        JOIN dbo.OrderedItem oi ON oi.OrderID = r.OrderID
        JOIN dbo.Item        it ON it.ItemID  = oi.ItemID
        GROUP BY r.TableID
    )
    MERGE dbo.TableAnalytics AS tgt
    USING TableAgg AS src
      ON tgt.TableID = src.TableID
     AND tgt.[Year]  = src.[Year]
     AND tgt.[Month] = src.[Month]
    WHEN MATCHED THEN
        UPDATE SET
            tgt.TimesUsed = tgt.TimesUsed + src.TimesUsed,
            tgt.Revenue   = tgt.Revenue   + src.Revenue
    WHEN NOT MATCHED THEN
        INSERT (TableID, [Year], [Month], TimesUsed, Revenue)
        VALUES (src.TableID, src.[Year], src.[Month], src.TimesUsed, src.Revenue);
END
GO
