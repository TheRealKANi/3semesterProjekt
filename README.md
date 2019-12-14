# 3. Semester Project - Group0 - DMAA0918

#### We are a group of Computer Science Students From UCN Aalborg (Sofiendalsvej) in Denmark
#### For our third semester project we chose to design a rudimentary Game and GameEngine from scratch.

Our goal for this project was to develop a simple 2D MMO Shooting Game.
We went into this project with confidence, and despite the fact, that we completely underestimated 
the complexity of how a Game Engine is built, we conquered all the unforeseen challenges that arose randomly throughout the project.
All in all, we ended up with a project that we with confidence, can call ~~semi~~ successfull. 
The Project is developed as a distributed system, and is designed as a N-Layered, N-Tiered Architecture
that consists of a __Server__, __Dedicated Client__ and a __Web Page__.

Our Dedicated Client is using [WPF Canvas](https://docs.microsoft.com/en-us/dotnet/api/system.windows.controls.canvas?view=netframework-4.8) as the game arena.

The Web Page is designed with [ASP.Net](https://dotnet.microsoft.com/apps/aspnet).

The server that connects it all is using a self-hosted [SignalR](https://dotnet.microsoft.com/apps/aspnet/signalr) service to provide real-time Game Client responsiveness. The Web Client is utilizing a [WCF Service Endpoint](https://docs.microsoft.com/en-us/dotnet/framework/wcf/index).


Finally, the user data is stored on a [Azure Hosted MSSQL Database](https://azure.microsoft.com/da-dk/services/sql-database/).

