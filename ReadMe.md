
## Project title
Description.

**Author**: Layla Alvey
**Built with:** .NET Core


## Instructions

**Prerequisites**: 
* Visual Studio
* .Net 4.7

1. Clone the repo
   ```sh
   git clone https://github.com/codinatrix/WeatherReport.git
   ``` 
   or download from https://github.com/codinatrix/WeatherReport/archive/refs/heads/master.zip
2. Open the `WeatherReport` folder
3. Open `WeatherReport.sln`
4. Right click on the solution in Solution Explorer and choose `Properties`.
5. Select `Multiple startup projects` and Choose `Start` as the Action for both projects.
7. Press F5 on your keyboard or click on the green play button at the top of Visual Studio

## Todo
Improvements that should be made to this app, in order of priority:

 - Bundle this into an executable or deployable file, depending on requirements.
 - Interrupt ReadKey() for smoother end to console program.
 - Create a better structure in the console portion of the app so everything isn't in Program.cs.
 - Better ways of storing endpoints. This is simple with appsettings.json in .Net Core, but I need to research how it is done in .Net 4.7.
 - Check if it would be useful or necessary to accept parameters such as period or station that would construct the correct endpoint.
 - More error handling and return more informative error messages from the Web API portion of the app. Inform the client if data was not found for some stations.
 - Validate responses in CLI portion of the app.
 - Add a spinner to the console app while it was busy retrieving data, to show the user that it is working.
 - Move the HTTP logic to its own WeatherReport.Common project.
 - Create a Swagger.

## Contact
[LinkedIn](https://www.linkedin.com/in/laylaalvey/)
Project Link: [https://github.com/codinatrix/WeatherReport](https://github.com/codinatrix/WeatherReport)

