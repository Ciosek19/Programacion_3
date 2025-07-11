IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'VozDelEsteBD')
BEGIN
    CREATE DATABASE VozDelEsteBD;
END
GO
-- Cambiar al contexto de la base de datos solo despu�s de que exista
USE VozDelEsteBD;
GO
DROP TABLE IF EXISTS UsuarioRol;
GO
DROP TABLE IF EXISTS RolPermiso;
GO
DROP TABLE IF EXISTS ComentarioPrograma;
GO
DROP TABLE IF EXISTS ConductorProgramaRadio;
GO
DROP TABLE IF EXISTS Programacion;
GO
DROP TABLE IF EXISTS Conductor;
GO
DROP TABLE IF EXISTS Usuario;
GO
DROP TABLE IF EXISTS Permiso;
GO
DROP TABLE IF EXISTS Rol;
GO
DROP TABLE IF EXISTS Persona;
GO
DROP TABLE IF EXISTS ProgramaRadio;
GO
DROP TABLE IF EXISTS Patrocinador;
GO
DROP TABLE IF EXISTS Noticia;
GO
DROP TABLE IF EXISTS CotizacionMoneda;
GO
DROP TABLE IF EXISTS Clima;
GO
-- Tabla Persona
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Persona')
BEGIN
    CREATE TABLE Persona(
        Id INT PRIMARY KEY IDENTITY,
        Nombre NVARCHAR(50) NOT NULL,
        Apellido NVARCHAR(50) NOT NULL,
		ImagenUrl NVARCHAR(500),
        FechaNacimiento DATETIME NOT NULL
    );
END
GO
-- Tabla Permiso
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Permiso')
BEGIN
    CREATE TABLE Permiso(
        Id INT PRIMARY KEY IDENTITY,
        Nombre NVARCHAR(50) NOT NULL
    );
END
GO
-- Tabla Rol
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Rol')
BEGIN
    CREATE TABLE Rol(
        Id INT PRIMARY KEY IDENTITY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE
    );
END
GO
-- Tabla RolPermiso
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'RolPermiso')
BEGIN
    CREATE TABLE RolPermiso(
        RolID INT NOT NULL,
        PermisoID INT NOT NULL,
		PRIMARY KEY (RolID,PermisoID),
		FOREIGN KEY (RolID) REFERENCES Rol(Id),
		FOREIGN KEY (PermisoID) REFERENCES Permiso(Id)
    );
END
GO
-- Tabla Usuario
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Usuario')
BEGIN
	CREATE TABLE Usuario (
		Id INT PRIMARY KEY IDENTITY,
		PersonaID INT NOT NULL UNIQUE,
		NombreUsuario NVARCHAR(50) NOT NULL,
		Email NVARCHAR(100) NOT NULL UNIQUE,
		Clave NVARCHAR(255) NOT NULL,
		Silenciado BIT NOT NULL DEFAULT 0,
		FOREIGN KEY (PersonaID) REFERENCES Persona(Id)
	);
END
GO
-- Tabla UsuarioRol
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'UsuarioRol')
BEGIN
    CREATE TABLE UsuarioRol(
        UsuarioID INT NOT NULL,
        RolID INT NOT NULL,
		PRIMARY KEY (UsuarioID,RolID),
		FOREIGN KEY (UsuarioID) REFERENCES Usuario(Id),
		FOREIGN KEY (RolID) REFERENCES Rol(Id)
    );
END
GO
-- Tabla Conductor (vinculada a Persona)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Conductor')
BEGIN
    CREATE TABLE Conductor(
        Id INT PRIMARY KEY IDENTITY,
        PersonaID INT NOT NULL UNIQUE,
        Biografia NVARCHAR(MAX),
        FOREIGN KEY (PersonaID) REFERENCES Persona(Id)
    );
END
GO
-- Tabla ProgramaRadio
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ProgramaRadio')
BEGIN
    CREATE TABLE ProgramaRadio(
        Id INT PRIMARY KEY IDENTITY,
        Nombre NVARCHAR(50) NOT NULL,
        ImagenUrl NVARCHAR(500),
        Descripcion NVARCHAR(MAX),
		Duracion TIME NOT NULL
    );
