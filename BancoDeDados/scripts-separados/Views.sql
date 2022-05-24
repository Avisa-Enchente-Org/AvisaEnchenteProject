CREATE VIEW vw_usuarios AS
	SELECT u.id, u.nome_completo, u.email, u.tipo_usuario, u.cidade_atendida_id, u.primeiro_login FROM usuarios u
GO