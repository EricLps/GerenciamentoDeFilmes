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