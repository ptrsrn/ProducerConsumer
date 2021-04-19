# Create user

After `docker-compose up -d` we need to create a user that will allow use to connect from other docker containers, e.g. from our consumer

```
docker exec -it <mysql_container_id> exec
```
 
then

```
mysql -u root - p
```

when prompted for password enter the root password set in docker compose

## create the new user

form the mysql prompt execute the following

```
    CREATE USER 'user'@'localhost' IDENTIFIED BY 'password';
    CREATE USER 'user'@'%' IDENTIFIED BY 'password';
    GRANT ALL ON *.* TO 'user'@'localhost';
    GRANT ALL ON *.* TO 'user'@'%';
    flush privileges;
```

after this we can connect with the user `user` having password `password`
