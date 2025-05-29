-- 1) Criar o banco de dados 
IF DB_ID('OfiPecas') IS NULL
    CREATE DATABASE OfiPecas;
GO

USE OfiPecas;
GO

-- 2) Tabela CLIENTE
CREATE TABLE dbo.CLIENTE
(
    id_cliente    INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_CLIENTE PRIMARY KEY,
    nome          NVARCHAR(100)        NOT NULL,
    email         NVARCHAR(150)        NOT NULL CONSTRAINT UQ_CLIENTE_EMAIL UNIQUE,
    senha         NVARCHAR(255)        NOT NULL
);
GO

-- 3) Tabela CATEGORIA
CREATE TABLE dbo.CATEGORIA
(
    id_categoria  INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_CATEGORIA PRIMARY KEY,
    nome          NVARCHAR(100)        NOT NULL
);
GO

-- 4) Tabela PECA (produto/peça)
CREATE TABLE dbo.PECA
(
    id_peca       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_PECA PRIMARY KEY,
    nome          NVARCHAR(150)        NOT NULL,
    preco         DECIMAL(18,2)        NOT NULL,
    estoque       INT                  NOT NULL CONSTRAINT DF_PECA_ESTOQUE DEFAULT(0),
    id_categoria  INT                  NOT NULL
        CONSTRAINT FK_PECA_CATEGORIA FOREIGN KEY(id_categoria)
        REFERENCES dbo.CATEGORIA(id_categoria)
        ON UPDATE CASCADE
        ON DELETE NO ACTION
);
GO

-- 5) Tabela CARRINHO
CREATE TABLE dbo.CARRINHO
(
    id_carrinho   INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_CARRINHO PRIMARY KEY,
    id_cliente    INT                  NOT NULL
        CONSTRAINT FK_CARRINHO_CLIENTE FOREIGN KEY(id_cliente)
        REFERENCES dbo.CLIENTE(id_cliente)
        ON UPDATE CASCADE
        ON DELETE CASCADE
);
GO

-- 6) Tabela ITEM_CARRINHO
CREATE TABLE dbo.ITEM_CARRINHO
(
    id_item       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_ITEM_CARRINHO PRIMARY KEY,
    id_carrinho   INT                  NOT NULL,
    id_peca       INT                  NOT NULL,
    quantidade    INT                  NOT NULL CONSTRAINT CK_ITEM_CARRINHO_QTD CHECK(quantidade > 0),
    
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
