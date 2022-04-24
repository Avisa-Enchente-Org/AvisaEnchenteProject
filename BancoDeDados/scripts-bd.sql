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
	[tipo_usuario] VARCHAR(60) NOT NULL,
	[cidade_atendida_id] INT NOT NULL,
	[primeiro_login] BIT NOT NULL,

	CONSTRAINT [pk_usuarios_id] PRIMARY KEY([id]),
	CONSTRAINT [fk_usuarios_cidade_atendida_id] FOREIGN KEY([cidade_atendida_id])
        REFERENCES [dbo].[cidades_atendidas]([id])
)
GO
