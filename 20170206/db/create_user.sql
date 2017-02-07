/*Создаем пользователей под коннектом sysdba*/
drop user modern cascade;
create user modern identified by modern
default tablespace USERS temporary tablespace TEMP profile DEFAULT;
grant resource, dba, connect to modern;

drop user auth cascade;
create user auth identified by Ijf34nfD
default tablespace USERS temporary tablespace TEMP profile DEFAULT;
grant resource, dba, connect to auth;

/*Импортируем данные из файла дампа*/
imp system/sys@XE file="C:\temp\dna.dmp" fromuser="modern, auth" touser="modern, auth"

grant execute on dbms_crypto to modern;
grant execute on modern.logon to auth;
grant select on sys.DBA_CONSTRAINTS to modern;
grant select on sys.DBA_CONSTRAINTS to system;

/*Компилируем все процедуры и пакеты*/


