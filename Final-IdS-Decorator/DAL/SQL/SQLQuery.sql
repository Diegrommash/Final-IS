-- 1. Crear la base si no existe (fuera de transacción)
IF DB_ID('Final-DB') IS NULL
BEGIN
    CREATE DATABASE [Final-DB];
END
GO

-- 2. Seleccionar la base
USE [Final-DB];
GO

-- 3. Eliminar procedimientos si existen
IF OBJECT_ID('SP_LOGIN_JUGADOR', 'P') IS NOT NULL
    DROP PROCEDURE SP_LOGIN_JUGADOR;
GO

IF OBJECT_ID('SP_BUSCAR_TODOS_ITEM', 'P') IS NOT NULL
    DROP PROCEDURE SP_BUSCAR_TODOS_ITEM;
GO

-- 4. Eliminar tablas en orden correcto (primero dependientes)
IF OBJECT_ID('Item', 'U') IS NOT NULL
    DROP TABLE Item;
GO

IF OBJECT_ID('Estadistica', 'U') IS NOT NULL
    DROP TABLE Estadistica;
GO

IF OBJECT_ID('TipoItem', 'U') IS NOT NULL
    DROP TABLE TipoItem;
GO

IF OBJECT_ID('Jugador', 'U') IS NOT NULL
    DROP TABLE Jugador;
GO

