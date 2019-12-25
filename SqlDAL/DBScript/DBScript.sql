USE [RestMediaServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP TABLE IF EXISTS dbo.[Like]
GO
DROP TABLE IF EXISTS dbo.[Opinion]
GO
DROP TABLE IF EXISTS dbo.[Topic]
GO
DROP TABLE IF EXISTS dbo.[Video]
GO
DROP TABLE IF EXISTS dbo.[Advertiser]
GO
DROP TABLE IF EXISTS dbo.[Friend]
GO
DROP TABLE IF EXISTS dbo.[Member]
GO
/***********/
/*  Member */
/***********/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Member](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[IsActive] [bit] NULL,
	[Dob] [datetime] NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP PROCEDURE IF EXISTS [DAH_Member_Delete]
GO
create procedure [dbo].[DAH_Member_Delete]
@Id int
as
begin
	set nocount on;
	delete from [Member]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [DAH_Member_GetAll]
GO
create procedure [dbo].[DAH_Member_GetAll]
as
begin
	set nocount on;
	select * from [Member]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [DAH_Member_Insert]
GO
CREATE procedure [dbo].[DAH_Member_Insert]
@Email nvarchar(50),
@Alias nvarchar(50),
@IsActive bit,
@Dob datetime
as
begin
	set nocount on;
	insert into [Member](Email, Alias, IsActive,Dob)
	values(@Email, @Alias, @IsActive, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Member_Scalar]
GO
create procedure [dbo].[DAH_Member_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [Member]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Member_Update]
GO

create procedure [dbo].[DAH_Member_Update]
@Id int,
@Email nvarchar(255),
@Alias nvarchar(255),
@Dob datetime,
@IsActive bit
as
begin
	set nocount on;
	update [Member]
	set Email=@Email, 
		Alias=@Alias, 
		Dob = @Dob,
		IsActive = @IsActive
	where id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Member_GetByAlias
GO

create procedure DAH_Member_GetByAlias
@Alias nvarchar(255)
as
begin
	set nocount on;
	select * from [Member]
	where alias = @Alias
end
GO

DROP PROCEDURE IF EXISTS DAH_Member_GetById
GO

create procedure DAH_Member_GetById
@Id int
as
begin
	set nocount on;
	select * from [Member]
	where id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Member_GetByIsActive
GO

create procedure DAH_Member_GetByIsActive
@IsActive bit
as
begin
	set nocount on;
	select * from [Member]
	where IsActive = @IsActive
end
GO

/***********/
/*  Friend */
/***********/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Friend](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] int FOREIGN KEY REFERENCES Member(Id),
	[FriendId] int FOREIGN KEY REFERENCES Member(Id),
	[Dob] [datetime] NULL,
 CONSTRAINT [PK_Friend] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP PROCEDURE IF EXISTS [DAH_Friend_Delete]
GO
create procedure [dbo].[DAH_Friend_Delete]
@Id int
as
begin
	set nocount on;
	delete from [Friend]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [DAH_Friend_GetAll]
GO
create procedure [dbo].[DAH_Friend_GetAll]
as
begin
	set nocount on;
	select * from [Friend]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [DAH_Friend_Insert]
GO
CREATE procedure [dbo].[DAH_Friend_Insert]
@MemberId int,
@FriendId int,
@Dob datetime
as
begin
	set nocount on;
	insert into [Friend](MemberId, FriendId, Dob)
	values(@MemberId, @FriendId, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Friend_Scalar]
