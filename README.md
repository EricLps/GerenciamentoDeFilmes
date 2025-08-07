# 🎬 **FilmesAPI: Sistema de Gerenciamento de Filmes**
Este é um projeto de console .NET Core que demonstra operações CRUD (Create, Read, Update, Delete) em um banco de dados SQL Server Express, utilizando o Entity Framework Core. Este projeto foi desenvolvido como parte de um **Bootcamp da Digital Innovation One (DIO)**.

## 🚀 Como Executar o Projeto
Siga os passos abaixo para configurar e executar a aplicação ``FilmesAPI`` em sua máquina.

**Pré-requisitos**
Certifique-se de ter os seguintes softwares instalados:

- **SQL Server Express**: Uma versão gratuita do SQL Server para desenvolvimento.

  - Download SQL Server Express

- **SQL Server Management Studio (SSMS)**: Ferramenta gráfica para gerenciar bancos de dados SQL Server.

  - Download SSMS

- **.NET SDK 8.0 (LTS)**: O SDK necessário para construir e executar aplicações .NET.

  - Download .NET SDK

- **IDE (JetBrains Rider ou Visual Studio)**: Ambiente de Desenvolvimento Integrado.

  - JetBrains Rider (Recomendado)

  - Visual Studio Community (para Windows)

## 📦 **Configuração do Banco de Dados**
1. **Conecte-se ao SQL Server Express no SSMS**:

  - **Abra o SQL Server Management Studio (SSMS)**.

  - Na janela de conexão, use localhost\SQLEXPRESS como "Nome do servidor" e "Autenticação do Windows". Se encontrar um erro de certificado, marque a caixa "Trust Server Certificate" e tente novamente.

2. **Crie o Banco de Dados ``FilmesDB``**:

  - No "Pesquisador de Objetos", clique com o botão direito em "Bancos de Dados" e selecione "Novo Banco de Dados...".

  - Nomeie o banco de dados como FilmesDB e clique em OK.

3.** Crie a Tabela ``Filmes``**:

  - Expanda ``FilmesDB`` -> ``Tabelas``.

  - Clique com o botão direito em **"Tabelas"** e selecione **"Nova Tabela..."**.

  - Configure as colunas da seguinte forma:
  ```
  Nome da Coluna	Tipo de Dados	Permitir Nulos
  Id	int	Desmarcado
  Titulo	nvarchar(255)	Desmarcado
  Genero	nvarchar(100)	Marcado
  DuracaoEmMinutos	int	Marcado
  ClassificacaoEtaria	nvarchar(50)	Marcado
  Lancamento	datetime	Marcado
  ```

  -  **Defina ``Id`` como Chave Primária**: Clique com o botão direito na margem esquerda da coluna ``Id`` e selecione "Definir Chave Primária".

  - **Configure ``Id`` para Auto-Incremento (Identity)**: Com a coluna ``Id`` selecionada, nas "Propriedades da Coluna" (abaixo), expanda "Especificação da Identidade" e defina ``(Is Identity)`` como ``Yes``.

  - **Importante**: Se você encontrar um erro ao salvar que menciona "recriação de tabela", vá em **Ferramentas (Tools) > Opções (Options...) > Designers > Designers de Tabelas e Bancos de Dados**, e **desmarque** "Evitar salvar alterações que exijam recriação de tabela". Salve a tabela com o nome ``Filmes``. Após salvar, é recomendado re-marcar essa opção.

