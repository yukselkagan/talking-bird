# Talking Bird

## About

Talking bird is a microservice project inspired by Twitter.
Have 5 services:  
1)Identity service: Handle authentication  
2)Post service: Handle main post    objectives like share something or like.  
3)Comment service: Handle comments for posts  
4)Stock service: Keep static files  
5)Trend service: Decide trends based on posts

Every service have it's own database and business logic.
Services communicate with each other both asynchronous and synchronous. For asynchronous communication using RabbitMQ, for synchronous communication using http methods.


