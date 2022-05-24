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