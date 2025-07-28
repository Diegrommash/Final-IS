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
IF OBJECT_ID('SP_LOGIN_JUGADOR', 'P') IS NOT NULL DROP PROCEDURE SP_LOGIN_JUGADOR;
IF OBJECT_ID('SP_BUSCAR_TODOS_ITEM', 'P') IS NOT NULL DROP PROCEDURE SP_BUSCAR_TODOS_ITEM;
IF OBJECT_ID('SP_GUARDAR_PERSONAJE', 'P') IS NOT NULL DROP PROCEDURE SP_GUARDAR_PERSONAJE;
IF OBJECT_ID('SP_GUARDAR_PERSONAJE_ITEM', 'P') IS NOT NULL DROP PROCEDURE SP_GUARDAR_PERSONAJE_ITEM;
IF OBJECT_ID('SP_BUSCAR_ITEMS_DE_PERSONAJE_ORDENADOS', 'P') IS NOT NULL DROP PROCEDURE SP_BUSCAR_ITEMS_DE_PERSONAJE_ORDENADOS;
IF OBJECT_ID('SP_BUSCAR_PERSONAJE', 'P') IS NOT NULL DROP PROCEDURE SP_BUSCAR_PERSONAJE;
IF OBJECT_ID('SP_GUARDAR_PERSONAJE_DE_JUGADOR', 'P') IS NOT NULL DROP PROCEDURE SP_GUARDAR_PERSONAJE_DE_JUGADOR;
IF OBJECT_ID('SP_BUSCAR_PERSONAJES_DE_JUGADOR', 'P') IS NOT NULL DROP PROCEDURE SP_BUSCAR_PERSONAJES_DE_JUGADOR;
IF OBJECT_ID('SP_AGREGAR_MISION', 'P') IS NOT NULL DROP PROCEDURE SP_AGREGAR_MISION;
IF OBJECT_ID('SP_MODIFICAR_MISION', 'P') IS NOT NULL DROP PROCEDURE SP_MODIFICAR_MISION;
IF OBJECT_ID('SP_ELIMINAR_MISION', 'P') IS NOT NULL DROP PROCEDURE SP_ELIMINAR_MISION;
IF OBJECT_ID('SP_ASIGNAR_MISION', 'P') IS NOT NULL DROP PROCEDURE SP_ASIGNAR_MISION;
IF OBJECT_ID('SP_QUITAR_MISION', 'P') IS NOT NULL DROP PROCEDURE SP_QUITAR_MISION;
IF OBJECT_ID('SP_OBTENER_ARBOL_MISIONES', 'P') IS NOT NULL DROP PROCEDURE SP_OBTENER_ARBOL_MISIONES;
IF OBJECT_ID('SP_OBTENER_SUBARBOL_MISION', 'P') IS NOT NULL DROP PROCEDURE SP_OBTENER_SUBARBOL_MISION;
IF OBJECT_ID('SP_COMPLETAR_MISION', 'P') IS NOT NULL DROP PROCEDURE SP_COMPLETAR_MISION;
GO

