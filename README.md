# Web API Gateway [![CircleCI](https://circleci.com/gh/joaopedropio/web-api-gateway/tree/master.svg?style=svg)](https://circleci.com/gh/joaopedropio/web-api-gateway/tree/master)

The Web API Gateway is a Dotnet Core API is the gateway to the microservices of the Social Movie project. 
This gateway is only for web apps use. Soon, we'll have the Mobile API Gateway service to mobile clients.

Environment variables
* REDIS_IP: ip or domain of the Redis instance
* SERVICES: default services. Each service is separated by a semi-colon and each service has a name and a url separated by a colon. Example string -> "facebook,http://www.facebook.com;hotmail,http://www.hotmail.com"