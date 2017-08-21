Epi Info Cloud Data Capture
The vision of the Epi Info™ Cloud Data Capture system is to provide public health 
community a data collection tool that allows authorized public health professionals 
to collect and manage the data on the web and mobile devices on premises and in the 
field in a secure manner.

It is an enterprise grade, secure, scalable and distributed data capture and management 
solution for Internet connected computers and mobile devices. This free and open source 
solution extends Epi Info™ for a variety of public health use cases within an enterprise 
by making forms designed by epidemiologists using Epi Info™ for Windows available on web 
and mobile browsers and allowing authorized users within the organization with varied 
privileges to participate in data collection and management of data stored in a centralized 
database. With the integration of the Epi Info™ Cloud Data Analytics product, real time 
data analysis can be performed on the collected data to make prompt and informed decisions.


## Installing / Getting started
The project orginally started on Codeplex: https://ewe.codeplex.com/
It was migrated to Github on August 21, 2017. 
The repository can be cloned to your machine and opened in Visual Studio. It should be 
able to compile after restoring nuget package and .NET and ASP.NET MVC dependencies.
The database for the project can be found in Epi Info Web Database repository:
https://github.com/Epi-Info/Epi-Info-Database-Web

## Developing
The open source repository is developed and maintained by by CDC's Epi Info team. 
You can contact the Epi Info core team at epiinfo@cdc.gov to become part of the 
team contributing to the project. The project can be forked.

### Deploying / Publishing
The project can be published directly to a web server or Azure website using 
Visual Studio's standard publishing process.
The compiled package for configuring on a VM or Azure website is available to 
download from following page: 
https://www.cdc.gov/epiinfo/support/downloads.html


## Features
The Epi Info™ Cloud Data Capture system enables the following functionalities on the web:
-	Web enablement: Epi Info™ Cloud Data Capture enables forms designed in Epi Info™ 7 on 
	the web and mobile devices for data collection by an organization.
-	Quick response time: A form designed by an Epidemiologist can be immediately made 
	accessible to everyone involved in data collection during a public health event 
	immediately via web or mobile for faster data collection
-	Broader reach: The data can be collected by data collector in the field using mobile 
	devices or on premise using web browser.
-	Multi user data collection: Data can be collected by multiple users at the same time 
	using the system for a single public health event or multiple public health events.
-	Centralized data management: The Epi Info™ Cloud Data Capture system collects and manages 
	data in a single database for all the Epi Info™ forms enabled on the web if configured to 
	do so. It can also be independently deployed for one event with centralized data management 
	capability for single event.
-	Enterprise database integration: The Epi Info™ Cloud Data Capture system can be configured 
	to write the data in real time to Epi Info SQL Server database on the network that represents 
	a database for a specific public health event from the centralized database.
-	Distributed analysis: The data on the network database can be accessed using Epi Info™ 7 or 
	Epi Info™ Web Analytics and Visualization product for real time analytics.
-	Role based access: The Epi Info™ Cloud Data Capture system enables role based access to the 
	system where authorized users of the system can enable Epi Info™ 7 forms on the web, collect 
	data and perform administrative functions.

## Configuration
The configuration details for configuring the Epi Info™ Cloud Data Capture system on server are 
provided in deployment document 'EpiInfoCloudDataCaptureDeployment.pdf' included in the install 
package. The document provides detailed instructions on configuring the system on web server and
the supporting database on SQL Server. The document is available for download at:
ftp://ftp.cdc.gov/pub/software/epi_info/EICDC/EpiInfoCloudDataCaptureDeployment.pdf

## Contributing
If you'd like to contribute, please fork the repository and use a feature
branch. Pull requests are warmly welcome.

## Licensing
The code in this project is licensed under Apache Apache License Version 2.0