END
GO
-- Tabla Programacion
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Programacion')
BEGIN
    CREATE TABLE Programacion(
        Id INT PRIMARY KEY IDENTITY,
        ProgramaID INT NOT NULL,
        FechaHorario DATETIME NOT NULL,
        FOREIGN KEY (ProgramaID) REFERENCES ProgramaRadio(Id)
    );
END
GO
-- Tabla ConductorProgramaRadio (muchos a muchos)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ConductorProgramaRadio')
BEGIN
    CREATE TABLE ConductorProgramaRadio(
        ConductorID INT NOT NULL,
        ProgramaID INT NOT NULL,
        PRIMARY KEY (ConductorID, ProgramaID),
        FOREIGN KEY (ConductorID) REFERENCES Conductor(Id),
        FOREIGN KEY (ProgramaID) REFERENCES ProgramaRadio(Id)
    );
END
GO
-- Tabla Patrocinador
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Patrocinador')
BEGIN
    CREATE TABLE Patrocinador(
        Id INT PRIMARY KEY IDENTITY,
        Nombre NVARCHAR(50) NOT NULL,
        Descripcion NVARCHAR(MAX),
        TransmisionDiaria INT,
		UrlImagen NVARCHAR(255)
    );
END
GO
-- Tabla Noticia
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Noticia')
BEGIN
    CREATE TABLE Noticia(
        Id INT PRIMARY KEY IDENTITY,
        Titulo NVARCHAR(100) NOT NULL,
        Contenido NVARCHAR(MAX) NOT NULL,
        FechaPublicacion DATETIME DEFAULT GETDATE() NOT NULL,
        Imagen NVARCHAR(500)
    );
END
GO
-- Tabla ComentarioPrograma
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'ComentarioPrograma')
BEGIN
    CREATE TABLE ComentarioPrograma(
        Id INT PRIMARY KEY IDENTITY,
		UsuarioID INT NOT NULL,
        ProgramaID INT NOT NULL,
        Comentario NVARCHAR(MAX) NOT NULL,
        FOREIGN KEY (UsuarioID) REFERENCES Usuario(Id),
        FOREIGN KEY (ProgramaID) REFERENCES ProgramaRadio(Id)
    );
END
GO
-- Tabla CotizacionMoneda
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'CotizacionMoneda')
BEGIN
    CREATE TABLE CotizacionMoneda(
        Id INT PRIMARY KEY IDENTITY,
        Fecha DATETIME NOT NULL,
        TipoMoneda NVARCHAR(50) NOT NULL,
        ValorCompra DECIMAL(10,2) NOT NULL,
		ValorVenta DECIMAL(10,2) NOT NULL
    );
END
GO
-- Tabla Clima
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clima')
BEGIN
    CREATE TABLE Clima(
        Id INT PRIMARY KEY IDENTITY,
        Fecha DATETIME NOT NULL,
        Temperatura DECIMAL(5,2) NOT NULL,
        Icono NVARCHAR(500),
		Humedad INT NOT NULL,
		Viento INT NOT NULL,
		Condicion VARCHAR(100) NOT NULL
    );
END
GO
-- DATOS

