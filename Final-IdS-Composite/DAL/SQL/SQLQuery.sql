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
--IF OBJECT_ID('SP_LOGIN_JUGADOR', 'P') IS NOT NULL
--    DROP PROCEDURE SP_LOGIN_JUGADOR;
--GO



-- 5. Crear tablas y poblar datos dentro de transacción
BEGIN TRY
    BEGIN TRANSACTION;

    -- Tabla de Misiones
    CREATE TABLE Mision (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    Dificultad INT NOT NULL DEFAULT 1,
    EsCompleta BIT NOT NULL DEFAULT 0,
    EsCompuesta BIT NOT NULL DEFAULT 0
    );
    -- Tabla para relaciones Padre-Hijo (Composite)
    CREATE TABLE MisionRelacion (
        MisionPadreId INT NOT NULL,
        MisionHijaId INT NOT NULL,
        PRIMARY KEY (MisionPadreId, MisionHijaId),
        FOREIGN KEY (MisionPadreId) REFERENCES Mision(Id) ON DELETE CASCADE,
        FOREIGN KEY (MisionHijaId) REFERENCES Mision(Id) ON DELETE CASCADE
    );

    -- Tabla para Recompensas de la Misión
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
--Agregar
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

-- Modificar
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

--Eliminar
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

-- Asignar Misión Padre-Hija (Composite)
CREATE OR ALTER PROCEDURE SP_ASIGNAR_MISION
    @MisionPadreId INT,
    @MisionHijaId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validaciones
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

-- Quitar Misión Padre-Hija (Composite)
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

        DELETE FROM MisionRelacion
        WHERE MisionPadreId = @MisionPadreId AND MisionHijaId = @MisionHijaId;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

-- Obtener árbol de misiones (Composite)
CREATE OR ALTER PROCEDURE SP_OBTENER_ARBOL_MISIONES
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH CTE_Misiones AS (
        SELECT 
            m.Id,
            m.Nombre,
            m.Descripcion,
            m.Dificultad,
            m.EsCompleta,
            m.EsCompuesta,
            CAST(NULL AS INT) AS PadreId,
            CAST(m.Nombre AS NVARCHAR(MAX)) AS Ruta
        FROM Mision m
        WHERE m.Id NOT IN (SELECT MisionHijaId FROM MisionRelacion)

        UNION ALL

        SELECT 
            h.Id,
            h.Nombre,
            h.Descripcion,
            h.Dificultad,
            h.EsCompleta,
            h.EsCompuesta,
            r.MisionPadreId AS PadreId,
            CAST(cte.Ruta + ' -> ' + h.Nombre AS NVARCHAR(MAX)) AS Ruta
        FROM MisionRelacion r
        INNER JOIN Mision h ON r.MisionHijaId = h.Id
        INNER JOIN CTE_Misiones cte ON r.MisionPadreId = cte.Id
    )
    SELECT *
    FROM CTE_Misiones
    ORDER BY Ruta;
END;
GO

-- Obtener subárbol de una misión específica (Composite)
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
        SELECT 
            m.Id,
            m.Nombre,
            m.Descripcion,
            m.Dificultad,
            m.EsCompleta,
            m.EsCompuesta,
            CAST(NULL AS INT) AS PadreId
        FROM Mision m
        WHERE m.Id = @Id
        
        UNION ALL
        
        SELECT 
            h.Id,
            h.Nombre,
            h.Descripcion,
            h.Dificultad,
            h.EsCompleta,
            h.EsCompuesta,
            r.MisionPadreId AS PadreId
        FROM MisionRelacion r
        INNER JOIN Mision h ON r.MisionHijaId = h.Id
        INNER JOIN CTE_SubArbol cte ON r.MisionPadreId = cte.Id
    )
    SELECT *
    FROM CTE_SubArbol
    ORDER BY PadreId, Nombre;
END;
GO

-- Completar una misión
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

        UPDATE Mision
        SET EsCompleta = 1
        WHERE Id = @Id;
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO