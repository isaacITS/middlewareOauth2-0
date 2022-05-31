create database middlewareITS;
use middlewareITS;


create table usuarios(
	id int primary key identity(1,1),
	nombre varchar(50) not null,
	fechaAlta datetime not null,
	puesto varchar(50) not null,
	email varchar(60) unique,
	pass varchar(150) not null,
	activo bit not null
);

create table proyectos(
	id int primary key identity(1,1),
	nombre varchar(50) unique,
	descripcion nvarchar,
	usuario varchar(50) not null,
	tipoCifrado varchar(50),
	metodoAutenticacion varchar(50),
	pass varchar(150) not null,
	activo bit not null
);

insert into usuarios values('Administrador', GETUTCDATE(), 'Admin', 'admin@admin.com', HASHBYTES('SHA2_256','admin123'), 1);

insert into usuarios values('Administrador', GETUTCDATE(), 'Admin2', 'admin2@mail.com', 'admin123', 0);

select * from proyectos;