## Overview

This is the home page for the CQSplit nuget package - a library designed to help implement the CQRS pattern. It also includes packages to help implement event sourcing and messaging between services.

This repository contains two parts:
 - The CQSplit library
 - A sample project using this library. The sample application is very much a work in progress! However it does implement the CQRS pattern using the CQSplit packages.

## One-Time Setup

* Install [Docker](https://docs.docker.com/docker-for-windows/install/)
* Right-click on the Docker system tray icon and select "Switch to Windows Containers"
* Open PowerShell and run .\Setup.ps1 once, choosing passwords for the different components at the selected prompts - these will be stored in a .env file in the root for use by other scripts later. The .env file is in the .gitignore file.

## To Run Tests for CQSplit Library

* Open PowerShell and run .\build.ps1 -Target Run-CQSplit-Tests

## To Run Sample Application

* The setup can be verified by running .\build.ps1 -Target Run-Sample-Application-Tests
* Run. \build.ps1 -Target Run-Sample-Application