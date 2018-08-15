## Overview

This is the home page for the CQSplit nuget package - a library designed to help implement the CQRS pattern. It also includes packages to help implement event sourcing and messaging between services.

This repository contains two parts:
 - The CQSplit library
 - A sample project using this library

## To Run Tests for CQSplit Library

* Install [Docker](https://docs.docker.com/docker-for-windows/install/)
* Open PowerShell and run .\build.ps1 -Target Run-CQSplit-Tests

## To Run Sample Application

* Install [Docker](https://docs.docker.com/docker-for-windows/install/)
* Open PowerShell and run .\Setup.ps1 once, choosing passwords for the different components at the selected prompts
* The setup can be verified by running .\build.ps1 -Target Run-Sample-Application-Tests
* [TODO] Run. \build.ps1 -Target Run-Sample-Application