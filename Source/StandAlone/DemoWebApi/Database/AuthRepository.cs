namespace DemoWebApi.Database;
using Dapper;
using DemoWebApi.Auth;
using DemoWebApi.Database.Entities;
using Microsoft.AspNetCore.Identity;
using NJsonSchema;
using System.Data;

public class AuthRepository(IDbConnection connection)
{
    public bool CreateUser(string fullName, string email, string password)
    {
        var passwordHash = string.IsNullOrEmpty(password) ? null : SecretHash.Hash(password);
        var sql = @"
insert into [User] (FullName, Email, PasswordHash) 
values (@fullName, @email, @passwordHash)
;";
        var affected = connection.Execute(sql, new { fullName, email, passwordHash });
        return affected == 1;
    }

    public bool UpdateUser(int userId, string? fullName, string? email, string? password)
    {
        var passwordHash = string.IsNullOrEmpty(password) ? null : SecretHash.Hash(password);

        var sql = @"
UPDATE [User]
SET 
    [FullName] = COALESCE(@fullName, [FullName]),
    [Email] = COALESCE(@email, [Email]),
    [PasswordHash] = COALESCE(@passwordHash, [PasswordHash]),
    [UpdatedAt] = SYSDATETIMEOFFSET()
WHERE [UserId] = @userId;
";
        var affected = connection.Execute(sql, new { userId, fullName, email, passwordHash });
        return affected == 1;
    }

    public UserRecord? GetUserRecord(int? userId, string email)
    {
        var sql = @"
select u.*
from [User] u where u.[UserId] = @userId or u.[Email] = @email  ";

        return connection.QueryFirstOrDefault<UserRecord>(sql, new { userId, email });
    }

    public UserInfo? GetUserInfo(int userId)
    {
        var sql = @"
select u.*
from [User] u where  u.[UserId] = @userId

select r.*
from [Role] r
join [UserRole] ur on ur.[RoleId] = r.[RoleId] and ur.[UserId] = @userId
 
select p.* 
from [Permission] p 
join [UserRole] ur on ur.[RoleId] = p.[RoleId] and ur.[UserId] = @userId 
";

        using var multi = connection.QueryMultiple(sql, new { userId });

        var user = multi.ReadFirst<UserRecord>();
        if (user == null)
        {
            return null;
        }

        var roles = multi.Read<RoleRecord>().ToList();
        var permissions = multi.Read<PermissionRecord>().ToList();

        var userInfo = new UserInfo
        {
            UserId = user.UserId,
            Email = user.Email,
            FullName = user.FullName,
            Roles = roles?.Select(r => new RoleInfo
            {
                RoleId = r.RoleId,
                DisplayName = r.DisplayName,
                Name = r.Name,
                Permissions = permissions?.Where(p => p.RoleId == r.RoleId).Select(p => p.Value).ToArray() ?? [],
            }).ToArray() ?? [],
        };

        return userInfo;
    }

    public List<RoleInfo> GetRoleInfoList()
    {
        var sql = @"
select r.* from [Role] r  
 
select p.* from [Permission] p 
";
        using var multi = connection.QueryMultiple(sql);

        var roles = multi.Read<RoleRecord>().ToList();
        var permissions = multi.Read<PermissionRecord>().ToList();

        var result = roles.Select(r => new RoleInfo
        {
            RoleId = r.RoleId,
            DisplayName = r.DisplayName,
            Name = r.Name,
            Permissions = permissions?.Where(p => p.RoleId == r.RoleId).Select(p => p.Value).ToArray() ?? [],
        }).ToList();

        return result;
    }

    public int ConfigRole(string name, string displayName)
    {
        var sql = $@"
MERGE INTO [Role] AS T 
USING (
    Select Name=@name, DisplayName=@displayName
) AS S 
ON S.[Name] = T.[Name]
WHEN MATCHED THEN
    UPDATE SET 
        T.DisplayName = S.DisplayName,
        T.UpdatedAt = sysdatetimeoffset() 
WHEN NOT MATCHED THEN
    INSERT ([Name], [DisplayName])
    VALUES (S.[Name], S.[DisplayName]) 
;";
        var affected = connection.Execute(sql, new { name, displayName });
        return affected;
    }

    public int ConfigRolePermissions(int roleId, string[] permissions)
    {
        var sql = $@"
MERGE INTO [Permission] AS T 
USING (
    SELECT {@roleId} AS [RoleId], [Value] 
    FROM (VALUES {string.Join(",", permissions.Select(p => $"('{p}')"))}) AS P([Value])
) AS S 
ON S.[RoleId] = T.[RoleId] AND s.[Value] = t.[Value] 
WHEN NOT MATCHED THEN
    INSERT ([RoleId], [Value])
    VALUES (S.[RoleId], S.[Value])
WHEN NOT MATCHED BY SOURCE AND [RoleId] = {roleId} THEN
    DELETE
;";
        var affected = connection.Execute(sql);
        return affected;
    }

    public int ConfigUserRoles(int userId, int[] roleIds)
    {
        var sql = $@"
MERGE INTO [UserRole] AS T 
USING (
    Select UserId=@userId, RoleId=@roleId 
) AS S 
ON S.[RoleId] = T.[RoleId] AND s.[UserId] = t.[UserId] 
WHEN NOT MATCHED THEN
    INSERT ([UserId], [RoleId])
    VALUES (S.[UserId], S.[RoleId])
WHEN NOT MATCHED BY SOURCE AND [UserId] = {userId} THEN
    DELETE
;";

        var items = roleIds.Select(r => { return new { userId, roleId = r }; }).ToList();

        var affected = connection.Execute(sql, items);
        return affected;
    }
}
