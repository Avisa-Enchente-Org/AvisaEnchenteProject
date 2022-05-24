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

-- exec sp_insert_usuarios 'Admin', 'admin@admin.com', '123456', 2, 1
-- exec sp_insert_estados_atendidos 'São Paulo', 'SP', '35'
-- exec sp_insert_cidades_atendidas 'São Bernardo do Campo', '3548708', 1
-- exec sp_insert_pontos_sensoriamento 'urn:ngsi-ld:sensor:001', 0, 1, -23.7360896, -46.5825083, 1