/*
	Se for a primeira vez que ir� utilizar esse script,
	remova o coment�rio da Cria��o do Banco de Dados logo abaixo
*/

CREATE DATABASE [avisa_enchente_db]
GO

USE [avisa_enchente_db]
GO

CREATE TABLE [dbo].[estados_atendidos](
	[id] INT IDENTITY(1,1) NOT NULL,
	[descricao] VARCHAR(MAX) NOT NULL,
	[uf] VARCHAR(2) NOT NULL,
	[codigo_estado] VARCHAR(2) NOT NULL,

	CONSTRAINT [pk_estados_atendidos_id] PRIMARY KEY([id])
)
GO


CREATE TABLE [dbo].[cidades_atendidas](
	[id] INT IDENTITY(1,1) NOT NULL,
	[descricao] VARCHAR(max) NOT NULL,
	[codigo_cidade] VARCHAR(20) NOT NULL,
	[estado_atendido_id] INT NOT NULL,

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
    [helix_id] VARCHAR(100) NOT NULL,
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
	[nivel_pluviosidade] DECIMAL(6,2) NOT NULL,
	[vazao_agua] DECIMAL(6,2) NOT NULL,
	[altura_agua] DECIMAL(6,2) NOT NULL,
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


-- VIEWS

CREATE VIEW vw_usuarios AS
	SELECT u.id, u.nome_completo, u.email, u.tipo_usuario, u.cidade_atendida_id, u.primeiro_login FROM usuarios u
GO

-- FUNCTIONS

-- FUNCTIONS Ponto Sensoriamento
CREATE FUNCTION fnc_listar_pontos_sensoriamento()
RETURNS TABLE AS
RETURN
(
	SELECT p.*, 
	u.nome_completo as 'nome_completo_usuario',
	c.descricao as 'cidade_descricao',
	c.codigo_cidade,
	e.id as 'estado_atendido_id',
	e.descricao as 'estado_descricao',
	e.uf as 'estado_uf',
	E.codigo_estado
	FROM [dbo].[pontos_sensoriamento] p
	INNER JOIN [dbo].[usuarios] u ON u.id = p.usuario_id 
	INNER JOIN [dbo].[cidades_atendidas] c ON c.id = p.cidade_atendida_id
	INNER JOIN [dbo].[estados_atendidos] e ON e.id = c.estado_atendido_id
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

CREATE PROCEDURE spProximoId (@tabela VARCHAR(max)) AS
BEGIN
	DECLARE @sql VARCHAR(MAX)
	SET @sql = 'SELECT IDENT_CURRENT (' + '''' + @tabela + '''' + ') + 1 AS Current_Identity'
	EXEC(@sql)
END
GO

-- STORED PROCEDURES PERSONALIZADAS

-- SP's Usuarios
CREATE PROCEDURE sp_listar_usuarios
AS
BEGIN

	SELECT * FROM vw_usuarios

END
GO

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

CREATE PROCEDURE sp_listar_usuarios_administradores
AS
BEGIN
	SELECT * FROM vw_usuarios u
	WHERE u.tipo_usuario = 2
END
GO

CREATE PROCEDURE sp_consultar_usuario_com_endereco
(
	@id INT
)
AS 
BEGIN

	SELECT u.*, 
	c.descricao as 'cidade_descricao',
	c.codigo_cidade,
	e.id as 'estado_atendido_id',
	e.descricao as 'estado_descricao',
	e.uf as 'estado_uf',
	E.codigo_estado
	FROM vw_usuarios u
	INNER JOIN [dbo].[cidades_atendidas] c ON c.id = u.cidade_atendida_id
	INNER JOIN [dbo].[estados_atendidos] e ON e.id = c.estado_atendido_id
	WHERE u.id = @id

END
GO

-- SP's Pontos de Sensoriamento
CREATE PROCEDURE sp_consultar_pontos_sensoriamento 
(
	@id INT
)
AS
BEGIN
	
	SELECT p.*, 
	u.nome_completo as 'nome_completo_usuario',
	c.descricao as 'cidade_descricao',
	c.codigo_cidade,
	e.id as 'estado_atendido_id',
	e.descricao as 'estado_descricao',
	e.uf as 'estado_uf',
	E.codigo_estado
	FROM [dbo].[pontos_sensoriamento] p
	INNER JOIN [dbo].[usuarios] u ON u.id = p.usuario_id 
	INNER JOIN [dbo].[cidades_atendidas] c ON c.id = p.cidade_atendida_id
	INNER JOIN [dbo].[estados_atendidos] e ON e.id = c.estado_atendido_id
	WHERE p.id = @id

END
GO


CREATE PROCEDURE sp_listar_pontos_sensoriamento
AS
BEGIN
	
	SELECT * FROM fnc_listar_pontos_sensoriamento()

END
GO

CREATE PROCEDURE sp_insert_pontos_sensoriamento
(
	@helix_id VARCHAR(100),
	@ativo_helix BIT,
	@cidade_atendida_id INT,
	@latitude DECIMAL(9,6),
	@longitude DECIMAL(9,6),
	@usuario_id INT
)
AS
BEGIN
	INSERT INTO [dbo].[pontos_sensoriamento]
	(helix_id, ativo_helix, cidade_atendida_id, latitude, longitude, usuario_id)
	VALUES
	(@helix_id, @ativo_helix, @cidade_atendida_id, @latitude, @longitude, @usuario_id)
END
GO

CREATE PROCEDURE sp_update_pontos_sensoriamento
(
	@id INT,
	@helix_id VARCHAR(100),
	@ativo_helix BIT,
	@cidade_atendida_id INT,
	@latitude DECIMAL(9,6),
	@longitude DECIMAL(9,6),
	@usuario_id INT
)
AS
BEGIN
	UPDATE [dbo].[pontos_sensoriamento] SET
	helix_id = @helix_id, 
	ativo_helix = @ativo_helix, 
	cidade_atendida_id = @cidade_atendida_id, 
	latitude = @latitude, 
	longitude = @longitude, 
	usuario_id = @usuario_id
	WHERE id = @id
END
GO

CREATE PROCEDURE sp_pesquisa_avancada_pontos_sensoriamento
(
	@helixId VARCHAR(100),
	@ativo varchar(1),
	@usuarioId VARCHAR(MAX),
	@estadoId VARCHAR(MAX),
	@cidadeId VARCHAR(MAX)
)
AS
BEGIN

	SELECT p.*, 
	u.nome_completo as 'nome_completo_usuario',
	c.descricao as 'cidade_descricao',
	c.codigo_cidade,
	e.id as 'estado_atendido_id',
	e.descricao as 'estado_descricao',
	e.uf as 'estado_uf',
	E.codigo_estado
	FROM [dbo].[pontos_sensoriamento] p
	INNER JOIN [dbo].[usuarios] u ON u.id = p.usuario_id 
	INNER JOIN [dbo].[cidades_atendidas] c ON c.id = p.cidade_atendida_id
	INNER JOIN [dbo].[estados_atendidos] e ON e.id = c.estado_atendido_id
	WHERE p.helix_id  LIKE '%' + @helixId +'%'
	AND p.ativo_helix LIKE '%' + @ativo +'%'
	AND p.usuario_id LIKE '%' + @usuarioId +'%'
	AND p.cidade_atendida_id   LIKE '%' + @cidadeId +'%'
	AND c.estado_atendido_id LIKE '%' + @estadoId +'%'

END
GO

CREATE PROCEDURE sp_consulta_ponto_de_sensoriamento_por_helixid
(
	@helixid VARCHAR(100)
)
AS
BEGIN
	SELECT * FROM [dbo].[pontos_sensoriamento] p
	WHERE p.helix_id = @helixid
END
GO

CREATE PROCEDURE sp_listar_pontos_por_cidade
(
	@cidade_atendida_id INT
)
AS 
BEGIN

	SELECT p.*, 
	u.nome_completo as 'nome_completo_usuario',
	c.descricao as 'cidade_descricao',
	c.codigo_cidade,
	e.id as 'estado_atendido_id',
	e.descricao as 'estado_descricao',
	e.uf as 'estado_uf',
	E.codigo_estado
	FROM [dbo].[pontos_sensoriamento] p
	INNER JOIN [dbo].[usuarios] u ON u.id = p.usuario_id 
	INNER JOIN [dbo].[cidades_atendidas] c ON c.id = p.cidade_atendida_id
	INNER JOIN [dbo].[estados_atendidos] e ON e.id = c.estado_atendido_id
	WHERE p.cidade_atendida_id = @cidade_atendida_id

END
GO

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

-- SP's Sensoriamento Atual


CREATE PROCEDURE sp_consulta_sensoriamentoAtual_por_pontoDeSensoriamentoId
(
	@ponto_sensoriamento_id INT
)
AS
BEGIN
	SELECT * FROM [dbo].[sensoriamento_atual] s
	WHERE s.ponto_sensoriamento_id = @ponto_sensoriamento_id
END
GO


CREATE PROCEDURE sp_update_sensoriamento_atual
(
	@id INT,
	@ponto_sensoriamento_id INT,
	@nivel_pluviosidade DECIMAL(6,2),
	@vazao_da_agua DECIMAL(6,2),
	@altura_agua DECIMAL(6,2)
)
AS
BEGIN
	UPDATE [dbo].[sensoriamento_atual] SET
	nivel_pluviosidade = @nivel_pluviosidade, 
	altura_agua = @altura_agua,
	vazao_agua = @vazao_da_agua,
	ultima_atualizacao = GETDATE()
	WHERE id = @id AND ponto_sensoriamento_id = @ponto_sensoriamento_id
END
GO

CREATE PROCEDURE sp_listar_sensoriamento_atual_por_cidade
(
	@cidade_atendida_id INT
)
AS 
BEGIN

	SELECT s.*, 
	p.latitude,
	p.longitude,
	p.cidade_atendida_id,
	p.helix_id
	FROM [dbo].[sensoriamento_atual] s
	INNER JOIN [dbo].[pontos_sensoriamento] p ON p.id = s.ponto_sensoriamento_id
	WHERE p.cidade_atendida_id = @cidade_atendida_id

END
GO

CREATE PROCEDURE sp_listar_sensoriamento_atual
AS 
BEGIN

	SELECT s.*, 
	p.latitude,
	p.longitude,
	p.cidade_atendida_id,
	p.helix_id
	FROM [dbo].[sensoriamento_atual] s
	INNER JOIN [dbo].[pontos_sensoriamento] p ON p.id = s.ponto_sensoriamento_id

END
GO

-- SP's Parametros Notificação

CREATE PROCEDURE sp_insert_notificacoes_parametros
(
	@ponto_sensoriamento_id INT,
	@tipo_risco INT,
	@nivel_pluviosidade DECIMAL(6,2),
	@vazao_da_agua DECIMAL(6,2),
	@altura_agua DECIMAL(6,2)
)
AS
BEGIN
	INSERT INTO [dbo].[notificacoes_parametros]
	(tipo_risco, nivel_pluviosidade, vazao_agua, altura_agua, ponto_sensoriamento_id)
	VALUES
	(@tipo_risco,
	@nivel_pluviosidade, 
	@vazao_da_agua,
	@altura_agua,
	@ponto_sensoriamento_id)
END
GO


CREATE PROCEDURE sp_listar_notificacoes_parametros_por_pds
(
	@ponto_sensoriamento_id INT
)
AS
BEGIN
	SELECT * FROM [dbo].[notificacoes_parametros] WHERE ponto_sensoriamento_id = @ponto_sensoriamento_id
END
GO

CREATE PROCEDURE sp_listar_sensoriamento_atual
AS 
BEGIN

	SELECT s.*, 
	p.latitude,
	p.longitude,
	p.cidade_atendida_id,
	p.helix_id
	FROM [dbo].[sensoriamento_atual] s
	INNER JOIN [dbo].[pontos_sensoriamento] p ON p.id = s.ponto_sensoriamento_id

END
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

	DECLARE @cidade_atendida_id INT
	DECLARE @ponto_sensoriamento_id INT

	SELECT @ponto_sensoriamento_id = id, @cidade_atendida_id = cidade_atendida_id FROM deleted

	DELETE FROM [dbo].[sensoriamento_atual]  
		WHERE [ponto_sensoriamento_id] = @ponto_sensoriamento_id

	DELETE FROM [dbo].[notificacoes_historico]  
		WHERE [ponto_sensoriamento_id] = @ponto_sensoriamento_id

	DELETE FROM [dbo].[pontos_sensoriamento]
		WHERE id = @ponto_sensoriamento_id

	if (SELECT COUNT(p.id) FROM [dbo].[pontos_sensoriamento] p WHERE p.cidade_atendida_id = @cidade_atendida_id) = 0 BEGIN
		DELETE [dbo].[cidades_atendidas] 
			WHERE id = @cidade_atendida_id
	END

	SET NOCOUNT OFF
END
GO


CREATE TRIGGER trg_atualizaDependenciasDoPontoDeSensoriamento ON [dbo].[pontos_sensoriamento]
FOR UPDATE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @cidade_atendida_id INT = (SELECT cidade_atendida_id FROM deleted)

	if (SELECT COUNT(p.id) FROM [dbo].[pontos_sensoriamento] p WHERE p.cidade_atendida_id = @cidade_atendida_id) = 0 BEGIN
		DELETE [dbo].[cidades_atendidas] 
			WHERE id = @cidade_atendida_id
	END

	SET NOCOUNT OFF
END
GO


CREATE TRIGGER trg_excluiEstadoCasoNaoHajaCidades ON [dbo].[cidades_atendidas]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @estado_atendido_id INT 
	DECLARE @cidade_atendida_id INT

	SELECT @estado_atendido_id = estado_atendido_id, @cidade_atendida_id = id FROM deleted

	if (SELECT COUNT(p.id) FROM [dbo].[pontos_sensoriamento] p WHERE p.cidade_atendida_id = @cidade_atendida_id) > 0 BEGIN
		ROLLBACK TRANSACTION
		RETURN
	END

	UPDATE [dbo].[usuarios] SET
	cidade_atendida_id = null,
	primeiro_login = 1 
	WHERE cidade_atendida_id = @cidade_atendida_id

	DELETE FROM [dbo].[cidades_atendidas]
		WHERE id = @cidade_atendida_id

	if (SELECT COUNT(c.id) FROM [dbo].[cidades_atendidas] c WHERE c.estado_atendido_id = @estado_atendido_id) = 0 BEGIN
		DELETE FROM [dbo].[estados_atendidos]  
			WHERE id = @estado_atendido_id
	END
	SET NOCOUNT OFF
END
GO

-- exec sp_insert_usuarios 'Admin', 'admin@admin.com', '123456', 2, 1
-- exec sp_insert_estados_atendidos 'São Paulo', 'SP', '35'
-- exec sp_insert_cidades_atendidas 'São Bernardo do Campo', '3548708', 1
-- exec sp_insert_pontos_sensoriamento 'urn:ngsi-ld:entity:001', 0, 1, -23.7360896, -46.5825083, 1
