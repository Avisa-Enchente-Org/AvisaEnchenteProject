/*
	Se for a primeira vez que ir� utilizar esse script,
	remova o coment�rio da Cria��o do Banco de Dados logo abaixo
*/

/*CREATE DATABASE [avisa_enchente_db]
GO*/

USE [avisa_enchente_db]
GO

CREATE TABLE [dbo].[estados_atendidos](
	[id] INT IDENTITY(1,1) NOT NULL,
	[descricao] VARCHAR(max) NOT NULL,
	[uf] VARCHAR(2) NOT NULL,
	[codigo_estado] VARCHAR(2) NOT NULL,

	CONSTRAINT [pk_estados_atendidos_id] PRIMARY KEY([id]),
)
GO

CREATE TABLE [dbo].[cidades_atendidas](
	[id] INT IDENTITY(1,1) NOT NULL,
	[descricao] VARCHAR(max) NOT NULL,
	[codigo_cidade] VARCHAR(20) NOT NULL,
	[estado_atendido_id] INT NOT NULL,
	[latitude_ref] DECIMAL(9,6) NOT NULL,
	[longitude_ref] DECIMAL(9,6) NOT NULL,

	CONSTRAINT [pk_cidades_atendidas_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_cidades_atendidas_estado_atendido_id] FOREIGN KEY([estado_atendido_id])
        REFERENCES [dbo].[estados_atendidos]([id])
)
GO
 

