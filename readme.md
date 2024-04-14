This is example of client-server applications architecture. 
Server is implementing authentication with custom bearer schemem, where a GUID is useed as token.
Client app connects to the server, when the authentication is passed, server provides the client with bearer token, that could be used for authentication in further requests.
Token has its TTL, each request is refreshing TTL which is keept in a session on MSSQL db.
Client app could query server for current user data or user list if client has a valid token.

Server app uses CQRS, unit of work, reporistories patterns.
For request validation and projection of data, FluentValidation and Automapper are used.

## Running instruction
Install `docker desktop`
### Integration tests
Just `run the tests`, they will use docker containerized MSSQL server for each tests.

### Running the client and the server
1. Run the docker-compose from VS, it will set up Web app, and its database as container with docker compose.
2. Wait a momemnt for docker to startup the containers and run migrations(you don't want to use the client app whean in meantime docker is reseting the database...).
3. Run the `simple_authentication_client_console` which is the console client of the app. Ensure it receives exposed web api url in parameter. 
4. You can default user credentials for testing: username=User1 password=User1
5. You can connect to MSSQL in docker from host by using address 127.0.0.1:1433, database SimpleAuthentication and credentials: username=SA, password=password123! If necessary, own db instance could be set up in appsettings.