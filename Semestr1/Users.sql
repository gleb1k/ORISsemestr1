CREATE TABLE [dbo].[Users] (
    [Login]             VARCHAR (50)  NOT NULL,
    [Password]          VARCHAR (50)  NOT NULL,
    [Age]               VARCHAR (3)   NULL,
    [Mobile]            VARCHAR (15)  NULL,
    [FavoriteAnimeName] VARCHAR (100) NULL,
    PRIMARY KEY CLUSTERED ([Login] ASC),
);