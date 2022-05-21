using Integracoes;
using Integracoes.Models.Helix.PontoDeSensoriamento;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MVCAvisaEnchenteProject.Infrastructure.DAO;
using MVCAvisaEnchenteProject.Models.Entidades;
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
        private readonly RegistroSensoriamentoDAO _registrosSensoriamentoDAO;
        private readonly PontoDeSensoriamentoDAO _pontoDeSensoriamentoDAO;

        public RotinaUpdateService(ILogger<RotinaUpdateService> logger)
        {
            _logger = logger;
            _integrecaoHelix = new IntegracaoHelix(new Integracoes.Api.HelixApi());
            _registrosSensoriamentoDAO = new RegistroSensoriamentoDAO();
            _pontoDeSensoriamentoDAO = new PontoDeSensoriamentoDAO();

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
            try
            {
                var listaSensores = _pontoDeSensoriamentoDAO.Listar();
                var consultaEntidadesHelix = await _integrecaoHelix.ListEntities<PontoDeSensoriamentoHelixEntity>();

                foreach (var ls in listaSensores)
                {
                    var sensor = consultaEntidadesHelix.ToList().Where(cEH => cEH.Id == ls.HelixId).FirstOrDefault();

                    _logger.LogInformation("helixid: {sensor}", ls.HelixId);

                    var registroSensoriamento = new RegistroSensoriamento(ls.Id);
                    if (sensor == null)
                    {
                        var randomNum = new Random();
                        registroSensoriamento.AlturaAgua = randomNum.Next(200) + randomNum.NextDouble();
                        registroSensoriamento.NivelPluviosidade = randomNum.Next(200) + randomNum.NextDouble();
                        registroSensoriamento.VazaoDaAgua = randomNum.Next(200) + randomNum.NextDouble();
                    }
                    else
                    {
                        registroSensoriamento.AlturaAgua = sensor.RiverHeight.Value;
                        registroSensoriamento.NivelPluviosidade = sensor.RainIntensity.Value;
                        registroSensoriamento.VazaoDaAgua = sensor.RiverFlowrate.Value;
                    }

                    _logger.LogInformation($"AlturaAgua: {registroSensoriamento.AlturaAgua} - NivelPluviosidade: {registroSensoriamento.NivelPluviosidade} - VazaoDaAgua: {registroSensoriamento.VazaoDaAgua}");

                    _registrosSensoriamentoDAO.Inserir(registroSensoriamento);
                }
            }
            catch (Exception e)
            {
                _logger.LogInformation($"EXCEPTION: {e.Message}");
            }

        }
    }
}
