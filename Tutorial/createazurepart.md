
# Creating the Azure Items

The initial steps are creating the necessary resources on your Azure subscription to allow for the application to run.

# ON YOUR AZURE SUBSCRIPTION

You’ll need to access your azure subscription and follow the steps below

[Create Azure AD B2C](#azure-ad-b2c)  
[Register an App in the Microsoft Application Registration Portal](#register-in-the-microsoft-application-registration-portal)  
[Configure Azure AD B2C](#configure-azure-ad-b2c)  
[Register your Application with Azure AD](#register-your-application-with-azure-ad)  
[Creating the Resource Group](#creating-the-resource-group)  
[Create the Azure Cosmos DB instance](#create-the-azure-cosmos-db-instance)  
[Creating the KeyVault](#creating-the-keyvault)

---

## Azure AD B2C

An Azure AD B2C needs to be created to authenticate to the application (this will allow users to authenticate with their Microsoft accounts, Facebook, Google, etc.)

  1. Press __+ Create a resource__ and type _`Azure Active Directory B2C`_ in the search box on the new blade and press Enter  

  2. Press __Create__  

  3. Select __Create a new Azure AD B2C Tenant__  

  4. Select an organization name and an initial domain (also refered to as tenant) – make a note of those values on your text document, as B2C.OrganizationName and B2C.Domain, respectively  

  5. Set the country to __United Kingdom__  

  6. Press __Create__  

[Back to top](#creating-the-azure-items)

---

## Register in the Microsoft Application Registration Portal

This application needs to be registered on the [Microsoft Application Registration Portal](https://apps.dev.microsoft.com/) for you to be able to use a Microsoft Accounts as an authentication provider. You can access it with your own Microsoft account (@hotmail.com, @live.com, @outlook.com, etc.)

1. Sign in to the Microsoft Application Registration Portal with your Microsoft account credentials  

2. In the upper-right corner, select __Add an app__  

3. Provide a Name for your application and click __Create__  

4. On the registration page, copy the value of _Application Id_ and make a note of it on your text document, as MSA.ApplicationID. You will use it to configure your Microsoft account as an identity provider in your tenant  

5. Select __Add platform__, and then and choose __Web__  

6. Enter _`https://login.microsoftonline.com/te/{tenant}.onmicrosoft.com/oauth2/authresp`_ in Redirect URLs. Replace _{tenant}_ with your tenant's name (for example, if your AD B2C domain is contosob2c, the whole url would be https://login.microsoftonline.com/te/contosob2c.onmicrosoft.com/oauth2/authresp)  

7. Select __Generate New Password__ under __Application Secrets__. Copy the new password displayed on screen to your text file as MSA.ApplicationSecret (you need it to configure a Microsoft account as an identity provider in your tenant)  

8. Press __Save__

[Back to top](#creating-the-azure-items)

---

## Configure Azure AD B2C

Back on the Azure Portal, make sure you're using the directory that contains your Azure AD B2C tenant by switching to it in the top-right corner of the Azure portal. Select your subscription information, and then select Switch Directory.

1. Choose __All services__ in the top-left corner of the Azure portal, search for and select __Azure AD B2C__  
2. __Setting up AD B2C to work with a Microsoft Account__  

   2.1. Select __Identity providers__, and then select __Add__  

   2.2. Provide a Name. For example, enter _`MSA`_  

   2.3. Select __Identity provider type__, select __Microsoft Account__, and click __OK__  

   2.4. Select __Set up this identity provider__ and enter the _Application Id_ (that you recorded earlier as __MSA.ApplicationID__) and enter the _password_ (that you recorded as the __MSA.ApplicationSecret__) of the Microsoft account application that you created earlier  

   2.5. Click __OK__ and then click __Create__ to save your Microsoft account configuration  

3. __Setting up an account for your application to use Azure AD B2C__  

   3.1. Press __Applications__ and select __+ Add__.  
      - Give it a name (save it in your text file as B2C.Application Name)
      - set __Web App/WebApi__ to __Yes__
      - Set __Allow Implicit Flow__ to __Yes__
      - Type _`https://localhost:44391`_ as the __Reply Url__ (you might need to go back here later and update the url to reflect the port choice of your application)  

   3.2. Once the application is created on Azure AD B2C, make a note of the ApplicationID  

   3.3. Select it and add the following __Reply URLs__ (you might need to go back here later and update the url to reflect the port choice of your application):

    - `https://localhost:44391/signin/b2c_1_edit_profile`
    - `https://localhost:44391/signin/b2c_1_sign_up_in`
    - `https://localhost:44391/signin/b2c_1_sign_in`
    - `https://localhost:44391/signin/b2c_1_sign_up`
  
   3.4. Press __Save__  

   3.5. Press __Sign-up or sign-in policies__  
      - press __+ Add__
      - set the name to _`B2C_1_sign_up_in`_
      - select the application that you just created (You should have saved the name as B2C.ApplicationName)
      - set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_sign_up_in`_
      - set the domain to _`login.microsoftonline.com`_
      - select the Display Name, Given Name and Surname on the __Profile Attributes__ and __Application Claims__ blades
      - press __Create__  

   3.6. Press __Edit Profile policies__  
      - press __+ Add__
      - set the name to _`B2C_1_edit_profile`_
      - select the application that you just created (You should have saved the name as B2C.ApplicationName)
      - set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_edit_profile`_
      - set the domain to _`login.microsoftonline.com`_
      - select the Display Name, Given Name and Surname on the __Profile Attributes__ and __Application Claims__ blades  
      - press __Create__  

   3.7. Press __Sign-up policies__  
      - press __+ Add__
      - set the name to _`B2C_1_sign_up`_
      - select the application that you just created (You should have saved the name as B2C.ApplicationName)
      - set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_sign_up`_
      - set the domain to _`login.microsoftonline.com`_
      - select the Display Name, Given Name and Surname on the __Profile Attributes__ and __Application Claims__ blades
      - press __Create__  

   3.8. Press __Sign-in policies__  
      - press __+ Add__
      - set the name to _`B2C_1_sign_in`_
      - select the application that you just created (You should have saved the name as B2C.ApplicationName)
      - set the __Reply url__ to _`https://localhost:44391/signin/b2c_1_sign_in`_
      - set the domain to _`login.microsoftonline.com`_
      - select the Display Name, Given Name and Surname on the __Application Claims__ blade
      - press __Create__  

   3.9. Choose __All services__ in the top-left corner of the Azure portal, search for and select __Azure Active Directory__  

   3.10. Create a user that will allow you to test your application  

[Back to top](#creating-the-azure-items)

---

## Register your Application with Azure AD

If you switched to another directory on the previous step, repeat the steps to get to the initial directory again

We need to register the application with Azure AD in order for the application to be able to access the resources

1. On the main search box type _`Azure Active Directory`_ and select the option under __Services__  

2. Press __App Registrations__ and select __+ New application registration__ on the options  

3. Give your application a name (save it in your file as AAD.Name), set the type to _"Web app / API”_ and set the Sign-on URL to _'http://localhost'_. 

4. Once the application is saved, copy the Application ID into your file and save it as AAD.ApplicationID

4. Access the __App registration__
   - select __Settings__
   - press __Keys__
   - on the __Passwords__ sections 
      - create a new key with the name _'ClientSecret'_
      - select a duration and press __Save__
      - Copy the value of the key to your file and save it as AAD.ClientSecret, as you will need it later and it won't be shown to you again later (if you forget to do it or if you lose it, you can later delete the key and recreate it)  

[Back to top](#creating-the-azure-items)

---

## Creating the Resource Group

Create a Resource Group

1. Access your azure subscription and press __+ Create a Resource__  

2. On the search box that shows up, type _`Resource Group`_  

3. Select __Resource Group__, from Microsoft  

4. Press __Create__ on the new blade  

5. Give it a name and save it on your file as RG.Name, select the subscription and the resource group location (if you are in the UK, _North Europe_ is a good choice)  

6. Press __Create__ to create the Resource Group  

[Back to top](#creating-the-azure-items)

---

## Create the Azure Cosmos DB instance

1. Once the Resource Group is created, access it, and press __+ Add__  

2. On the search box that shows up, type _`CosmosDB`_  

3. Select __Azure Cosmos DB__, from Microsoft  

4. Press __Create__ on the new blade  
   - give it a name
   - select _“SQL”_ as the __API__
   - select the subscription
   - and under Resource Group, select __“Use existing”__ and on the drop down, select the resource group that was created previously and that you saved on your file as RG.Name.
   - Don’t forget to set the Location to the appropriate value  

6. Press __Create__ to create the Azure Cosmos DB  

7. Once the instance is created, press Keys and make a note of the URI and the Primary Key in your file as CosmosDB.Uri and CosmosDB.PrimaryKey, respectivelly  

8. Press __Data Explorer__ and select __New Database__. Type a name (_`tickme`_ is a good option) and save the name in your file as CosmosDB.DatabaseName

9. Click __...__ that shows up when you move the mouse over the newly created database and select New Collection. Type _`events`_ in the __Collection Id__ field, make sure you select __Fixed Size__ and press __OK__

10. This Collection needs to be populated with some data, so expand the _“events”_ collection, and lets add a few shows. For each of the code blocks below, press __New Document__, type the _json_ in the code blocks and press __Save__  

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

11.	Click __...__ that shows up when you move the mouse over the database and select __New Collection__. Type _`user`_ in the __Collection Id__ field, make sure you select __Fixed Size__ and press __OK__

12.	Click __...__ that shows up when you move the mouse over the database and select __New Collection__. Type _`tickets`_ in the __Collection Id__ field, make sure you select __Fixed Size__ and press __OK__  

[Back to top](#creating-the-azure-items)

---

## Creating the KeyVault

Finally, lets create the Key Vault

1. On the resource group blade, press __+ Add__ again

2. On the search box that shows up, type _`Azure Key Vault`_

3. Select __Azure Key Vault__, from Microsoft

4. Press __Create__ on the new blade
   - Give it a name and save it in your file as KeyVault.Name
   - select the subscription and under Resource Group
   - select __Use existing__
   - on the drop down, select the resource group that was created previously and that you saver on your file as RG.Name
   - Don’t forget to set the Location to the appropriate value, but you can leave the other options with the defaults

5. Once the KeyVault is created, select __Access Policies__ and press __+ Add__
   - On __Select Principal__, select the __App Registration__ you created previously (you saved the name as AAD.Name)
   - On __Secret permissions__, check only the __Get__ and __List__ options
   - Leave all the other options as they are and press __OK__

6. Select Secrets and press __+ Generate/Import__
   - on the Name, type _`DataStore-DatabaseName`_
   - set the Value to _`CosmosDB.DatabaseName`_
   - press __Create__
7. Select Secrets and press __+ Generate/Import__
   - on the Name, type _`DataStore-EndpointUri`_
   - set the Value to _`CosmosDB.Uri`_
   - press __Create__
8. Select Secrets and press __+ Generate/Import__
   - on the Name, type _`DataStore-PrimaryKey`_
   - set the Value to _`CosmosDB.PrimaryKey`_
   - press __Create__
9. Select Secrets and press __+ Generate/Import__
   - on the Name, type _`Auth-ClientId`_
   - set the Value to _`B2C.ApplicationId`_
   - press __Create__
10. Select Secrets and press __+ Generate/Import__
    - on the Name, type _`Auth-TenantName`_
    - set the Value to _`B2C.Domain`_
    - press __Create__  

[Back to top](#creating-the-azure-items)

With this, the Azure setup is done for now.

---

[Previous Step - Initial Setup](./initialsetup.md)  
[Next Step - Prepare your Workstation](./prepareworkstation.md)  
[Back to Start](../README.md)
