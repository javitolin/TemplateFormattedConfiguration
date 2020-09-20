namespace TemplateFormattedConfiguration
{
    public class TemplateFormattedConfigurationSettings
    {
        public char TemplateCharacterStart { get; set; } = '{';

        public char TemplateCharacterEnd { get; set; } = '}';

        public char EscapeTemplateCharacter { get; set; } = '\\';

        public bool RemoveEscapedCharacters { get; set; } = true;

        public bool ThrowIfNotFound { get; set; } = true;
    }
}