-- Tabla Persona
INSERT INTO Persona (Nombre, Apellido, ImagenUrl, FechaNacimiento) VALUES
('Ana', 'Garc�a', NULL, '1982-04-15'),
('Luis', 'Fern�ndez', NULL, '1978-09-10'),
('Sof�a', 'Mart�nez', NULL, '1990-11-05'),
('Carlos', 'Rodr�guez', NULL, '1985-01-20'),
('Mar�a', 'L�pez', NULL, '1992-06-25'),
('Jos�', 'Hern�ndez', NULL, '1980-12-30'),
('Laura', 'G�mez', NULL, '1988-03-14'),
('Miguel', 'D�az', NULL, '1975-07-07'),
('Isabel', 'P�rez', NULL, '1995-02-28'),
('Pedro', 'S�nchez', NULL, '1983-08-18'),
('Elena', 'Ram�rez', NULL, '1987-05-22'),
('Jorge', 'Torres', NULL, '1979-10-09'),
('Luc�a', 'Flores', NULL, '1991-04-02'),
('Diego', 'Vargas', NULL, '1984-11-11'),
('Carolina', 'Mendoza', NULL, '1993-09-17'),
('Andr�s', 'Castillo', NULL, '1981-03-29'),
('Natalia', 'Rojas', NULL, '1989-12-05'),
('Ricardo', 'Morales', NULL, '1977-06-06'),
('Ver�nica', 'Ortiz', NULL, '1994-07-23'),
('Fernando', 'Silva', NULL, '1986-01-13');
GO
-- Tabla Permiso
INSERT INTO Permiso (Nombre) VALUES 
('Gestion Programas'),
('Gestion Patrocinadores'),
('Gestion Noticias'),
('Gestion Usuarios'),
('Gestion Roles');
GO
-- Tabla Rol
INSERT INTO Rol (Nombre) VALUES
('Admin'),
('Editor'),
('Oyente');
GO
-- Tabla RolPermiso
-- Permisos para Admin (RolID = 1): todos los permisos
INSERT INTO RolPermiso (RolID, PermisoID) VALUES
(1, 1),
(1, 2),
(1, 3),
(1, 4),
(2, 3),
(1, 5);
GO
-- Tabla Usuario
INSERT INTO Usuario (PersonaID, NombreUsuario, Email, Clave, Silenciado) VALUES
(1, 'ana.garcia', 'ana.garcia@example.com', '1234', 0),
(2, 'luis.fernandez', 'luis.fernandez@example.com', '1234', 1), -- silenciado
(3, 'sofia.martinez', 'sofia.martinez@example.com', '1234', 0),
(4, 'carlos.rodriguez', 'carlos.rodriguez@example.com', '1234', 0),
(5, 'maria.lopez', 'maria.lopez@example.com', '1234', 0),
(6, 'jose.hernandez', 'jose.hernandez@example.com', '1234', 0);

GO
-- Tabla UsuarioRol
INSERT INTO UsuarioRol (UsuarioID, RolID) VALUES
(1, 1), -- Ana Garc�a: Admin
(2, 3), -- Luis Fern�ndez: Oyente (silenciado)
(3, 2), -- Sof�a Mart�nez: Editor
(4, 3), -- Carlos Rodr�guez: Oyente
(5, 3), -- Mar�a L�pez: Oyente
(6, 3); -- Jos� Hern�ndez: Oyente
GO


-- Tabla Conductor
INSERT INTO Conductor (PersonaID, Biografia) VALUES
(7, 'Laura G�mez es una conductora con amplia experiencia en radio y comunicaci�n.'),
(8, 'Miguel D�az tiene una trayectoria destacada en programas matutinos.'),
(9, 'Isabel P�rez es reconocida por su voz c�lida y entrevistas profundas.'),
(10, 'Pedro S�nchez aporta frescura y carisma a sus programas.'),
(11, 'Elena Ram�rez se especializa en m�sica y cultura.'),
(12, 'Jorge Torres es un conductor con gran conocimiento en deportes.'),
(13, 'Luc�a Flores es una joven promesa en el �mbito radial.'),
(14, 'Diego Vargas tiene experiencia en programas de tecnolog�a y ciencia.'),
(15, 'Carolina Mendoza es conocida por su estilo din�mico y cercano.'),
(16, 'Andr�s Castillo aporta an�lisis y opini�n en sus programas.'),
(17, 'Natalia Rojas tiene un enfoque en noticias y actualidad.'),
(18, 'Ricardo Morales es experto en m�sica cl�sica y jazz.'),
(19, 'Ver�nica Ortiz destaca en programas de entretenimiento.'),
(20, 'Fernando Silva es un conductor veterano con amplia audiencia.');
GO
-- Tabla ProgramaRadio
INSERT INTO ProgramaRadio (Nombre, ImagenUrl, Descripcion, Duracion) VALUES
('Ma�anas Vibrantes', NULL, 'Un programa matutino lleno de energ�a, noticias y buena m�sica para empezar el d�a.', '01:30:00'),
('Tardes de Jazz', NULL, 'Selecci�n especial de jazz cl�sico y contempor�neo para acompa�ar tus tardes.', '02:00:00'),
('Noticias al Instante', NULL, 'Actualidad y an�lisis de las noticias m�s relevantes del d�a.', '01:00:00'),
('Hora de Rock', NULL, 'Lo mejor del rock nacional e internacional con entrevistas exclusivas.', '01:45:00'),
('Cultura y Letras', NULL, 'Espacio dedicado a la literatura, el arte y la cultura en general.', '01:15:00'),
('Deportes en Vivo', NULL, 'Cobertura y debate de los eventos deportivos m�s importantes.', '02:30:00'),
('Tecnolog�a Hoy', NULL, 'Novedades y tendencias tecnol�gicas explicadas para todos.', '01:20:00'),
('Noche de Nostalgia', NULL, 'M�sica de todas las �pocas para revivir grandes recuerdos en la noche.', '03:00:00');
GO

