using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace Sessions_app.Models
{
    public class RotaSegura
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "A localização é obrigatória")]
        [StringLength(200, ErrorMessage = "A localização deve ter no máximo 200 caracteres")]
        public string Localizacao { get; set; }

        [StringLength(100, ErrorMessage = "As coordenadas devem ter no máximo 100 caracteres")]
        public string Coordenadas { get; set; }

        [NotMapped]
        [ValidateNever]
        [JsonIgnore]
        public List<Link> Links { get; set; } = new List<Link>(); // Inicializada aqui
    }
}