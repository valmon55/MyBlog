SELECT t.Tag_Name,
		a.ArticleDate,
		a.Title,
		a.Content,
		u.UserName
  FROM myblog.dbo.Tags t
  inner join myblog.dbo.ArticleTag at
  on t.Id = at.TagsId
  inner join myblog.dbo.Articles a
  on a.Id = at.ArticlesId
  inner join myblog.dbo.AspNetUsers u
  on a.UserId = u.Id
