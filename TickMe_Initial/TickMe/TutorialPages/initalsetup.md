# INITIAL SETUP

The application runs on Azure, in a webapp, or on your local machine during development, using Azure AD for authentication, CosmosDB to store data and KeyVault to store secrets. As part of the development team, the first step is to recreate the environment.
Your workstation should have installed:

- __Microsoft Visual Studio 2017__, with the options to develop for the web development enabled. It can be downloaded from [https://visualstudio.microsoft.com](https://visualstudio.microsoft.com);
- __Microsoft .Net Core SDK__, the latest version, that can be obtained from [https://www.microsoft.com/net/download](https://www.microsoft.com/net/download);
- __Docker__, the selected container solution, which you can download from [https://www.docker.com/get-started](https://www.docker.com/get-started);
- __Git for Windows__, obtainable from [https://git-scm.com/download/win](https://git-scm.com/download/win). While not strictly necessary, it makes things easier.

You’ll also need:

- An __Azure__ subscription;
- A __Microsoft account__ (if you don’t have one, you can get it at [https://live.com](https://live.com)
- A __Docker Account__ (you can register for one when downloading docker - see above)

To make it easier to know where to configure the multiple values we will configure and will need to use, copy the following template to a text editor of your choice and fill in the values as noted on the following steps

```text
Subscription A is the one where you will be creating the resources
Subscription B is the one where Azure AD B2C lives

Microsoft Application Registration:
- MSA.ApplicationId:  
- MSA.ApplicationSecret:  

Azure AD B2C on subscription B:
- B2C.OrganizationName:
- B2C.Domain:  .onmicrosoft.com  
- B2C.ApplicationId:  
- B2C.RedirectUrl: https://login.microsoftonline.com/te/{<B2C.Organization name from above>}.onmicrosoft.com/oauth2/authresp

Azure AD on subscription A:
- AAD.ApplicationId:  
- AAD.ApplicationSecret:  
```


[Next Step - Setting Things Up](createazurepart.md)  
[Back to Start](../README.md)