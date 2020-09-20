using Microsoft.Extensions.Configuration;

namespace TemplateFormattedConfiguration
{
    public class TemplateFormattedConfiguration
    {
        public TemplateFormattedConfigurationSettings TemplatedConfigurationSettings { get; }

        public IConfiguration Configuration { get; }

        public TemplateFormattedConfiguration(TemplateFormattedConfigurationSettings templatedConfigurationSettings, IConfiguration configuration)
        {
            TemplatedConfigurationSettings = templatedConfigurationSettings;
            Configuration = configuration;
        }

        public void Run()
        {
            new TemplateFormattedConfigurationProvider(this).Load();
        }

    }
}
