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

create table metodosAuth(
	id int primary key identity(1,1),
	nombre varchar(50) not null
);
insert into metodosAuth values('Cuenta Google');
insert into metodosAuth values('Cuenta Facebook');
insert into metodosAuth values('Cuenta Twitter');
insert into metodosAuth values('Correo y contrasena');
insert into metodosAuth values('Cuenta Github');
insert into metodosAuth values('Cuenta Apple');
insert into metodosAuth values('Numero de telefono');
insert into metodosAuth values('Cuenta Microsoft');
insert into metodosAuth values('Cuenta Yahoo');

create table proyectos(
	id int primary key identity(1,1),
	nombre varchar(50) unique,
	descripcion varchar(max),
	fechaAlta datetime not null,
	metodosAutenticacion varchar(300) not null,
	activo bit not null,
	idUsuarioRegsitra int foreign key references usuarios(id)
);


create table usuariosProyecto(
	id int primary key identity(1,1),
	nombreCompleto varchar(150) not null,
	email varchar(50) not null,
	pass varchar(200) not null,
	fechaCreacion datetime,
	fechaAcceso datetime,
	idProyecto int foreign key references proyectos(id)
);