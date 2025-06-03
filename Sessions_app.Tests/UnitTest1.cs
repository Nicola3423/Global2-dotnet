using Sessions_app.Models;
using System.ComponentModel.DataAnnotations;
using Xunit;
using System.Collections.Generic;
using System.Linq;

namespace Sessions_app.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Usuario_Deve_Ser_Valido_Quando_Preenchido_Corretamente()
        {
            var usuario = new Usuario
            {
                Nome = "João da Silva",
                Email = "joao@email.com"
            };

            var result = ValidaModelo(usuario);

            Assert.Empty(result); 
        }

        [Fact]
        public void Usuario_Deve_Falhar_Quando_Nome_E_Email_Estao_Vazios()
        {
            var usuario = new Usuario();

            var result = ValidaModelo(usuario);

            Assert.Contains(result, x => x.MemberNames.Contains("Nome"));
            Assert.Contains(result, x => x.MemberNames.Contains("Email"));
        }

        [Fact]
        public void Risco_Deve_Ser_Valido_Quando_Preenchido_Corretamente()
        {
            var risco = new Risco
            {
                Descricao = "Deslizamento de terra",
                Nivel = 3
            };

            var result = ValidaModelo(risco);

            Assert.Empty(result);
        }

        [Fact]
        public void RotaSegura_Deve_Falhar_Se_Localizacao_Nao_For_Preenchida()
        {
            var rota = new RotaSegura
            {
                Coordenadas = "-23.5555,-46.6666"
            };

            var result = ValidaModelo(rota);

            Assert.Contains(result, x => x.MemberNames.Contains("Localizacao"));
        }

        [Fact]
        public void Lista_De_Links_Deve_Ser_Inicializada()
        {
            var usuario = new Usuario();
            var risco = new Risco();
            var rota = new RotaSegura();

            Assert.NotNull(usuario.Links);
            Assert.NotNull(risco.Links);
            Assert.NotNull(rota.Links);
        }

        private List<ValidationResult> ValidaModelo(object modelo)
        {
            var context = new ValidationContext(modelo);
            var results = new List<ValidationResult>();
            Validator.TryValidateObject(modelo, context, results, true);
            return results;
        }
    }
}