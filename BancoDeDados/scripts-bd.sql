/*
	Se for a primeira vez que irá utilizar esse script,
	remova o comentário da Criação do Banco de Dados logo abaixo
*/

/*CREATE DATABASE [avisa_enchente_db]
GO*/

USE [avisa_enchente_db]
GO

CREATE TABLE [dbo].[estados_atendidos](
	id INT IDENTITY(1,1) NOT NULL,
	descricao VARCHAR(max) NOT NULL,
	uf VARCHAR(2) NOT NULL,
	codigo_estado VARCHAR(2) NOT NULL,

	CONSTRAINT [pk_estados_atendidos_id] PRIMARY KEY([id]),
)
GO

CREATE TABLE [dbo].[cidades_atendidas](
	id INT IDENTITY(1,1) NOT NULL,
	descricao VARCHAR(max) NOT NULL,
	codigo_cidade VARCHAR(20) NOT NULL,
	estado_atendido_id INT NOT NULL,
	latitude_ref DECIMAL(9,6) NOT NULL,
	longitude_ref DECIMAL(9,6) NOT NULL,

	CONSTRAINT [pk_cidades_atendidas_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_cidades_atendidas_estado_atendido_id] FOREIGN KEY([estado_atendido_id])
        REFERENCES [dbo].[estados_atendidos]([id])
)
GO
 

CREATE TABLE [dbo].[usuarios](
	id INT IDENTITY(1,1) NOT NULL,
    [nome_completo] VARCHAR(100) NOT NULL,
	[email] VARCHAR(200) NOT NULL,
	[senha] VARCHAR(60) NOT NULL,	
	[tipo_usuario] INT NOT NULL,
	[cidade_atendida_id] INT NULL,
	[primeiro_login] BIT NOT NULL,

	CONSTRAINT [pk_usuarios_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_usuarios_cidade_atendida_id] FOREIGN KEY([cidade_atendida_id])
        REFERENCES [dbo].[cidades_atendidas]([id])
)
GO


-- STORED PROCEDURES PADRAO

CREATE PROCEDURE sp_Consulta
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

CREATE PROCEDURE sp_Delete
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

CREATE PROCEDURE sp_Listagem
(
	@tabela VARCHAR(MAX)
)
AS
BEGIN
	DECLARE @sql VARCHAR(MAX);
	DECLARE @function VARCHAR(MAX);	

	SET @function =
		CASE @tabela
			WHEN 'usuario' THEN  'fnc_ListaUsuarios()'
		END

	SET @sql = 'select * from ' + @function
	EXEC(@sql)
END
GO

-- STORED PROCEDURES PERSONALIZADAS

CREATE PROCEDURE sp_Registrar_Usuario
(
	@nome_completo VARCHAR(150),
	@email VARCHAR(200),
	@senha VARCHAR(150),
	@tipo_usuario INT,
	@primeiro_login BIT
)
AS
BEGIN
	INSERT INTO usuarios
	(nome_completo, email, senha, tipo_usuario, primeiro_login)
	VALUES
	(@nome_completo, @email, @senha, @tipo_usuario, @primeiro_login)
END
GO

CREATE PROCEDURE sp_Login_Usuario
(
	@email VARCHAR(200),
	@senha VARCHAR(150)
)
AS
BEGIN
	SELECT * FROM usuarios u
	WHERE u.email = @email and u.senha = @senha
END
GO

CREATE PROCEDURE sp_Define_Cidade_Usuario
(
	@usuarioId INT,
	@CidadeId INT
)
AS
BEGIN
	UPDATE usuarios 
	SET cidade_atendida_id = @CidadeId
	WHERE id = @usuarioId 

	IF((SELECT primeiro_login FROM usuarios WHERE id = @usuarioId) = 1) BEGIN
		UPDATE usuarios 
		SET primeiro_login = 0
		WHERE id = @usuarioId
	END
END
GO

CREATE PROCEDURE sp_Consulta_Usuario_Por_Email
(
	@email VARCHAR(200)
)
AS
BEGIN
	SELECT * FROM usuarios u
	WHERE u.email = @email
END
GO


-- FUNCTIONS

CREATE FUNCTION fnc_ListaUsuarios()
RETURNS TABLE AS
RETURN
(
	SELECT * FROM usuarios
)
GO

