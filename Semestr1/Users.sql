CREATE TABLE [dbo].[Users] (
    [Id]       INT          IDENTITY (1, 1)  NOT NULL PRIMARY KEY,
    [Login]    VARCHAR (50) NOT NULL,
    [Password] VARCHAR (50) NOT NULL,
    [Age] INT NOT NULL,
    [Mobile] VARCHAR (50) NOT NULL,
    [FavoriteAnimeName] VARCHAR (50) NOT NULL,
    CHECK (Age>=1),
    CHECK (LEN(Login) >= 4),
    CHECK (LEN(Password) >=4),
    CHECK (LEN(Mobile) >=3),
);