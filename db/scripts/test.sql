DECLARE @orderID INT;

INSERT INTO dbo.Orders(TableID, StaffID, OrderStatus, PaymentStatus)
VALUES (100, 1, 'Received', 'Pending');

SET @orderID = SCOPE_IDENTITY();

INSERT INTO dbo.OrderedItem(OrderID, ItemID, Quantity)
VALUES (@orderID, 10, 2),
       (@orderID, 11, 1);

UPDATE dbo.Orders
SET OrderStatus = 'Ready'
WHERE OrderID = @orderID;

SELECT * FROM dbo.ItemAnalytics  WHERE ItemID IN (10,11);
SELECT * FROM dbo.TableAnalytics WHERE TableID = 100;
