CREATE DATABASE AntiFraudTransaction;
USE AntiFraudTransaction;

IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'transaction_state' AND type = 'U')
BEGIN
	CREATE TABLE transaction_state (
		[id] INT PRIMARY KEY IDENTITY(1,1),
		[description] VARCHAR(50) NOT NULL		
	);
END
GO

INSERT INTO transaction_state([description]) VALUES ('pending'),('approved'),('rejected');

IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'transaction_op' AND type = 'U')
BEGIN
	CREATE TABLE transaction_op (
		[id] UNIQUEIDENTIFIER PRIMARY KEY,
		[id_state] INT NOT NULL		
		CONSTRAINT fk_transaction_state FOREIGN KEY ([id_state]) REFERENCES [transaction_state]([id])
	);
END
GO

----------------------- STORED PROCEDURES -----------------
CREATE OR ALTER PROCEDURE usp_transaction_op_insert
	@id UNIQUEIDENTIFIER,
    @id_state int
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO transaction_op (id, id_state)
    VALUES (@id, @id_state);    
END;
GO

CREATE OR ALTER PROCEDURE usp_transaction_op_update
	@id UNIQUEIDENTIFIER,
    @id_state int
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE transaction_op SET id_state = @id_state
    WHERE id = @id;
END;
GO
