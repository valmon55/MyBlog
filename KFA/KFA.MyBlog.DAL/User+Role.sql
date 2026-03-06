SELECT [UserName]
		, u.Last_Name
		,u.First_Name
      ,[Email]
	  ,r.Name RoleName
  FROM AspNetUsers u
  inner join AspNetUserRoles ur
  on u.Id = ur.UserId
  inner join AspNetRoles r
  on r.Id = ur.RoleId