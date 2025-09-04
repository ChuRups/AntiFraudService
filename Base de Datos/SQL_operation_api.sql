CREATE DATABASE AntiFraud
USE AntiFraud
----------------------- TABLES -----------------
IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'customer' AND type = 'U')
BEGIN
	CREATE TABLE customer (
		[id] UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
		[name] VARCHAR(100)
	);
END
GO


IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'operation' AND type = 'U')
BEGIN
	CREATE TABLE operation (
		[id] UNIQUEIDENTIFIER PRIMARY KEY,
		[id_customer] UNIQUEIDENTIFIER  NOT NULL,
		[amount] DECIMAL(10,2),
		[operation_date] DATE,
		CONSTRAINT fk_operation_customer FOREIGN KEY ([id_customer]) REFERENCES [customer]([id])
	);
END
GO


----------------------- STORED PROCEDURES -----------------
CREATE OR ALTER PROCEDURE usp_operation_insert
	@id UNIQUEIDENTIFIER,
    @id_customer UNIQUEIDENTIFIER,
    @amount DECIMAL(10,2),
    @operation_date DATE
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Operation (id, id_customer, amount, operation_date)
    VALUES (@id, @id_customer, @amount, @operation_date);
    SELECT SCOPE_IDENTITY() AS OperationId;
END;
GO

CREATE OR ALTER PROCEDURE usp_operation_sum_by_customer_and_day
    @id_customer UNIQUEIDENTIFIER,
    @day DATE
AS
BEGIN
    SET NOCOUNT ON;

    SELECT SUM(amount) AS TotalAmount
    FROM operation
    WHERE id_customer = @id_customer
      AND operation_date = @day;
END;
GO

INSERT INTO customer(name) VALUES('Cliente1');
INSERT INTO customer(name) VALUES('Cliente2');
