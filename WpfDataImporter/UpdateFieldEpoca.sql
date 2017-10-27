
UPDATE SportMatch
SET SportMatch.Epoca = CASE 
	WHEN CAST(MONTH(SportMatch.[Date]) as VARCHAR(2)) in ('8','9','10','11','12') THEN CAST('EPOCA '+ CAST(YEAR(SportMatch.[Date]) as varchar(4)) +'/'+CAST(YEAR(SportMatch.[Date]) + 1 as varchar(4))  as varchar(100))
	WHEN CAST(MONTH(SportMatch.[Date]) as VARCHAR(2)) in ('1','2','3','4','5') THEN CAST('EPOCA '+ CAST(YEAR(SportMatch.[Date]) - 1  as varchar(4)) +'/'+CAST(YEAR(SportMatch.[Date])  as varchar(4))  as varchar(100))
	-- WHEN MONTH(sm.[Date]) <= 7 THEN 'EPOCA '+ (YEAR(sm.[Date]) -1) +'/'+ YEAR(sm.[Date])
	ELSE 'oops'
END 



select * from SportMatch sm order by Date desc
select COUNT(*) from SportMatch sm
select COUNT(*) from SportMatch sm where (sm.AtOdd >= 3 OR DrawOdd >= 3 OR HtOdd >= 3)

select 
	CAST(YEAR(sm.[Date]) as varchar(4)) + right('00' + CAST(MONTH(sm.[Date]) as varchar(2)),2) as Monthly, 
	Count(ID) AS AverageGamesMonth
from SportMatch sm  
where (sm.AtOdd >= 2 OR DrawOdd >= 2 OR HtOdd >= 2)
group by CAST(YEAR(sm.[Date]) as varchar(4)) + right('00' + CAST(MONTH(sm.[Date]) as varchar(2)),2)
order by CAST(YEAR(sm.[Date]) as varchar(4)) + right('00' + CAST(MONTH(sm.[Date]) as varchar(2)),2)


