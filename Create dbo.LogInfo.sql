USE [MyDataBase]
GO

/****** Object: Table [dbo].[LogInfo] Script Date: 9/25/18 11:27:20 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[LogInfo] (
    [Id]      INT          IDENTITY (1, 1) NOT NULL,
    [Time]    DATETIME     NOT NULL,
    [Message] TEXT         NOT NULL,
    [Type]    VARCHAR (20) NOT NULL
);


