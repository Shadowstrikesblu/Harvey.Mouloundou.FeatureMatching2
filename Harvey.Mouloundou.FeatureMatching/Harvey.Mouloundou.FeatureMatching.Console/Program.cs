using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Mouloundou.FeatureMatching;

namespace Harvey.Mouloundou.FeatureMatching.Console
{
    class Program
    {
        static async Task Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please provide the object image path and the scene images directory as arguments.");
                return;
            }

            var objectImagePath = args[0];
            var sceneImagesDirectory = args[1];

            var objectDetection = new ObjectDetection();

            var objectImageData = await File.ReadAllBytesAsync(objectImagePath);
            var imagesSceneData = new List<byte[]>();

            foreach (var imagePath in Directory.EnumerateFiles(sceneImagesDirectory))
            {
                var imageBytes = await File.ReadAllBytesAsync(imagePath);
                imagesSceneData.Add(imageBytes);
            }

            var results = await objectDetection.DetectObjectInScenesAsync(objectImageData, imagesSceneData);

            foreach (var result in results)
            {
                Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(result.Points));
            }
        }
    }
}