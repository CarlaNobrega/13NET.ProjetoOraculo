using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using StackExchange.Redis;
using System.Threading.Tasks;

namespace _13NET.ProjetoOraculo
{
    public class BotResposta
    {
        const string CHANNEL_PERGUNTAS = "perguntas";
        private readonly IDatabase _db;
        private readonly ISubscriber _pub;


        public BotResposta(string connectionString)
        {
            var client = ConnectionMultiplexer.Connect(connectionString);
            var db = client.GetDatabase();
            var pub = client.GetSubscriber();

            this._db = db;
            this._pub = pub;
        }

        public void Responder(int id, string nomeGrupo, string texto)
        {
            string idPergunta = $"P{id}";
            
            bool wasSet = _db.HashSet(idPergunta, nomeGrupo, texto, When.NotExists);
        }

        public async Task<Pergunta> EsperarPerguntaAsync(int id)
        {            
            await Task.Delay(1000);

            var pergunta = LerPergunta(id);
            if (pergunta != null)
            {
                return pergunta;
            }
            
            while (pergunta == null)
            {
                await Task.Delay(2000);
                pergunta = LerPergunta(id);
            }            

            return pergunta;
        }

        private Pergunta LerPergunta(int id)
        {
            string perguntaId = "P" + id.ToString();

            var perguntaCompleta = _db.StringGet(perguntaId).ToString();

            if (string.IsNullOrEmpty(perguntaCompleta))
            {
                return null;
            }

            var perguntaSplit = perguntaCompleta.Split(':');           

            Pergunta pergunta = new Pergunta { ID = perguntaSplit[0], Texto = perguntaSplit[1] };

            return pergunta;
        }
    }
}
