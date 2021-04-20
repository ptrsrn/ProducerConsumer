# Create database ser

After `docker-compose up -d` we need to create a user that will allow use to connect from other docker containers, e.g. from our consumer

To get the root password from the newly created mysql container 

```
docker logs  <mysql_container_id> 2>&1 | grep GENERATED
```

then

```
docker exec -it <mysql_container_id> bash
```
 
then

```
mysql -u root - p
```

when prompted for password enter the root password 

From the mysql prompt execute the following

```
    CREATE USER 'user'@'localhost' IDENTIFIED BY 'password';
    CREATE USER 'user'@'%' IDENTIFIED BY 'password';
    GRANT ALL ON *.* TO 'user'@'localhost';
    GRANT ALL ON *.* TO 'user'@'%';
    flush privileges;
```

after this we can connect with the user `user` having password `password`

## Reset root password

```
ALTER USER 'root'@'localhost' IDENTIFIED BY 'password';
```

# Migrations

Before the database can be initialized we nned to install the `ef` tool

```
dotnet tool install --global dotnet-ef
```

Make sure the the `ef` tool is in the PATH

```
export PATH=$PATH:~/.dotnet/tools
```

Next migrate the database

```
cd DataLayer
dotnet ef database update
```

