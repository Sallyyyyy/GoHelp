USE GSSWEB
GO
SELECT ClassID,ClassName,ISNULL([2016],0) as CNT2016,ISNULL([2017],0) as CNT2017,ISNULL([2018],0) as CNT2018,ISNULL([2019],0) as CNT2019
FROM
	(SELECT BD.BOOK_CLASS_ID as [ClassID],BC.BOOK_CLASS_NAME as [ClassName],
	DATEPART(YEAR,BLR.LEND_DATE) as [year],COUNT(*) AS Cnt
	FROM dbo.BOOK_LEND_RECORD as BLR
	INNER JOIN dbo.BOOK_DATA as BD ON BLR.BOOK_ID=BD.BOOK_ID
	INNER JOIN dbo.BOOK_CLASS as BC ON BD.BOOK_CLASS_ID = BC.BOOK_CLASS_ID
	GROUP BY BD.BOOK_CLASS_ID,BC.BOOK_CLASS_NAME,DATEPART(YEAR,BLR.LEND_DATE)) AS Y
PIVOT (SUM(Cnt) FOR [year] IN ([2016],[2017],[2018],[2019])) AS pvt
ORDER BY ClassID


