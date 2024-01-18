namespace Harvey.Mouloundou.FeatureMatching;
using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;


class MainProgram
{
    static async Task Main(string[] args)
    {
        var objectDetection = new ObjectDetection();

        var objectImageData = await File.ReadAllBytesAsync("C:\\Users\\hmoul\\OneDrive\\Documents\\2024\\DOT NET\\Mockup\\Harvey.Mouloundou.FeatureMatching\\Harvey.Mouloundou.FeatureMatching.Tests\\Mouloundou-Harvey-object.jpg");
        var imagesSceneData = new List<byte[]>();

        foreach (var imagePath in Directory.EnumerateFiles("C:\\Users\\hmoul\\OneDrive\\Documents\\2024\\DOT NET\\Mockup\\Harvey.Mouloundou.FeatureMatching\\Harvey.Mouloundou.FeatureMatching.Tests\\Scenes"))
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