* OurAuth.Demo.Api: https://api.demo.ourauth.com
* OurAuth.Demo: https://demo.ourauth.com

# Settings on the.ourauth.com
## Api resource (for local)
Name: ourauth.demo.api.local

## Api Scope (for local)
Name: ourauth.demo.api.local

## Client resouce (for local)
ClientId: OurAuth.Demo.Api.Swagger.Local
Protocol Type: OpenID Connect
Require Client Secret: No
Requir Pkce: Yes
Allowed Scopes: ourauth.demo.api
Redirect Uris: http://localhost:5000/swagger/oauth2-redirect.html
	https://localhost:5001/swagger/oauth2-redirect.html
Allowed Grant Types: authorization_code

Front Channel Logout Session Required: Yes
Back Channel Logout Session Required: Yes
Allowed Cors Origins: http://localhost:5000
	https://localhost:5001

Require Consent: Yes
Allow Remember Consent: Yes

## Client resource (for BlazorServer)
ClientId: OurAuth.Demo.Api.BlazorServer.Local
Grant Types: authorization code with PKCE and client credentials
access token lifetime: 75 seconds
allowed scopes: openid profile email ourauth.demo.api offline_access
