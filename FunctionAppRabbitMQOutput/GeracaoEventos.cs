using System;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using FunctionAppRabbitMQOutput.Contagem;

namespace FunctionAppRabbitMQOutput
{
    public static class GeracaoEventos
    {
        private const string QUEUE_NAME = "queue-testes";
        private static readonly Contador CONTADOR = new Contador();
        
        [Function("GeracaoEventos")]
        [RabbitMQOutput(QueueName = QUEUE_NAME, ConnectionStringSetting = "RabbitMQConnection")]
        public static ResultadoContador Run([TimerTrigger("*/5 * * * * *")] FunctionContext context)
        {
            var logger = context.GetLogger("GeracaoEventos");

            CONTADOR.Incrementar();
            
            string momento = $"Evento gerado em {DateTime.Now:HH:mm:ss}";
            logger.LogInformation(momento);
            logger.LogInformation($"Valor do contador = {CONTADOR.ValorAtual}");

            return new ()
            {
                ValorAtual = CONTADOR.ValorAtual,
                Local = CONTADOR.Local,
                Kernel = CONTADOR.Kernel,
                Framework = CONTADOR.Framework,
                Mensagem = $"{Environment.GetEnvironmentVariable("Mensagem")} | {momento}"
            };
        }
    }
}