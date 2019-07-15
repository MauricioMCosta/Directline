# Directline
Directline Bridge implementation 

## Introduction

This is an offline Directline protocol implementation. I did this work based on Typescript implementation you find here: https://github.com/ryanvolum/offline-directline/

The need of this bridge is to allow access to Microsoft Bot Framework's bot instances that can't be shared in Azure. This bot was tested following a simple path. 
- WebChat.js client: 
- This Directline implementation
- A sample bot built with MSBF v4 (EchoBot)

THIS PRODUCT IS NOT SUITABLE FOR PRODUCTION ENVIRONMENT. AS IS IT's ONLY FOR EDUCATIONAL PURPOSES. 
For me it's enough for the work I'm developing. :)

> This documentation is not ready yet... See todo below:

## How To...

### ...build
You can either load the files using Visual Studio 2017/2019 or VSCode. Trigger build from there. Otherwise, you can go command line

```
dotnet build
```

### ...run

Via command line:

```
dotnet run
```

### ... configure

The most essential configuration is in *applicationconfig.json*. There's no much parameter to be configured there. See here a sample:
```
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug"
    }
  },
  "AllowedHosts": "*",
  "Jwt": {
    "Key": "ThisismySecretKey",
    "Issuer": "dell.com"
  }
}
```

Define logging level either *Debug*, *Information*, *Warning* or *Error*. This allows the generation of fewer or more detailed messages during the execution of the engine.

The Jwt stuff see security section.

## Security

It relies on *Authorization: Bearer ...* header and uses JWT (JavaWebToken) to authenticate the client.

### Server Configuration

Just configure the *Key* and *Issuer* entries. The key can be anything. just pick a random string and put there. It is used to generate a hash and sign the JWT Token. All tokens generated before a change on key will be invalidated. Keep this file safe.

### Client Configuration

The clients which will connect to this service, most of them will be Either SkypeToDirecline or any other implementation of WebChat or ChatBot Client which follows the Microsoft's BotFramewor. Go to the UI of Directline

*http://the_server_of_directline:port/index.html*

Go in Generate Token, and fulfill the parameters there.

- Bot URL is the endpoint of the BOT Service (afaik, it shoud end with /api/messages)
- Bot Password and Username are optional and is not being used at moment. (In the future it will)

Take the token generated and configure in every client which will interact with the directline (Except BOT Service).

## TODO List

- User identification at webchat client.
- Implement websocket
- Remove expired conversations
- Keep writing the documentation ! :)

# Disclaimer

Do I need to repeat those BLA BLA BLA? It's your own RISK. Let me out of your issues. For good souls, if you found this product interesting and want to enhance it, talk to me. You're welcome!
