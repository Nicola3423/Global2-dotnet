using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Sessions_app.Models
{
    public class Risco
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A descrição é obrigatória")]
        [StringLength(500, ErrorMessage = "A descrição deve ter no máximo 500 caracteres")]
        public string Descricao { get; set; }

        public int Nivel { get; set; }

        [NotMapped]
        [ValidateNever]
        [JsonIgnore]
        public List<Link> Links { get; set; } = new List<Link>(); // Inicialize a lista aqui
    }
}