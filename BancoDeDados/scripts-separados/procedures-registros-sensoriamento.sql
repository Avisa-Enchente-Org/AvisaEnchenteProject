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

