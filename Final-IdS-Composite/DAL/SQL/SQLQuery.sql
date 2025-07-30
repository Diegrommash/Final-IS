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

-- Asignar recompensas a mision
CREATE OR ALTER PROCEDURE SP_ASIGNAR_RECOMPENSA
    @MisionId INT,
    @ItemId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @MisionId)
        BEGIN
            RAISERROR('La misión no existe.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM Item WHERE Id = @ItemId)
        BEGIN
            RAISERROR('El  no existe.', 16, 1);
            RETURN;
        END

        IF EXISTS (SELECT 1 FROM MisionItemRecompensa WHERE MisionId = @MisionId AND ItemId = @ItemId)
        BEGIN
            RAISERROR('La relación ya existe.', 16, 1);
            RETURN;
        END

        INSERT INTO MisionItemRecompensa (MisionId, ItemId)
        VALUES (@MisionId, @ItemId);
    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO


CREATE OR ALTER PROCEDURE SP_ELIMINAR_RECOMPENSA
    @MisionId INT,
    @ItemId INT
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY

        IF NOT EXISTS (SELECT 1 FROM Mision WHERE Id = @MisionId)
        BEGIN
            RAISERROR('La misión no existe.', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM Item WHERE Id = @ItemId)
        BEGIN
            RAISERROR('El  no existe.', 16, 1);
            RETURN;     
        END

        DELETE FROM MisionItemRecompensa WHERE MisionId = @MisionId AND ItemId = @ItemId;

    END TRY
    BEGIN CATCH
        THROW;
    END CATCH
END;
GO

CREATE OR ALTER PROCEDURE SP_BUSCAR_RECOMPENSAS
    @MisionId INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT I.Id, I.Nombre, I.Poder, I.Defensa, I.EstadisticaId, I.TipoItemId
    FROM MisionItemRecompensa MIR
    JOIN Item I ON I.Id = MIR.ItemId
    WHERE MIR.MisionId = @MisionId;
END;
GO
