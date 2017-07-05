# CQRS Tutorial

This code is just me following along with the [cqrs.nu tutorial](http://cqrs.nu/tutorial) around event sourcing and CQRS.

# Setup

* Open PowerShell (as an Administrator)
* Install [Chocolatey](https://chocolatey.org/)
* Run setup.ps1

This sets up a [local management console](http://localhost:15672/) where you can inspect / manipulate RabbitMQ messages and queues etc - you can log in with username and password both set as "guest".

# Database

* Add a new database called CQRSTutorial to your local SQL Server instance.
* Run the scripts in Scripts\, as per the order of the file names.

I will look to set up an automated database deployment powershell script at a later stage - a quick google found [this](https://github.com/pnowosie/Simple-Migration/blob/master/migrate.ps1) (might or might be suitable - not my focus for now)