GO
create procedure [dbo].[DAH_Friend_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [Friend]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS DAH_Friend_GetByMember
GO

create procedure DAH_Friend_GetByMember
@MemberId int
as
begin
	set nocount on;
	select Friend.Id, Friend.MemberId, Friend.FriendId, Friend.Dob, Member.Email, Member.Alias, Member.IsActive, Member.Dob as FDob from [Friend] inner join Member on Friend.FriendId = Member.id
	where MemberId = @MemberId
end
GO

DROP PROCEDURE IF EXISTS DAH_Friend_GetByMember
GO

create procedure DAH_Friend_GetByMember
@MemberId int,
@TopicId int
as
begin
	set nocount on;
	select Friend.Id, Friend.MemberId, Friend.FriendId, Friend.Dob, Member.Email, Member.Alias, Member.IsActive, Member.Dob as FDob from [Friend] 
		inner join Member on Friend.FriendId = Member.id
		inner join Topic on Friend.FriendId = Topic.Memberid and Topic.Id = @TopicId
	where MemberId = @MemberId
end
GO

DROP PROCEDURE IF EXISTS DAH_Friend_GetById
GO

create procedure DAH_Friend_GetById
@Id int
as
begin
	set nocount on;
	select * from [Friend]
	where id = @Id
end
GO

/*********/
/* TOPIC */
/*********/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Topic](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Dob] [datetime] NULL

 CONSTRAINT [PK_Topic] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP PROCEDURE IF EXISTS [DAH_Topic_Delete]
GO

create procedure [dbo].[DAH_Topic_Delete]
@Id int
as
begin
	set nocount on;
	delete from [Topic]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [DAH_Topic_GetAll]
GO

create procedure [dbo].[DAH_Topic_GetAll]
as
begin
	set nocount on;
	select * from [Topic]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Topic_Insert]
GO

CREATE procedure [dbo].[DAH_Topic_Insert]
@Description nvarchar(255),
@Dob datetime

as
begin
	set nocount on;
	insert into [Topic](Description, Dob)
	values(@Description, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Topic_Scalar]
GO


create procedure [dbo].[DAH_Topic_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [Topic]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Topic_Update]
GO

create procedure [dbo].[DAH_Topic_Update]
@Id int,
@Description nvarchar(255),
@Dob datetime

as
begin
	set nocount on;
	update [Topic]
	set Description=@Description, 
		Dob = @Dob
	where id = @Id
end
GO


DROP PROCEDURE IF EXISTS DAH_Topic_GetById
GO

create procedure DAH_Topic_GetById
@Id int
as
begin
	set nocount on;
	select * from [Topic]
	where id = @Id
end
GO

/***********/
/* Like */
/***********/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Like](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] FOREIGN KEY REFERENCES Member(Id),
	[TopicId] [int] FOREIGN KEY REFERENCES Topic(Id),
	[Comment] [varchar](255),
	[Dob] [datetime] NULL

CONSTRAINT [PK_Like] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP PROCEDURE IF EXISTS [DAH_Like_Delete]
GO

create procedure [dbo].[DAH_Like_Delete]
@Id int
as
begin
	set nocount on;
	delete from [Like]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
DROP PROCEDURE IF EXISTS [DAH_Like_GetAll]
GO
create procedure [dbo].[DAH_Like_GetAll]
as
begin
	set nocount on;
	select * from [Like]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Like_Insert]
GO

CREATE procedure [dbo].[DAH_Like_Insert]
@MemberId int,
@TopicId int,
@Comment nvarchar(255),
@Dob datetime

as
begin
	set nocount on;
	insert into [Like](MemberId, TopicId, Comment, Dob)
	values(@MemberId, @TopicId, @Comment, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Like_Scalar]
GO

create procedure [dbo].[DAH_Like_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [Like]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Like_Update]
GO

create procedure [dbo].[DAH_Like_Update]
@Id int,
@Comment nvarchar(255),
@Dob datetime

as
begin
	set nocount on;
	update [Like]
	set Comment=@Comment, 
		Dob = @Dob
	where id = @Id
end
GO


DROP PROCEDURE IF EXISTS DAH_Like_GetById
GO

create procedure DAH_Like_GetById
@Id int
as
begin
	set nocount on;
	select * from [Like]
	where id = @Id
end
GO

/***********/
/* Opinion */
/***********/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Opinion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MemberId] [int] FOREIGN KEY REFERENCES Member(Id),
	[TopicId] [int] FOREIGN KEY REFERENCES Topic(Id),
	[Comment] [nvarchar](255),
	[Dob] [datetime] NULL

