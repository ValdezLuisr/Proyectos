CREATE DATABASE TutoriasAcademicas;
GO

USE TutoriasAcademicas;
GO

-----------------------------------------------------
-- 1. Tabla de Roles (Alumno / Maestro )
-----------------------------------------------------
CREATE TABLE Roles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL UNIQUE
);

INSERT INTO Roles (Nombre) VALUES 
('Alumno'),
('Maestro'),


-----------------------------------------------------
-- 2. Tabla de Usuarios (usada para login)
--    Aquí guardas a alumnos y maestros MANUALMENTE
-----------------------------------------------------
CREATE TABLE Usuarios (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    NombreCompleto VARCHAR(150) NOT NULL,
    Correo VARCHAR(150) NOT NULL UNIQUE,
    ContrasenaHash VARCHAR(500) NOT NULL,   -- Guarda hash, no texto plano
    RolId INT NOT NULL,
    CONSTRAINT FK_Usuarios_Roles FOREIGN KEY (RolId)
        REFERENCES Roles(Id)
);

-----------------------------------------------------
-- 3. Tabla de Tutorías (solo maestros pueden crearlas)
-----------------------------------------------------
CREATE TABLE Tutorias (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Materia VARCHAR(150) NOT NULL,
    Descripcion VARCHAR(500),
    Cupo INT NOT NULL,
    MaestroId INT NOT NULL,
	Horario varchar(500) NOT NULL,
	Modalidad varchar (50),
	Aula varchar(20),
    CONSTRAINT FK_Tutorias_Usuarios FOREIGN KEY (MaestroId)
        REFERENCES Usuarios(Id)
);

-----------------------------------------------------
-- 4. Inscripción de tutorías (solo alumnos)
-----------------------------------------------------
CREATE TABLE Inscripciones (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    TutoriaId INT NOT NULL,
    AlumnoId INT NOT NULL,

    CONSTRAINT FK_Inscripciones_Tutorias FOREIGN KEY (TutoriaId)
        REFERENCES Tutorias(Id),

    CONSTRAINT FK_Inscripciones_Usuarios FOREIGN KEY (AlumnoId)
        REFERENCES Usuarios(Id)
);

-----------------------------------------------------
-- 5. Evitar inscripciones duplicadas
-----------------------------------------------------
ALTER TABLE Inscripciones
ADD CONSTRAINT UQ_Inscripcion UNIQUE (TutoriaId, AlumnoId);

-----------------------------------------------------
-- 5. Datos de prueba
-----------------------------------------------------
INSERT INTO Usuarios (NombreCompleto, Correo, ContrasenaHash, RolId)
values('Luis ALexander Vasquez Valdez','2512532024@mail.utec.edu.sv','12345', 2);
INSERT INTO Usuarios (NombreCompleto, Correo, ContrasenaHash, RolId)
VALUES ('Amy Alejandra Guerrero Gonzalez', '2504482024@mail.utec.edu.sv', '12345', 1);  -- Alumno
INSERT INTO Usuarios (NombreCompleto, Correo, ContrasenaHash, RolId)
VALUES ('Carlos Alejandro Pérez Sola', 'carlos.perez@mail.utec.edu.sv', '12345', 2);  -- Maestro

INSERT INTO Tutorias (Materia, Descripcion, Cupo, MaestroId,Horario, Modalidad, Aula)	
VALUES ('Algoritmos', 'Pseint', '10',2,'Miercoles-Sabado 6:30-8:00', 'Enlinea',' Teams');--Tutoria
select * from  Usuarios
select * from Tutorias
select * from Inscripciones
GO

