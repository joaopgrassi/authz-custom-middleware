# Authorization in ASP.NET Core

This repo contains the source code for a series of blog posts I'm writing about authorization in ASP.NET Core. You can check all the posts here: [Authorization in ASP.NET Core - Series](https://blog.joaograssi.com/series/authorization-in-asp.net-core).


## Blog posts and code - how to follow

Since there's a lot to cover, this repo is going to be updated incrementally, together with each post.

To make it practical for you to both read the post and follow through with the code, there will be a branch for each post I publish. This way you can see the *snapshot* of how it looked like when I published the post. 

The `main` branch will contain the latest state always.

## Running the app:

The app uses a SQL Server database to store its data. At the root of the repo, you can find a `docker-compose.yml` which will start both SQL Server and IdentityServer for you. 

After executing `docker-compose up`, you can start debugging the API using your preferred IDE. The `API` project will automatically create and migrate the database using EF Migrations.

The initial migration will seed the database with users and permissions so you don't have to do anything to get started.

The users you can use to login are: (If you ever used IdentityServer you might be familiar with them :smile:).

| User | Password |
|------|----------|
| bob | bob |
| alice | alice |


If all worked you should see the Swagger page and should be able to authenticate and call the endpoint.

### Requisites

- .NET SDK 6.*
- Docker/Compose (only if you want to run the app locally)
