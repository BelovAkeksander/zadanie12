using System;
using System.IO;
using Microsoft.ML;
using Microsoft.ML.Data;

namespace YourNamespace
{
    public class ModelInput
    {
        [ColumnName("ImageSource"), LoadColumn(0)]
        public string ImageSource { get; set; }

        // Добавьте другие свойства модели ввода, если это необходимо
    }

    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

        // Добавьте другие свойства модели вывода, если это необходимо
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Загружаем модель
                MLContext mlContext = new MLContext();
                string modelPath = Path.Combine(AppContext.BaseDirectory, "models", "C:\\Users\\Alex\\Desktop\\zadanie\\MLModel1.mlnet"); // Проверьте правильный формат файла

                // Проверьте существование файла перед загрузкой
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"Модель не найдена по пути: {modelPath}");
                }

                ITransformer mlModel;
                using (var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    mlModel = mlContext.Model.Load(stream, out var modelInputSchema);
                }

                // Создаем прогнозировщик
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

                // Загружаем изображение для теста
                var imageData = new ModelInput()
                {
                    ImageSource = "C:\\Users\\Alex\\Desktop\\dataset\\13\\icons8-13-48.png" // Убедитесь, что путь к изображению правильный
                };

                // Делаем предсказание
                var prediction = predEngine.Predict(imageData);

                // Выводим результат
                Console.WriteLine($"Predicted digit: {prediction.PredictedLabel}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Произошла ошибка: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
            }
        }
    }
}