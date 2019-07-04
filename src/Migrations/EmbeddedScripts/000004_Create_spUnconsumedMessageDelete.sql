CREATE PROCEDURE dbo.spUnconsumedMessageDelete
(
	@messageId uniqueidentifier
)
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM [dbo].[UnconsumedMessage]
	WHERE MessageId = @messageId;
END
GO