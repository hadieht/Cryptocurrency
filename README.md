# Cryptocurrency Info

> A simple console app for show cryptocurrency in other currenciess

- [Cryptocurrency Info ](#truelayer-hachernews)
  - [Prerequisites](#prerequisites)
  - [Build with](#build-with)
  - [Settings](#settings)
  - [Running the app](#running-the-app)
  - [Docker](#docker)
## Prerequisites

To Build this application you need the following:

- [Visual Studio 2019 (Latest version)](https://visualstudio.microsoft.com/) installed on your development machine. This app Run with .Net 5

## Build with
- [.Net 5](https://dotnet.microsoft.com/download/dotnet/5.0)

## Settings
- API's URL for find Latest Price and Supportive Currencies exist in appsettings.json file .


## Running the app

To run the app, follow the steps.

1 - Open Command Prompt and change the path to [application execution folder](./Cryptocurrency-Executable)

2 - Run the application by the following command

```
Cryptocurrency.exe 

```

## Docker

Target OS: Linux

Build : 
```
docker build -t cryptoimage:latest . 
```

Run :
```
docker run -it --rm cryptoimage:latest

```

