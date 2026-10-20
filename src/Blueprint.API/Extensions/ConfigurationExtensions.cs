using Microsoft.Extensions.Configuration.Json;

namespace Blueprint.API.Extensions;

public static class ConfigurationExtensions
{
    public static void PrintAllSections(this IConfiguration configuration)
    {
        if (configuration is not IConfigurationRoot configRoot)
            return;

        // 1. Filter only JSON files (appsettings) and .env / Environment providers
        var appProviders = configRoot.Providers
            .Where(p => p is JsonConfigurationProvider ||
                        p.GetType().Name.Contains("EnvironmentVariables"))
            .ToList();

        // 2. Collect only keys originating from your JSON files or .env (ignoring raw OS keys)
        // A .env variable in ASP.NET Core uses '__', which maps to a nested key containing ':'
        var allowedKeys = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        foreach (var provider in appProviders)
        {
            if (provider is JsonConfigurationProvider)
            {
                // Add all keys from appsettings files
                foreach (var key in GetKeys(provider, null))
                    allowedKeys.Add(key);
            }
            else
            {
                // From environment variables, keep ONLY project keys (contain ':' like Jwt:Key, ConnectionStrings:...)
                foreach (var key in GetKeys(provider, null))
                {
                    if (key.Contains(':'))
                        allowedKeys.Add(key);
                }
            }
        }

        // 3. Print grouped by root section
        var rootSections = allowedKeys
            .Select(k => k.Split(':')[0])
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(s => s);

        foreach (var sectionKey in rootSections)
        {
            Console.WriteLine($"\n--- [{sectionKey}] ---");

            var sectionKeys = allowedKeys
                .Where(k => k.StartsWith($"{sectionKey}:", StringComparison.OrdinalIgnoreCase) ||
                            k.Equals(sectionKey, StringComparison.OrdinalIgnoreCase))
                .OrderBy(k => k);

            foreach (var key in sectionKeys)
            {
                var value = configuration[key];
                if (!string.IsNullOrEmpty(value))
                {
                    var sourceFile = GetSourceFile(configRoot, key);
                    Console.WriteLine($"{key} = {value} [{sourceFile}]");
                }
            }
        }
    }

    private static string GetSourceFile(IConfigurationRoot root, string key)
    {
        foreach (var provider in root.Providers.Reverse())
        {
            if (provider.TryGet(key, out _))
            {
                if (provider is JsonConfigurationProvider json)
                    return json.Source.Path ?? "appsettings.json";

                if (provider.GetType().Name.Contains("EnvironmentVariables"))
                    return ".env";

                return provider.ToString()!;
            }
        }

        return "Unknown";
    }

    private static IEnumerable<string> GetKeys(IConfigurationProvider provider, string? parentPath)
    {
        var childKeys = provider.GetChildKeys(Enumerable.Empty<string>(), parentPath).Distinct();

        foreach (var childKey in childKeys)
        {
            var fullKey = parentPath == null ? childKey : $"{parentPath}:{childKey}";
            yield return fullKey;

            foreach (var nestedKey in GetKeys(provider, fullKey))
                yield return nestedKey;
        }
    }
}