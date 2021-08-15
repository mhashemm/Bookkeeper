### To run the app you need Docker installed and run the following commands.

```
docker compose up db --build
```
In a new window run the following commands:
```
docker exec -it bookkeeper-db /opt/mssql-tools/bin/sqlcmd -S db -U sa -P P@ssW0rd! -i ./CreateDbs.sql
```
```
docker compose up ui --build
```
Open with: http://localhost:5000