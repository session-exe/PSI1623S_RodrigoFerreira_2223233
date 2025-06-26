# 🔧 OfiPeças - Gestão de Encomendas para Oficinas

**OfiPeças** é uma aplicação de desktop desenvolvida em **C# com Windows Forms** e **SQL Server**, concebida para modernizar e otimizar o processo de encomenda de peças em oficinas automóveis.

Este projeto foi desenvolvido no âmbito da **Prova de Aptidão Profissional**.

---

## 🚀 Funcionalidades Principais

- **Sistema de Autenticação Seguro**
  - Login, Registo e Recuperação de Password.
  - Passwords armazenadas com *hashing* e *salt*.

- **Dois Níveis de Utilizador**
  - Utilizadores Normais (mecânicos, funcionários)
  - Administradores (acesso a funcionalidades avançadas)

- **Catálogo de Produtos Dinâmico**
  - Exibição de todas as peças da base de dados
  - Pesquisa por nome e filtro por categoria

- **Ciclo de Compra Completo**
  - **Carrinho de Compras**: Adição, gestão de quantidades e remoção de itens
  - **Checkout Simulado**: Formulário com validação de dados
  - **Gestão de Stock**: Stock atualizado automaticamente após compra

- **Área de Cliente**
  - Histórico de encomendas
  - Exportação de faturas em PDF
  - Painel de definições para editar dados e alterar password

- **Painel de Administração Completo**
  - **Gestão de Peças**: CRUD completo + gestão de imagens
  - **Gestão de Categorias**: Adicionar, editar e remover
  - **Gestão de Utilizadores**: Visualização e gestão de permissões

---

## 🛠️ Tecnologias Utilizadas

- **Linguagem**: C# (.NET Framework)  
- **Interface Gráfica**: Windows Forms  
- **Base de Dados**: Microsoft SQL Server  

### 📦 Bibliotecas Externas (NuGet)

- [`Guna.UI2.WinForms`](https://www.nuget.org/packages/Guna.UI2.WinForms) - Interface moderna e apelativa
- [`PDFsharp`](https://www.nuget.org/packages/PDFsharp/) - Geração de documentos PDF
- [`DotNetEnv`](https://www.nuget.org/packages/DotNetEnv/) - Gestão segura de credenciais
- [`Microsoft.Data.SqlClient`](https://www.nuget.org/packages/Microsoft.Data.SqlClient) - Driver de SQL Server

### 🧰 Ferramentas de Desenvolvimento

- Visual Studio 2022  
- SQL Server Management Studio (SSMS)  
- Git & GitHub  

---

## ⚙️ Instalação e Configuração

Siga os passos abaixo para executar o projeto localmente:

### 1. Pré-requisitos

- Visual Studio 2022 com workload de desenvolvimento .NET para desktop
- Instância do Microsoft SQL Server ativa

### 2. Base de Dados

- Abra o **SQL Server Management Studio (SSMS)**
- Execute o script SQL (`script_completo.sql`) incluído no repositório para criar a base de dados `OfiPecas`

### 3. Configuração do Ambiente

- Na raiz do projeto, crie um ficheiro chamado `.env`
- Adicione as credenciais de acesso à base de dados:

```env
DB_SERVER="NOME_DO_SEU_SERVIDOR"
DB_NAME="OfiPecas"
DB_USER="SEU_UTILIZADOR_SQL"
DB_PASSWORD="SUA_PASSWORD_SQL"
```

### 4. Executar a Aplicação

- Abra a solução `.sln` no **Visual Studio**
- Restaure os pacotes NuGet (caso não sejam restaurados automaticamente):
  - Vá a: `Tools > NuGet Package Manager > Manage NuGet Packages for Solution`
  - Clique em **Restore**
- Compile e execute o projeto pressionando **F5**

---

## 🖼️ Screenshots da Aplicação

> ⚠️ Substitui os textos abaixo pelas imagens reais

### 🔐 Ecrã de Login
![Login](https://github.com/user-attachments/assets/a6c5f542-dc9a-4811-9f7c-9384c3521351)


---

### 🛒 Loja Principal
![Loja](https://github.com/user-attachments/assets/48492bde-814e-453e-b386-58a8640fd281)

---

### 🛠️ Painel de Administração
![image](https://github.com/user-attachments/assets/0a8f3c73-1b0a-4bc4-93ca-1b7724efc1d0)

![image](https://github.com/user-attachments/assets/d61db489-3330-4123-8b77-fc0dfcb64c73)

![image](https://github.com/user-attachments/assets/acc7b6b6-d9db-452f-b3bf-e9d8e0add4b3)

---

## 👨‍💻 Desenvolvido por

**Rodrigo Ferreira**  
Projeto realizado no âmbito da **Prova de Aptidão Profissional**
