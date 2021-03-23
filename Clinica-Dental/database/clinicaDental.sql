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
	CONSTRAINT PK_Cita_idCita
		PRIMARY KEY CLUSTERED (idCita),
	CONSTRAINT Fk_Cita$Pertenece$HistorialClinico
		foreign key (idHistorialClinico) references Pacientes.HistorialClinico(idHistorialClinico)
)
GO


-- Crear las tablas de HistorialConsulta
CREATE TABLE Pacientes.HistorialConsulta (
	idHistorialConsulta INT NOT NULL IDENTITY,
	idHistorialClinico INT NOT NULL,
	motivoConsulta VARCHAR(200) NOT NULL,
	identidadPaciente VARCHAR(20) NOT NULL,
	CONSTRAINT PK_HistorialConsulta_idHistorialConsulta
		PRIMARY KEY CLUSTERED (idHistorialConsulta),
	CONSTRAINT Fk_HistorialConsulta$Pertenece$Paciente
		foreign key (identidadPaciente) references Pacientes.Paciente(identidad),
	CONSTRAINT Fk_HistorialConsulta$Pertenece$HistorialClinico
		foreign key (idHistorialClinico) references Pacientes.HistorialClinico(idHistorialClinico)
)
GO


-- Crear las tablas de DetalleTratamiento
CREATE TABLE Pacientes.DetalleTratamiento (
	idTratamiento INT NOT NULL IDENTITY,
	duracionTratamiento VARCHAR(200) NOT NULL,
	indicaciones VARCHAR(200) NOT NULL,
	precio DECIMAL NOT NULL,
	CONSTRAINT PK_DetalleTratamiento_idTratamiento
		PRIMARY KEY CLUSTERED (idTratamiento)
)
GO

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
	CONSTRAINT PK_Empleado_identidad
		PRIMARY KEY CLUSTERED (identidad),
	CONSTRAINT Fk_Empleado$Pertenece$Puesto
		foreign key (idPuesto) references Empleados.Puesto(idPuesto)
)
GO

--1. Tabla los tratamoientos por consulta
create table Pacientes.Tratamiento
(
	idTratamiento int not null,
	idHistorialConsulta int not null,
	constraint PK_Tratamiento_idTratamiento_idHistorialConsulta
		primary key clustered(idTratamiento, idHistorialConsulta), --Para acceder a los datos
	constraint Fk_Tratamiento$Existe$DetalleTratamiento
		foreign key (idTratamiento) references Pacientes.DetalleTratamiento(idTratamiento),
	constraint Fk_Tratamiento$Pertenece$HistorialConsulta
		foreign key (idHistorialConsulta) references Pacientes.HistorialConsulta(idHistorialConsulta)
)
GO