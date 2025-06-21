-- Garante que estás a usar a base de dados correta
USE OfiPecas;
GO

-- 1. APAGA DADOS ANTIGOS (OPCIONAL: Executa esta parte se quiseres começar do zero)
-- A ordem é importante para respeitar as chaves estrangeiras.
DELETE FROM dbo.ITEM_ENCOMENDA;
DELETE FROM dbo.ITEM_CARRINHO;
DELETE FROM dbo.ENCOMENDA;
DELETE FROM dbo.CARRINHO;
DELETE FROM dbo.PECA;
DELETE FROM dbo.CATEGORIA;
GO

-- Reinicia os contadores de ID para que comecem em 1 novamente
DBCC CHECKIDENT ('dbo.ITEM_ENCOMENDA', RESEED, 0);
DBCC CHECKIDENT ('dbo.ENCOMENDA', RESEED, 0);
DBCC CHECKIDENT ('dbo.ITEM_CARRINHO', RESEED, 0);
DBCC CHECKIDENT ('dbo.CARRINHO', RESEED, 0);
DBCC CHECKIDENT ('dbo.PECA', RESEED, 0);
DBCC CHECKIDENT ('dbo.CATEGORIA', RESEED, 0);
GO


-- 2. INSERIR 5 NOVAS CATEGORIAS
INSERT INTO dbo.CATEGORIA (nome) VALUES
('Travões'),          -- ID 1
('Filtros e Óleos'),  -- ID 2
('Motor e Ignição'),  -- ID 3
('Suspensão e Direção'), -- ID 4
('Sistema Elétrico'); -- ID 5
GO


-- 3. INSERIR 10 NOVAS PEÇAS
-- Usamos a mesma imagem placeholder para todos os testes
DECLARE @img VARBINARY(MAX) = 0x47494638396101000100800000000000FFFFFF21F90401000000002C00000000010001000002024401003B;

INSERT INTO dbo.PECA (nome, preco, estoque, id_categoria, imagem) VALUES
-- Categoria: Travões (ID 1)
('Kit Discos de Travão Brembo Max', 112.50, 15, 1, @img),
('Pastilhas de Travão ATE Ceramic', 58.90, 40, 1, @img),

-- Categoria: Filtros e Óleos (ID 2)
('Filtro de Ar MANN-FILTER', 18.20, 150, 2, @img),
('Óleo de Motor Castrol EDGE 5W-30 (5L)', 45.99, 80, 2, @img),

-- Categoria: Motor e Ignição (ID 3)
('Vela de Ignição NGK Iridium', 12.75, 200, 3, @img),
('Kit de Correia de Distribuição Gates', 145.00, 25, 3, @img),

-- Categoria: Suspensão e Direção (ID 4)
('Amortecedor Dianteiro Bilstein B4', 89.90, 30, 4, @img),
('Braço de Suspensão Lemförder', 65.00, 50, 4, @img),

-- Categoria: Sistema Elétrico (ID 5)
('Bateria Varta Blue Dynamic 74Ah', 95.50, 60, 5, @img),
('Alternador Bosch', 230.00, 10, 5, @img);
GO

PRINT '5 categorias e 10 peças foram inseridas com sucesso!';
SELECT * FROM dbo.CATEGORIA;
SELECT * FROM dbo.PECA;
GO