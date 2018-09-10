# TickMe
A sample ASP.NET Core application to sell tickets that serves as a base for a Containers Workshop.
This is the first part of the tutorial.
You can find the second part [here](https://github.com/etpedror/TickMeMicroservices), and the final one [here](https://github.com/etpedror/TickMeContainers)
## OVERVIEW
TickMe is a ticket selling website that is growing in popularity. While the current monolithic application still works, to cope with higher loads in an efficient manner, it was decided to go for a microservices architecture and to use containers. 
## INITIAL SETUP
The application runs on Azure, in a webapp, using Azure AD for authentication, CosmosDB to store data and KeyVault to store secrets. As part of the development team, the first step is to recreate the environment.
Your workstation should have installed:
- __Microsoft Visual Studio 2017__, with the options to develop for the web development enabled. It can be downloaded from https://visualstudio.microsoft.com/;
- __Microsoft .Net Core SDK__, the latest version, that can be obtained from https://www.microsoft.com/net/download;
- __Docker__, the selected container solution, which you can download from https://www.docker.com/get-started;
-	__Git for Windows__, obtainable from https://git-scm.com/download/win. While not strictly necessary, it makes things easier.

You’ll also need:
-	An __Azure__ subscription;
-	A __Microsoft account__ (if you don’t have one, you can get it at https://live.com)
- A __Docker Account__ (you can register for one when downloading docker - see above)

---
## SETTING THINGS UP

The initial steps are creating the necessary resources on your Azure subscription to allow for the application to run in its monolithic form and obtain the app from GitHub.

### ON YOUR AZURE SUBSCRIPTION
You’ll need to access your azure subscription and:
#### Azure AD B2C
An Azure AD B2C needs to be created to authenticate to the application (this will allow users to authenticate with their Microsoft accounts, Facebook, Google, etc.)
1. Press __+ Create a resource__ and type _`Azure Active Directory B2C`_ in the search box on the new blade and press Enter.
2. Press __Create__
3. Select __Create a new Azure AD B2C Tenant__
4. Select an organization name and an initial domain – make a note of those values
5. Set the country to __United Kingdom__
6. Press __Create__

#### Register in the Microsoft Application Registration Portal
This application needs to be registered on the [Microsoft Application Registration Portal](https://apps.dev.microsoft.com/), that you can access with your Microsoft account
1. Sign in to the Microsoft Application Registration Portal with your Microsoft account credentials
2. In the upper-right corner, select __Add an app__
3. Provide a Name for your application and click __Create__
4. On the registration page, copy the value of _Application Id_. You will use it to configure your Microsoft account as an identity provider in your tenant.
5. Select __Add platform__, and then and choose __Web__.
6. Enter _`https://login.microsoftonline.com/te/{tenant}.onmicrosoft.com/oauth2/authresp`_ in Redirect URLs. Replace _{tenant}_ with your tenant's name (for example, contosob2c).
7. Select __Generate New Password__ under __Application Secrets__. Copy the new password displayed on screen. You need it to configure a Microsoft account as an identity provider in your tenant. This password is an important security credential.
8. Press __Save__

#### Configure Azure AD B2C to work with Microsoft Accounts
Back on the Azure Portal, make sure you're using the directory that contains your Azure AD B2C tenant by switching to it in the top-right corner of the Azure portal. Select your subscription information, and then select Switch Directory.
1.	Choose __All services__ in the top-left corner of the Azure portal, search for and select __Azure AD B2C__.
2.	Select __Identity providers__, and then select __Add__.
3.	Provide a Name. For example, enter _`MSA`_.
4.	Select __Identity provider type__, select __Microsoft Account__, and click __OK__.
5.	Select __Set up this identity provider__ and enter the _`<Application Id>`_ that you recorded earlier as the __Client ID__ and enter the password that you recorded as the __Client secret__ of the Microsoft account application that you created earlier.
6.	Click __OK__ and then click __Create__ to save your Microsoft account configuration.
7.	Press __Applications__ and select __+ Add__. Give it a name and set __Web App/WebApi__ to __Yes__. Set __Allow Implicit Flow__ to __Yes__. Type _`https://localhost:44391`_ as the __Reply Url__
8. Once the application is created on Azure AD B2C, make a note of the ApplicationID. 
9. Select it and add the following __Reply URLs__ (you might need to go back here and update the url to reflect the port choice of your application):

    - `https://localhost:44391/signin/b2c_1_edit_profile`
    - `https://localhost:44391/signin/b2c_1_sign_up_in`
    - `https://localhost:44391/signin/b2c_1_sign_in`
    - `https://localhost:44391/signin/b2c_1_sign_up`
  
10.	Press __Save__
11.	Press __Sign-up or sign-in policies__, press __+ Add__, set the name to _`B2C_1_sign_up_in`_, select the application that you just created, set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_sign_up_in`_, set the domain to _`login.microsoftonline.com`_, select the Display Name, Given Name and Surname on the __Profile Attributes__ and __Application Claims__ blades and press __Create__
12.	Press __Sign-up or sign-in policies__, press __+ Add__, set the name to _`B2C_1_edit_profile`_, select the application that you just created, set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_edit_profile`_, set the domain to _`login.microsoftonline.com`_, select the Display Name, Given Name and Surname on the __Profile Attributes__ and __Application Claims__ blades and press __Create__
13.	Press __Sign-up or sign-in policies__, press __+ Add__, set the name to _`B2C_1_sign_up`_, select the application that you just created, set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_sign_up`_, set the domain to _`login.microsoftonline.com`_, select the Display Name, Given Name and Surname on the __Profile Attributes__ and __Application Claims__ blades and press __Create__
14.	Press __Sign-up or sign-in policies__, press __+ Add__, set the name to _`B2C_1_sign_in`_, select the application that you just created, set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_sign_in`_, set the domain to _`login.microsoftonline.com`_, select the Display Name, Given Name and Surname on the __Application Claims__ blade and press __Create__
15.	Choose __All services__ in the top-left corner of the Azure portal, search for and select __Azure Active Directory__
16.	Create a user that will allow you to test your application

#### Register your Application with Azure AD

If you switched to another directory on the previous step, repeat the steps to get to the initial directory again

We need to register the application with Azure AD in order for the application to be able to access the resources
1. On the main search box type _`Azure Active Directory`_ and select the option under __Services__
2. Press __App Registrations__ and select __+ New application registration__ on the options
3. Give your application a name, set the type to _"Web app / API”_ and set the Sign-on URL to _'http://localhost'_
4. Access the __App registration__, select __Settings__, press __Keys__ and on the __Passwords__ sections create a new key with the name _'ClientSecret'_, select a duration and press __Save__. Copy the value of the key, as you will need it later and it won't be shown to you again later (if you forget to do it or if you lose it, you can later delete the key and recreate it)

#### Creating the Resource Group
Create a Resource Group
1. Access your azure subscription and press __+ Create a Resource__
2. On the search box that shows up, type _`Resource Group`_
3. Select __Resource Group__, from Microsoft
4. Press __Create__ on the new blade
5. Give it a name, select the subscription and the resource group location (if you are in the UK, _North Europe_ is a good choice)
6. Press __Create__ to create the Resource Group
#### Create the Azure Cosmos DB instance
1. Once the Resource Group is created, access it, and press __+ Add__
2. On the search box that shows up, type _`CosmosDB`_
3. Select __Azure Cosmos DB__, from Microsoft 
4. Press __Create__ on the new blade
5. Give it a name, select _“SQL”_ as the __API__, select the subscription, and under Resource Group, select __“Use existing”__ and on the drop down, select the resource group that was created previously. Don’t forget to set the Location to the appropriate value 
6. Press __Create__ to create the Azure Cosmos DB
7. Once the instance is created, press Keys and make a note of:
   - URI
   - PrimaryKey
8. Press __Data Explorer__ and select __New Database__. Type _`tickme`_ and make a note of it as you will need it later.
9. Click __...__ that shows up when you move the mouse over the newly created database and select New Collection. Type _`events`_ in the __Collection Id__ field, make sure you select __Fixed Size__ and press __OK__. This Collection needs to be populated with some data, so expand the _“events”_ collection, and
  - Press New Document, type
```javascript
{
    "id": "fded6e77-8e72-401a-bbf9-7953f5faa374",
    "title": "Kings of Leon Live",
    "startdate": "2018-08-28T20:00:00Z",
    "duration": "02:00:00",
    "venue": "O2 Arena",
    "price": 50,
    "totalavailabletickets": 1000
}
```
And press __Save__
  - Press New Document, type
```javascript
{
    "id": "e185a588-9fae-4be5-8bb7-d9dfeed6e99b",
    "title": "The Killers Live",
    "startdate": "2018-09-28T20:30:00.000000Z",
    "duration": "02:00:00",
    "venue": "O2 Arena",
    "price": 45,
    "totalavailabletickets": 1000
}
```
And press __Save__
  - Press New Document, type
```javascript
{
    "id": "2e6010f5-ef3f-4c21-98c5-9fb92781281e",
    "title": "Pearl Jam",
    "startdate": "2018-08-31T20:00:00.000000Z",
    "duration": "03:00:00",
    "venue": "Wembley",
    "price": 50,
    "totalavailabletickets": 10000
}
```
And press __Save__
  - Press New Document, type
```javascript
{
    "id": "dbba999c-bec8-497b-a08b-f3bd2b0aff96",
    "title": "Reading Festival",
    "startdate": "2019-08-25T00:00:00.000000Z",
    "duration": "23:59:00",
    "venue": "Reading Festival Grounds",
    "price": 75,
    "totalavailabletickets": 65000
}
```
And press __Save__
  - Press New Document, type
```javascript
{
    "id": "c83333e4-ad45-435c-a28b-f4bf227698bd",
    "title": "NOS Alive",
    "startdate": "2019-07-11T14:00:00.000000Z",
    "duration": "10:00:00",
    "venue": "Lisbon, Portugal",
    "price": 50,
    "totalavailabletickets": 100000
}
```
And press __Save__

10.	Click __...__ that shows up when you move the mouse over the database and select __New Collection__. Type _`user`_ in the __Collection Id__ field, make sure you select __Fixed Size__ and press __OK__
11.	Click __...__ that shows up when you move the mouse over the database and select __New Collection__. Type _`tickets`_ in the __Collection Id__ field, make sure you select __Fixed Size__ and press __OK__

#### Creating the KeyVault
Finally, lets create the Key Vault
1. On the resource group blade, press __+ Add__ again
2. On the search box that shows up, type _`Azure Key Vault`_
3. Select __Azure Key Vault__, from Microsoft 
4. Press __Create__ on the new blade
5. Give it a name, select the subscription and under Resource Group, select __Use existing__ and on the drop down, select the resource group that was created previously. Don’t forget to set the Location to the appropriate value, but you can leave the other options with the defaults
6. Once the KeyVault is created, select __Access Policies__ and press __+ Add__
7. On __Select Principal__, select the __App Registration__ you created previously
8. On __Secret permissions__, check only the __Get__ and __List__ options
9. Leave all the other options as they are and press __OK__
10. Select Secrets and press __+ Generate/Import__, on the Name, type _`DataStore-DatabaseName`_, set the Value to _`<Database created on Azure Cosmos DB>`_ and press __Create__
11.	Select Secrets and press __+ Generate/Import__, on the Name, type _`DataStore-EndpointUri`_, set the Value to _`<URI of the Azure Cosmos DB>`_ and press __Create__
12.	Select Secrets and press __+ Generate/Import__, on the Name, type _`DataStore-PrimaryKey`_, set the Value to _`<Primary Key of the Azure Cosmos DB>`_ and press __Create__
13.	Select Secrets and press __+ Generate/Import__, on the Name, type _`Auth-ClientId`_, set the Value to _`<ApplicationID registered on Azure B2C>`_ and press __Create__
14.	Select Secrets and press __+ Generate/Import__, on the Name, type _`Auth-TenantName`_, set the Value to _`<tenant name of your Azure B2C>`_ and press __Create__

With this, the Azure setup is done for now.

---
### ON YOUR WORKSTATION
You will need to obtain the source code from Git Hub. 
Here’s how to do it:
1. Press the __Windows__ key, and type _`cmd`_
2. Type _`cd\`_ to navigate to the root of your hard drive
3. Type _`md Code`_ to create a folder with the name _“Code”_
4. Type _`cd Code`_ to navigate to the newly created folder
5. Type _`git clone https://github.com/etpedror/TickMe.git`_
6. A folder gets created under _“C:\Code”_ with the name _“TickMe”_, with the code of the initial app;
7. Close the Command window
8. Open __Visual Studio__, select __File__ > __Open__ > __Project/Solution__ and navigate to the newly created folder, selecting the _TickMe_Initial.sln_ file.
The newly opened solution consists of 3 projects, __TestMe__, __TickMe__ and __TickMeHelpers__. The first one is a console app that allows for you to run any custom test, TickMe is the webapp and TickMeHelpers is a set of helper classes that help with common operations.
On the project TickMe, we will need to set some values on the __appsettings.json__ file:
```javascript
"KeyVault": {
    "Vault": "<Name of the Azure Key Vault that was created previously (just the name)>",
    "ClientId": "<The ApplicationID of the Application that was registered previously>",
    "ClientSecret": "<The ClientSecret defined previously>"
  }
```

Run the application! You should be able to see the entry page, but should get a redirect error on login. That is related to the port that your website binds to for https. By now, your URL should look like _`https://localhost:{port}`_. You need to go back to your __Azure AD B2C__ setup and edit the urls so that they have the correct port. In my case, the url is _`https://localhost:44391`_ so I would need to update the urls to 

    - `https://localhost:44391/signin/b2c_1_edit_profile`
    - `https://localhost:44391/signin/b2c_1_sign_up_in`
    - `https://localhost:44391/signin/b2c_1_sign_in`
    - `https://localhost:44391/signin/b2c_1_sign_up`
    
Once you do it, you should be able to run the application and login, as well as see the events and buy tickets.

Proceed to the [Second Part of this Workshop](https://github.com/etpedror/TickMeMicroservices), where we will turn this application into a Microservices oriented application
