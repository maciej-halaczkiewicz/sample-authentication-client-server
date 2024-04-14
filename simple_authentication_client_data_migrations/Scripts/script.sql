IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;
GO

BEGIN TRANSACTION;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231221221013_InitialMigration'
)
BEGIN
    CREATE TABLE [dbo].[AuditHistory] (
        [Id] bigint NOT NULL,
        [Type] nvarchar(50) NOT NULL,
        [PrimaryKey] int NOT NULL,
        [Data] nvarchar(max) NOT NULL,
        [Operation] nvarchar(50) NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] datetime2 NOT NULL,
        CONSTRAINT [PK_AuditHistory] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231221221013_InitialMigration'
)
BEGIN
    CREATE TABLE [dbo].[Role] (
        [Id] int NOT NULL IDENTITY,
        [Name] nvarchar(50) NOT NULL,
        [Description] nvarchar(255) NOT NULL,
        [IsActive] bit NOT NULL,
        CONSTRAINT [PK_Role] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231221221013_InitialMigration'
)
BEGIN
    CREATE TABLE [dbo].[User] (
        [Id] int NOT NULL IDENTITY,
        [Username] nvarchar(50) NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [ModifiedOn] datetime2 NOT NULL,
        [CreatedBy] nvarchar(50) NOT NULL,
        [ModifiedBy] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_User] PRIMARY KEY ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231221221013_InitialMigration'
)
BEGIN
    CREATE TABLE [dbo].[UserRole] (
        [Id] int NOT NULL IDENTITY,
        [UserId] int NOT NULL,
        [RoleId] int NOT NULL,
        [CreatedOn] datetime2 NOT NULL,
        [CreatedBy] nvarchar(50) NOT NULL,
        CONSTRAINT [PK_UserRole] PRIMARY KEY ([Id]),
        CONSTRAINT [FK_UserRole_Role] FOREIGN KEY ([RoleId]) REFERENCES [dbo].[Role] ([Id]),
        CONSTRAINT [FK_UserRole_User] FOREIGN KEY ([UserId]) REFERENCES [dbo].[User] ([Id])
    );
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231221221013_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_UserRole_RoleId] ON [dbo].[UserRole] ([RoleId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231221221013_InitialMigration'
)
BEGIN
    CREATE INDEX [IX_UserRole_UserId] ON [dbo].[UserRole] ([UserId]);
END;
GO

IF NOT EXISTS (
    SELECT * FROM [__EFMigrationsHistory]
    WHERE [MigrationId] = N'20231221221013_InitialMigration'
)
BEGIN
    INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
    VALUES (N'20231221221013_InitialMigration', N'8.0.0');
END;
GO

COMMIT;
GO

