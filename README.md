# Core API Template

Template project of system with Rest Core API, ASP.Net Core MVC Client, Unit test of the API controllers on a local machine with an local database

## Getting Started

Clone or copy the project into Visual Studio.

### Prerequisites

What things you need to install the software and how to install them

```
Visual Studio 2019
```

### Installing

A step by step series of examples that tell you how to get a development env running

CreateLocalDB

```
Run project CreateLocalDB first
```

Run both the projects

```
CoreAPI
CoreMVCClient
```

Get the opened client browser to see the system
## remarks
This system is build on a template of a database api/asp.net MVC client

###Parts not finished:

```
- background task processing system : Asked is to start a task (configured) in a background pool and return the controller call 
Made some preparations but could not get it to work.
The idea is that task are started in a queue and that after start it returns
- Unit test : some unit test were added but they were of the controllers of the template
```
###Architecture
```
Domain layers : Batch, bulk
Vertical : Controllers -> Services - > Contexts (Models)
Service : two methods Check and GetLogs are common for both serivices : they reside under one base interface
Scalability by background pool of tasks
```


## Running the tests

Run the test in CoreXUnitTest

### Break down into end to end tests

Postman test

```
To run Batch tasks (fire and forget ):
POST api/Batch Body : {"id":1,"description":"abc"}
outcome : all tasks will be excuted one by one.
```

### And coding style tests

Explain what these tests test and why

```
Give an example
```

## Deployment

Deploy this on a live system

##  issues on this theme
* - asp.net core 2.x, the setup of the taskqueu - execute in background pattern - [Stackoverflow](https://stackoverflow.com/questions/49813628/run-a-background-task-from-a-controller-action-in-asp-net-core-2) 
* - asp.net core 3.x, issue to apply above for core 3.x [Stackoverflow](https://stackoverflow.com/questions/58436732/migration-to-asp-core-3-how-to-migrate-backround-services-service-added-throu) 
* - start task from hosted background service into the batch/bulk domain [Stackoverflow](https://stackoverflow.com/questions/52020799/net-core-dependency-injection-to-hosted-service) 
* - enum does not convert properly [Stackoverflow](https://stackoverflow.com/questions/58440400/enum-type-no-longer-working-in-net-core-3-0-frombody-request-object) 


## Contributing


## Versioning


## Authors

* **N van Veen ** - *Initial work* - [Fullstackstories](http://fullstackstories.net)


## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments


