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

        
    }

    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public string PredictedLabel { get; set; }

   
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
               
                MLContext mlContext = new MLContext();
                string modelPath = Path.Combine(AppContext.BaseDirectory, "models", "C:\\Users\\Alex\\Desktop\\zadanie\\MLModel1.mlnet"); // Проверьте правильный формат файла

              
                if (!File.Exists(modelPath))
                {
                    throw new FileNotFoundException($"Модель не найдена по пути: {modelPath}");
                }

                ITransformer mlModel;
                using (var stream = new FileStream(modelPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    mlModel = mlContext.Model.Load(stream, out var modelInputSchema);
                }

           
                var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

              
                var imageData = new ModelInput()
                {
                    ImageSource = "C:\\Users\\Alex\\Desktop\\dataset\\13\\icons8-13-48.png"
                };

               
                var prediction = predEngine.Predict(imageData);

            
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
