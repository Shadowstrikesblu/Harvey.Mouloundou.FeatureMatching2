using System;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;
using Harvey.Mouloundou.FeatureMatching;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapPost("/detect", async (HttpContext context) =>
{
    var objectDetection = new ObjectDetection();

    // TODO: Replace with your actual logic for getting the object and scene images
    var objectImageData = await File.ReadAllBytesAsync("C:\\Users\\hmoul\\OneDrive\\Documents\\2024\\DOT NET\\Mockup\\Harvey.Mouloundou.FeatureMatching\\Harvey.Mouloundou.FeatureMatching.Tests\\Mouloundou-Harvey-object.jpg\"");
    var imagesSceneData = new List<byte[]>();

    foreach (var imagePath in Directory.EnumerateFiles("C:\\Users\\hmoul\\OneDrive\\Documents\\2024\\DOT NET\\Mockup\\Harvey.Mouloundou.FeatureMatching\\Harvey.Mouloundou.FeatureMatching.Tests\\Scenes"))
    {
        var imageBytes = await File.ReadAllBytesAsync(imagePath);
        imagesSceneData.Add(imageBytes);
    }

    var results = await objectDetection.DetectObjectInScenesAsync(objectImageData, imagesSceneData);

    await context.Response.WriteAsJsonAsync(results);
});
app.MapPost("/FeatureMatching", async ([FromForm] IFormFileCollection files) =>
{
    if (files.Count != 2)
        return Results.BadRequest();

    using var objectSourceStream = files[0].OpenReadStream();
    using var objectMemoryStream = new MemoryStream();
    objectSourceStream.CopyTo(objectMemoryStream);
    var imageObjectData = objectMemoryStream.ToArray();

    using var sceneSourceStream = files[1].OpenReadStream();
    using var sceneMemoryStream = new MemoryStream();
    sceneSourceStream.CopyTo(sceneMemoryStream);
    var imageSceneData = sceneMemoryStream.ToArray();

    // Your implementation code
    var objectDetection = new ObjectDetection();
    var imagesSceneData = new List<byte[]> { imageSceneData };
    var results = await objectDetection.DetectObjectInScenesAsync(imageObjectData, imagesSceneData);

    if (results.Count == 0)
        return Results.NotFound();

    // The method below allows to return an image from a byte array,
    return Results.File(results[0].ImageData, "image/png");
}).DisableAntiforgery();
app.Run();