# Preparing your workstation

The workstation preparation is quick and easy.

1. Open a terminal window
   - On Windows, press the __Windows__ key, type _`cmd`_ and press _`Enter`_
   - On OSX, press __cmd__ + __space__, type _`terminal`_ and press _`Enter`_
   - On Linux, press __ctrl__ + __alt__ + __T__

2. Navigate to the folder where you wish to have the code on

3. Type _`git clone https://github.com/etpedror/TickMe.git`_. This creates a clone of this repository on your workstation.

4. Using Visual Studio or Visual Studio Code, open the file _appsettings.json_ that you can find in the folder _TickMe_ inside the folder that was cloned from GitHub. You will need to add some configuration values:
```javascript
    "KeyVault": {
        "Vault": "<KV.Name on your file>",
        "ClientId": "<AAD.ApplicationId on your file>",
        "ClientSecret": "<AAD.ClientSecret on your file>"
    }
```

Run the application! You should be able to see the entry page, but should get a redirect error on login. That is related to the port that your website binds to for https. By now, your URL should look like _`https://localhost:{port}`_. You need to go back to your __Azure AD B2C__ setup and edit the urls so that they have the correct port. In my case, the url is _`https://localhost:44391`_ so I would need to update the urls to 

    - `https://localhost:44391/signin/b2c_1_edit_profile`
    - `https://localhost:44391/signin/b2c_1_sign_up_in`
    - `https://localhost:44391/signin/b2c_1_sign_in`
    - `https://localhost:44391/signin/b2c_1_sign_up`
    
Once you do it, you should be able to run the application and login, as well as see the events and buy tickets.

Proceed to the [Second Part of this Workshop](https://github.com/etpedror/TickMeMicroservices), where we will turn this application into a Microservices oriented application

---

[Previous Step - Create Azure Part](./createazurepart.md)   
[Back to Start](../README.md)