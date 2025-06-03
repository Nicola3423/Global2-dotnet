using Sessions_app.Models;
using Sessions_app.Repositories;
using Sessions_app.Services;

namespace Sessions_app.Service
{
    public class RiscoService
    {
        private readonly IRiscoRepository _repository;
        private readonly RiskPredictionService _predictionService;
        private readonly RabbitMqService _rabbitMqService;

        public RiscoService(
            IRiscoRepository repository,
            RiskPredictionService predictionService,
            RabbitMqService rabbitMQService)
        {
            _repository = repository;
            _predictionService = predictionService;
            _rabbitMqService = rabbitMQService;
        }

        public async Task<Risco> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Risco>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task AddAsync(Risco entity)
        {
            if (entity.Nivel == 0)
            {
                entity.Nivel = _predictionService.PredictRiskLevel(entity.Descricao);
            }
            await _repository.AddAsync(entity);

            _rabbitMqService.PublishRiscoCreated(entity);
        }

        public async Task UpdateAsync(Risco entity)
        {
            await _repository.UpdateAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
