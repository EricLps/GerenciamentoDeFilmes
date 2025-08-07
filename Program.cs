using Microsoft.EntityFrameworkCore;
using FilmesAPI.Models;
using System;
using System.Linq;

namespace FilmesAPI
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionString = "Server=localhost\\SQLEXPRESS;Database=FilmesDB;Trusted_Connection=True;Encrypt=False;TrustServerCertificate=True;";
            var optionsBuilder = new DbContextOptionsBuilder<FilmesDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            Console.WriteLine("--- Aplicação de Gerenciamento de Filmes ---");

            while (true)
            {
                Console.WriteLine("\nEscolha uma opção:");
                Console.WriteLine("1. Adicionar um novo filme");
                Console.WriteLine("2. Listar todos os filmes");
                Console.WriteLine("3. Buscar filme por ID");
                Console.WriteLine("4. Atualizar filme por ID");
                Console.WriteLine("5. Excluir filme por ID");
                Console.WriteLine("0. Sair");
                Console.Write("Opção: ");

                var option = Console.ReadLine();
                Console.WriteLine();

                using (var context = new FilmesDbContext(optionsBuilder.Options))
                {
                    switch (option)
                    {
                        case "1":
                            AdicionarFilme(context);
                            break;
                        case "2":
                            ListarFilmes(context);
                            break;
                        case "3":
                            BuscarFilmePorId(context);
                            break;
                        case "4":
                            AtualizarFilme(context);
                            break;
                        case "5":
                            ExcluirFilme(context);
                            break;
                        case "0":
                            Console.WriteLine("Saindo da aplicação. Até mais!");
                            return;
                        default:
                            Console.WriteLine("Opção inválida. Por favor, tente novamente.");
                            break;
                    }
                }
            }
        }

        static void AdicionarFilme(FilmesDbContext context)
        {
            Console.WriteLine("--- Adicionar Novo Filme ---");
            var filme = new Filme();

            Console.Write("Título: ");
            filme.Titulo = Console.ReadLine();

            Console.Write("Gênero: ");
            filme.Genero = Console.ReadLine();

            Console.Write("Duração em Minutos (opcional): ");
            if (int.TryParse(Console.ReadLine(), out int duracao))
            {
                filme.DuracaoEmMinutos = duracao;
            }

            Console.Write("Classificação Etária (opcional): ");
            filme.ClassificacaoEtaria = Console.ReadLine();

            Console.Write("Data de Lançamento (AAAA-MM-DD, opcional): ");
            if (DateTime.TryParse(Console.ReadLine(), out DateTime lancamento))
            {
                filme.Lancamento = lancamento;
            }

            context.Filmes.Add(filme);
            context.SaveChanges();
            Console.WriteLine($"Filme '{filme.Titulo}' adicionado com sucesso! ID: {filme.Id}");
        }

        static void ListarFilmes(FilmesDbContext context)
        {
            Console.WriteLine("--- Lista de Filmes ---");
            var filmes = context.Filmes.ToList();

            if (!filmes.Any())
            {
                Console.WriteLine("Nenhum filme encontrado.");
                return;
            }

            foreach (var filme in filmes)
            {
                Console.WriteLine($"ID: {filme.Id}, Título: {filme.Titulo}, Gênero: {filme.Genero ?? "N/A"}, Duração: {filme.DuracaoEmMinutos?.ToString() ?? "N/A"} min, Classificação: {filme.ClassificacaoEtaria ?? "N/A"}, Lançamento: {filme.Lancamento?.ToShortDateString() ?? "N/A"}");
            }
        }

        static void BuscarFilmePorId(FilmesDbContext context)
        {
            Console.WriteLine("--- Buscar Filme por ID ---");
            Console.Write("Digite o ID do filme: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var filme = context.Filmes.FirstOrDefault(f => f.Id == id);

                if (filme != null)
                {
                    Console.WriteLine($"\nFilme encontrado:");
                    Console.WriteLine($"ID: {filme.Id}, Título: {filme.Titulo}, Gênero: {filme.Genero ?? "N/A"}, Duração: {filme.DuracaoEmMinutos?.ToString() ?? "N/A"} min, Classificação: {filme.ClassificacaoEtaria ?? "N/A"}, Lançamento: {filme.Lancamento?.ToShortDateString() ?? "N/A"}");
                }
                else
                {
                    Console.WriteLine($"Filme com ID {id} não encontrado.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, digite um número.");
            }
        }

        static void AtualizarFilme(FilmesDbContext context)
        {
            Console.WriteLine("--- Atualizar Filme ---");
            Console.Write("Digite o ID do filme que deseja atualizar: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var filme = context.Filmes.FirstOrDefault(f => f.Id == id);

                if (filme != null)
                {
                    Console.WriteLine($"Filme atual: '{filme.Titulo}'. Digite o novo título (ou Enter para manter):");
                    var novoTitulo = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoTitulo))
                    {
                        filme.Titulo = novoTitulo;
                    }

                    Console.WriteLine($"Gênero atual: '{filme.Genero ?? "N/A"}'. Digite o novo gênero (ou Enter para manter):");
                    var novoGenero = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novoGenero))
                    {
                        filme.Genero = novoGenero;
                    }

                    Console.WriteLine($"Duração atual: {filme.DuracaoEmMinutos?.ToString() ?? "N/A"} min. Digite a nova duração em minutos (ou Enter para manter):");
                    if (int.TryParse(Console.ReadLine(), out int novaDuracao))
                    {
                        filme.DuracaoEmMinutos = novaDuracao;
                    }

                    Console.WriteLine($"Classificação atual: '{filme.ClassificacaoEtaria ?? "N/A"}'. Digite a nova classificação (ou Enter para manter):");
                    var novaClassificacao = Console.ReadLine();
                    if (!string.IsNullOrWhiteSpace(novaClassificacao))
                    {
                        filme.ClassificacaoEtaria = novaClassificacao;
                    }

                    Console.WriteLine($"Lançamento atual: {filme.Lancamento?.ToShortDateString() ?? "N/A"}. Digite a nova data de lançamento (AAAA-MM-DD, ou Enter para manter):");
                    if (DateTime.TryParse(Console.ReadLine(), out DateTime novoLancamento))
                    {
                        filme.Lancamento = novoLancamento;
                    }

                    context.Filmes.Update(filme);
                    context.SaveChanges();
                    Console.WriteLine($"Filme com ID {id} atualizado com sucesso!");
                }
                else
                {
                    Console.WriteLine($"Filme com ID {id} não encontrado.");
                }
            }
            else
            {
                Console.WriteLine("ID inválido. Por favor, digite um número.");
            }
        }

        static void ExcluirFilme(FilmesDbContext context)
        {
            Console.WriteLine("--- Excluir Filme ---");
            Console.Write("Digite o ID do filme que deseja excluir: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var filme = context.Filmes.FirstOrDefault(f => f.Id == id);

                if (filme != null)
                {
                    context.Filmes.Remove(filme);
                    context.SaveChanges();
                    Console.WriteLine($"Filme com ID {id} ('{filme.Titulo}') excluído com sucesso!");
                }
                else
                {
                    Console.WriteLine($"Filme com ID {id} não encontrado.");
                }
            }
            else
            {
                    Console.WriteLine("ID inválido. Por favor, digite um número.");
            }
        }
    }
}