CREATE FUNCTION [dbo].[udf-Range-Date] (@R1 datetime,@R2 datetime,@Part varchar(10),@Incr int)
Returns Table
Return (
    with cte0(M)   As (Select 1+Case @Part When 'YY' then DateDiff(YY,@R1,@R2)/@Incr When 'QQ' then DateDiff(QQ,@R1,@R2)/@Incr When 'MM' then DateDiff(MM,@R1,@R2)/@Incr When 'WK' then DateDiff(WK,@R1,@R2)/@Incr When 'DD' then DateDiff(DD,@R1,@R2)/@Incr When 'HH' then DateDiff(HH,@R1,@R2)/@Incr When 'MI' then DateDiff(MI,@R1,@R2)/@Incr When 'SS' then DateDiff(SS,@R1,@R2)/@Incr End),
         cte1(N)   As (Select 1 From (Values(1),(1),(1),(1),(1),(1),(1),(1),(1),(1)) N(N)),
         cte2(N)   As (Select Top (Select M from cte0) Row_Number() over (Order By (Select NULL)) From cte1 a, cte1 b, cte1 c, cte1 d, cte1 e, cte1 f, cte1 g, cte1 h ),
         cte3(N,D) As (Select 0,@R1 Union All Select N,Case @Part When 'YY' then DateAdd(YY, N*@Incr, @R1) When 'QQ' then DateAdd(QQ, N*@Incr, @R1) When 'MM' then DateAdd(MM, N*@Incr, @R1) When 'WK' then DateAdd(WK, N*@Incr, @R1) When 'DD' then DateAdd(DD, N*@Incr, @R1) When 'HH' then DateAdd(HH, N*@Incr, @R1) When 'MI' then DateAdd(MI, N*@Incr, @R1) When 'SS' then DateAdd(SS, N*@Incr, @R1) End From cte2 )

    Select RetSeq = N+1
          ,RetVal = D 
     From  cte3,cte0 
     Where D<=@R2
)
/*
Max 100 million observations -- Date Parts YY QQ MM WK DD HH MI SS
Syntax:
Select * from [dbo].[udf-Range-Date]('2016-10-01','2020-10-01','YY',1) 
Select * from [dbo].[udf-Range-Date]('2016-01-01','2017-01-01','MM',1) 
*/
--Select DateR1=RetVal,DateR2=DateAdd(DD,6,RetVal) from [dbo].[udf-Range-Date]('01 jan 2017','25 jan 2017','DD',7)

GO

