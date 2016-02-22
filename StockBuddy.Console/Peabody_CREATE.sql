USE [master]
GO
/****** Object:  Database [Peabody]    Script Date: 12/07/2015 21:39:54 ******/
CREATE DATABASE [Peabody] ON  PRIMARY 
( NAME = N'Peabody', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\Peabody.mdf' , SIZE = 11264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'Peabody_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10_50.SQLEXPRESS\MSSQL\DATA\Peabody_log.ldf' , SIZE = 84416KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [Peabody] SET COMPATIBILITY_LEVEL = 100
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Peabody].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Peabody] SET ANSI_NULL_DEFAULT OFF
GO
ALTER DATABASE [Peabody] SET ANSI_NULLS OFF
GO
ALTER DATABASE [Peabody] SET ANSI_PADDING OFF
GO
ALTER DATABASE [Peabody] SET ANSI_WARNINGS OFF
GO
ALTER DATABASE [Peabody] SET ARITHABORT OFF
GO
ALTER DATABASE [Peabody] SET AUTO_CLOSE OFF
GO
ALTER DATABASE [Peabody] SET AUTO_CREATE_STATISTICS ON
GO
ALTER DATABASE [Peabody] SET AUTO_SHRINK OFF
GO
ALTER DATABASE [Peabody] SET AUTO_UPDATE_STATISTICS ON
GO
ALTER DATABASE [Peabody] SET CURSOR_CLOSE_ON_COMMIT OFF
GO
ALTER DATABASE [Peabody] SET CURSOR_DEFAULT  GLOBAL
GO
ALTER DATABASE [Peabody] SET CONCAT_NULL_YIELDS_NULL OFF
GO
ALTER DATABASE [Peabody] SET NUMERIC_ROUNDABORT OFF
GO
ALTER DATABASE [Peabody] SET QUOTED_IDENTIFIER OFF
GO
ALTER DATABASE [Peabody] SET RECURSIVE_TRIGGERS OFF
GO
ALTER DATABASE [Peabody] SET  DISABLE_BROKER
GO
ALTER DATABASE [Peabody] SET AUTO_UPDATE_STATISTICS_ASYNC OFF
GO
ALTER DATABASE [Peabody] SET DATE_CORRELATION_OPTIMIZATION OFF
GO
ALTER DATABASE [Peabody] SET TRUSTWORTHY OFF
GO
ALTER DATABASE [Peabody] SET ALLOW_SNAPSHOT_ISOLATION OFF
GO
ALTER DATABASE [Peabody] SET PARAMETERIZATION SIMPLE
GO
ALTER DATABASE [Peabody] SET READ_COMMITTED_SNAPSHOT OFF
GO
ALTER DATABASE [Peabody] SET HONOR_BROKER_PRIORITY OFF
GO
ALTER DATABASE [Peabody] SET  READ_WRITE
GO
ALTER DATABASE [Peabody] SET RECOVERY SIMPLE
GO
ALTER DATABASE [Peabody] SET  MULTI_USER
GO
ALTER DATABASE [Peabody] SET PAGE_VERIFY CHECKSUM
GO
ALTER DATABASE [Peabody] SET DB_CHAINING OFF
GO
USE [Peabody]
GO
/****** Object:  StoredProcedure [dbo].[sp_MACD]    Script Date: 12/07/2015 21:39:56 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Ryan May
-- Create date: 12/5/2015
-- Description:	Calculates MACD values
-- =============================================
CREATE PROCEDURE [dbo].[sp_MACD]
    @Symbol varchar(50)
AS 

    SET NOCOUNT ON;
    
    IF OBJECT_ID('tempdb..#mod_goog_data') IS NOT NULL
  DROP TABLE #mod_goog_data

--Setup our Working table for EMA's
select row_number() over (order by History.Date) n
	,History.Date
	,History.ClosePrice
	,CAST(null as decimal(8,2)) [ema9]
	,CAST(null as decimal(8,2)) [ema12]
	,CAST(null as decimal(8,2)) [ema26]
	into #mod_goog_data
from
 History
 WHERE
 History.Symbol = @Symbol

 create clustered index ix_n on #mod_goog_data(n)



--declare variables needed
declare @ema_1_intervals int, @ema_2_intervals int, @ema_3_intervals int, @K1 decimal(4,3), @K2 decimal(4,3), @K3 decimal(4,3)
declare @prev_ema_1 decimal(8,2), @prev_ema_2 decimal(8,2), @prev_ema_3 decimal(8,2), @initial_sma_1 decimal(8,2), @initial_sma_2 decimal(8,2), @initial_sma_3 decimal(8,2)
declare @anchor int, @anchor2 int

--Setup the default MACD intervals
set @ema_1_intervals = 12
set @ema_2_intervals = 26
set @ema_3_intervals = 9
set @K1 = 2/(1 + @ema_1_intervals + .000)
set @K2 = 2/(1 + @ema_2_intervals + .000)
set @K3 = 2/(1 + @ema_3_intervals + .000)
	
--Capture the averages prior to the start of the EMA interval period
select @initial_sma_1 = avg(case when n < @ema_1_intervals then ClosePrice else null end),
	@initial_sma_2 = avg(case when n < @ema_2_intervals then ClosePrice else null end)
from #mod_goog_data
where n < @ema_1_intervals or n < @ema_2_intervals

--Carry over update statement
update t1 
	set 
	@prev_ema_1 = case when n < @ema_1_intervals then null
			when n = @ema_1_intervals then t1.ClosePrice * @K1 + @initial_sma_1 * (1-@K1)
			when n > @ema_1_intervals then t1.ClosePrice * @K1 + @prev_ema_1 * (1-@K1)
			end,
	@prev_ema_2 = case when n < @ema_2_intervals then null 
			when n = @ema_2_intervals then t1.ClosePrice * @K2 + @initial_sma_2 * (1-@K2)
			when n > @ema_2_intervals then t1.ClosePrice * @K2 + @prev_ema_2 * (1-@K2)
			end,
	ema12 = @prev_ema_1,
	ema26 = @prev_ema_2,
	@anchor = n --anchor so that carryover works properly
	from #mod_goog_data t1 with (TABLOCKX)
	OPTION (MAXDOP 1)
	
IF OBJECT_ID('tempdb..#mod_goog_data2') IS NOT NULL
  DROP TABLE #mod_goog_data2

--Setup our table for Signal Line
select row_number() over (order by #mod_goog_data.Date) n
	,#mod_goog_data.Date
	,#mod_goog_data.ema12 - #mod_goog_data.ema26 macd
	,#mod_goog_data.ema9
	into #mod_goog_data2
from
 #mod_goog_data
 WHERE ema26 is not null
	
	--Capture the averages prior to the start of the EMA interval period
select @initial_sma_3 = avg(case when n < @ema_3_intervals then macd else null end)
from #mod_goog_data2
where n < @ema_3_intervals

--Carry over update statement
update t2 
	set 
	@prev_ema_3 = case when n < @ema_3_intervals then macd * @K3 + @initial_sma_3 * (1-@K3)
			when n = @ema_3_intervals then macd * @K3 + @initial_sma_3 * (1-@K3)
			when n > @ema_3_intervals then macd * @K3 + @prev_ema_3 * (1-@K3)
			end,
	ema9 = @prev_ema_3,
	@anchor2 = n --anchor so that carryover works properly
	from #mod_goog_data2 t2 with (TABLOCKX)
	OPTION (MAXDOP 1)

	--SELECT Date, Macd, ema9 Signal, macd - ema9 Divergence FROM #mod_goog_data2
	--SELECT Date, ema12, ema26, ema12 - ema26 FROM #mod_goog_data
	UPDATE History SET MACD = #mod_goog_data2.macd, Divergence = #mod_goog_data2.macd - #mod_goog_data2.ema9 
	FROM History INNER JOIN #mod_goog_data2 ON History.Date = #mod_goog_data2.Date WHERE Symbol = @Symbol
GO
/****** Object:  Table [dbo].[History]    Script Date: 12/07/2015 21:39:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[History](
	[Symbol] [varchar](50) NOT NULL,
	[Date] [date] NOT NULL,
	[OpenPrice] [money] NOT NULL,
	[ClosePrice] [money] NOT NULL,
	[AdustedClosePrice] [money] NOT NULL,
	[PreviousClosePrice] [money] NOT NULL,
	[HighPrice] [money] NOT NULL,
	[LowPrice] [money] NOT NULL,
	[Volume] [int] NOT NULL,
	[MACD] [decimal](8, 2) NULL,
	[Divergence] [decimal](8, 2) NULL,
 CONSTRAINT [PK_History] PRIMARY KEY CLUSTERED 
(
	[Symbol] ASC,
	[Date] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Trigger [RecalculateMACD]    Script Date: 12/07/2015 21:39:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE TRIGGER [dbo].[RecalculateMACD]
   ON  [dbo].[History]
   AFTER UPDATE,INSERT
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @action as char(1);
	DECLARE	@Symbol varchar(50);
	DECLARE @NumRecords int;
	
    SET @action = 'I'; -- Set Action to Insert by default.
    IF EXISTS(SELECT * FROM DELETED)
    BEGIN
        SET @action = 
            CASE
                WHEN EXISTS(SELECT * FROM INSERTED) THEN 'U' -- Set Action to Updated.
                ELSE 'D' -- Set Action to Deleted.       
            END
    END
	
	IF @action = 'U' AND UPDATE (ClosePrice)
    BEGIN
		SELECT
			@Symbol = inserted.Symbol
		FROM
			inserted
		INNER JOIN
			deleted
		ON inserted.Date = deleted.Date AND inserted.Symbol = deleted.Symbol
		-- It's an update if the record is in BOTH inserted AND deleted
		
		EXEC [dbo].[sp_MACD]
		@Symbol
	END 
	ELSE IF @action = 'I'
	BEGIN
		SELECT
			@Symbol = inserted.Symbol
		FROM
			inserted
		
		SELECT
			@NumRecords = Count(*)
		FROM
			History
		WHERE
			Symbol = @Symbol
		
		IF @NumRecords >= 250
		BEGIN
			EXEC [dbo].[sp_MACD]
			@Symbol
		END
	END
END
GO
/****** Object:  Table [dbo].[Technical]    Script Date: 12/07/2015 21:39:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Technical](
	[TechnicalID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Technical] PRIMARY KEY CLUSTERED 
(
	[TechnicalID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Stock]    Script Date: 12/07/2015 21:39:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Stock](
	[StockID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Symbol] [varchar](50) NOT NULL,
 CONSTRAINT [PK_Stock] PRIMARY KEY CLUSTERED 
(
	[StockID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Computation]    Script Date: 12/07/2015 21:39:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Computation](
	[ComputationID] [int] IDENTITY(1,1) NOT NULL,
	[TechnicalID] [int] NOT NULL,
	[StockID] [int] NOT NULL,
	[CurrentValue] [float] NULL,
	[RealTimeValue] [float] NULL,
	[RealTimePrice] [money] NULL,
 CONSTRAINT [PK_Computation] PRIMARY KEY CLUSTERED 
(
	[ComputationID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  ForeignKey [FK_Computation_Stock_StockID]    Script Date: 12/07/2015 21:39:57 ******/
ALTER TABLE [dbo].[Computation]  WITH CHECK ADD  CONSTRAINT [FK_Computation_Stock_StockID] FOREIGN KEY([StockID])
REFERENCES [dbo].[Stock] ([StockID])
GO
ALTER TABLE [dbo].[Computation] CHECK CONSTRAINT [FK_Computation_Stock_StockID]
GO
/****** Object:  ForeignKey [FK_Computation_Technical_TechnicalID]    Script Date: 12/07/2015 21:39:57 ******/
ALTER TABLE [dbo].[Computation]  WITH CHECK ADD  CONSTRAINT [FK_Computation_Technical_TechnicalID] FOREIGN KEY([TechnicalID])
REFERENCES [dbo].[Technical] ([TechnicalID])
GO
ALTER TABLE [dbo].[Computation] CHECK CONSTRAINT [FK_Computation_Technical_TechnicalID]
GO
