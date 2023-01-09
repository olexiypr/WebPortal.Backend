# WebPortal 
Platform where people can create articles
\
![Build](https://github.com/olexiypr/WebPortal.Backend/actions/workflows/develop_webportalwebapii.yml/badge.svg?branch=develop)


# Functionality
**For unregistered users**\
\
On this platform, you can browse articles, see the most popular ones, search for them, view author profiles, search for authors, browse articles by tags, and of course register
\
\
**For registered users**\
\
Log into your account
create articles, modify them, publish, delete, upload an avatar, change information about yourself, your avatar, comment on articles, reply to comments, like them, dislikes rate the article, likes, dislikes.
# Technology stack
- ASP.NET Core 6
- REST API
- Multi-layered architecture
- PostgreSQL and EF 6
- Authentication and authorization capabilities with JWT Bearer auth
- Data validation with FluentValidation
- API documentation with Swagger and Swagger UI

Features:
- [x] Memory caching
- [x] Logging
- [x] Add hang fire to clear the number of article views per day, week 
- [ ] Live chat with SignalR
- [ ] Сonfirmation registration and password recovery by email
- [ ] Unit tests
- [ ] Password hashing
