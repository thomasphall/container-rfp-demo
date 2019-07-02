CREATE PROCEDURE dbo.spUnconsumedMessageDelete
(
	@clientName varchar(100),
	@messageId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[UnconsumedMessage]
	WHERE ClientName = @clientName
		AND MessageId = @messageId;
END
GO