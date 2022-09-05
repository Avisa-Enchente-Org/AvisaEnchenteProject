-- SP's Estados Atendidos

CREATE PROCEDURE sp_insert_estados_atendidos
(
	@descricao VARCHAR(MAX),
	@uf VARCHAR(2),
	@codigo_estado VARCHAR(2)
)
AS
BEGIN
	INSERT INTO [dbo].[estados_atendidos]
	(descricao, uf, codigo_estado)
	VALUES
	(@descricao, @uf, @codigo_estado)
END
GO

CREATE PROCEDURE sp_update_estados_atendidos
(
	@id int,
	@descricao VARCHAR(MAX),
	@uf VARCHAR(2),
	@codigo_estado VARCHAR(2)
)
AS
BEGIN
	UPDATE [dbo].[estados_atendidos] SET
	descricao = @descricao, 
	uf = @uf, 
	codigo_estado = @codigo_estado
	WHERE id = @id
END
GO

CREATE PROCEDURE sp_consulta_estado_atendido_por_codigo
(
	@codigo VARCHAR(2)
)
AS
BEGIN
	SELECT * FROM [dbo].[estados_atendidos] e
	WHERE e.codigo_estado = @codigo
END
GO

CREATE PROCEDURE sp_listar_estados_atendidos
AS
BEGIN
	SELECT * FROM [dbo].[estados_atendidos]
END
GO

-- SP'S Cidades Atendidas

 
CREATE PROCEDURE sp_insert_cidades_atendidas
(
	@descricao VARCHAR(MAX),
	@codigo_cidade VARCHAR(20),
	@estado_atendido_id INT
)
AS
BEGIN
	INSERT INTO [dbo].[cidades_atendidas]
	(descricao, codigo_cidade, estado_atendido_id)
	VALUES
	(@descricao, @codigo_cidade, @estado_atendido_id)
END
GO


CREATE PROCEDURE sp_update_cidades_atendidas
(
	@id INT,
	@descricao VARCHAR(MAX),
	@codigo_cidade VARCHAR(20),
	@estado_atendido_id INT
)
AS
BEGIN
	UPDATE [dbo].[cidades_atendidas] SET
	descricao = @descricao, 
	codigo_cidade = @codigo_cidade,
	estado_atendido_id = @estado_atendido_id
	WHERE id = @id
END
GO

CREATE PROCEDURE sp_consulta_cidade_atendida_por_codigo
(
	@codigo VARCHAR(20)
)
AS
BEGIN
	SELECT * FROM [dbo].[cidades_atendidas] c
	WHERE c.codigo_cidade = @codigo
END
GO

CREATE PROCEDURE sp_listar_cidades_atendidas
AS
BEGIN
	SELECT * FROM [dbo].[cidades_atendidas]
END
GO

CREATE PROCEDURE sp_listar_cidades_atendidas_por_estado
(
	@estado_atendido_id INT
)
AS
BEGIN
	SELECT * FROM [dbo].[cidades_atendidas] WHERE estado_atendido_id = @estado_atendido_id
END
GO