USE OfiPecas;
GO

-- 1) Inserir um administrador
INSERT INTO dbo.UTILIZADOR
    (username, email, senha, chave_recuperacao, is_admin, endereco, nome_empresa, telefone)
VALUES
    (
      'admin01', 
      'admin@ofipecas.local', 
      'HASH_DA_SENHA',              -- ex: resultado de AuthService.HashPassword("Admin!2025")
      'RKABCDEFGHIJ12345678',       -- 20 caracteres
      1,                            -- is_admin
      NULL,                         -- sem endereço
      NULL,                         -- sem nome_empresa
      NULL                          -- sem telefone
    );
GO

-- 2) Inserir um cliente
INSERT INTO dbo.UTILIZADOR
    (username, email, senha, chave_recuperacao, is_admin, endereco, nome_empresa, telefone)
VALUES
    (
      'cliente01', 
      'cliente@exemplo.com', 
      'HASH_DA_SENHA',              -- ex: resultado de AuthService.HashPassword("Cli#2025Senha")
      'ZYXWVUTSRQPONMLKJIHG',       -- 20 caracteres
      0,                            -- is_admin = false
      'Rua das Flores, 123',        -- endereço obrigatório
      'Ofi Serviços Lda',           -- nome_empresa obrigatório
      '912345678'                   -- telefone opcional
    );
GO