-- 4. Eliminar tablas en orden correcto
IF OBJECT_ID('MisionItemRecompensa', 'U') IS NOT NULL DROP TABLE MisionItemRecompensa;
IF OBJECT_ID('MisionRelacion', 'U') IS NOT NULL DROP TABLE MisionRelacion;
IF OBJECT_ID('Mision', 'U') IS NOT NULL DROP TABLE Mision;
IF OBJECT_ID('JugadorPersonaje', 'U') IS NOT NULL DROP TABLE JugadorPersonaje;
IF OBJECT_ID('PersonajeItem', 'U') IS NOT NULL DROP TABLE PersonajeItem;
IF OBJECT_ID('Personaje', 'U') IS NOT NULL DROP TABLE Personaje;
IF OBJECT_ID('Item', 'U') IS NOT NULL DROP TABLE Item;
IF OBJECT_ID('Estadistica', 'U') IS NOT NULL DROP TABLE Estadistica;
IF OBJECT_ID('TipoItem', 'U') IS NOT NULL DROP TABLE TipoItem;
IF OBJECT_ID('Jugador', 'U') IS NOT NULL DROP TABLE Jugador;
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
    VALUES ('adm', 'adm'),
           ('Hechicera', 'magia456'),
           ('ArqueroX', 'flecha789');

    -- TipoItem
    CREATE TABLE TipoItem (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE
    );

    INSERT INTO TipoItem (Nombre)
    VALUES ('Trabajo'),
           ('Arma'),
           ('Armadura'),
           ('Joya'),
           ('Pocion');

    -- Estadistica
    CREATE TABLE Estadistica (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(50) NOT NULL UNIQUE
    );

    INSERT INTO Estadistica (Nombre)
    VALUES ('Fuerza'),
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
    ('Armadura de tela', 5, 2 , (SELECT Id FROM Estadistica WHERE Nombre = 'Inteligencia'), (SELECT Id FROM TipoItem WHERE Nombre = 'Armadura')),
    ('Anillo de poder', 10, 0, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Joya')),
    ('Anillo de defensa', 0, 5, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Joya')),
    ('Anillo dual', 5, 5, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Joya')),
    ('Pocion de fuerza', 10, 0, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Pocion')),
    ('Pocion de defensa', 0, 10, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Pocion')),
    ('Pocion berzerker', 20, -5, (SELECT Id FROM Estadistica WHERE Nombre = 'No_posee'), (SELECT Id FROM TipoItem WHERE Nombre = 'Pocion'));

    -- Personaje
    CREATE TABLE Personaje (
        Id INT IDENTITY(1,1) PRIMARY KEY,
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

    -- Tabla de Misiones
    CREATE TABLE Mision (
        Id INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Descripcion NVARCHAR(255) NULL,
        Dificultad INT NOT NULL DEFAULT 1,
        EsCompleta BIT NOT NULL DEFAULT 0,
        EsCompuesta BIT NOT NULL DEFAULT 0
    );

    -- Relaciones Padre-Hijo (sin cascada doble)
    CREATE TABLE MisionRelacion (
        MisionPadreId INT NOT NULL,
        MisionHijaId INT NOT NULL,
        PRIMARY KEY (MisionPadreId, MisionHijaId),
        FOREIGN KEY (MisionPadreId) REFERENCES Mision(Id) ON DELETE CASCADE,
        FOREIGN KEY (MisionHijaId) REFERENCES Mision(Id) ON DELETE NO ACTION
    );

    -- Recompensas de la misión
    CREATE TABLE MisionItemRecompensa (
        MisionId INT NOT NULL,
        ItemId INT NOT NULL,
        PRIMARY KEY (MisionId, ItemId),
        FOREIGN KEY (MisionId) REFERENCES Mision(Id) ON DELETE CASCADE,
        FOREIGN KEY (ItemId) REFERENCES Item(Id)
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
    SELECT Id, Nombre FROM Jugador
    WHERE Nombre = @Nombre AND Contraseña = @Contraseña;
END
GO

CREATE PROCEDURE SP_BUSCAR_TODOS_ITEM
AS
BEGIN
    SET NOCOUNT ON;
    SELECT Id, Nombre, Poder, Defensa, EstadisticaId, TipoItemId FROM Item;
END
GO

CREATE PROCEDURE SP_GUARDAR_PERSONAJE
    @Nombre NVARCHAR(100),
    @NuevoId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Personaje (Nombre) VALUES (@Nombre);
        SET @NuevoId = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        SET @NuevoId = -1;
        THROW;
    END CATCH
END;
GO

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

CREATE PROCEDURE SP_BUSCAR_ITEMS_DE_PERSONAJE_ORDENADOS
    @PersonajeId INT
AS
BEGIN
    SELECT PII.ItemId AS Id, I.Nombre, I.Poder, I.Defensa, I.EstadisticaId, I.TipoItemId
    FROM PersonajeItem PII
    INNER JOIN Item I ON PII.ItemId = I.Id
    WHERE PII.PersonajeId = @PersonajeId
    ORDER BY PII.Orden DESC;
END;
GO

CREATE PROCEDURE SP_BUSCAR_PERSONAJE
    @Id INT
AS
BEGIN
    SELECT Id, Nombre FROM Personaje WHERE Id = @Id;
END;
GO

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

CREATE PROCEDURE SP_BUSCAR_PERSONAJES_DE_JUGADOR
    @JugadorId INT
AS
BEGIN
    BEGIN TRY
        SELECT PersonajeId FROM JugadorPersonaje WHERE JugadorId = @JugadorId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- 7.SP de Misiones (CRUD + Relaciones + Árbol)
CREATE OR ALTER PROCEDURE SP_AGREGAR_MISION
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255) = NULL,
    @Dificultad INT,
    @EsCompuesta BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        INSERT INTO Mision (Nombre, Descripcion, Dificultad, EsCompuesta)
        VALUES (@Nombre, @Descripcion, @Dificultad, @EsCompuesta);
        SELECT SCOPE_IDENTITY() AS NuevoId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_MODIFICAR_MISION
    @Id INT,
    @Nombre NVARCHAR(100),
    @Descripcion NVARCHAR(255) = NULL,
    @Dificultad INT,
    @EsCompuesta BIT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @Id)
        BEGIN
            RAISERROR('La misión no existe.', 16, 1);
            RETURN;
        END
        UPDATE Mision
        SET Nombre = @Nombre,
            Descripcion = @Descripcion,
            Dificultad = @Dificultad,
            EsCompuesta = @EsCompuesta
        WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_ELIMINAR_MISION
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @Id)
        BEGIN
            RAISERROR('La misión no existe.', 16, 1);
            RETURN;
        END
        DELETE FROM Mision WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_ASIGNAR_MISION
    @MisionPadreId INT,
    @MisionHijaId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF @MisionPadreId = @MisionHijaId
        BEGIN
            RAISERROR('Una misión no puede ser hija de sí misma.', 16, 1);
            RETURN;
        END
        IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @MisionPadreId)
        BEGIN
            RAISERROR('La misión padre no existe.', 16, 1);
            RETURN;
        END
        IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @MisionHijaId)
        BEGIN
            RAISERROR('La misión hija no existe.', 16, 1);
            RETURN;
        END
        IF EXISTS (SELECT 1 FROM MisionRelacion WHERE MisionPadreId = @MisionPadreId AND MisionHijaId = @MisionHijaId)
        BEGIN
            RAISERROR('La relación ya existe.', 16, 1);
            RETURN;
        END
        INSERT INTO MisionRelacion (MisionPadreId, MisionHijaId)
        VALUES (@MisionPadreId, @MisionHijaId);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_QUITAR_MISION
    @MisionPadreId INT,
    @MisionHijaId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM MisionRelacion WHERE MisionPadreId = @MisionPadreId AND MisionHijaId = @MisionHijaId)
        BEGIN
            RAISERROR('No existe la relación especificada.', 16, 1);
            RETURN;
        END
        DELETE FROM MisionRelacion WHERE MisionPadreId = @MisionPadreId AND MisionHijaId = @MisionHijaId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_OBTENER_ARBOL_MISIONES
AS
BEGIN
    SET NOCOUNT ON;
    ;WITH CTE_Misiones AS (
        SELECT m.Id, m.Nombre, m.Descripcion, m.Dificultad, m.EsCompleta, m.EsCompuesta,
               CAST(NULL AS INT) AS PadreId,
               CAST(m.Nombre AS NVARCHAR(MAX)) AS Ruta
        FROM Mision m
        WHERE m.Id NOT IN (SELECT MisionHijaId FROM MisionRelacion)
        UNION ALL
        SELECT h.Id, h.Nombre, h.Descripcion, h.Dificultad, h.EsCompleta, h.EsCompuesta,
               r.MisionPadreId AS PadreId,
               CAST(cte.Ruta + ' -> ' + h.Nombre AS NVARCHAR(MAX))
        FROM MisionRelacion r
        INNER JOIN Mision h ON r.MisionHijaId = h.Id
        INNER JOIN CTE_Misiones cte ON r.MisionPadreId = cte.Id
    )
    SELECT * FROM CTE_Misiones ORDER BY Ruta;
END;
GO

CREATE OR ALTER PROCEDURE SP_OBTENER_SUBARBOL_MISION
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @Id)
    BEGIN
        RAISERROR('La misión solicitada no existe.', 16, 1);
        RETURN;
    END
    ;WITH CTE_SubArbol AS (
        SELECT m.Id, m.Nombre, m.Descripcion, m.Dificultad, m.EsCompleta, m.EsCompuesta, CAST(NULL AS INT) AS PadreId
        FROM Mision m WHERE m.Id = @Id
        UNION ALL
        SELECT h.Id, h.Nombre, h.Descripcion, h.Dificultad, h.EsCompleta, h.EsCompuesta, r.MisionPadreId
        FROM MisionRelacion r
        INNER JOIN Mision h ON r.MisionHijaId = h.Id
        INNER JOIN CTE_SubArbol cte ON r.MisionPadreId = cte.Id
    )
    SELECT * FROM CTE_SubArbol ORDER BY PadreId, Nombre;
END;
GO

CREATE OR ALTER PROCEDURE SP_COMPLETAR_MISION
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @Id)
        BEGIN
            RAISERROR('La misión no existe.', 16, 1);
            RETURN;
        END
        UPDATE Mision SET EsCompleta = 1 WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO