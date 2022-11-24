drop table Users;
drop table Animes;
drop table Comments;
drop table Posts;

CREATE TABLE Animes (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL ,
    Name    VARCHAR (255) NOT NULL,
    Author VARCHAR (255) NOT NULL,
    Description TEXT NOT NULL,
    ImageUrl VARCHAR (1000)
);

CREATE TABLE Users (
    Id INT IDENTITY (1,1)PRIMARY KEY NOT NULL,
    Login VARCHAR (50)  NOT NULL,
    Password VARCHAR (50)  NOT NULL,
    Age INT,
    Mobile VARCHAR (15),
    FavoriteAnimeId INT REFERENCES Animes (Id) ON DELETE CASCADE,
);

--Пост с аниме. Как правильно привязать комменты?
CREATE TABLE Posts(
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL ,
    PostAuthorId INT REFERENCES Users (Id) NOT NULL,
    AnimeId INT REFERENCES Animes (Id) ON DELETE CASCADE NOT NULL ,
);

CREATE TABLE Comments(
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL ,
    CommentAuthorId INT REFERENCES Users (Id) NOT NULL,
    PostId INT REFERENCES Posts (Id) NOT NULL,
    Description TEXT NOT NULL,
    DateOfCreation DATE NOT NULL,
);









