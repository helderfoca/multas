using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace multas.Models
{
    public class Agentes
    {

        public Agentes()
        {
            ListaDeMultas = new HashSet<Multas>();
        }


        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ID { get; set; } // chave primária

        [Required(ErrorMessage ="O {0} é de preenchimento obrigatório!")] // o atributo nome é de preenchimento obrigatório
        [RegularExpression("[A-Z][a-záéíóúäëïöüàèìòùâêîôûãõç]+(( | de | da | dos | d'|-| e )[A-Z][a-záéíóúäëïöüàèìòùâêîôûãõç]+){1,3}", 
          ErrorMessage ="O nome apenas aceita letras. Cada nome começa por maiúscula")]
        [StringLength(40)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório!")]
        public string Fotografia { get; set; }

        [Required(ErrorMessage = "O {0} é de preenchimento obrigatório!")]
        [RegularExpression("[A-Za-záéíóúäëïöüàèìòùâêîôûãõç 0-9]+", ErrorMessage ="Escreva um nome aceitável...")]
        public string Esquadra { get; set; }

        // complementar a informação sobre o relacionamento
        // de um Agente com as Multas por ele 'passadas'

        public virtual ICollection<Multas> ListaDeMultas { get; set; }
    }
}