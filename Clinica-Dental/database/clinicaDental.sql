USE tempdb
go

-- Crear la base de datos
CREATE DATABASE ClinicaDental
GO

-- Utilizar la base de datos
USE ClinicaDental
GO

-- Crear los schema de la base de datos
CREATE SCHEMA Pacientes
GO

CREATE SCHEMA Empleados
GO

-- Crear las tablas de Paciente
CREATE TABLE Pacientes.Paciente (
	identidad VARCHAR(20) NOT NULL,
	nombres VARCHAR(50) NOT NULL,
	apellidos VARCHAR(50) NOT NULL,
	direccion VARCHAR(200) NOT NULL,
	correoElectronico VARCHAR(200) NOT NULL,
	celular VARCHAR(15) NOT NULL,
	fechaNacimiento DATE NOT NULL,
	sexo VARCHAR(15) NOT NULL,
	estado BIT NOT NULL,
	CONSTRAINT PK_Paciente_identidad
		PRIMARY KEY CLUSTERED (identidad)
)
GO

-- Crear las tablas de HistorialClinico
CREATE TABLE Pacientes.HistorialClinico (
	idHistorialClinico INT NOT NULL IDENTITY,
	identidadPaciente VARCHAR(20) NOT NULL,
	fechaCreacion DATE NOT NULL,
	observaciones VARCHAR(200) NOT NULL,
	afecciones VARCHAR(200) NOT NULL,
	estado BIT NOT NULL,
	CONSTRAINT PK_HistorialClinico_idHistorialClinico
		PRIMARY KEY CLUSTERED (idHistorialClinico),
	CONSTRAINT Fk_HistorialClinico$Pertenece$Paciente
		foreign key (identidadPaciente) references Pacientes.Paciente(identidad)
)
GO

-- Crear las tablas de Cita
CREATE TABLE Pacientes.Cita (
	idCita INT NOT NULL IDENTITY,
	idHistorialClinico INT NOT NULL,
	fechaCita DATE NOT NULL,
	hora TIME(2) NOT NULL,
	nota VARCHAR(200) NOT NULL,
	estado BIT NOT NULL,
	CONSTRAINT PK_Cita_idCita
		PRIMARY KEY CLUSTERED (idCita),
	CONSTRAINT Fk_Cita$Pertenece$HistorialClinico
		foreign key (idHistorialClinico) references Pacientes.HistorialClinico(idHistorialClinico)
)
GO

-- Crear las tablas de DetalleTratamiento
CREATE TABLE Pacientes.DetalleTratamiento (
	idTratamiento INT NOT NULL IDENTITY,
	nombreTratamiento VARCHAR(200) NOT NULL,
	duracionTratamiento VARCHAR(200) NOT NULL,
	indicaciones VARCHAR(200) NOT NULL,
	precio DECIMAL NOT NULL,
	estado BIT NOT NULL,
	CONSTRAINT PK_DetalleTratamiento_idTratamiento
		PRIMARY KEY CLUSTERED (idTratamiento)
)
GO
drop table Pacientes.DetalleTratamiento

-- Crear las tablas de Puesto
CREATE TABLE Empleados.Puesto (
	idPuesto INT NOT NULL IDENTITY,
	DescripcionPuesto VARCHAR(100) NOT NULL,
	CONSTRAINT PK_Puesto_idPuesto
		PRIMARY KEY CLUSTERED (idPuesto)
)
GO

-- Crear las tablas de Empleado
CREATE TABLE Empleados.Empleado (
	identidad VARCHAR(20) NOT NULL,
	nombres VARCHAR(50) NOT NULL,
	apellidos VARCHAR(50) NOT NULL,
	direccion VARCHAR(200) NOT NULL,
	correoElectronico VARCHAR(200) NOT NULL,
	celular VARCHAR(15) NOT NULL,
	sexo VARCHAR(15) NOT NULL,
	idPuesto INT NOT NULL,
	estado BIT NOT NULL,
	CONSTRAINT PK_Empleado_identidad
		PRIMARY KEY CLUSTERED (identidad),
	CONSTRAINT Fk_Empleado$Pertenece$Puesto
		foreign key (idPuesto) references Empleados.Puesto(idPuesto)
)
GO

-- Crear las tablas de HistorialConsulta
CREATE TABLE Pacientes.HistorialConsulta (
	idHistorialConsulta INT NOT NULL IDENTITY,
	idHistorialClinico INT NOT NULL,
	fechaConsulta DATE NOT NULL,
	motivoConsulta VARCHAR(200) NOT NULL,
	identidadEmpleado VARCHAR(20) NOT NULL,
	estado BIT NOT NULL,
	CONSTRAINT PK_HistorialConsulta_idHistorialConsulta
		PRIMARY KEY CLUSTERED (idHistorialConsulta),
	CONSTRAINT Fk_HistorialConsulta$Pertenece$Empleado
		foreign key (identidadEmpleado) references Empleados.Empleado(identidad),
	CONSTRAINT Fk_HistorialConsulta$Pertenece$HistorialClinico
		foreign key (idHistorialClinico) references Pacientes.HistorialClinico(idHistorialClinico)
)
GO
drop table Pacientes.HistorialConsulta

--1. Tabla los tratamientos por consulta
create table Pacientes.Tratamiento
(
	idTratamiento int not null,
	idHistorialConsulta int not null,
	estado BIT NOT NULL,
	constraint PK_Tratamiento_idTratamiento_idHistorialConsulta
		primary key clustered(idTratamiento, idHistorialConsulta), --Para acceder a los datos
	constraint Fk_Tratamiento$Existe$DetalleTratamiento
		foreign key (idTratamiento) references Pacientes.DetalleTratamiento(idTratamiento),
	constraint Fk_Tratamiento$Pertenece$HistorialConsulta
		foreign key (idHistorialConsulta) references Pacientes.HistorialConsulta(idHistorialConsulta)
)
GO
drop table Pacientes.Tratamiento
--Tabla de usuario

CREATE TABLE Empleados.Usuario (
	id INT NOT NULL IDENTITY,
	nombreCompleto VARCHAR(255) NOT NULL,
	username VARCHAR(100) NOT NULL,
	password VARCHAR(100) NOT NULL,
	estado BIT NOT NULL,
	idPuesto INT NOT NULL,
	CONSTRAINT PK_Usuario_id
		PRIMARY KEY CLUSTERED (id),
	CONSTRAINT Fk_Usuario$Pertenece$Puesto
		foreign key (idPuesto) references Empleados.Puesto(idPuesto)

)
GO

drop table Empleados.Usuario

-- Restricciones de las tablas

-- El sexo es: Masculino, Femenino.
ALTER TABLE Pacientes.Paciente WITH CHECK
	ADD CONSTRAINT CHK_Pacientes_Paciente$sexoPaciente
	CHECK (sexo IN('Masculino', 'Femenino'))
GO


-- No puede existir nombres de usuarios repetidos
ALTER TABLE Empleados.Usuario
	ADD CONSTRAINT AK_Empleados_Usuario_username
	UNIQUE NONCLUSTERED (username)
GO

-- La contraseña debe contener al menos 6 caracteres
ALTER TABLE Empleados.Usuario WITH CHECK
	ADD CONSTRAINT CHK_Empleados_Usuario$VerificarLongitudContraseña
	CHECK (LEN(password) >= 6)
GO


-- Ingresar datos

insert into Empleados.Usuario
values ('Heimy Daniela Garcia Giron','Daniela22','holamundo',1)

select * from Empleados.Usuario