CREATE TABLE [dbo].[usuarios](
	[id] INT IDENTITY(1,1) NOT NULL,
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


CREATE TABLE [dbo].[pontos_sensoriamento](
	[id] INT IDENTITY(1,1) NOT NULL,
    [descricao] VARCHAR(200) NOT NULL,
	[ativo_helix] BIT NOT NULL,
	[cidade_atendida_id] INT NULL,
	[latitude] DECIMAL(9,6) NOT NULL,
	[longitude] DECIMAL(9,6) NOT NULL,	
	[usuario_id] INT NULL,

	CONSTRAINT [pk_pontos_sensoriamento_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_pontos_sensoriamento_cidade_atendida_id] FOREIGN KEY([cidade_atendida_id])
        REFERENCES [dbo].[cidades_atendidas]([id]),
	CONSTRAINT [fk_pontos_sensoriamento_usuario_id] FOREIGN KEY([usuario_id])
        REFERENCES [dbo].[usuarios]([id])
)
GO

CREATE TABLE [dbo].[sensoriamento_atual](
	[id] INT IDENTITY(1,1) NOT NULL,
	[ponto_sensoriamento_id] INT NULL,
	[nivel_pluviosidade] decimal(6,2) NOT NULL,
	[vazao_agua] decimal(6,2) NOT NULL,
	[altura_agua] decimal(6,2) NOT NULL,
	[ultima_atualizacao] SMALLDATETIME NOT NULL DEFAULT GETDATE(),	
	[tipo_risco] INT NOT NULL,

	CONSTRAINT [pk_sensoriamento_atual_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_sensoriamento_atual_ponto_sensoriamento_id] FOREIGN KEY([ponto_sensoriamento_id])
        REFERENCES [dbo].[pontos_sensoriamento]([id])
)
GO

CREATE TABLE [dbo].[notificacoes_historico](
	[id] INT IDENTITY(1,1) NOT NULL,
	[ponto_sensoriamento_id] INT NULL,
	[nivel_pluviosidade] decimal(6,2) NOT NULL,
	[vazao_agua] decimal(6,2) NOT NULL,
	[altura_agua] decimal(6,2) NOT NULL,
	[data_notificacao] SMALLDATETIME NOT NULL DEFAULT GETDATE(),	
	[tipo_risco] INT NOT NULL,

	CONSTRAINT [pk_notificacoes_historico_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_notificacoes_historico_ponto_sensoriamento_id] FOREIGN KEY([ponto_sensoriamento_id])
        REFERENCES [dbo].[pontos_sensoriamento]([id])
)
GO


CREATE TABLE [dbo].[notificacoes_parametros](
	[id] INT IDENTITY(1,1) NOT NULL,
	[tipo_risco] INT NOT NULL,
	[nivel_pluviosidade] decimal(6,2) NOT NULL,
	[vazao_agua] decimal(6,2) NOT NULL,
	[altura_agua] decimal(6,2) NOT NULL,
	[ponto_sensoriamento_id] INT NULL,

	CONSTRAINT [pk_notificacoes_parametros_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_notificacoes_parametros_ponto_sensoriamento_id] FOREIGN KEY([ponto_sensoriamento_id])
        REFERENCES [dbo].[pontos_sensoriamento]([id])
)
GO


-- STORED PROCEDURES PADRAO

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

CREATE PROCEDURE sp_listar
(
	@tabela VARCHAR(MAX)
)
AS
BEGIN
	DECLARE @sql VARCHAR(MAX);
	DECLARE @function VARCHAR(MAX);	

	SET @function =
		CASE @tabela
			WHEN 'usuarios' THEN  'fnc_listar_usuarios()'
		END

	SET @sql = 'select * from ' + @function
	EXEC(@sql)
END
GO

-- STORED PROCEDURES PERSONALIZADAS

CREATE PROCEDURE sp_insert_usuarios
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

CREATE PROCEDURE sp_login_usuario
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

CREATE PROCEDURE sp_define_cidade_usuario
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

CREATE PROCEDURE sp_consulta_usuario_por_email
(
	@email VARCHAR(200)
)
AS
BEGIN
	SELECT * FROM usuarios u
	WHERE u.email = @email
END
GO

CREATE PROCEDURE sp_update_usuarios
(
	@id int,
	@nome_completo VARCHAR(150),
	@email VARCHAR(200),
	@senha VARCHAR(150),
	@tipo_usuario INT,
	@primeiro_login BIT
)
AS
BEGIN
	UPDATE usuarios SET
	nome_completo = @nome_completo, 
	email = @email, 
	senha = @senha, 
	tipo_usuario = @tipo_usuario, 
	primeiro_login = @primeiro_login
	WHERE id = @id
END
GO

CREATE PROCEDURE sp_pesquisa_avancada_usuarios
(
	@nome_completo VARCHAR(150),
	@email VARCHAR(200),
	@tipo_usuario varchar(2)
)
AS
BEGIN
	SELECT * FROM vw_usuarios 
	WHERE nome_completo  LIKE '%' + @nome_completo +'%'
	AND email LIKE '%' + @email +'%' 
	AND tipo_usuario LIKE '%' + @tipo_usuario +'%' 
END
GO


CREATE PROCEDURE sp_atualiza_usuario_admin
(
	@id int,
	@tipo_usuario INT
)
AS
BEGIN
	UPDATE usuarios SET
	tipo_usuario = @tipo_usuario
	WHERE id = @id
END
GO

-- FUNCTIONS

CREATE FUNCTION fnc_listar_usuarios()
RETURNS TABLE AS
RETURN
(
	SELECT * FROM vw_usuarios
)
GO

-- VIEWS

CREATE VIEW vw_usuarios AS
	SELECT u.id, u.nome_completo, u.email, u.tipo_usuario, u.cidade_atendida_id, u.primeiro_login FROM usuarios u
GO


-- TRIGGERS

CREATE TRIGGER trg_criaDependenciasDoPontoDeSensoriamento ON [dbo].[pontos_sensoriamento]
FOR INSERT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @ponto_sensoriamento_id INT = (SELECT id FROM inserted)

	INSERT INTO [dbo].[sensoriamento_atual] 
	(ponto_sensoriamento_id, nivel_pluviosidade, vazao_agua, altura_agua, tipo_risco)
	VALUES 
	(@ponto_sensoriamento_id, 0.0, 0.0, 0.0, 0)

	SET NOCOUNT OFF
END
GO

CREATE TRIGGER trg_excluiDependenciasDoPontoDeSensoriamento ON [dbo].[pontos_sensoriamento]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @ponto_sensoriamento_id INT = (SELECT id FROM deleted)

	DELETE FROM [dbo].[sensoriamento_atual]  
		WHERE [ponto_sensoriamento_id] = @ponto_sensoriamento_id

	DELETE FROM [dbo].[notificacoes_historico]  
		WHERE [ponto_sensoriamento_id] = @ponto_sensoriamento_id

	SET NOCOUNT OFF
END
GO

