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

multiple containers of the service can be started as follows

```
docker-compose up -d --scale <service>=<desired count>
```


# monitor
## rabbitMQ management interface
The management interface can be started by following this link [rabbitMQ](localhost:15672) username and password for this implementation is both `guest`

## monitor the whole system
The `Producer` and the `Consumer` can be monitored using the syslog container

```
docker exec <syslog_containter_id> tail -f /var/log/messages
```

This will stream all log messages.
