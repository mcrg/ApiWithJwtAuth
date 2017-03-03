# ApiWithJwtAuth
<p>Implementation of simple Web API with JWT authentication. Based on: https://stormpath.com/blog/token-authentication-asp-net-core</p>

<h2>Getting the JWT token</h2>

POST http://localhost:6130/login/login HTTP/1.1
User-Agent: Fiddler
Host: localhost:6130
Content-Length: 38
Content-Type: application/json

{"Login":"maciek","Password":"qwe123"}

<h2>Listing users (secured with authentication)</h2>

GET http://localhost:6130/secure/users HTTP/1.1
User-Agent: Fiddler
Host: localhost:6130
Content-Length: 0
Content-Type: application/json
Authorization: Bearer <token from "login" call response>

