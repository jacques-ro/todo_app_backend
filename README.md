# todo_app_backend
backend of a c# based todo app used as a lean learning platform

# Status

[![Deploy to Heroku](https://github.com/jacques-ro/todo_app_backend/actions/workflows/deploy-heroku.yml/badge.svg)](https://github.com/jacques-ro/todo_app_backend/actions/workflows/deploy-heroku.yml)

# General Notes

This readme assumes you are using VS Code. Explanations may refer to VS Code plugins.

# How to customize appsettings

Note: the app is now containerized so the connection strings for the local environment are always the same and should not be edited.

Copy the original appsettings.Development.json to a new appsettings.user.json. Don't push your `appsettings.user.json` to the repository.

# How to run the containerized environment

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
