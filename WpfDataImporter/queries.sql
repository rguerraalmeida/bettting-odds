--SELECT * 
--INTO [dbo].[SportMatch_BCK_20171203]
--FROM [dbo].[SportMatch]

select * FROM [dbo].[SportMatch] (nolock);
select * FROM [dbo].[SportMatch] (nolock) where Epoca is null;
select * FROM [dbo].[SportMatch] (nolock) where Epoca is not null;

-- update [dbo].[SportMatch] EPOCA 2016/2017
--delete from  [dbo].[SportMatch]  where Epoca is null; 

select * FROM [dbo].[SportMatch] (nolock) where Competition = 'Premier League' order by Date ;
