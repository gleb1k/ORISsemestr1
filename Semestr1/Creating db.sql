﻿drop table Users cascade;
drop table Animes cascade;
drop table Comments;
drop table Posts;

CREATE TABLE Animes
(
    Id          INT PRIMARY KEY GENERATED by default as identity,
    Name        VARCHAR(255) NOT NULL,
    Author      VARCHAR(255) NOT NULL,
    Description TEXT         NOT NULL,
    Genre       VARCHAR(255) NOT NULL,
    Studio      VARCHAR(255) NOT NULL,
    AgeRating   VARCHAR(30) NOT NULL,
    ImageUrl    VARCHAR(1000)
);

CREATE TABLE Users
(
    Id              INT PRIMARY KEY GENERATED by default as identity,
    Login           VARCHAR(50) NOT NULL,
    Password        VARCHAR(50) NOT NULL,
    Username        varchar(50),
    Age             INTEGER,
    Mobile          VARCHAR(50),
    AvatarUrl       VARCHAR(1000),
    FavoriteAnimeId INTEGER REFERENCES Animes (Id) ON DELETE CASCADE
);

--Пост с аниме. Как правильно привязать комменты?
CREATE TABLE Posts
(
    Id      INTEGER PRIMARY KEY GENERATED by default as identity,
    Name    VARCHAR(250)                                     NOT NULL,
    UserId  INTEGER REFERENCES Users (Id)                    NOT NULL,
    AnimeId INTEGER REFERENCES Animes (Id) ON DELETE CASCADE NOT NULL
);

CREATE TABLE Comments
(
    Id             INTEGER PRIMARY KEY GENERATED by default as identity,
    UserId         INTEGER REFERENCES Users (Id) NOT NULL,
    PostId         INTEGER REFERENCES Posts (Id) NOT NULL,
    Description    TEXT                          NOT NULL,
    DateOfCreation timestamp                     NOT NULL
);
