# NET6 with VueJS 3 (Typescript) Composition API (Quasar Framework) MovieAPP Project

<img alt="Movie" src="assets/Movie.gif"> </img>

## Features
- Entity Framework Core – Code First
- Response Wrappers
- Action Filters
- Net Core Identity with JWT Authentication,Refresh Token
- Role Based Authorization
- Database Seeding
- Custom Exception Handling Middleware,
- Caching (Memory and Redis single interface configure from appsettings.json file example using => Business Concrete Category Manager)
- Complete User Management  (Register / Generate Token / Forgot Password / Confirmation Mail)
- Logging (Serilog),Validation (Fluent Validation),Transaction,Exception,Performance with Aspects (Autofac,Castle.DynamicProxy)

## How To Start .Net API

For api, you must edit the appsettings.json file before typing these commands.
I used postgresql as database but you can change it

Docker added you can start project with docker, first you must look docker compose yaml file rediscache settings vs. and write 

```sh
docker compose -f "docker-compose.yml" up -d --build
```


```sh
dotnet ef database update --context MovieContext --project "DataAccess" --startup-project "WebAPI"
```

After these commands, a database will be created. 


Default Admin Account : 

```sh
Username : admin
Password : 159357456qW
```


## How To Start Quasar Project


```sh
npm install
quasar dev
```

## References

```sh
https://github.com/drehimself/laravel-movies-example
```

I made this project inspired by the project made with Laravel in the link, I made additions to it.
Login, Register, Favorites (Add-Remove), Dashboard Panel eg.

