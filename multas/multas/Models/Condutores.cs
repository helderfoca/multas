﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace multas.Models
{
    public class Condutores
    {

        public Condutores()
        {
            ListaDeMultas = new HashSet<Multas>();
        }

        [Key]

        public int ID { get; set; } // chave primária

        // dados próprios do condutor

        public string Nome { get; set; }

        public string BI { get; set; }

        public string Telemovel { get; set; }

        public DateTime DataNascimento { get; set; }

        // dados da carta de condução do condutor

        public string NumCartaConducao { get; set; }

        public string LocalEmissao { get; set; }

        public DateTime DataValidadeCarta { get; set; }

        // complementar a informação sobre o relacionamento
        // de um Condutor com as Multas que recebeu

        public virtual ICollection<Multas> ListaDeMultas { get; set; }

    }
}