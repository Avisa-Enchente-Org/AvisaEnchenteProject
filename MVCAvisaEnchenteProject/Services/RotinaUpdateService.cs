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
    public class RotinaUpdateService : BackgroundService
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

        protected async override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await AtualizarSensoriamentoAtual();
                await Task.Delay(30000, stoppingToken);
            }
        }

        private async Task AtualizarSensoriamentoAtual()
        {
            var listaSensores = _sensoriamentoAtualDAO.Listar();
            var consultaEntidadesHelix = await _integrecaoHelix.ListEntities<PontoDeSensoriamentoHelixEntity>();

            foreach (var ls in listaSensores)
            {
                var sensor = consultaEntidadesHelix.ToList().Where(cEH => cEH.Id == ls.PontoDeSensoriamento.HelixId).FirstOrDefault();

                _logger.LogInformation("helixid: {sensor}", ls.PontoDeSensoriamento.HelixId);

                if (sensor == null)
                {
                    var randomNum = new Random();
                    ls.AlturaAgua = randomNum.Next(200) + randomNum.NextDouble();
                    ls.NivelPluviosidade = randomNum.Next(200) + randomNum.NextDouble();
                    ls.VazaoDaAgua = randomNum.Next(200) + randomNum.NextDouble();
                    ls.UltimaAtualizacao = DateTime.Now;
                }
                else
                {
                    ls.AlturaAgua = sensor.WaterHeight.Value;
                    ls.NivelPluviosidade = sensor.RainIntensity.Value;
                    ls.VazaoDaAgua = sensor.FlowrateWater.Value;
                    ls.UltimaAtualizacao = DateTime.Now;
                }

                _logger.LogInformation($"AlturaAgua: {ls.AlturaAgua} - NivelPluviosidade: {ls.NivelPluviosidade} - VazaoDaAgua: {ls.VazaoDaAgua}");

                _sensoriamentoAtualDAO.Atualizar(ls);
            }
        }
    }
}
