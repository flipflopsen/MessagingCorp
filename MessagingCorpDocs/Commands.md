# SurrealDB Database

## From local

Start DB
`surreal start -b 127.0.0.1:35167 -u corpadmin -p lulz123 --auth`

Import SurrealQL
```bash
surreal import --conn http://localhost:8082 --user corpadmin --pass lulz123 --ns MessagingCorpGeneral --db users Database/SurrealDbInit.surql
```

## From Docker

1. Create Network
```bash
docker network create surrealnet
```

2. Start DB
```shell
sudo docker run --rm --pull always --name messagingcorp-main -p 8082:8082 -v /home/lance/Development/MessagingCorp/Database:/mydata surrealdb/surrealdb:latest start --log trace --user corpadmin --pass lulz123 -b 0.0.0.0:8082 --auth
```

3. Inspect network and get IP:
```shell
sudo docker network ls 

NETWORK ID     NAME                       DRIVER    SCOPE
28bc7497dcd4   surrealnet                 bridge    local

sudo docker network inspect -v 28bc7497dcd4
 "Containers": {
            "65073ab21ac278c319216cc0f24a74c197ad0e00b24189c138dcfd915fdd3e21": {
                "Name": "messagingcorp-main",
                "EndpointID": "6c30a446b51bcc1570915660e62dd7c4e0c5d07de80a1df36c6dda964a925888",
                "MacAddress": "02:42:ac:12:00:02",
                "IPv4Address": "172.18.0.2/16",
                "IPv6Address": ""
            }
        },
....

IP-Address of SurrealDB: 172.18.0.2
```

4. Import SurrealQL
```shell
sudo docker run --rm --name surrealdb-cmd --network surrealnet -d surrealdb/surrealdb:latest import --conn http://172.18.0.2:8082 --user corpadmin --pass lulz123 --ns MessagingCorpGeneral --db users Database/SurrealDbInit.surql
```