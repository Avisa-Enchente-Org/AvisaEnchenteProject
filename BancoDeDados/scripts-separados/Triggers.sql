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