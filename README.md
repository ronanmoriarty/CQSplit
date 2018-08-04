# CQRS Tutorial

This code is just me following along with the [cqrs.nu tutorial](http://cqrs.nu/tutorial) around event sourcing and CQRS.

# Setup
* Install [Docker](https://docs.docker.com/docker-for-windows/install/)
* Create a .env file in the repository editing the following line:
sa_password=some-password-of-your-choice
* Copy ./src/Cafe/Cafe.Waiter.EventProjecting.Service/appSettings.docker.json.example to ./src/Cafe/Cafe.Waiter.EventProjecting.Service/appSettings.docker.json and edit this new json file to use new usernames and passwords.
* Repeat the above step for ./src/Cafe/Cafe.Waiter.Command.Service/appSettings.docker.json.example and ./src/Cafe/Cafe.Waiter.Web/appSettings.docker.json.example.
* Run 'docker-compose up'
* Run 'Update-Settings-To-Use-Docker-Containers.ps1'
* Run 'docker container list' to verify that 6 containers are running:
  - RabbitMQ server
  - SQL Server to host the CQRSTutorial.Cafe.Waiter.ReadModel database
  - SQL Server to host the CQRSTutorial.Cafe.Waiter.WriteModel database
  - .NET Core service Cafe.Waiter.Command.Service
  - .NET Core service Cafe.Waiter.EventProjecting.Service
  - ASP.NET Core website Cafe.Waiter.Web

# Database

* Add a new database called CQRSTutorial to your local SQL Server instance.
* Run the scripts in Scripts\, as per the order of the file names.

I will look to set up an automated database deployment powershell script at a later stage - a quick google found [this](https://github.com/pnowosie/Simple-Migration/blob/master/migrate.ps1) (might or might be suitable - not my focus for now)

# Testing Setup Configured Correctly
* Open CQRSTutorial.sln and run all tests
* Run the MessageBusEventPublisherTests (won't run automatically when running all tests as they're marked explicit)
