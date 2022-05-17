using Integracoes;
using Integracoes.Models.Helix.PontoDeSensoriamento;
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
        private Timer _timer = null!;
        private int executionCount = 0;


        public RotinaUpdateService(ILogger<RotinaUpdateService> logger)
        {
            _logger = logger;
            _integrecaoHelix = new IntegracaoHelix(new Integracoes.Api.HelixApi());
            _sensoriamentoAtualDAO = new SensoriamentoAtualDAO();

        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(AtualizarSensoriamentoAtual, null, TimeSpan.Zero, TimeSpan.FromSeconds(60));

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("### Serviço Parando ###");
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private void AtualizarSensoriamentoAtual(object state)
        {
            var count = Interlocked.Increment(ref executionCount);

            _logger.LogInformation(
                "RotinaUpdateServie executando. Count: {Count}", count);
            _timer?.Change(Timeout.Infinite, 0);

            var listaSensores = _sensoriamentoAtualDAO.Listar();
            var consultaEntidadesHelix = _integrecaoHelix.ListEntities<PontoDeSensoriamentoHelixEntity>().Result;

            foreach (var ls in listaSensores)
            {
                var sensor = consultaEntidadesHelix.ToList().Where(cEH => cEH.Id == ls.PontoDeSensoriamento.HelixId).FirstOrDefault();

                _logger.LogInformation("Entidade: {sensor}", sensor);

                if (sensor == null)
                {
                    var randomNum = new Random();
                    ls.AlturaAgua = randomNum.Next() + randomNum.NextDouble();
                    ls.NivelPluviosidade = randomNum.Next() + randomNum.NextDouble();
                    ls.VazaoDaAgua = randomNum.Next() + randomNum.NextDouble();
                }
                else
                {
                    ls.AlturaAgua = sensor.WaterHeight.Value;
                    ls.NivelPluviosidade = sensor.RainIntensity.Value;
                    ls.VazaoDaAgua = sensor.FlowrateWater.Value;

                }

                _logger.LogInformation($"AlturaAgua: {ls.AlturaAgua} - NivelPluviosidade: {ls.NivelPluviosidade} - VazaoDaAgua: {ls.VazaoDaAgua}");

                _sensoriamentoAtualDAO.Atualizar(ls);
            }
        }
    }
}
