USE GSSWEB
GO
SELECT BLR.KEEPER_ID AS [KeepId],MM.USER_CNAME AS [CName],MM.USER_ENAME AS [EName],DATEPART(YEAR,BLR.LEND_DATE) as [BorrowYear],COUNT(*) as BorrowCnt
FROM dbo.BOOK_LEND_RECORD AS BLR 
INNER JOIN dbo.MEMBER_M AS MM
	ON BLR.KEEPER_ID=MM.USER_ID
GROUP BY BLR.KEEPER_ID,MM.USER_CNAME, MM.USER_ENAME,DATEPART(YEAR,BLR.LEND_DATE)
ORDER BY BLR.KEEPER_ID,DATEPART(YEAR,BLR.LEND_DATE)