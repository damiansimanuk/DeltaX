
create table [User]
(
    [UserId] int not null primary key identity(1,1),
    [Email] nvarchar(100) not null unique, 
    [FullName] nvarchar(100) not null,
    [PasswordHash] nvarchar(100) not null,
    [CreatedAt] datetimeoffset(0) not null default sysdatetimeoffset(),
    [UpdatedAt] datetimeoffset(0) not null default sysdatetimeoffset(),
    [Active] bit not null default (1)
) 
 
create table [Role]
(
    [RoleId] int not null primary key identity(1,1),
    [Name] nvarchar(50) not null,
    [DisplayName] nvarchar(100) not null,
    [CreatedAt] datetimeoffset(0) not null default sysdatetimeoffset(),
    [UpdatedAt] datetimeoffset(0) not null default sysdatetimeoffset(),
    [Active] bit not null default (1)
) 
 
CREATE TABLE [Permission] (
    [PermissionId] int not null primary key identity(1,1),
    [RoleId] int not null, 
    [Value] nvarchar (MAX) NULL,
    [CreatedAt] datetimeoffset(0) not null default sysdatetimeoffset(),
    [UpdatedAt] datetimeoffset(0) not null default sysdatetimeoffset(),
    [Active] bit not null default (1)
    CONSTRAINT [FK_RoleClaim_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]) ON DELETE CASCADE
);
 
CREATE TABLE [UserRole] (
    [UserId] int not null,
    [RoleId] int not null,
    CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED ([UserId] ASC, [RoleId] ASC),
    CONSTRAINT [FK_UserRole_Role_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [Role] ([Id]),
    CONSTRAINT [FK_UserRole_User_UserId] FOREIGN KEY ([UserId]) REFERENCES [User] ([Id])
); 
 