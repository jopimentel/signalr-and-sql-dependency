create database RealTimeDb
go

use RealTimeDb
go

create table Session
(
	Id			nvarchar(45) primary key not null,
	Code		nvarchar(500) not null,
	"User"		nvarchar(500) not null,
	Name		nvarchar(500) not null,
	Role		nvarchar(500) not null,
	LogonTime	DateTimeOffset default sysdatetimeoffset(),
	Status		nvarchar(500) not null
)
go


insert session values (lower(newid()), 'TR-5966', 'Samir', 'Administrator', 'Root', sysdatetimeoffset(), 'Active');
go