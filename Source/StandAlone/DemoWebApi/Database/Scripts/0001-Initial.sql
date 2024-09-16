CREATE TABLE [User]
(
    [UserId] int NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Email] nvarchar(100) NOT NULL UNIQUE, 
    [FullName] nvarchar(100) NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [CreatedAt] datetimeoffset(0) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [UpdatedAt] datetimeoffset(0) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [Active] bit NOT NULL DEFAULT (1)
);

CREATE TABLE [Role]
(
    [RoleId] int NOT NULL PRIMARY KEY IDENTITY(1,1),
    [Name] nvarchar(50) NOT NULL,
    [DisplayName] nvarchar(100) NOT NULL,
    [CreatedAt] datetimeoffset(0) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [UpdatedAt] datetimeoffset(0) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [Active] bit NOT NULL DEFAULT (1)
);

CREATE TABLE [Permission] (
    [PermissionId] int NOT NULL PRIMARY KEY IDENTITY(1,1),
    [RoleId] int NOT NULL, 
    [Value] nvarchar (max) NULL,
    [CreatedAt] datetimeoffset(0) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [UpdatedAt] datetimeoffset(0) NOT NULL DEFAULT SYSDATETIMEOFFSET(),
    [Active] bit NOT NULL DEFAULT (1),
    CONSTRAINT [FK_Permission_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]) ON DELETE CASCADE
);

CREATE TABLE [UserRole] (
    [UserId] int NOT NULL,
    [RoleId] int NOT NULL,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_UserRole_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([RoleId]),
    CONSTRAINT [FK_UserRole_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([UserId])
);