GO
-- Tabla Programacion
-- Programaci�n semanal de programas de radio
INSERT INTO Programacion (ProgramaID, FechaHorario) VALUES
-- D�a 1
(1, '2025-06-14 08:00:00'), -- Ma�anas Vibrantes
(3, '2025-06-14 12:00:00'), -- Noticias al Instante
(2, '2025-06-14 15:00:00'), -- Tardes de Jazz
(4, '2025-06-14 18:00:00'), -- Hora de Rock
(8, '2025-06-14 21:00:00'), -- Noche de Nostalgia


-- D�a 2
(1, '2025-06-15 08:00:00'),
(6, '2025-06-15 11:00:00'), -- Deportes en Vivo
(5, '2025-06-15 14:00:00'), -- Cultura y Letras
(2, '2025-06-15 17:00:00'),
(8, '2025-06-15 21:00:00'),

-- D�a 3
(1, '2025-06-16 08:00:00'),
(3, '2025-06-16 11:00:00'),
(7, '2025-06-16 14:00:00'), -- Tecnolog�a Hoy
(4, '2025-06-16 18:00:00'),
(8, '2025-06-16 21:00:00'),

-- D�a 4
(1, '2025-06-17 08:00:00'),
(5, '2025-06-17 11:00:00'),
(6, '2025-06-17 14:00:00'),
(2, '2025-06-17 17:00:00'),
(8, '2025-06-17 21:00:00'),

-- D�a 5
(1, '2025-06-18 08:00:00'),
(3, '2025-06-18 11:00:00'),
(5, '2025-06-18 14:00:00'),
(4, '2025-06-18 17:00:00'),
(8, '2025-06-18 21:00:00'),

-- D�a 6
(1, '2025-06-19 08:00:00'),
(7, '2025-06-19 11:00:00'),
(6, '2025-06-19 14:00:00'),
(2, '2025-06-19 17:00:00'),
(8, '2025-06-19 21:00:00'),

-- D�a 7
(1, '2025-06-20 08:00:00'),
(3, '2025-06-20 11:00:00'),
(5, '2025-06-20 14:00:00'),
(4, '2025-06-20 17:00:00'),
(8, '2025-06-20 21:00:00');
GO
-- Tabla ConductorProgramaRadio
-- Relaci�n muchos a muchos entre conductores y programas de radio
-- ConductorID va del 1 al 14 en orden de inserci�n

INSERT INTO ConductorProgramaRadio (ConductorID, ProgramaID) VALUES
-- Ma�anas Vibrantes
(2, 1),  -- Miguel D�az
(4, 1),  -- Pedro S�nchez
(14, 1), -- Fernando Silva

-- Tardes de Jazz
(13, 2), -- Ricardo Morales
(3, 2),  -- Isabel P�rez
(8, 2),  -- Diego Vargas

-- Noticias al Instante
(11, 3), -- Natalia Rojas
(10, 3), -- Andr�s Castillo
(1, 3),  -- Laura G�mez

-- Hora de Rock
(4, 4),  -- Pedro S�nchez
(7, 4),  -- Luc�a Flores
(9, 4),  -- Carolina Mendoza

-- Cultura y Letras
(5, 5),  -- Elena Ram�rez
(3, 5),  -- Isabel P�rez
(10, 5), -- Andr�s Castillo
(7, 5),  -- Luc�a Flores

-- Deportes en Vivo
(6, 6),  -- Jorge Torres
(14, 6), -- Fernando Silva

-- Tecnolog�a Hoy
(8, 7),  -- Diego Vargas
(10, 7), -- Andr�s Castillo
(1, 7),  -- Laura G�mez

