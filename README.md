# todo_app_backend
backend of a c# based todo app used as a lean learning platform

# Status

[![Unit Tests](https://github.com/jacques-ro/todo_app_backend/actions/workflows/unit-tests.yml/badge.svg)](https://github.com/jacques-ro/todo_app_backend/actions/workflows/unit-tests.yml) [![Deploy to Heroku](https://github.com/jacques-ro/todo_app_backend/actions/workflows/deploy-heroku.yml/badge.svg)](https://github.com/jacques-ro/todo_app_backend/actions/workflows/deploy-heroku.yml)

# General Notes

This readme assumes you are using VS Code. Explanations may refer to VS Code plugins.

# How to customize appsettings

Note: the app is now containerized so the connection strings for the local environment are always the same and should not be edited.

# How to run the containerized environment

## Prepare identity server environment variables

There are now two settings to be set for the identityserver middleware.

Create a new environment file for docker-compose named `user.env` next to the compose files.

Add the following two lines and replace both values according to your environment. The JWT_BEARER_AUTHORITY must match the domain endpoint of the app.

```
CLIENT_SECRET=SET_YOUR_SECRET_HERE
JWT_BEARER_AUTHORITY=http://localhost:5000
```

## Run the app

1. Right click on docker-compose.yml and choose `Compose Up`
2. Open your browser, navigate to http://localhost:5000/swagger/index.html

# How to debug the containerized environment

The application environment is setup using docker compose. The app makes use of a postgres database which is defined in the docker-compose.yml file.

In order to make debugging work, proceed as follows:

1. Ensure you have the `ms-azuretools.vscode-docker` extension installed
2. Right click on docker-compose.debug.yml and choose `Compose Up`
3. Set your break point
4. Attach to docker using the `Docker .NET Core Attach (Preview)` launch configuration
5. Open your browser, navigate to http://localhost:5000/swagger/index.html

For further information, refer to the vs code debugging guide https://code.visualstudio.com/docs/containers/docker-compose

# How to manually deploy to heroku

Install the heroku CLI (not explained here)

Login to heroku

```bash
heroku login
```

Login to heroku container registry

```bash
heroku container:login
```

push the app

```bash
heroku container:push -R -a <app-name>
```

release the app

```bash
heroku container:release web -a <app-name>
```

look at the output of the deployed container

```bash
heroku logs --tail -a <app-name>
```
