# DynamicRouting
Demo project to illustrate implementation of DynamicRouting solution in REST API

## Basics

Supports .NET 5.0
APIKeyValidation is disabled so you can access WeatherForecast endpoint

To enable APIKeyValidation, you need to uncomment line 59 in Startup.cs. Allowed keys and hosts are set up in appsettings.json for each endpoint.

Before uncommenting line 59, four endpoints avaialble 
    [POST] /linedata
    [POST] /magazinedata
    [POST] /visiondata
    [GET]  /WeatherForecast

Once line 59 is uncommented,[GET] /WeatherForecast will get 404 error because it's not configured in appSettings.json. All [POST] endpoints are still accessible but has to be accessed with the right HOST and XApiKey in header.

### How to run test

1. Start your API
1. Import the postman collection
1. APIKeyValidation is disabled by default
1. Enable by uncommenting the line above and rebuild
1. update test in post script to validate status code.