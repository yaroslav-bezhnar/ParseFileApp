USE [MyDataBase]
GO

/****** Object: Table [dbo].[Sentences] Script Date: 9/25/18 11:28:09 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Sentences] (
    [Id]          INT  IDENTITY (1, 1) NOT NULL,
    [Sentence]    TEXT NOT NULL,
    [Occurrences] INT  NOT NULL
);


