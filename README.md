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

The most essential configuration is in *applicationconfig.json* Make sure you put a valid bot service in the **BotFramework:Url** property.

## Security

It's on going... by now *Authorization: Bearer ...* doesn't work. So it's free to use.

## TODO List

- Support to Authorization header
- User identification at skype or webchat client.
- Enhance messaging to support streams, add and remove users
- Better management for Channel, Conversation and Activity.
- Keep writing the documentation ! :)

# Disclaimer

Do I need to repeat those BLA BLA BLA? It's your own RISK. Let me out of your issues. For good souls, if you found this product interesting and want to enhance it, talk to me. You're welcome!
