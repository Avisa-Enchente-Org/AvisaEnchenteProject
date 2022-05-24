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
