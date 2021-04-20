# initial

Make sure that the database is created and migrated before the other services are started.
This could be done by starting the sqlserver container

```
docker-compose up -d sqlserver
```

then execute the steps in the [DATABASE.md](DATABASE.md) for the running container.

Then the remaining containers can be started

```
docker-compose up -d
```

For now the `Producer` and the `Consumer` containers are both started with `sleep infinity`. This make sure that PID 1 does not terminate and the container keeps running.

Start the producer
```
docker exec <producer_container_id> dotnet run
```

Start the consumer
```
docker exec <producer_container_id> dotnet run
```

For a production setup both should be configured to startthe compiled binary. (using `CMD` in the Dockerfiles) 


# monitor
## rabbitMQ management interface
The management interface can be started by following this link [rabbitMQ](localhost:15672) username and password for this implementation is both `guest`

## monitor the whole system
The `Producer` and the `Consumer` can be monitored using the syslog container

```
docker exec <syslog_containter_id> tail -f /var/log/messages
```

This will stream all log messages.
