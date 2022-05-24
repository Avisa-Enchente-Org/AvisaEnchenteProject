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