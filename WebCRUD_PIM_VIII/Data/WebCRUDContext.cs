using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebCRUD_PIM_VIII.Models;

namespace WebCRUD_PIM_VIII.Data
{
    public class WebCRUDContext : DbContext
    {
        public WebCRUDContext (DbContextOptions<WebCRUDContext> options)
            : base(options)
        {
        }

        public DbSet<Pessoa> Pessoa { get; set; }

        public DbSet<Endereco> Endereco { get; set; }

        public DbSet<Telefone> Telefone { get; set; }

        public DbSet<TipoTelefone> TipoTelefone { get; set; }
    }
}
