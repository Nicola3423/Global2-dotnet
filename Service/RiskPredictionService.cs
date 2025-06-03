using Microsoft.ML.Data;
using Microsoft.ML;
using Sessions_app.Models;
using Sessions_app.Data;

namespace Sessions_app.Service
{
    public class RiskPredictionService
    {
        private readonly MLContext _mlContext;
        private ITransformer _model;
        private readonly DataContext _dataContext;

        public RiskPredictionService(DataContext dataContext)
        {
            _mlContext = new MLContext();
            _dataContext = dataContext;
            LoadOrTrainModel();
        }

        private void LoadOrTrainModel()
        {
            var modelPath = "ModeloRisco.zip";

            if (File.Exists(modelPath))
            {
                // Carrega o modelo existente
                _model = _mlContext.Model.Load(modelPath, out _);
            }
            else
            {
                // Treina um novo modelo
                TrainModel();
                SaveModel(modelPath);
            }
        }

        public void TrainModel()
        {
            // Busca dados históricos do banco
            var historicos = _dataContext.Riscos
                .Where(r => r.Nivel > 0)
                .Select(r => new RiskData
                {
                    Descricao = r.Descricao,
                    Nivel = (float)r.Nivel
                })
                .ToList();

            // Se não tiver dados suficientes, usa dados de exemplo
            if (!historicos.Any() || historicos.Count < 10)
            {
                historicos = GetSampleData();
            }

            IDataView trainingData = _mlContext.Data.LoadFromEnumerable(historicos);

            var pipeline = _mlContext.Transforms.Text.FeaturizeText(
                    outputColumnName: "Features",
                    inputColumnName: nameof(RiskData.Descricao))
                .Append(_mlContext.Regression.Trainers.Sdca(
                    labelColumnName: nameof(RiskData.Nivel),
                    maximumNumberOfIterations: 100));

            _model = pipeline.Fit(trainingData);
        }

        private List<RiskData> GetSampleData()
        {
            return new List<RiskData>
            {
                new RiskData { Descricao = "Inundação em área residencial", Nivel = 4 },
                new RiskData { Descricao = "Deslizamento de terra na encosta", Nivel = 5 },
                new RiskData { Descricao = "Incêndio florestal controlado", Nivel = 2 },
                new RiskData { Descricao = "Vazamento químico na indústria", Nivel = 5 },
                new RiskData { Descricao = "Queda de árvore na via pública", Nivel = 3 },
                new RiskData { Descricao = "Alagamento temporário", Nivel = 3 },
                new RiskData { Descricao = "Risco biológico em hospital", Nivel = 4 },
                new RiskData { Descricao = "Obra com risco de queda", Nivel = 4 },
                new RiskData { Descricao = "Tráfego intenso na via principal", Nivel = 2 },
                new RiskData { Descricao = "Erosão na margem do rio", Nivel = 4 },
                new RiskData { Descricao = "Vento forte na área urbana", Nivel = 3 },
                new RiskData { Descricao = "Temperatura extrema no centro", Nivel = 3 }
            };
        }

        private void SaveModel(string modelPath)
        {
            _mlContext.Model.Save(_model, null, modelPath);
        }

        public int PredictRiskLevel(string descricao)
        {
            var predictionEngine = _mlContext.Model.CreatePredictionEngine<RiskData, RiskPrediction>(_model);
            var prediction = predictionEngine.Predict(new RiskData { Descricao = descricao });

            // Garante que o nível fique entre 1-5
            return (int)System.Math.Clamp(prediction.NivelPredito, 1, 5);
        }
    }

    public class RiskData
    {
        public string Descricao { get; set; }
        public float Nivel { get; set; }
    }

    public class RiskPrediction
    {
        [ColumnName("Score")]
        public float NivelPredito { get; set; }
    }
}


