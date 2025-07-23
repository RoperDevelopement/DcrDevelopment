using System.Collections.Generic;

namespace SuperCRM.Migrations
{
	public class DbLogic
	{
		public static IEnumerable<string> GetCreateScripts()
		{
			return new List<string>
			{

				#region type IdList

				@"
CREATE TYPE [dbo].[IdList] AS TABLE(
	[id] [uniqueidentifier] NULL
)
",

				#endregion

				#region procedure UserPermits_get

				@"
create procedure [dbo].[UserPermits_get]
(
@userId uniqueidentifier
)
as
begin
declare @uPerms table(EntityId uniqueidentifier, PermissionCode nvarchar(40));
with AllPermissions
as
(select pa.EntityId, pa.PermissionCode, perm.EntityTypeCode
from UserPermit pa
join UserPermitGroup upg on upg.Id = pa.UserPermitGroupId
join Permission perm on pa.PermissionCode = perm.PermissionCode
where upg.UserId = @userId
union all
select pa.EntityId, ia.ImpliedPermissionCode, imPerm.EntityTypeCode
from ImpliedPermission ia 
join AllPermissions pa on pa.PermissionCode = ia.PermissionCode
join Permission imPerm on ia.ImpliedPermissionCode = imPerm.PermissionCode
)
insert into @uPerms
select distinct EntityId, PermissionCode from AllPermissions;

with Descendants
as
(select Id from [User]
where ParentId = @userId
union all
select u.Id as Id
from [User] u
join Descendants d on u.ParentId = d.Id
)
select Id as EntityId, PermissionCode 
from Descendants
join Permission perm on perm.EntityTypeCode = 'USER'
where perm.Kind between 1 and 2
union
select * from @uPerms

end
",

				#endregion

				#region procedure GroupPermits_get

				@"
create procedure [dbo].[GroupPermits_get]
(
@userPermitGroupId uniqueidentifier
)
as
begin
with AllPermissions
as
(select pa.EntityId, pa.PermissionCode, perm.EntityTypeCode
from UserPermit pa
join UserPermitGroup upg on upg.Id = pa.UserPermitGroupId
join Permission perm on pa.PermissionCode = perm.PermissionCode
where upg.Id = @userPermitGroupId
union all
select pa.EntityId, ia.ImpliedPermissionCode, imPerm.EntityTypeCode
from ImpliedPermission ia 
join AllPermissions pa on pa.PermissionCode = ia.PermissionCode
join Permission imPerm on ia.ImpliedPermissionCode = imPerm.PermissionCode
)
select distinct EntityId, PermissionCode from AllPermissions;

end
",

				#endregion
							   
				#region trigger AddNewPermissionToSuperAdmin

				@"
create trigger [dbo].[AddNewPermissionToSuperAdmin]
on [dbo].[Permission]
after insert
as
begin
insert into ImpliedPermission
(PermissionCode, ImpliedPermissionCode)
select 'SuperAdmin', PermissionCode
from inserted
where PermissionCode != 'SuperAdmin'
end
"

				#endregion

			};
		}

		public static IEnumerable<string> GetDropScripts()
		{
			return new List<string>
		{
			@"IF  EXISTS (SELECT * FROM sys.triggers WHERE object_id = OBJECT_ID(N'[dbo].[AddNewPermissionToSuperAdmin]'))
DROP TRIGGER [dbo].[AddNewPermissionToSuperAdmin]",
					   
			@"IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserPermits_get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[UserPermits_get]
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GroupPermits_get]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[GroupPermits_get]",
			
				@"
IF  EXISTS (SELECT * FROM sys.types st JOIN sys.schemas ss ON st.schema_id = ss.schema_id WHERE st.name = N'IdList' AND ss.name = N'dbo')
DROP TYPE [dbo].[IdList]
"};
		}

		public static IEnumerable<string> GetInsertScripts()
		{
			var permissions = new[]
			{
				// SuperAdmin must always be the first permission to be inserted as there's a trigger that automatically makes every new permission it's implied permission.
				"('SuperAdmin', 'ADMIN', 'Super administrator permission', 0)", // general permission
			};
			
			return new[]
			{
				@"insert into [dbo].[Permission]
(PermissionCode, EntityTypeCode, Description, Kind)
values" + string.Join(",\r\n", permissions),
			};
		}
	}
}