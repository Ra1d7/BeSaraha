<h1 align="center">BeSaraha Project ReadMe 🫡</h1>

BeSaraha is a .NET Core 6 MVC project with a SQL Server database , it allows users to send anonymous messages to other users of the website. with each user having their profile and a custom profile url which they choose at registeration or can edit in their profile settings,
the front-end is fully customized and responsive using bootstrap , jquery and toastr javascript library. the backend consists of the database and Dapper ORM to talk to the database.

Some of the notable features of BeSaraha include:

- User authentication using encrypted cookies to manage the user's state.
- User login / register with fully hashed passwords using SHA256 algorithim
- fully customized profiles including the profile url and user profile picture.
- Usage of Dapper to communicate with the SQL database, which stores all the information.
- Implementation of Bootstrap 5 with jQuery, Toastr javascript library to create a responsive UI.
- full protection against Cross-Site-Scripting (XSS) while sending and recieving messages.
- the repository contains the sql script to initiate the database after which the developer needs to set the correct connection string to in the appsettings.json file.

Overall, BeSaraha is a well-designed, secure, and user-friendly website that can be used as a base for building a more social media platform for sending and recieving anonymous messages. The use of modern technologies such as .NET Core, and Bootstrap 5 ensures that the website is fast, reliable, and responsive.
<hr/>

<h2 align="center"> Technologies used 🧪</h2>


<div align="center">
<a href="https://dotnet.microsoft.com/">
<img height="32" width="32" src="https://cdn.simpleicons.org/dotnet/purple"/>&nbsp;&nbsp;</a>
<a href="https://www.microsoft.com/en-us/sql-server/sql-server-downloads">
<img height="32" width="32" src="https://cdn.simpleicons.org/microsoftsqlserver/red"/>&nbsp;&nbsp;</a>
<a href="https://getbootstrap.com/">
<img height="32" width="32" src="https://cdn.simpleicons.org/bootstrap/#7952B3"/>&nbsp;&nbsp;</a>
<a href="https://dapper-tutorial.net/">
<img height="32" width="32" src="https://z2c2b4z9.stackpathcdn.com/images/logo256X256.png"/>&nbsp;&nbsp;</a>
<a href="https://en.wikipedia.org/wiki/HTML5">
<img height="32" width="32" src="https://cdn.simpleicons.org/html5/#E34F26"/>&nbsp;&nbsp;</a>
<a href="https://dotnet.microsoft.com/en-us/languages/csharp">
<img height="32" width="32" src="https://cdn.simpleicons.org/csharp/#239120"/>&nbsp;&nbsp;</a>
<a href="https://developer.mozilla.org/en-US/docs/Web/CSS">
<img height="32" width="32" src="https://cdn.simpleicons.org/css3/#1572B6"/>&nbsp;&nbsp;</a>
<a href="https://developer.mozilla.org/en-US/docs/Web/javascript">
<img height="32" width="32" src="https://cdn.simpleicons.org/javascript/#F7DF1E"/>&nbsp;&nbsp;</a>
<a href="https://jquery.com/">
<img height="32" width="32" src="https://cdn.simpleicons.org/jquery/#0769AD"/>&nbsp;&nbsp;</a>
<a href="https://github.com/CodeSeven/toastr">
<img height="32" width="32" src="https://nuget.org/Content/gallery/img/default-package-icon-256x256.png"/></a>
</div>
<hr/>
<h2 align="center"> Screenshots 🌠 </h2>
<p align="center">
  <img src="https://github.com/Ra1d7/BeSaraha/assets/25421570/2456a15c-d408-460f-9d40-89a88becbf88" width="80%">
</p>

<p align="center">
  <img src="https://github.com/Ra1d7/BeSaraha/assets/25421570/85b2f7c1-f783-40b0-88e6-3b07f83c76eb" width="80%">
</p>

<p align="center">
  <img src="https://github.com/Ra1d7/BeSaraha/assets/25421570/97b94545-5728-40a8-8a89-09bcf8829d57" width="80%">
</p>

<p align="center">
  <img src="https://github.com/Ra1d7/BeSaraha/assets/25421570/f699f418-93ff-4ea2-a7c8-7d0e0791b88e" width="80%">
</p>

</hr>
<h2 align="center"> Overview of the project 🗻</h2>

<h3 align="center"> Messages Page </h3>
<p align="center">
This is where users will be most active. Reading the anonymous messages that other users had sent them. 
it shows the recieved messages in chronological order based on their date recieved.
</p>

<h3 align="center"> Profile Page </h3>
<p align="center">
Contains the profile of the user, with their fullname & profile picture. With an option to update their profile information.
the url of this page is set by the user so they can set a custom url that points to their profile so they can share it with friends. it can be set at registeration or when editing your profile.
</p>

<h3 align="center"> Register Page </h3>
<p align="center">
This is where users first register on the website, if they'd like to recieve messages. As sending messages can be done without registeration.
they are asked for their fullname , email , password and if they'd like to set a custom profile url. (if not given the profile url is generated randomly and is guaranteed to be unique)
</p>

<h3 align="center"> Loign Page </h3>
<p align="center">
This page is pretty self explanatory as it allows registered users to login into the website.
 it assigns them a cookie at login which lasts about 20 minutes.
</p>
</hr>
