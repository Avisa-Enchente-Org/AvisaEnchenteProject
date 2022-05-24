CREATE PROCEDURE sp_consulta
(
	@id INT,
	@tabela VARCHAR(MAX)
)
AS
BEGIN
	DECLARE @sql VARCHAR(MAX);
	SET @sql = 'select * from ' + @tabela +
	' where id = ' + CAST(@id AS VARCHAR(MAX))
	EXEC(@sql)
END
GO

CREATE PROCEDURE sp_delete
(
	@id INT,
	@tabela VARCHAR(MAX)
)
AS
BEGIN
	DECLARE @sql VARCHAR(MAX);
	SET @sql = ' delete ' + @tabela +
	' where id = ' + CAST(@id AS VARCHAR(MAX))
	EXEC(@sql)
END
GO

CREATE PROCEDURE spProximoId (@tabela VARCHAR(max)) AS
BEGIN
	DECLARE @sql VARCHAR(MAX)
	SET @sql = 'SELECT IDENT_CURRENT (' + '''' + @tabela + '''' + ') + 1 AS Current_Identity'
	EXEC(@sql)
END
GO