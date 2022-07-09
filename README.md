# TemplateFormattedConfiguration
> An IConfiguration extension for enabling reuse of key-value configuration items using format characters

[![Build Status](https://dev.azure.com/asadodevculture/AsadoDevCultureProjects/_apis/build/status/TemplateFormattedConfiguration-Build?branchName=main)](https://dev.azure.com/asadodevculture/AsadoDevCultureProjects/_build/latest?definitionId=4&branchName=main)

## Installation
```
Install-Package TemplateFormattedConfiguration -Version 1.0
```

## Usage example
1. In your configuration, set values to be {another_key}
2. Call configuration.EnableTemplatedConfiguration();
3. The value of that keys will be replaces by another_key's value

appsettings.json:
{ 
"name" : "Steve",
"last_name" : "Jobs",
"full_name" : "{name} {last_name}"
}

configuration["full_name"] -> "Steve Jobs"

## Contributing
1. Fork it (<https://github.com/javitolin/TemplateFormattedConfiguration/fork>)
2. Create your feature branch (`git checkout -b feature/fooBar`)
3. Commit your changes (`git commit -am 'Add some fooBar'`)
4. Push to the branch (`git push origin feature/fooBar`)
5. Create a new Pull Request

## Meta

[AsadoDevCulture](https://AsadoDevCulture.com) 

[@jdorfsman](https://twitter.com/jdorfsman)

Distributed under the MIT license.
