using Entities.Notificacoes;
using System.ComponentModel.DataAnnotations;

namespace Entities.Entidades
{
    public class Base : Notifica
    {
        [Display(Name = "Codigo")]
        public int Id { get; set; }
        [Display(Name = "Name")]
        public string Nome { get; set; }
    }
}