-- Noche de Nostalgia
(12, 8), -- Ver�nica Ortiz
(13, 8), -- Ricardo Morales
(5, 8),  -- Elena Ram�rez
(3, 8);  -- Isabel P�rez
GO
-- Tabla Patrocinador
INSERT INTO Patrocinador (Nombre, Descripcion, TransmisionDiaria) VALUES
('Caf� Amanecer', 'Caf� artesanal que acompa�a tus ma�anas con el mejor sabor.', 5),
('TecnoMundo', 'Tu tienda de tecnolog�a con las �ltimas novedades en electr�nica.', 3),
('Viajes Solaris', 'Agencia de turismo que te lleva a conocer el mundo.', 4),
('Agua Cristalina', 'Agua mineral natural para mantenerte hidratado todo el d�a.', 2),
('Autos del Sur', 'Concesionaria con los mejores precios y financiaci�n del mercado.', 3),
('Librer�a Atenea', 'Especialistas en libros, cultura y eventos literarios.', 1),
('Fitness Plus', 'Gimnasios y centros de entrenamiento con cobertura nacional.', 2),
('Banco Uni�n', 'Tu banco de confianza con soluciones para cada etapa de tu vida.', 4),
('Moda Urbana', 'Ropa moderna para j�venes y adultos con estilo.', 2),
('Helados Delicia', 'Los mejores helados artesanales con sabores �nicos.', 3);
GO
-- Tabla Noticia
-- Inserci�n de 12 noticias con contenido detallado
INSERT INTO Noticia (Titulo, Contenido, FechaPublicacion, Imagen) VALUES
('Nueva ley de tr�nsito entrar� en vigor el pr�ximo mes',
'El gobierno anunci� la implementaci�n de una nueva ley de tr�nsito que busca reducir los accidentes viales en un 30% durante el primer a�o. Entre las principales modificaciones se incluyen el aumento en las sanciones por exceso de velocidad, obligatoriedad del uso del cintur�n de seguridad en todos los asientos del veh�culo y controles de alcoholemia m�s estrictos. Las autoridades aseguran que esta normativa ha sido trabajada con expertos y ser� acompa�ada por campa�as educativas.', GETDATE(), NULL),

('Festival Internacional de Cine se celebrar� en nuestra ciudad',
'Durante la primera semana de julio, la ciudad ser� sede del Festival Internacional de Cine, que reunir� a directores, actores y productores de todo el mundo. Se proyectar�n m�s de 100 pel�culas en distintas sedes culturales, con entradas accesibles y charlas abiertas al p�blico. El evento busca promover la diversidad cultural y posicionar a la ciudad como un nuevo polo audiovisual.', GETDATE(), NULL),

('Avances en la vacuna contra el virus respiratorio estacional',
'Un equipo de cient�ficos del Instituto Nacional de Salud logr� avances significativos en la creaci�n de una vacuna contra el virus respiratorio estacional, que afecta a miles de personas cada a�o. Seg�n los estudios cl�nicos, la f�rmula ha demostrado un 85% de efectividad en adultos mayores. El proceso de aprobaci�n est� en marcha y se espera su distribuci�n para fin de a�o.', GETDATE(), NULL),

('Se inaugura un nuevo parque ecol�gico en el sur de la ciudad',
'Con el objetivo de promover el contacto con la naturaleza y generar conciencia ambiental, se ha inaugurado un nuevo parque ecol�gico en el sur de la ciudad. El espacio cuenta con senderos interpretativos, �reas de picnic, espacios educativos y una zona especial para la observaci�n de aves. El proyecto fue financiado con fondos municipales y donaciones privadas.', GETDATE(), NULL),

('El sector tecnol�gico genera m�s de 10.000 nuevos empleos',
'Seg�n un reciente informe del Ministerio de Producci�n, el sector tecnol�gico gener� m�s de 10.000 nuevos empleos en el �ltimo semestre. Las �reas m�s demandadas incluyen desarrollo de software, ciberseguridad y an�lisis de datos. Se proyecta que esta tendencia continuar� en alza debido a la transformaci�n digital en empresas e instituciones.', GETDATE(), NULL),

