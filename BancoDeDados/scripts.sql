/*********************************************************
       PROJETO: ALERTA ENCHENTE !
 *                                                       *
       AUTORES: NOME                           RA
                Abrão Astério Jr.              081200035
                Alexandre Bezerra              081200034
                Cayo Vinicius Neves Magalhães  081200050
                Daniel Santos de Sousa         081200021
                Francisco Tommasi Silveira     081200018
 *                                                       *
      MAIS INFO: Projeto Multidisciplinar desenvolvido
                 para o 5o Semestre do Curso de Eng. da
                 Computação da Faculdade Eng. Salvador
                 Arena.
                 O código foi feito para uso em um
                 dispositivo NodeMCU 1.0 (ESP12F)
 *********************************************************/

/*
	IMPORTANTE:
	Se for a primeira vez que irÁ utilizar esse script,
	remova o comentário da Criação do Banco de Dados logo abaixo
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
	[imagem_perfil] VARBINARY(max),

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

CREATE TABLE [dbo].[registros_sensoriamento](
	[id] INT IDENTITY(1,1) NOT NULL,
	[ponto_sensoriamento_id] INT NULL,
	[nivel_pluviosidade] decimal(6,2) NOT NULL,
	[vazao_agua] decimal(6,2) NOT NULL,
	[altura_agua] decimal(6,2) NOT NULL,
	[data_registro] DATETIME NOT NULL,	
	[tipo_risco] INT NOT NULL DEFAULT 0,

	CONSTRAINT [pk_registros_sensoriamento_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_registros_sensoriamento_ponto_sensoriamento_id] FOREIGN KEY([ponto_sensoriamento_id])
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

CREATE TABLE [dbo].[notificacoes_historico](
	[id] INT IDENTITY(1,1) NOT NULL,
	[registro_sensoriamento_id] INT NOT NULL,

	CONSTRAINT [pk_notificacoes_historico_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_notificacoes_historico_registros_sensoriamento_id] FOREIGN KEY([registro_sensoriamento_id])
        REFERENCES [dbo].[registros_sensoriamento]([id])
)
GO

-- VIEWS

CREATE VIEW vw_usuarios AS
	SELECT u.id, u.nome_completo, u.email, u.tipo_usuario, u.cidade_atendida_id, u.primeiro_login, u.imagem_perfil FROM usuarios u
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

CREATE PROCEDURE sp_update_imagem_perfil
(
	@id int,
	@imagem_perfil VARBINARY(max)
)
AS
BEGIN
	UPDATE usuarios SET
	imagem_perfil = @imagem_perfil
	WHERE id = @id
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

-- SP's Registros Sensoriamento


CREATE PROCEDURE sp_insert_registros_sensoriamento
(
	@ponto_sensoriamento_id INT,
	@nivel_pluviosidade DECIMAL(6,2),
	@vazao_da_agua DECIMAL(6,2),
	@altura_agua DECIMAL(6,2)
)
AS
BEGIN
	INSERT INTO [dbo].[registros_sensoriamento]
	(nivel_pluviosidade, altura_agua, vazao_agua, ponto_sensoriamento_id, data_registro)
	VALUES
	(@nivel_pluviosidade, @altura_agua, @vazao_da_agua, @ponto_sensoriamento_id, GETDATE())
END
GO

CREATE PROCEDURE sp_consulta_sensoriamentoAtual_por_pontoDeSensoriamentoId
(
	@ponto_sensoriamento_id INT
)
AS
BEGIN
	SELECT TOP 1 * FROM [dbo].[registros_sensoriamento] s
	WHERE s.ponto_sensoriamento_id = @ponto_sensoriamento_id
	ORDER BY data_registro DESC
END
GO

CREATE PROCEDURE sp_consulta_registros_sensoriamento_diario_por_pontoDeSensoriamentoId
(
	@ponto_sensoriamento_id INT
)
AS
BEGIN
	SELECT * FROM [dbo].[registros_sensoriamento] s
	WHERE s.ponto_sensoriamento_id = @ponto_sensoriamento_id 
	AND (datediff(dd,data_registro, getdate()) = 0)
END
GO

CREATE PROCEDURE sp_consulta_media_sensoriamento_de_15__porpontoDeSensoriamentoId
(
	@ponto_sensoriamento_id INT
)
AS
BEGIN
	SELECT AVG(s.vazao_agua) as 'media_vazao', AVG(s.altura_agua) as 'media_altura', AVG(s.nivel_pluviosidade) as 'media_chuva', DAY(data_registro) as 'dia' from registros_sensoriamento s
	WHERE s.ponto_sensoriamento_id = @ponto_sensoriamento_id
	AND (datediff(dd, s.data_registro, getdate()) <= 16)
	GROUP BY DAY(s.data_registro)
END
GO

CREATE PROCEDURE sp_consulta_ultimos_50_registros_sensoriamento_por_pontoDeSensoriamentoId
(
	@ponto_sensoriamento_id INT
)
AS
BEGIN
	SELECT TOP 50 * FROM [dbo].[registros_sensoriamento] s
	WHERE s.ponto_sensoriamento_id = @ponto_sensoriamento_id 
	AND (datediff(dd,data_registro, getdate()) = 0)
	ORDER BY data_registro DESC
END
GO

CREATE PROCEDURE sp_listar_sensoariamento_atual_por_cidade
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
	INTO #temp FROM [dbo].[registros_sensoriamento] s
	INNER JOIN [dbo].[pontos_sensoriamento] p ON p.id = s.ponto_sensoriamento_id WHERE 1 = 2

	DECLARE @ponto_sensoriamento_id INT;

	DECLARE cursor_pds CURSOR STATIC FORWARD_ONLY FOR
	SELECT id FROM [dbo].[pontos_sensoriamento] WHERE cidade_atendida_id = @cidade_atendida_id

	OPEN cursor_pds
	FETCH NEXT FROM cursor_pds INTO @ponto_sensoriamento_id

	WHILE @@fetch_Status = 0 
	BEGIN
		
		INSERT INTO #temp
		SELECT TOP 1 s.*, 
		p.latitude,
		p.longitude,
		p.cidade_atendida_id,
		p.helix_id
		FROM [dbo].[registros_sensoriamento] s
		INNER JOIN [dbo].[pontos_sensoriamento] p ON p.id = s.ponto_sensoriamento_id WHERE p.id = @ponto_sensoriamento_id
		ORDER BY s.data_registro DESC
		
	FETCH NEXT FROM cursor_pds INTO @ponto_sensoriamento_id
	END

	CLOSE cursor_pds
	DEALLOCATE cursor_pds

	SELECT * FROM #temp

END
GO

CREATE PROCEDURE sp_pesquisa_avancada_alertas_risco
(
	@ponto_sensoriamento_id VARCHAR(MAX),
	@tipo_risco VARCHAR(MAX),
	@estadoId VARCHAR(MAX),
	@cidadeId VARCHAR(MAX)
)
AS
BEGIN

	SELECT s.*,
	p.helix_id
	FROM [dbo].[notificacoes_historico] n
	INNER JOIN [dbo].[registros_sensoriamento] s ON s.id = n.registro_sensoriamento_id
	INNER JOIN [dbo].[pontos_sensoriamento] p ON p.id = s.ponto_sensoriamento_id
	INNER JOIN [dbo].[cidades_atendidas] c ON c.id = p.cidade_atendida_id
	WHERE s.ponto_sensoriamento_id  LIKE '%' + @ponto_sensoriamento_id +'%'
	AND s.tipo_risco LIKE '%' + @tipo_risco +'%'
	AND p.cidade_atendida_id   LIKE '%' + @cidadeId +'%'
	AND c.estado_atendido_id LIKE '%' + @estadoId +'%'

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

CREATE PROCEDURE sp_update_notificacoes_parametros
(
	@id INT,
	@ponto_sensoriamento_id INT,
	@tipo_risco INT,
	@nivel_pluviosidade DECIMAL(6,2),
	@vazao_da_agua DECIMAL(6,2),
	@altura_agua DECIMAL(6,2)
)
AS
BEGIN
	UPDATE [dbo].[notificacoes_parametros]
	SET
	tipo_risco = @tipo_risco,
	nivel_pluviosidade = @nivel_pluviosidade, 
	vazao_agua = @vazao_da_agua,
	altura_agua = @altura_agua,
	ponto_sensoriamento_id = @ponto_sensoriamento_id
	WHERE id = @id
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


--SP's Notificações histórico

CREATE PROCEDURE sp_consulta_ultimos_50_alertas_sensoriamento_por_pontoDeSensoriamentoId
(
	@ponto_sensoriamento_id INT
)
AS
BEGIN
	SELECT TOP 50 s.* FROM [dbo].[notificacoes_historico] n
	INNER JOIN [dbo].[registros_sensoriamento] s ON s.id = n.registro_sensoriamento_id
	WHERE s.ponto_sensoriamento_id = @ponto_sensoriamento_id 
	AND (datediff(dd, s.data_registro, getdate()) = 0)
	ORDER BY s.data_registro DESC
END
GO



--TRIGGERS

CREATE TRIGGER trg_criaDependenciasDoPontoDeSensoriamento ON [dbo].[pontos_sensoriamento]
FOR INSERT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @ponto_sensoriamento_id INT = (SELECT id FROM inserted)

	INSERT INTO [dbo].[registros_sensoriamento] 
	(ponto_sensoriamento_id, nivel_pluviosidade, vazao_agua, altura_agua, tipo_risco, data_registro)
	VALUES 
	(@ponto_sensoriamento_id, 0.0, 0.0, 0.0, 0, GETDATE())

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

	DELETE FROM [dbo].[registros_sensoriamento]  
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

ALTER TRIGGER trg_excluiDependenciasRegistroSensoriamento ON [dbo].[registros_sensoriamento]
INSTEAD OF DELETE
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @registro_sensoriamento_id INT
	


	DECLARE cursor_notificacoes CURSOR STATIC FORWARD_ONLY FOR
	SELECT id FROM deleted

	OPEN cursor_notificacoes
	FETCH NEXT FROM cursor_notificacoes INTO @registro_sensoriamento_id

	WHILE @@fetch_Status = 0 
	BEGIN

		DELETE FROM [dbo].[notificacoes_historico]  
			WHERE [registro_sensoriamento_id] = @registro_sensoriamento_id

		DELETE FROM [dbo].[registros_sensoriamento]  
			WHERE id = @registro_sensoriamento_id
		
	FETCH NEXT FROM cursor_notificacoes INTO @registro_sensoriamento_id
	END

	CLOSE cursor_notificacoes
	DEALLOCATE cursor_notificacoes

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

CREATE TRIGGER trg_atualizaTipoDoRisco ON [dbo].[registros_sensoriamento]
INSTEAD OF INSERT
AS
BEGIN
	SET NOCOUNT ON

	DECLARE @registro_sensoriamento_id INT;
	DECLARE @ponto_sensoriamento_id INT;

	DECLARE @ultimo_tipo_risco INT;
	DECLARE @tipo_risco_atual INT;
	DECLARE @grau_atual decimal(6,2);
	DECLARE @grau_param decimal(6,2);
	DECLARE @tipo_risco_param decimal(6,2);

	SELECT
		@ponto_sensoriamento_id = ponto_sensoriamento_id, 
		@grau_atual = ((vazao_agua * 2) + (altura_agua) + (nivel_pluviosidade))
	from inserted;

	DECLARE cursor_parametros CURSOR STATIC FORWARD_ONLY FOR
	SELECT ((vazao_agua * 2) + (altura_agua) + (nivel_pluviosidade)), tipo_risco FROM [dbo].[notificacoes_parametros]  WHERE ponto_sensoriamento_id = @ponto_sensoriamento_id

	SET @tipo_risco_atual = 0;

	OPEN cursor_parametros
	FETCH NEXT FROM cursor_parametros INTO @grau_param, @tipo_risco_param

	WHILE @@fetch_Status = 0 
	BEGIN

		IF (@grau_atual >= @grau_param) BEGIN
			SET @tipo_risco_atual = @tipo_risco_param
		END
		
	FETCH NEXT FROM cursor_parametros INTO @grau_param, @tipo_risco_param
	END

	CLOSE cursor_parametros
	DEALLOCATE cursor_parametros

	SELECT TOP 1 @ultimo_tipo_risco = tipo_risco FROM [dbo].[registros_sensoriamento] s
	WHERE s.ponto_sensoriamento_id = @ponto_sensoriamento_id 
	ORDER BY s.data_registro DESC

	INSERT INTO [dbo].[registros_sensoriamento] 
	(nivel_pluviosidade, altura_agua, vazao_agua, ponto_sensoriamento_id, data_registro, tipo_risco)
	SELECT nivel_pluviosidade, altura_agua, vazao_agua, ponto_sensoriamento_id, data_registro, @tipo_risco_atual FROM inserted;
	IF (@ultimo_tipo_risco <> @tipo_risco_atual AND @tipo_risco_atual <> 0 ) BEGIN

		INSERT INTO [dbo].[notificacoes_historico] 
		(registro_sensoriamento_id)
		SELECT MAX(id) FROM registros_sensoriamento
	END

	SET NOCOUNT OFF
END
GO


-- exec sp_insert_usuarios 'Admin', 'admin@admin.com', '123456', 2, 1
-- exec sp_insert_estados_atendidos 'São Paulo', 'SP', '35'
-- exec sp_insert_cidades_atendidas 'São Bernardo do Campo', '3548708', 1
-- exec sp_insert_pontos_sensoriamento 'urn:ngsi-ld:sensor:001', 0, 1, -23.7360896, -46.5825083, 1


-- SELECT * FROM pontos_sensoriamento
-- SELECT * FROM notificacoes_parametros 
-- SELECT * FROM registros_sensoriamento
-- SELECT * FROM notificacoes_historico


