using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using WebCRUD_PIM_VIII.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebCRUD_PIM_VIII
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();

            //Endereco endereco = new Endereco { Logradouro = "Rua Tibiriçá", Numero = 22, Cep = 25640130, Bairro = "Jardim", Cidade = "Campinas", Estado = "RJ" };

            //TipoTelefone tipoTelefone1 = new TipoTelefone { Tipo = "Celular" };
            //TipoTelefone tipoTelefone2 = new TipoTelefone { Tipo = "Fixo" };

            //Telefone[] telefones = new Telefone[2];

            //telefones[0] = new Telefone { Numero = 32886787, DDD = 19, TipoTelefone = tipoTelefone1 };
            //telefones[1] = new Telefone { Numero = 928334403, DDD = 19, TipoTelefone = tipoTelefone2 };

            //Pessoa pessoa = new Pessoa { Nome = "Ruan Rodrigues Mendes", CPF = 22278261128, Endereco = endereco, Telefones = telefones};

            //PessoaDAO.Exclua(PessoaDAO.Consulte(42178261122));

            //PessoaDAO.Altere(pessoa);

            //PessoaDAO.Insira(pessoa);

            //Pessoa result = PessoaDAO.Consulte(42178261121);
            //Console.WriteLine($"{result}");
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
