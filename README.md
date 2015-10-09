# BitLySharp
.NET C# Library of bit.ly API for Windows 10, Windows 8.1, Winodws Phone 8.1

Example
```c#
 //Password Authentication
 BitLy client = new BitLy("CLIENT_ID", "CLIENT_SECRET", GrantType.Password, "username", "password");
 AuthenticationInfo info = await client.GetAccessTokenAsync(); //Get Access Token
 LinkInfo linfo = await client.GetUrlInfoAsync(info.AccessToken, "http://bit.ly/1RmnUT", "http://bit.ly/ze6poY" );
 
 //Web-Flow Authentication
 BitLy client = new BitLy("CLIENT_ID", "CLIENT_SECRET", "Redirect URL");
 AuthenticationInfo info = await client.AuthorizeAsync() //Get Access Token, Use WebAuthenticatinBroker
 LinkInfo linfo = await client.GetUrlInfoAsync(info.AccessToken, "http://bit.ly/1RmnUT", "http://bit.ly/ze6poY" );
 
```

All the Result Objects have the JsonString property if you need the json string returned from bit.ly


