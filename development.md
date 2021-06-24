# Development Log

[Back to README](README.md#change-log)

+ 06/15/2021
  + Scaffolded out app pages.
  + Created models, interface, service repository, and transient for the rummy score keeper.
  + Created, seeded and migrated the database.
  + Made a view component for the rummy score sheet.
  + Installed the following packages:
    + `Microsoft.AspNetCore.Mvc.NewtonsoftJson`
    + `Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation`
    + `Microsoft.EntityFrameworkCore`
    + `Microsoft.EntityFrameworkCore.SqlServer`
    + `Microsoft.EntityFrameworkCore.Tools`
    + `Newtonsoft.Json`
    + `System.Configuration.ConfigurationManager`
  + 06/16/2021
    + Moved player names and wins to view component.
    + Built out methods to add scores to the database and to the `PlayerScore List`
    + Wrote logic to add the scores as they are submitted updating the view.  Game resets when a player reaches goal.
    + Added properties and logic to hide and show menu windows at the appropriate times.
    + Made a tracking cookie to keep track of the current users game id.
  + 06/17/2021
    + Stripped some menus and options away to make things feel less clunky.  
    + Finished full CRUD actions for all tables, including score sheet creation and deletion.
    + Scaffolded out the HTML markup for the page layouts.
    + Finished site wide layout CSS.
  + 06/23/2021
    + Finished site wide modules CSS.
    + Built out instructions page.
    + Took one last pass at the over all style and cleaned things up for deployment.

[Back to README](README.md#change-log)