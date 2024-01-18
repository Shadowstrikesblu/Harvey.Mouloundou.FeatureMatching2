using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
namespace Harvey.Mouloundou.FeatureMatching.Tests;

public class FeatureMatchingUnitTest
{
    [Fact]
    public async Task ObjectShouldBeDetectedCorrectly()
    {
        var executingPath = GetExecutingPath();
        var imageScenesData = new List<byte[]>();
        foreach (var imagePath in Directory.EnumerateFiles(Path.Combine(executingPath,
                     "Scenes")))
        {
            var imageBytes = await File.ReadAllBytesAsync(imagePath);
            imageScenesData.Add(imageBytes);
        }

        var objectImageData = await File.ReadAllBytesAsync(Path.Combine(executingPath,
            "Mouloundou-Harvey-object.png"));
        var detectObjectInScenesResults = await new
            ObjectDetection().DetectObjectInScenesAsync(objectImageData, imageScenesData);

        Assert.Equal("[{\"X\":1216,\"Y\":2200},{\"X\":333,\"Y\":3231},{\"X\":1145,\"Y\":3900},{\"X\":1912,\"Y\":2793}]\n",JsonSerializer.Serialize(detectObjectInScenesResults[0].Points));

        Assert.Equal("[{\"X\":1050,\"Y\":1921},{\"X\":1495,\"Y\":3119},{\"X\":2460,\"Y\":2721},{\"X\":1877,\"Y\":1553}]\n",JsonSerializer.Serialize(detectObjectInScenesResults[1].Points));
    }

    private static string GetExecutingPath()
    {
        var executingAssemblyPath = Assembly.GetExecutingAssembly().Location;
        var executingPath = Path.GetDirectoryName(executingAssemblyPath);
        return executingPath;
    }
}