-- 5. Crear tablas y poblar datos dentro de transacción
BEGIN TRY
    BEGIN TRANSACTION;

    -- Jugador
    CREATE TABLE Jugador (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE,
        Contraseña NVARCHAR(100) NOT NULL
    );

    INSERT INTO Jugador (Nombre, Contraseña)
    VALUES 
    ('adm', 'adm'),
    ('Hechicera', 'magia456'),
    ('ArqueroX', 'flecha789');

    -- TipoItem
    CREATE TABLE TipoItem (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE
    );

    INSERT INTO TipoItem (Nombre)
    VALUES 
    ('Trabajo'),
    ('Arma'),
    ('Armadura'),
    ('Joya'),
    ('Pocion');

    -- Estadistica
    CREATE TABLE Estadistica (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE
    );

    -- Insertar Estadisticas
    INSERT INTO Estadistica (Nombre)
    VALUES 
    ('Fuerza'),
    ('Agilidad'),
    ('Inteligencia'),
    ('No_posee');

    -- Item
    CREATE TABLE Item (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE,
        Poder INT NOT NULL,
        Defensa INT NOT NULL,
        EstadisticaId INT NOT NULL,
        TipoItemId INT NOT NULL,
        FOREIGN KEY (EstadisticaId) REFERENCES Estadistica(Id),
        FOREIGN KEY (TipoItemId) REFERENCES TipoItem(Id)
    );

    -- Insertar Items
    INSERT INTO Item (Nombre, Poder, Defensa, EstadisticaId, TipoItemId)
    VALUES 
    ('Guerrero', 10, 5, (SELECT Id FROM Estadistica WHERE Nombre = 'Fuerza'), (SELECT Id FROM TipoItem WHERE Nombre = 'Trabajo')),
    ('Arquero', 15, 1, (SELECT Id FROM Estadistica WHERE Nombre = 'Agilidad'), (SELECT Id FROM TipoItem WHERE Nombre = 'Trabajo')),
    ('Mago', 20, 0, (SELECT Id FROM Estadistica WHERE Nombre = 'Inteligencia'), (SELECT Id FROM TipoItem WHERE Nombre = 'Trabajo')),
    ('Ladron', 15, 3, (SELECT Id FROM Estadistica WHERE Nombre = 'Agilidad'), (SELECT Id FROM TipoItem WHERE Nombre = 'Trabajo')),
    ('Espada corta', 4, 1, (SELECT Id FROM Estadistica WHERE Nombre = 'Agilidad'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Espada larga', 6, 2, (SELECT Id FROM Estadistica WHERE Nombre = 'Fuerza'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Espada Bastarda', 8, 3, (SELECT Id FROM Estadistica WHERE Nombre = 'Fuerza'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Mandoble', 10, 3, (SELECT Id FROM Estadistica WHERE Nombre = 'Fuerza'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Arco corto', 5, 1, (SELECT Id FROM Estadistica WHERE Nombre = 'Agilidad'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Arco largo', 10, 2, (SELECT Id FROM Estadistica WHERE Nombre = 'Agilidad'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Ballesta', 7, 1, (SELECT Id FROM Estadistica WHERE Nombre = 'Agilidad'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Baculo', 20, 0, (SELECT Id FROM Estadistica WHERE Nombre = 'Inteligencia'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Varita', 10, 0, (SELECT Id FROM Estadistica WHERE Nombre = 'Inteligencia'), (SELECT Id FROM TipoItem WHERE Nombre = 'Arma')),
    ('Armadura de placas', 1, 10, (SELECT Id FROM Estadistica WHERE Nombre = 'Fuerza'), (SELECT Id FROM TipoItem WHERE Nombre = 'Armadura')),
    ('Armadura de cuero', 2, 5, (SELECT Id FROM Estadistica WHERE Nombre = 'Agilidad'), (SELECT Id FROM TipoItem WHERE Nombre = 'Armadura')),
    ('Armadura de tela', 5,2 , (SELECT Id FROM Estadistica WHERE Nombre = 'Inteligencia'), (SELECT Id FROM TipoItem WHERE Nombre = 'Armadura')),
    ('Anillo de poder', 10, 0, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Joya')),
    ('Anillo de defensa', 0, 5, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Joya')),
    ('Anillo dual', 5, 5, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Joya')),
    ('Pocion de fuerza', 10, 0, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Pocion')),
    ('Pocion de defensa', 0, 10, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Pocion')),
    ('Pocion berzerker', 20, -5, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Pocion'));

    -- Personaje
    CREATE TABLE Personaje (
    Id INT PRIMARY KEY IDENTITY(1,1),
    Nombre NVARCHAR(100) NOT NULL
    );

    -- Relacion entre Personaje y Item
    CREATE TABLE PersonajeItem (
    PersonajeId INT NOT NULL,
    ItemId INT NOT NULL,
    Orden INT NOT NULL,
    PRIMARY KEY (PersonajeId, ItemId),
    FOREIGN KEY (PersonajeId) REFERENCES Personaje(Id),
    FOREIGN KEY (ItemId) REFERENCES Item(Id)
    );

    -- Relacion entre Jugador y Personaje
    CREATE TABLE JugadorPersonaje (
    JugadorId INT NOT NULL,
    PersonajeId INT NOT NULL,
    PRIMARY KEY (JugadorId, PersonajeId),
    FOREIGN KEY (JugadorId) REFERENCES Jugador(Id),
    FOREIGN KEY (PersonajeId) REFERENCES Personaje(Id)
    );

    COMMIT TRANSACTION;
    PRINT 'Transacción completada exitosamente.';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error: ' + ERROR_MESSAGE();
END CATCH;
GO



-- 6. Crear procedimientos
CREATE PROCEDURE SP_LOGIN_JUGADOR
    @Nombre NVARCHAR(50),
    @Contraseña NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre
    FROM Jugador
    WHERE Nombre = @Nombre AND Contraseña = @Contraseña;
END
GO

--Buscar todos los items
CREATE PROCEDURE SP_BUSCAR_TODOS_ITEM
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Poder, Defensa, EstadisticaId, TipoItemId
    FROM Item;
END
GO

--Guardar personaje
CREATE PROCEDURE SP_GUARDAR_PERSONAJE
    @Nombre NVARCHAR(100),
    @NuevoId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO Personaje (Nombre)
        VALUES (@Nombre);

        SET @NuevoId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        SET @NuevoId = -1;
        THROW;
    END CATCH
END;
GO

--Guardar items y relacionar con jugador
CREATE PROCEDURE SP_GUARDAR_PERSONAJE_ITEM
    @PersonajeId INT,
    @ItemId INT,
    @Orden INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO PersonajeItem (PersonajeId, ItemId, Orden)
        VALUES (@PersonajeId, @ItemId, @Orden);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Buscar items de un personaje ordenados por OrdenAplicacion
CREATE PROCEDURE SP_BUSCAR_ITEMS_DE_PERSONAJE_ORDENADOS
    @PersonajeId INT
AS
BEGIN
    SELECT
        PII.ItemId AS Id,
        I.Nombre,
        I.Poder,
        I.Defensa,
        I.EstadisticaId,
        I.TipoItemId
    FROM  PersonajeItem PII
    INNER JOIN Item I ON PII.ItemId = I.Id
    WHERE PII.PersonajeId = @PersonajeId
    ORDER BY PII.Orden DESC;
END;
GO

-- Buscar personaje por ID
CREATE PROCEDURE SP_BUSCAR_PERSONAJE
    @Id INT
AS
BEGIN
    SELECT
        Id,
        Nombre
    FROM Personaje
    WHERE Id = @Id;
END;
GO

--Guardar personaje de jugador
CREATE PROCEDURE SP_GUARDAR_PERSONAJE_DE_JUGADOR
    @JugadorId INT,
    @PersonajeId INT
AS
BEGIN

    BEGIN TRY

        INSERT INTO JugadorPersonaje (JugadorId, PersonajeId)
        VALUES (@JugadorId, @PersonajeId);

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Buscar personajes de un jugador
CREATE PROCEDURE SP_BUSCAR_PERSONAJES_DE_JUGADOR
    @JugadorId INT
AS
BEGIN

    BEGIN TRY

        SELECT PersonajeId
        FROM JugadorPersonaje

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO