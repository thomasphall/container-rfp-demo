CREATE PROCEDURE dbo.spUnconsumedMessageInsert
(
	@clientName varchar(100),
	@messageId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO [dbo].[UnconsumedMessage]
	(
		[ClientName],
		[MessageId]
	)
	VALUES
	(
		@clientName,
		@messageId
	)
END
GO