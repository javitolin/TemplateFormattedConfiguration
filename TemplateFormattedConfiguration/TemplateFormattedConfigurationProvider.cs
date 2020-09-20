using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TemplateFormattedConfiguration
{
    public class TemplateFormattedConfigurationProvider
    {
        private readonly Stack<string> _context = new Stack<string>();
        private readonly IConfiguration _configuration;
        private readonly char _startChar;
        private readonly char _endChar;
        private readonly char _escapeChar;
        private readonly bool _removeEscapeCharacters;
        private readonly bool _throwIfNotFound;

        private string _currentPath;

        public TemplateFormattedConfigurationProvider(TemplateFormattedConfiguration templateFormattedConfigurationSource)
        {
            _configuration = templateFormattedConfigurationSource.Configuration;
            _startChar = templateFormattedConfigurationSource.TemplatedConfigurationSettings.TemplateCharacterStart;
            _endChar = templateFormattedConfigurationSource.TemplatedConfigurationSettings.TemplateCharacterEnd;
            _escapeChar = templateFormattedConfigurationSource.TemplatedConfigurationSettings.EscapeTemplateCharacter;
            _removeEscapeCharacters = templateFormattedConfigurationSource.TemplatedConfigurationSettings.RemoveEscapedCharacters;
            _throwIfNotFound = templateFormattedConfigurationSource.TemplatedConfigurationSettings.ThrowIfNotFound;
        }

        public void Load()
        {
            foreach (var child in _configuration.GetChildren())
                ParseTemplatedConfiguration(child);
        }

        private void ParseTemplatedConfiguration(IConfigurationSection child)
        {
            EnterContext(child.Key);
            if (child.Value != null)
            {
                var found = new List<string>();
                GetAllTemplatedWords(child.Value, found);

                var currentValue = _configuration[_currentPath];
                foreach (var templateKey in found)
                {
                    var toReplace = _startChar + templateKey + _endChar;
                    var newValue = _configuration[templateKey];
                    if (string.IsNullOrWhiteSpace(newValue) && _throwIfNotFound)
                        throw new ArgumentException($"Key [{templateKey}] was not found");

                    currentValue = currentValue.Replace(toReplace, newValue);
                }

                if (_removeEscapeCharacters)
                {
                    var escapedStart = _escapeChar + "" + _startChar;
                    var escapedEnd = _escapeChar + "" + _endChar;
                    currentValue = currentValue.Replace(escapedStart, _startChar.ToString())
                        .Replace(escapedEnd, _endChar.ToString());
                }

                _configuration[_currentPath] = currentValue;
            }

            foreach (var currentChild in child.GetChildren())
                ParseTemplatedConfiguration(currentChild);

            ExitContext();
        }

        private void GetAllTemplatedWords(string stringToFind, List<string> found)
        {
            int indexStart = stringToFind.IndexOf(_startChar);
            if (indexStart < 0)
                return;

            if (indexStart > 0 && stringToFind[indexStart - 1] == _escapeChar)
            {
                GetAllTemplatedWords(stringToFind.Substring(indexStart + 1), found);
                return;
            }

            int indexClose = stringToFind.IndexOf(_endChar);
            if (indexClose < 0)
                return;

            if (indexClose < indexStart)
            {
                GetAllTemplatedWords(stringToFind.Substring(indexClose + 1), found);
                return;
            }

            if (indexClose > 0 && stringToFind[indexClose - 1] == _escapeChar)
            {
                GetAllTemplatedWords(stringToFind.Substring(indexClose + 1), found);
                return;
            }

            string templatedKey = stringToFind.Substring(indexStart + 1,
                indexClose - indexStart - 1);

            found.Add(templatedKey);

            GetAllTemplatedWords(stringToFind.Substring(indexClose + 1), found);
        }

        private void EnterContext(string context)
        {
            _context.Push(context);
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }

        private void ExitContext()
        {
            _context.Pop();
            _currentPath = ConfigurationPath.Combine(_context.Reverse());
        }
    }
}
