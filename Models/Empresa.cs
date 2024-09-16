using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sprint1_2semestre.Models
{
    public class Empresa
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 caracteres")]
        public string Cnpj { get; set; } = string.Empty;

        [Required(ErrorMessage = "O email é obrigatório")]
        [EmailAddress(ErrorMessage = "Formato de email inválido")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "A senha é obrigatória")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter no mínimo 6 caracteres")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O setor de atuação é obrigatório")]
        public string SetorAtuacao { get; set; } = string.Empty;

        // Ignorar KPIs na serialização para evitar ciclos de referência
        [JsonIgnore]
        public List<KPI> KPIs { get; set; } = new List<KPI>();
    }
}