CONSTRAINT [PK_Opinion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
DROP PROCEDURE IF EXISTS [DAH_Opinion_Delete]
GO

create procedure [dbo].[DAH_Opinion_Delete]
@Id int
as
begin
	set nocount on;
	delete from [Opinion]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Opinion_GetAll]
GO

create procedure [dbo].[DAH_Opinion_GetAll]
as
begin
	set nocount on;
	select * from [Opinion]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Opinion_Insert]
GO

CREATE procedure [dbo].[DAH_Opinion_Insert]
@MemberId int,
@TopicId int,
@Comment nvarchar(255),
@Dob datetime

as
begin
	set nocount on;
	insert into [Opinion](MemberId, TopicId, Comment, Dob)
	values(@MemberId, @TopicId, @Comment, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Opinion_Scalar]
GO

create procedure [dbo].[DAH_Opinion_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [Opinion]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS DAH_Opinion_GetById
GO

create procedure DAH_Opinion_GetById
@Id int
as
begin
	set nocount on;
	select * from [Opinion]
	where id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Opinion_GetByOpinionId
GO

create procedure DAH_Opinion_GetByOpinionId
@MemberId int
as
begin
	set nocount on;
	select * from [Opinion]
	where MemberId = @MemberId
end
GO




/***********/
/* Advertiserr */
/***********/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Advertiser](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](255),
	[Email] [varchar](255),
	[IsAccepted] [bit],
	[Dob] [datetime] NULL

CONSTRAINT [PK_Advertiser] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP PROCEDURE IF EXISTS [DAH_Advertiserr_Delete]
GO

create procedure [dbo].[DAH_Advertiserr_Delete]
@Id int
as
begin
	set nocount on;
	delete from [Advertiser]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Advertiser_GetAll]
GO

create procedure [dbo].[DAH_Advertiser_GetAll]
as
begin
	set nocount on;
	select * from [Advertiser]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Advertiser_Insert]
GO

CREATE procedure [dbo].[DAH_Advertiser_Insert]
@Name [varchar](255),
@Email [varchar](255),
@IsAccepted bit,
@Dob datetime

as
begin
	set nocount on;
	insert into [Advertiser](Name, Email, IsAccepted, Dob)
	values(@Name, @Email, @IsAccepted, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Advertiser_Scalar]
GO

create procedure [dbo].[DAH_Advertiser_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [Advertiser]
end
GO

DROP PROCEDURE IF EXISTS DAH_Advertiser_GetById
GO

create procedure DAH_Advertiser_GetById
@Id int
as
begin
	set nocount on;
	select * from [Advertiser]
	where id = @Id
end
GO



/***********/
/* Video */
/***********/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Video](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[AdvertiserId] [int] FOREIGN KEY REFERENCES Advertiser(Id),
	[Url] [nvarchar](255),
	[Dob] [datetime] NULL

CONSTRAINT [PK_Video] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

DROP PROCEDURE IF EXISTS [DAH_Video_Delete]
GO

create procedure [dbo].[DAH_Video_Delete]
@Id int
as
begin
	set nocount on;
	delete from [Video]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Video_GetAll]
GO

create procedure [dbo].[DAH_Video_GetAll]
as
begin
	set nocount on;
	select * from [Video]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Video_GetAll]
GO

CREATE procedure [dbo].[DAH_Video_GetAll]
@AdvertiseId int,
@Url int,
@Dob datetime

as
begin
	set nocount on;
	insert into [Video](AdvertiserId, Url, Dob)
	values(@AdvertiseId, @Url, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

DROP PROCEDURE IF EXISTS [DAH_Video_Scalar]
GO

create procedure [dbo].[DAH_Video_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [Video]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


DROP PROCEDURE IF EXISTS DAH_Video_GetById
GO

create procedure DAH_Video_GetById
@Id int
as
begin
	set nocount on;
	select * from [Video]
	where id = @Id
end
GO
