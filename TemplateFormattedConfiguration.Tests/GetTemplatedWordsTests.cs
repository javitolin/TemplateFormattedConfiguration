using System.Collections.Generic;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace TemplateFormattedConfiguration.Tests
{
    public class GetTemplatedWordsTests
    {
        private TemplateFormattedConfigurationProvider _provider;
        public GetTemplatedWordsTests()
        {
            _provider = new TemplateFormattedConfigurationProvider(
                new TemplateFormattedConfiguration(new TemplateFormattedConfigurationSettings
                    {
                        EscapeTemplateCharacter = '\\',
                        TemplateCharacterEnd = '}',
                        TemplateCharacterStart = '{',
                        RemoveEscapedCharacters = true
                    },
                    new ConfigurationRoot(new List<IConfigurationProvider>())));
        }

        private void GetAllTemplatedWords(string stringToFind, List<string> found)
        {
            MethodInfo getAllTemplaedWordsInfo = _provider.GetType().GetMethod("GetAllTemplatedWords",
                BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.NotNull(getAllTemplaedWordsInfo);

            getAllTemplaedWordsInfo.Invoke(_provider, new object[] { stringToFind, found });
        }

        [Fact]
        public void GetTemplatedWords_Ok()
        {
            var stringToFind = "asaaa{hello}World{hi}";
            var found = new List<string>();
            GetAllTemplatedWords(stringToFind, found);
            Assert.Collection(found, 
                word => Assert.Equal("hello", word),
                word => Assert.Equal("hi", word));
        }
        
        [Fact]
        public void GetTemplatedWords_Escaped()
        {
            var stringToFind = "asaaa\\{hello}World{hi}";
            var found = new List<string>();
            GetAllTemplatedWords(stringToFind, found);
            Assert.Collection(found, 
                word => Assert.Equal("hi", word));
        }
        
        [Fact]
        public void GetTemplatedWords_EscapedEnd()
        {
            var stringToFind = "asaaa{hello\\}World{hi}";
            var found = new List<string>();
            GetAllTemplatedWords(stringToFind, found);
            Assert.Collection(found, 
                word => Assert.Equal("hi", word));
        }
        
        [Fact]
        public void GetTemplatedWords_EscapedBoth()
        {
            var stringToFind = "asaaa\\{hello\\}World{hi}";
            var found = new List<string>();
            GetAllTemplatedWords(stringToFind, found);
            Assert.Collection(found, 
                word => Assert.Equal("hi", word));
        }
        
        [Fact]
        public void GetTemplatedWords_None()
        {
            var stringToFind = "asaaa";
            var found = new List<string>();
            GetAllTemplatedWords(stringToFind, found);
            Assert.Empty(found);
        }

        [Fact]
        public void GetTemplatedWords_AllTemplated()
        {
            var stringToFind = "{hello}";
            var found = new List<string>();
            GetAllTemplatedWords(stringToFind, found);
            Assert.Collection(found,
                word => Assert.Equal("hello", word));
        }
    }
}