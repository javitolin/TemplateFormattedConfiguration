using Microsoft.Extensions.Configuration;

namespace TemplateFormattedConfiguration
{
    public static class TemplateFormattedConfigurationExtensions
    {

        public static IConfiguration EnableTemplatedConfiguration(this IConfiguration configuration,
            TemplateFormattedConfigurationSettings settings = null)
        {
            settings ??= new TemplateFormattedConfigurationSettings();
            var formattedConfiguration = new TemplateFormattedConfiguration(settings, configuration);
            formattedConfiguration.Run();
            return configuration;
        }
    }
}