('Encuesta revela que el 70% de los ciudadanos desea m�s transporte p�blico',
'Una encuesta realizada por la Universidad Nacional indica que el 70% de los ciudadanos considera urgente mejorar la red de transporte p�blico. Las principales quejas se relacionan con la frecuencia de los servicios, el estado de las unidades y la falta de conexiones con zonas perif�ricas. Expertos aseguran que invertir en movilidad sostenible es clave para el desarrollo urbano.', GETDATE(), NULL),

('Campa�a de donaci�n de sangre supera expectativas',
'La �ltima campa�a de donaci�n de sangre organizada por el Hospital Central logr� recolectar m�s de 3.000 unidades en una semana, superando ampliamente las expectativas. Las autoridades agradecieron la participaci�n masiva de la comunidad y destacaron la importancia de mantener las reservas en niveles adecuados durante todo el a�o.', GETDATE(), NULL),

('Artistas locales presentan exposici�n colectiva en el museo de arte',
'Un grupo de artistas locales present� una exposici�n colectiva en el Museo de Arte Contempor�neo, con obras que abarcan desde la pintura hasta la instalaci�n y el videoarte. La muestra, titulada "Reflejos de nuestra tierra", estar� abierta durante dos meses y busca visibilizar el talento emergente y fomentar la producci�n cultural regional.', GETDATE(), NULL),

('La inflaci�n registra una leve baja por tercer mes consecutivo',
'Los datos del Instituto Nacional de Estad�stica revelan que la inflaci�n ha registrado una leve baja por tercer mes consecutivo, situ�ndose en un 2,3% mensual. Esta tendencia positiva ha sido atribuida a pol�ticas de control de precios y estabilidad cambiaria. Sin embargo, economistas advierten que a�n persisten desaf�os estructurales.', GETDATE(), NULL),

('Estudiantes desarrollan app para ayudar a personas mayores con la tecnolog�a',
'Estudiantes de ingenier�a inform�tica de la Universidad T�cnica lanzaron una aplicaci�n pensada para asistir a personas mayores en el uso de tecnolog�a. La app ofrece asistencia paso a paso para el uso de funciones b�sicas como llamadas, mensajes y videollamadas. Tambi�n incluye botones grandes y modo de lectura f�cil.', GETDATE(), NULL),

('Nueva edici�n de la Feria del Libro re�ne a miles de visitantes',
'La Feria del Libro abri� sus puertas con m�s de 300 editoriales, presentaciones de autores y actividades para todas las edades. Durante el fin de semana se registraron m�s de 50.000 visitantes, lo que marca un r�cord hist�rico para el evento. La feria continuar� durante 10 d�as con entrada libre y gratuita.', GETDATE(), NULL),

('Concierto solidario re�ne fondos para centro de salud rural',
'Este fin de semana se realiz� un concierto solidario a beneficio del Centro de Salud Rural de San Pedro. Participaron artistas reconocidos y bandas emergentes, logrando reunir fondos para la compra de equipamiento m�dico. La comunidad agradeci� el gesto y destac� el poder de la m�sica como herramienta solidaria.', GETDATE(), NULL);
GO
-- Tabla ComentarioPrograma
-- Inserci�n de 20 comentarios en programas de radio
INSERT INTO ComentarioPrograma (UsuarioID, ProgramaID, Comentario) VALUES
(1, 1, '�Este programa me llena de energ�a cada ma�ana!'),
(3, 1, 'Muy buena selecci�n musical para empezar el d�a.'),
(4, 1, 'Escucharlo se ha vuelto parte de mi rutina diaria.'),
(5, 2, 'El jazz de las tardes me relaja mucho despu�s del trabajo.'),
(6, 2, 'Gran programa, ojal� tuviera m�s tiempo de emisi�n.'),
(1, 2, 'Excelente curadur�a musical.'),
(3, 3, 'Me gusta c�mo abordan las noticias con objetividad.'),
(5, 3, 'Gracias por mantenernos informados d�a a d�a.'),
(6, 3, 'Periodismo serio y de calidad, los felicito.'),
(1, 4, '�El bloque de rock cl�sico fue espectacular!'),
(3, 4, 'Me encantan las entrevistas con m�sicos nacionales.'),
(4, 4, 'Gran contenido, cada semana sorprenden.'),
(5, 5, 'Siempre descubro algo nuevo en este programa cultural.'),
(6, 5, 'Los debates sobre literatura son lo mejor.'),
(1, 6, 'Informes deportivos muy completos y bien explicados.'),
(3, 6, 'Gracias por cubrir tambi�n deportes menos populares.'),
(4, 7, 'Muy interesante lo de la inteligencia artificial de hoy.'),
(5, 7, 'Ideal para los que queremos entender la tecnolog�a.'),
(6, 8, 'Canciones que me hacen viajar en el tiempo, �gracias!'),
(3, 8, 'Este programa me trae muchos recuerdos felices.');
GO
-- Nota: UsuarioID 2 (luis.fernandez) est� silenciado, por lo tanto no se incluye.

