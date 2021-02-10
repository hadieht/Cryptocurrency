## 1. How long did you spend on the coding assignment? What would you add to your solution if you had more time? If you didn't spend much time on the coding assignment then use this as an opportunity to explain what you would add.

I spent about 17 hours. If I have time, I’d add middleware for API invoke to record API response time, and then I’d try to find out how much time takes to update the exchange rate of currencies and then I’d use Cache to decrease API call times.

## 2. What was the most useful feature that was added to the latest version of your language of choice? Please include a snippet of code that shows how you've used it.
I think the biggest one is cross-platform .Net core. We can host and run .Net core in Linux, Windows, and Mac without any difficulty. And also use Docker to build and run apps.
Another one is Dependency Injection; we can use it very easily and without using a third-party library in .net core to follow the SOLID principle and clean architecture.

## 3. How would you track down a performance issue in production? Have you ever had to do this?
Yes, I have had to do this before, I start with monitoring the API call middle-ware to find where the problem point is, if it’s in the programming logic, I will be refactoring and consider the performance of code, if it’s related to Database, I would add some index for Tables and may write new SPs with high-performance queries. In the incident that happened to me. We had long response-times and we had to find-out the log in the opening socket in GRPC API. And convert singleton to just open socket one time in microservices.

## 4. What was the latest technical book you have read or tech conference you have been to? What did you learn?
I started to learn about DDD. I had read Exploring CQRS and Event Sourcing (Microsoft). I learn about how it is useful when we use this approach in software that have complex business. It is a very good idea for using in complex business belonging to simple CRUD.

## 5. What do you think about this technical assessment?
It is very good. Because I have time to do what I can without any stress. 

## 6. Please, describe yourself using JSON

```

{
	"Name": "Hadi",
	"LastName": "Ehtearmi",
	"Email": "hadi.ehterami@gmail.com",
	"Birthday": "1984-09-15T00:00:00",
	"Skills": [ "Asp.NET", ".NET Core", "T-SQL", "Scrum", "Microservice", "CI/CD", "Git", "RESTFul", "Docker", "Kubernetes", "SOLID" ],
	"Experirncies": [
		{
			"Title": "Technical Team Lead",
			"CompanyName": "Rayanmehr",
			"StartDate": "2020-03-01T00:00:00",
			"EndDate": "0001-01-01T00:00:00",
			"CurrentlyWorking": true,
			"Responsibilities": [ "Implement infrastructure for android online games with microservice architecture on Kubernetes using asp.net Core, MQTT, Restful API, GRPC", "Start Implement logging in microservices with Elastic and MongoDb and setup monitoring use Prometheus + Grafana for services Health monitor in Kubernetes" ]
		},
		{
			"Title": "Senior .NET Developer | Scrum Master",
			"CompanyName": "Chargoon",
			"StartDate": "2018-09-01T00:00:00",
			"EndDate": "2020-03-01T00:00:00",
			"CurrentlyWorking": false,
			"Responsibilities": [ "Refactoring Logistics Inventory Software using Onion Architecture, REACT, Dapper, MSSQL 2017 and .NET", "Reviewed written codes and designed solutions created by the team members to control the quality of codes and fix business and technical problems " ]
		},
		{
			"Title": ".NET Developer",
			"CompanyName": "Chargoon",
			"StartDate": "2015-03-01T00:00:00",
			"EndDate": "2018-09-01T00:00:00",
			"CurrentlyWorking": false,
			"Responsibilities": [ "Implement Web Service with WCF for communicating with Core-Banking Software to Send accounting documents", "Design and Implement new features with ASP.NET Web-Form, HTML, jQuery, JavaScript, CSS, T-SQL Projects that I contributed to (Inventory, Fixed Assets, Commercial and Purchasing web-apps) " ]
		},
		{
			"Title": "Full-Stack Developer",
			"CompanyName": "Chargoon",
			"StartDate": "2011-06-01T00:00:00",
			"EndDate": "2015-03-01T00:00:00",
			"CurrentlyWorking": false,
			"Responsibilities": null
		},
		{
			"Title": "Full-Stack Developer",
			"CompanyName": "ASD",
			"StartDate": "2008-12-01T00:00:00",
			"EndDate": "2015-06-01T00:00:00",
			"CurrentlyWorking": false,
			"Responsibilities": null
		}
	],
	"Educations": [
		{
			"School": "Islamic Azad University",
			"Degree": "Bachelor of Science",
			"FieldOfStudy": "Computer Software Engineering",
			"StartDate": "2004-01-01T00:00:00",
			"EndDate": "2008-01-01T00:00:00"
		}
	],
	"Languages": [
		{
			"Name": "Azerbaijani",
			"Proficiency": "Native"
		},
		{
			"Name": "English",
			"Proficiency": "Advanced"
		},
		{
			"Name": "Turkish",
			"Proficiency": "Advanced"
		},
		{
			"Name": "Persian",
			"Proficiency": "Bilingual proficiency"
		}
	]
}
```
