using InteractiveKernel = Microsoft.DotNet.Interactive.Kernel;
using System.IO;
using System.Text.Json;

public static class Configuration
{
    private const string configFile = "../../examples/config/settings.json";

    public static async Task<OpenAiSettings> GetSettings(OpenAiProvider openAiProvider)
    {
        var settings = new OpenAiSettings();
        if (File.Exists(configFile))
        {
            System.Console.WriteLine($"Reading settings from {configFile}.");
            var json = File.ReadAllText(configFile);
            settings = JsonSerializer.Deserialize<OpenAiSettings>(json);
        }

        if (string.IsNullOrEmpty(settings.ApiKey))
        {
            settings.ApiKey = await InteractiveKernel.GetInputAsync("API key.");
        }

        if (string.IsNullOrEmpty(settings.Deployment))
        {
            settings.Deployment = await InteractiveKernel.GetInputAsync("Model name.");
        }

        if (string.IsNullOrEmpty(settings.Url))
        {
            settings.Url = await InteractiveKernel.GetInputAsync("URL.");
        }

        if (string.IsNullOrEmpty(settings.Organisation) && openAiProvider == OpenAiProvider.OpenAi)
        {
            settings.Organisation = await InteractiveKernel.GetInputAsync("Organisation.");
        }

        var text = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(configFile, text);

        return settings;
    }
}

public sealed class OpenAiSettings
{
    public string ApiKey { get; set; }

    public string Deployment { get; set; }

    public string Url { get; set; }

    public string Organisation { get; set; }
}

public enum OpenAiProvider
{
    Azure,
    OpenAi
}