-- Tabla Cotizacion Moneda
-- Cotizaciones del 3 al 9 de junio de 2025 en pesos uruguayos (UYU)

-- 3 de junio 
INSERT INTO CotizacionMoneda (Fecha, TipoMoneda, ValorCompra, ValorVenta) VALUES
('2025-06-03', 'USD', 38.20, 39.90),
('2025-06-03', 'EUR', 41.10, 42.80),
('2025-06-03', 'BRL', 7.45, 7.70),
('2025-06-03', 'ARS', 0.031, 0.034),

-- 4 de junio
('2025-06-04', 'USD', 38.30, 40.00),
('2025-06-04', 'EUR', 41.20, 42.90),
('2025-06-04', 'BRL', 7.46, 7.71),
('2025-06-04', 'ARS', 0.031, 0.034),

-- 5 de junio
('2025-06-05', 'USD', 38.10, 39.80),
('2025-06-05', 'EUR', 41.05, 42.70),
('2025-06-05', 'BRL', 7.42, 7.68),
('2025-06-05', 'ARS', 0.030, 0.033),

-- 6 de junio
('2025-06-06', 'USD', 38.35, 40.05),
('2025-06-06', 'EUR', 41.30, 43.00),
('2025-06-06', 'BRL', 7.50, 7.75),
('2025-06-06', 'ARS', 0.031, 0.035),

-- 7 de junio
('2025-06-07', 'USD', 38.25, 39.95),
('2025-06-07', 'EUR', 41.15, 42.85),
('2025-06-07', 'BRL', 7.48, 7.73),
('2025-06-07', 'ARS', 0.031, 0.034),

-- 8 de junio
('2025-06-08', 'USD', 38.40, 40.10),
('2025-06-08', 'EUR', 41.40, 43.10),
('2025-06-08', 'BRL', 7.51, 7.76),
('2025-06-08', 'ARS', 0.032, 0.035),

-- 9 de junio
('2025-06-09', 'USD', 38.50, 40.20),
('2025-06-09', 'EUR', 41.50, 43.20),
('2025-06-09', 'BRL', 7.55, 7.80),
('2025-06-09', 'ARS', 0.032, 0.035);
GO
-- Tabla Clima
-- Datos de clima del 3 al 9 de junio de 2025
INSERT INTO Clima (Fecha, Temperatura, Icono, Humedad, Viento, Condicion) VALUES
('2025-06-03', 17.5, 'nublado.png', 68, 15, 'Nublado'),
('2025-06-04', 20.2, 'parcial_nublado.png', 62, 12, 'Parcialmente nublado'),
('2025-06-05', 22.8, 'soleado.png', 55, 10, 'Soleado'),
('2025-06-06', 19.4, 'lluvia_ligera.png', 78, 20, 'Lluvia ligera'),
('2025-06-07', 16.1, 'tormenta.png', 85, 25, 'Tormenta el�ctrica'),
('2025-06-08', 18.9, 'nublado.png', 70, 18, 'Nublado'),
('2025-06-09', 21.6, 'soleado.png', 60, 14, 'Soleado');

-- Programaci�n para hoy: 2025-07-02
INSERT INTO Programacion (ProgramaID, FechaHorario) VALUES
(1, '2025-07-02 08:00:00'),  -- Ma�anas Vibrantes
(3, '2025-07-02 11:00:00'),  -- Noticias al Instante
(2, '2025-07-02 14:00:00'),  -- Tardes de Jazz
(4, '2025-07-02 17:00:00'),  -- Hora de Rock
(8, '2025-07-02 21:00:00');  -- Noche de Nostalgia

select * from ProgramaRadio