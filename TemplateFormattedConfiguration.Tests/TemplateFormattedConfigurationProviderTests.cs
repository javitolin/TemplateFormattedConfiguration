using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace TemplateFormattedConfiguration.Tests
{
    public class TemplateFormattedConfigurationProviderTests
    {
        [Fact]
        public void EnableTemplatedConfiguration_ChangeOneKey()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["ThisKey"] =  "Should be {SomeKey}"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);
            
            IConfiguration configuration = builder.Build();
            configuration.EnableTemplatedConfiguration();
            Assert.Equal("Should be GlobalValue", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_ChangeTwoKeys()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["SomeKey_2"] = "GlobalValue2",
                ["ThisKey"] = "{SomeKey_2} be {SomeKey}"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            configuration.EnableTemplatedConfiguration();
            Assert.Equal("GlobalValue2 be GlobalValue", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_NoChanges()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["SomeKey_2"] = "GlobalValue2",
                ["ThisKey"] = "don't change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            configuration.EnableTemplatedConfiguration();
            Assert.Equal("don't change me", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_EscapedRemoveStart()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["SomeKey_2"] = "GlobalValue2",
                ["ThisKey"] = "\\{SomeKey_2} change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            configuration.EnableTemplatedConfiguration();
            Assert.Equal("{SomeKey_2} change me", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_EscapedRemoveEnd()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["SomeKey_2"] = "GlobalValue2",
                ["ThisKey"] = "{SomeKey_2\\} change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            configuration.EnableTemplatedConfiguration();
            Assert.Equal("{SomeKey_2} change me", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_EscapedRemoveBoth()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["SomeKey_2"] = "GlobalValue2",
                ["ThisKey"] = "\\{SomeKey_2\\} change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            configuration.EnableTemplatedConfiguration();
            Assert.Equal("{SomeKey_2} change me", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_EscapedDontRemove()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["SomeKey_2"] = "GlobalValue2",
                ["ThisKey"] = "\\{SomeKey_2} change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            var settings = new TemplateFormattedConfigurationSettings();
            settings.RemoveEscapedCharacters = false;

            configuration.EnableTemplatedConfiguration(settings);
            Assert.Equal("\\{SomeKey_2} change me", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_ChangeTemplatedCharacters()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["ThisKey"] = "(SomeKey) change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            var settings = new TemplateFormattedConfigurationSettings();
            settings.TemplateCharacterStart = '(';
            settings.TemplateCharacterEnd = ')';

            configuration.EnableTemplatedConfiguration(settings);
            Assert.Equal("GlobalValue change me", configuration["ThisKey"]);
        }

        [Fact]
        public void EnableTemplatedConfiguration_NotFound_ThrowsException()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["ThisKey"] = "{NOTAREALYKEY} change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();

            Assert.Throws<ArgumentException>(() => 
                configuration.EnableTemplatedConfiguration());
        }
        [Fact]
        public void EnableTemplatedConfiguration_NotFound_DontThrowException()
        {
            var keys = new Dictionary<string, string>
            {
                ["SomeKey"] = "GlobalValue",
                ["ThisKey"] = "{NOTAREALYKEY} change me"
            };

            var builder = new ConfigurationBuilder()
                .AddInMemoryCollection(keys);

            IConfiguration configuration = builder.Build();
            var settings = new TemplateFormattedConfigurationSettings();
            settings.ThrowIfNotFound = false;

            configuration.EnableTemplatedConfiguration(settings);
            Assert.Equal(" change me", configuration["ThisKey"]);
        }
    }
}