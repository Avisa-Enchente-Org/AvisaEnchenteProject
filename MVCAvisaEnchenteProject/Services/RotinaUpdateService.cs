using Integracoes;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MVCAvisaEnchenteProject.Services
{
    public class RotinaUpdateService : IHostedService
    {
        private readonly ILogger _logger;
        private readonly IntegracaoHelix _integrecaoHelix;
        private readonly SensoriamentoAtualDAO _sensoriamentoAtualDAO;


        public RotinaUpdateService(ILogger<RotinaUpdateService> logger)
        {
            _logger = logger;
            _integrecaoHelix = new IntegracaoHelix(new Integracoes.Api.HelixApi());
            _sensoriamentoAtualDAO = new SensoriamentoAtualDAO();

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            new Timer(AtualizarSensoriamentoAtual, null,TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Serviço Parando ###");
            _logger.LogInformation($"{DateTime.Now}");
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            
        }

        private void AtualizarSensoriamentoAtual(object state)
        {
            _logger.LogInformation("### Executando Processo ###");
        }
    }
}
