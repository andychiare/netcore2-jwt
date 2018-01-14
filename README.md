# netcore2-jwt
This is a .NET Core 2 sample project showing how to use JWTs (_JSON Web Tokens_). It is the companion code for the article [Securing ASP.NET Core 2.0 Applications with JWTs](https://auth0.com/blog/securing-asp-dot-net-core-2-applications-with-jwts/).

The *JWT* project defines a Web API application whose authentication is based on JWT.


## Running the project ##


The solution contains a _Test_ project with four integration tests validating the application behaviour.
You can run the tests from Visual Studio 2017 or by typing `dotnet test` in a command window.

If you want to interactively test the application, you can use [Postman](https://www.getpostman.com/) or any other Http client.

1. Run the project from Visual Studio 2017 or by typing `dotnet run` in a command window
2. Launch _Postman_ and make a GET request as follows:

```
    GET http://localhost:63939/api/books HTTP/1.1
    cache-control: no-cache
    Accept: */*
    Host: localhost:63939
    accept-encoding: gzip, deflate
    Connection: keep-alive
```

This should return a 401 HTTP status code (_Unauthorized_)

3. Make a POST request like the following:

```
    POST http://localhost:63939/api/token HTTP/1.1
    cache-control: no-cache
    Content-Type: application/json
    Accept: */*
    Host: localhost:63939
    accept-encoding: gzip, deflate
    content-length: 39
    Connection: keep-alive
    
    {username: "mario", password: "secret"}
```

It returns a JSON object like the following:

```
    {
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJNYXJpbyBSb3NzaSIsImVtYWlsIjoibWFyaW8ucm9zc2lAZG9tYWluLmNvbSIsImp0aSI6IjVkNTRkMzIwLWQ3N2EtNDFhMy1iZTcwLTc2M2UyMGRmMjE3MyIsImV4cCI6MTUxMTE3NzQwMywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo2MzkzOS8iLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjYzOTM5LyJ9.g0yooTf3DJO43yL8bT4VE_VIdc5WHFhCVb3u9Jg7VTk"
    }
```

4. The following GET request

```
    GET http://localhost:63939/api/books HTTP/1.1
    cache-control: no-cache
    Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJNYXJpbyBSb3NzaSIsImVtYWlsIjoibWFyaW8ucm9zc2lAZG9tYWluLmNvbSIsImp0aSI6IjVkNTRkMzIwLWQ3N2EtNDFhMy1iZTcwLTc2M2UyMGRmMjE3MyIsImV4cCI6MTUxMTE3NzQwMywiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo2MzkzOS8iLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjYzOTM5LyJ9.g0yooTf3DJO43yL8bT4VE_VIdc5WHFhCVb3u9Jg7VTk
    Accept: */*
    Host: localhost:63939
    accept-encoding: gzip, deflate
    Connection: keep-alive
```

returns the following response:

```
	[
	    {
	        "author": "Ray Bradbury",
	        "title": "Fahrenheit 451",
			"ageRestriction": false
	    },
	    {
	        "author": "Gabriel García Márquez",
	        "title": "One Hundred years of Solitude",
			"ageRestriction": false
	    },
	    {
	        "author": "George Orwell",
	        "title": "1984",
			"ageRestriction": false
	    },
	    {
	        "author": "Anais Nin",
	        "title": "Delta of Venus",
			"ageRestriction": true
	    }
	]
```

