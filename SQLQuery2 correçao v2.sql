-- 1) Criar o banco de dados 
IF DB_ID('OfiPecas') IS NULL
    CREATE DATABASE OfiPecas;
GO

USE OfiPecas;
GO

-- 2) Tabela UTILIZADOR 
CREATE TABLE dbo.UTILIZADOR
(
    id_utilizador     INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_UTILIZADOR PRIMARY KEY,
    username          NVARCHAR(50)      NOT NULL CONSTRAINT UQ_UTILIZADOR_USERNAME UNIQUE,
    email             NVARCHAR(150)     NOT NULL CONSTRAINT UQ_UTILIZADOR_EMAIL UNIQUE,
    senha             NVARCHAR(255)     NOT NULL,
    chave_recuperacao NVARCHAR(255)     NOT NULL,
    is_admin          BIT               NOT NULL CONSTRAINT DF_UTILIZADOR_ADMIN DEFAULT 0,
    endereco          NVARCHAR(255)     NULL,
    nome_empresa      NVARCHAR(100)     NULL,
    telefone          NVARCHAR(20)      NULL
);
GO

-- 3) Tabela CATEGORIA
CREATE TABLE dbo.CATEGORIA
(
    id_categoria  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_CATEGORIA PRIMARY KEY,
    nome          NVARCHAR(100)     NOT NULL
);
GO

-- 4) Tabela PECA (com campo BLOB para imagens)
CREATE TABLE dbo.PECA
(
    id_peca       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PECA PRIMARY KEY,
    nome          NVARCHAR(150)     NOT NULL,
    preco         DECIMAL(18,2)     NOT NULL,
    estoque       INT               NOT NULL CONSTRAINT DF_PECA_ESTOQUE DEFAULT 0,
    imagem        VARBINARY(MAX)    NOT NULL,
    id_categoria  INT               NOT NULL
        CONSTRAINT FK_PECA_CATEGORIA FOREIGN KEY(id_categoria)
        REFERENCES dbo.CATEGORIA(id_categoria)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- 5) Tabela CARRINHO (1:1 com UTILIZADOR)
CREATE TABLE dbo.CARRINHO
(
    id_carrinho   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_CARRINHO PRIMARY KEY,
    id_utilizador INT               NOT NULL
        CONSTRAINT FK_CARRINHO_UTILIZADOR FOREIGN KEY(id_utilizador)
        REFERENCES dbo.UTILIZADOR(id_utilizador)
        ON UPDATE CASCADE
        ON DELETE CASCADE,
    -- Garante que cada utilizador tenha apenas um carrinho
    CONSTRAINT UQ_CARRINHO_UTILIZADOR UNIQUE (id_utilizador)
);
GO

-- 6) Tabela ITEM_CARRINHO (1 carrinho : N itens)
CREATE TABLE dbo.ITEM_CARRINHO
(
    id_item       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ITEM_CARRINHO PRIMARY KEY,
    id_carrinho   INT               NOT NULL,
    id_peca       INT               NOT NULL,
    quantidade    INT               NOT NULL CONSTRAINT CK_ITEM_CARRINHO_QTD CHECK(quantidade > 0),
    
    CONSTRAINT FK_ITEMCARRINHO_CARRINHO FOREIGN KEY(id_carrinho)
        REFERENCES dbo.CARRINHO(id_carrinho)
        ON UPDATE CASCADE
        ON DELETE CASCADE,

    CONSTRAINT FK_ITEMCARRINHO_PECA FOREIGN KEY(id_peca)
        REFERENCES dbo.PECA(id_peca)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- Índices adicionais para otimizar pesquisas
CREATE INDEX IX_ITEM_CARRINHO_CARRINHO ON dbo.ITEM_CARRINHO(id_carrinho);
CREATE INDEX IX_PECA_CATEGORIA        ON dbo.PECA(id_categoria);
GO