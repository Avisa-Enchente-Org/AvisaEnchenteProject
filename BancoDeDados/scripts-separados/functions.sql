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