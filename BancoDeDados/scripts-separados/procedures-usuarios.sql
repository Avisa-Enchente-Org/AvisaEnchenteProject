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