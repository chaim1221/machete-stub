sudo docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'passw0rD' -Q "CREATE USER dev WITH PASSWORD 'passw0rD';"
sudo docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'passw0rD' -Q "EXEC sys.sp_addsrvrolemember @loginame = N'dev', @rolename = N'sysadmin';"
sudo docker exec -it sql1 /opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P 'passw0rD' -Q "ALTER SERVER ROLE [sysadmin] ADD MEMBER [dev]"
