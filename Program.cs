using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _13NET.ProjetoOraculo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Respostas");

            string connectionString = (args.Length > 0) ? args[0] : "localhost";
            var botRespostas = new BotResposta(connectionString);


            for (int i = 1; ; i++)
            {
                var pergunta = botRespostas.EsperarPerguntaAsync(i).Result;

                if (pergunta == null)
                {
                    Console.WriteLine("Nenhuma pergunta");
                }
                else
                {

                    Console.WriteLine($"  - [{pergunta.ID}] : {pergunta.Texto}");

                    Console.WriteLine("Dê uma resposta para a pergunta: ");

                    string texto = Console.ReadLine();

                    if (texto == null || texto == "")
                        break;

                    botRespostas.Responder(i, "GRUPO1", texto);

                }

                Console.WriteLine("----------------------------");
            }


        }
    }
}
