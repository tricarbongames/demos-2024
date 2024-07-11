# Deck Manager Service

## Overview
This repository contains the source code for the Deck Manager Service.  I started with a .NET 8 REST API template project to allow for easy CI deployments to Azure.  I then implemented more code as needed to meet the requirements from the PDF.  

## How to Use
I used Swagger to generate documentation.  Which can be found here:
```
https://deck-demo.azurewebsites.net/swagger/index.html
```

You can also make a direct call to the service using any REST client of your choice.  For example, the `dealcard` action can be called using this endpoint:
```
https://deck-demo.azurewebsites.net/dealer/dealcard
```
