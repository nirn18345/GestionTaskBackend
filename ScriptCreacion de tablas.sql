-- =======================================
-- Crear esquema si no existe
-- =======================================
IF NOT EXISTS (SELECT * FROM sys.schemas WHERE name = 'core')
    EXEC('CREATE SCHEMA core;');


-- =======================================
-- Crear tabla: Role
-- =======================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'Role' AND type = 'U')
CREATE TABLE core.Role (
    RoleId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    RoleName NVARCHAR(50) NOT NULL, -- Ej: Administrador, Supervisor, Empleado
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100),
    UpdatedAt DATETIME,
    UpdatedBy NVARCHAR(100)
);


-- =======================================
-- Crear tabla: Account (para login)
-- =======================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'Account' AND type = 'U')
CREATE TABLE core.Account (
    AccountId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Email NVARCHAR(100) UNIQUE NOT NULL,
    Password NVARCHAR(200) NOT NULL, -- Encriptada/Hasheada
    IsActive BIT DEFAULT 1,
    LastLoginAt DATETIME,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100),
    UpdatedAt DATETIME,
    UpdatedBy NVARCHAR(100)
);


-- =======================================
-- Crear tabla: User
-- =======================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'User' AND type = 'U')
CREATE TABLE core.[User] (
    UserId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Dni NVARCHAR(13),
    FirstName NVARCHAR(100) NOT NULL,
    LastName NVARCHAR(100),
    Phone NVARCHAR(20),
    AccountId UNIQUEIDENTIFIER NOT NULL,
    RoleId UNIQUEIDENTIFIER NOT NULL,
    IsActive BIT DEFAULT 1,
    CreatedAt DATETIME DEFAULT GETDATE(),
    CreatedBy NVARCHAR(100),
    UpdatedAt DATETIME,
    UpdatedBy NVARCHAR(100),
    FOREIGN KEY (AccountId) REFERENCES core.Account(AccountId),
    FOREIGN KEY (RoleId) REFERENCES core.Role(RoleId)
);


-- =======================================
-- Crear tabla: Task
-- =======================================
IF NOT EXISTS (SELECT * FROM sys.objects WHERE name = 'Task' AND type = 'U')
CREATE TABLE core.Task (
    TaskId UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
    Title NVARCHAR(200) NOT NULL,
    Description NVARCHAR(MAX),
    AssignedTo UNIQUEIDENTIFIER, -- FK: UserId
    Status NVARCHAR(20),
    DueDate DATETIME,
    Priority NVARCHAR(20),
    CreatedBy NVARCHAR(100),
    CreatedAt DATETIME DEFAULT GETDATE(),
    UpdatedBy NVARCHAR(100),
    UpdatedAt DATETIME,
    FOREIGN KEY (AssignedTo) REFERENCES core.[User](UserId)
);


-- =======================================
-- Insertar roles base: Administrador, Supervisor, Empleado
-- =======================================
INSERT INTO core.Role (RoleId, RoleName, IsActive, CreatedAt, CreatedBy)
VALUES 
    (NEWID(), 'Administrador', 1, GETDATE(), 'system'),
    (NEWID(), 'Supervisor', 1, GETDATE(), 'system'),
    (NEWID(), 'Empleado', 1, GETDATE(), 'system');


-- =======================================
-- Insertar cuenta y usuario administrador
-- =======================================

-- Obtener RoleId del rol Administrador
DECLARE @RoleId UNIQUEIDENTIFIER = (
    SELECT TOP 1 RoleId 
    FROM core.Role 
    WHERE RoleName = 'Administrador'
);

-- Generar identificadores para la cuenta y el usuario
DECLARE @AccountId UNIQUEIDENTIFIER = NEWID();
DECLARE @UserId UNIQUEIDENTIFIER = NEWID();

-- Insertar cuenta de acceso para administrador
INSERT INTO core.Account (
    AccountId,
    Email,
    Password,
    IsActive,
    CreatedAt,
    CreatedBy
) VALUES (
    @AccountId,
    'admin@sistema.com',
    '03AC674216F3E15C761EE1A5E255F067953623C8B388B4459E13F978D7C846F4', -- SHA-256 de 'admin'
    1,
    GETDATE(),
    'system'
);

-- Insertar usuario Administrador
INSERT INTO core.[User] (
    UserId,
    FirstName,
    LastName,
    Phone,
    AccountId,
    RoleId,
    IsActive,
    CreatedAt,
    CreatedBy
) VALUES (
    @UserId,
    'Administrador',
    'General',
    '0999999999',
    @AccountId,
    @RoleId,
    1,
    GETDATE(),
    'system'
);

GO