## 💻 Configuração do Projeto .NET
1. **Clone ou Crie o Projeto**:

  - Se você clonou este repositório, abra-o no Rider/Visual Studio.

  - Se você está criando do zero:

    - No Rider, crie uma "**New Solution**" do tipo "**Console Application**" (C#) e nomeie-a como ``FilmesAPI``.

2. **Instale os Pacotes NuGet**:

  - No "**Solution Explorer**" do seu IDE, clique com o botão direito no projeto ``FilmesAPI``.

  - Selecione "Manage NuGet Packages..." e instale:

    - ``Microsoft.EntityFrameworkCore.SqlServer`` (versão 9.0.8 ou compatível com .NET 8.0)

    - ``Microsoft.EntityFrameworkCore.Design`` (versão 9.0.8 ou compatível com .NET 8.0)

  - Alternativamente, no Terminal na pasta raiz do projeto (``FilmesAPI``), execute:

  ```
  dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 9.0.8
  dotnet add package Microsoft.EntityFrameworkCore.Design --version 9.0.8
  ```

3. **Crie a Pasta ``Models`` e as Classes**:

  - Crie uma pasta chamada ``Models`` dentro do seu projeto ``FilmesAPI``.

  - ``Models/Filme.cs``:
  ```
  using System;
  using System.ComponentModel.DataAnnotations;
  
  namespace FilmesAPI.Models
  {
      public class Filme
      {
          [Key]
          public int Id { get; set; }
          public string Titulo { get; set; }
          public string Genero { get; set; }
          public int? DuracaoEmMinutos { get; set; }
          public string ClassificacaoEtaria { get; set; }
          public DateTime? Lancamento { get; set; }
      }
  }
  ```
  - ``Models/FilmesDbContext.cs``:

  ```
  using Microsoft.EntityFrameworkCore;
  
  namespace FilmesAPI.Models
  {
      public class FilmesDbContext : DbContext
      {
          public FilmesDbContext(DbContextOptions<FilmesDbContext> options) : base(options)
          {
          }
  
          public DbSet<Filme> Filmes { get; set; }
  
          protected override void OnModelCreating(ModelBuilder modelBuilder)
          {
              modelBuilder.Entity<Filme>()
                  .Property(f => f.Titulo)
                  .IsRequired();
          }
      }
  }
  ```
4.** Crie o ``FilmesDbContextFactory.cs`` (para Migrações)**:

  - Na pasta raiz do seu projeto ``FilmesAPI``, crie um novo arquivo:

  - ``FilmesDbContextFactory.cs``:
  ```
  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.Design;
  using FilmesAPI.Models;
  
  namespace FilmesAPI
  {
      public class FilmesDbContextFactory : IDesignTimeDbContextFactory<FilmesDbContext>
      {
          public FilmesDbContext CreateDbContext(string[] args)
          {
              var connectionString = "Server=localhost\\SQLEXPRESS;Database=FilmesDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
              var optionsBuilder = new DbContextOptionsBuilder<FilmesDbContext>();
              optionsBuilder.UseSqlServer(connectionString);
              return new FilmesDbContext(optionsBuilder.Options);
          }
      }
  }
  ```

## 🔄 Migrações do Entity Framework Core
Mesmo tendo criado a tabela manualmente, é importante que o EF Core "saiba" sobre ela através de uma migração.

1. Instale a Ferramenta dotnet-ef (se ainda não tiver):

  - No Terminal (na pasta raiz do projeto ``FilmesAPI``):
  ```
  dotnet tool install --global dotnet-ef
  # ou
  dotnet tool update --global dotnet-ef
  ```

2. **Adicione a Migração Inicial**:

  - No Terminal (na pasta raiz do projeto ``FilmesAPI``):
  ```
  dotnet ef migrations add InitialCreate
  ```

3. **Aplique a Migração ao Banco de Dados**:

  - No Terminal (na pasta raiz do projeto ``FilmesAPI``):
  ```
  dotnet ef database update
  ```
  - **Observação**: Se o comando falhar dizendo que a tabela ``Filmes`` já existe, você tem duas opções:

    - **Opção Recomendada**: Exclua a tabela ``dbo.Filmes`` do seu ``FilmesDB`` no SSMS e rode ``dotnet ef database`` update novamente.

    - **Opção Avançada**: Em alguns cenários, você pode querer forçar o EF a marcar a migração como aplicada sem executá-la (mas para este projeto, a exclusão e recriação é mais simples e garante o controle do EF).

## ▶️ Executando a Aplicação

1.** No JetBrains Rider**:

  - Clique no botão **"Run"** (triângulo verde) na barra de ferramentas superior.

  - Ou clique com o botão direito no projeto ``FilmesAPI`` no "Solution Explorer" e selecione **"Run"**.

2.** Interaja com o Console**:

  - Um menu aparecerá no terminal do Rider.

  - Use a opção ``1`` para adicionar filmes, ``2`` para listar, ``3`` para buscar, ``4`` para atualizar e ``5`` para excluir.

  - Verifique no SSMS (``dbo.Filmes`` -> "Selecionar as 1000 Primeiras Linhas") para confirmar que os dados estão sendo persistidos no banco de dados.

-------

Sinta-se à vontade para explorar e modificar o código!
Desenvolvido por Eric Lopes Winkelmann para um Bootcamp da Digital Innovation One (DIO).
