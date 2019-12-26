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
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL unique,
	[IsActive] [bit] NOT NULL,
	[Dob] [datetime] NOT NULL,
 CONSTRAINT [PK_Member] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
CREATE UNIQUE NONCLUSTERED INDEX [IDX_MemberAlias] ON [dbo].[Member]
(
	[Alias] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

DROP PROCEDURE IF EXISTS [DAH_Member_Delete]
GO
create procedure [dbo].[DAH_Member_Delete]
@Id [bigint]
as
begin
	set nocount on;
	delete from [Member]
	where Id = @Id
end
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

DROP PROCEDURE IF EXISTS [DAH_Member_Update]
GO
create procedure [dbo].[DAH_Member_Update]
@Id [bigint],
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

	select @@ROWCOUNT
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
@Id [bigint]
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
CREATE TABLE [dbo].[Friend](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MemberId] [bigint] FOREIGN KEY REFERENCES Member(Id),
	[FriendId] [bigint] FOREIGN KEY REFERENCES Member(Id),
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
@Id [bigint]
as
begin
	set nocount on;
	delete from [Friend]
	where Id = @Id

	select @@ROWCOUNT
end
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

DROP PROCEDURE IF EXISTS [DAH_Friend_Insert]
GO
CREATE procedure [dbo].[DAH_Friend_Insert]
@MemberId bigint,
@FriendId bigint,
@Dob datetime
as
begin
	set nocount on;
	insert into [Friend](MemberId, FriendId, Dob)
	values(@MemberId, @FriendId, @Dob)

	select @@IDENTITY
end
GO

DROP PROCEDURE IF EXISTS DAH_Friend_GetAll
GO
create procedure DAH_Friend_GetAll
as
begin
	set nocount on;
	select Friend.Id, Friend.MemberId, Friend.Dob, Member.Id as FriendId, Member.Email as FEmail, Member.Alias as FAlias, Member.IsActive as FIsActive, Member.Dob as FDob from [Friend] 
		inner join Member on Friend.FriendId = Member.id
end
GO

DROP PROCEDURE IF EXISTS DAH_Friend_GetById
GO
create procedure DAH_Friend_GetById
@Id [bigint]
as
begin
	set nocount on;
	select Friend.Id, Friend.MemberId, Friend.Dob, Member.Id as FriendId,  Member.Email as FEmail, Member.Alias as FAlias, Member.IsActive as FIsActive, Member.Dob as FDob from [Friend] 
		inner join Member on Friend.FriendId = Member.id
		where Friend.Id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Friend_GetByMemberAlias
GO
create procedure DAH_Friend_GetByMemberAlias
@Alias nvarchar(255)
as
begin
	set nocount on;
	select Friend.Id, Friend.MemberId, Friend.Dob, Member.Id as FriendId, Member.Email as FEmail, Member.Alias as FAlias, Member.IsActive as FIsActive, Member.Dob as FDob from [Friend] 
		inner join Member on Friend.FriendId = Member.id
		inner join Member as FromMember on Friend.MemberId = FromMember.id
		where FromMember.Alias = @Alias
end
GO

DROP PROCEDURE IF EXISTS DAH_Friend_GetByMemberId
GO
create procedure DAH_Friend_GetByMemberId
@Id [bigint]
as
begin
	set nocount on;
	select Friend.Id, Friend.MemberId, Friend.Dob, Member.Id as FriendId, Member.Email as FEmail, Member.Alias as FAlias, Member.IsActive as FIsActive, Member.Dob as FDob from [Friend] 
		inner join Member on Friend.FriendId = Member.id
		inner join Member as FromMember on Friend.MemberId = FromMember.id
		where FromMember.Id = @Id
end
GO

/*********/
/* TOPIC */
/*********/
CREATE TABLE [dbo].[Topic](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
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
@Id [bigint]
as
begin
	set nocount on;
	delete from [Topic]
	where Id = @Id

	select @@ROWCOUNT
end
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

DROP PROCEDURE IF EXISTS [DAH_Topic_Update]
GO
create procedure [dbo].[DAH_Topic_Update]
@Id [bigint],
@Description nvarchar(255),
@Dob datetime
as
begin
	set nocount on;
	update [Topic]
	set Description=@Description, 
		Dob = @Dob
	where id = @Id

	select @@ROWCOUNT
end
GO


DROP PROCEDURE IF EXISTS DAH_Topic_GetById
GO
create procedure DAH_Topic_GetById
@Id [bigint]
as
begin
	set nocount on;
	select * from [Topic]
	where id = @Id
end
GO

/***********/
/* Opinion */
/***********/
CREATE TABLE [dbo].[Opinion](
	[Id] bigint IDENTITY(1,1) NOT NULL,
	[MemberId] bigint FOREIGN KEY REFERENCES Member(Id),
	[TopicId] bigint FOREIGN KEY REFERENCES Topic(Id),
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
@Id bigint
as
begin
	set nocount on;
	delete from [Opinion]
	where Id = @Id

	select @@ROWCOUNT
end
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

DROP PROCEDURE IF EXISTS [DAH_Opinion_GetAllFull]
GO
create procedure [dbo].[DAH_Opinion_GetAllFull]
as
begin
	set nocount on;
	select	Opinion.Id, Opinion.MemberId, Opinion.TopicId, Opinion.Comment, 
			Member.Email as MEmail, Member.Alias as MAlias, Member.Dob as MDob,
			Topic.Description as TDescription, Topic.Dob as TDob
	from [Opinion]
	inner join Member on Opinion.MemberId = Member.Id
	inner join Topic on Opinion.TopicId = Topic.Id
end
GO

DROP PROCEDURE IF EXISTS [DAH_Opinion_Insert]
GO
CREATE procedure [dbo].[DAH_Opinion_Insert]
@MemberId bigint,
@TopicId bigint,
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

DROP PROCEDURE IF EXISTS DAH_Opinion_GetById
GO
create procedure DAH_Opinion_GetById
@Id bigint
as
begin
	set nocount on;
	select * from [Opinion]
	where id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Opinion_GetByMember
GO
create procedure DAH_Opinion_GetByMember
@MemberId bigint
as
begin
	set nocount on;
	select * from [Opinion]
	where MemberId = @MemberId
end
GO

DROP PROCEDURE IF EXISTS DAH_Opinion_GetFullByMember
GO
create procedure DAH_Opinion_GetFullByMember
@MemberId bigint
as
begin
	set nocount on;
	select	Opinion.Id, Opinion.MemberId, Opinion.TopicId, Opinion.Comment, 
			Member.Email as MEmail, Member.Alias as MAlias, Member.Dob as MDob,
			Topic.Description as TDescription, Topic.Dob as TDob
	from [Opinion]
	inner join Member on Opinion.MemberId = Member.Id
	inner join Topic on Opinion.TopicId = Topic.Id
	where MemberId = @MemberId
end
GO

DROP PROCEDURE IF EXISTS DAH_Opinion_GetByTopic
GO
create procedure DAH_Opinion_GetByTopic
@TopicId bigint
as
begin
	set nocount on;
	select * from [Opinion]
	where TopicId = @TopicId
end
GO

DROP PROCEDURE IF EXISTS DAH_Opinion_GetFullByTopic
GO
create procedure DAH_Opinion_GetFullByTopic
@TopicId bigint
as
begin
	set nocount on;
	select	Opinion.Id, Opinion.MemberId, Opinion.TopicId, Opinion.Comment, 
			Member.Email as MEmail, Member.Alias as MAlias, Member.Dob as MDob,
			Topic.Description as TDescription, Topic.Dob as TDob
	from [Opinion]
	inner join Member on Opinion.MemberId = Member.Id
	inner join Topic on Opinion.TopicId = Topic.Id
	where Opinion.TopicId = @TopicId
end
GO

/***********/
/* Like    */
/***********/
CREATE TABLE [dbo].[Like](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[MemberId] bigint FOREIGN KEY REFERENCES Member(Id),
	[OpinionId] bigint FOREIGN KEY REFERENCES Opinion(Id),
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
@Id [bigint]
as
begin
	set nocount on;
	delete from [Like]
	where Id = @Id

	select @@ROWCOUNT
end
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

DROP PROCEDURE IF EXISTS [DAH_Like_Insert]
GO
CREATE procedure [dbo].[DAH_Like_Insert]
@MemberId bigint,
@OpinionId bigint,
@Dob datetime
as
begin
	set nocount on;
	insert into [Like](MemberId, OpinionId, Dob)
	values(@MemberId, @OpinionId, @Dob)
	select @@IDENTITY
end
GO

DROP PROCEDURE IF EXISTS DAH_Like_GetById
GO
create procedure DAH_Like_GetById
@Id bigint
as
begin
	set nocount on;
	select * from [Like]
	where id = @Id
end
GO


/***************/
/* Advertiserr */
/**************/
CREATE TABLE [dbo].[Advertiser](
	[Id] bigint IDENTITY(1,1) NOT NULL,
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
@Id bigint
as
begin
	set nocount on;
	delete from [Advertiser]
	where Id = @Id

	select @@ROWCOUNT
end
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

DROP PROCEDURE IF EXISTS DAH_Advertiser_GetById
GO
create procedure DAH_Advertiser_GetById
@Id bigint
as
begin
	set nocount on;
	select * from [Advertiser]
	where id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Advertiser_GetByEmail
GO
create procedure DAH_Advertiser_GetByEmail
@Email nvarchar(255)
as
begin
	set nocount on;
	select * from [Advertiser]
	where Email = @Email
end
GO

DROP PROCEDURE IF EXISTS DAH_Advertiser_GetByIsAccepted
GO
create procedure DAH_Advertiser_GetByIsAccepted
@IsAccepted bit
as
begin
	set nocount on;
	select * from [Advertiser]
	where IsAccepted = @IsAccepted
end
GO

/***********/
/* Video   */
/***********/
CREATE TABLE [dbo].[Video](
	[Id] bigint IDENTITY(1,1) NOT NULL,
	[AdvertiserId] bigint FOREIGN KEY REFERENCES Advertiser(Id),
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
@Id bigint
as
begin
	set nocount on;
	delete from [Video]
	where Id = @Id

	select @@ROWCOUNT
end
GO

DROP PROCEDURE IF EXISTS [DAH_Video_Insert]
GO
create procedure [dbo].[DAH_Video_Insert]
@AdvertiserId bigint,
@Url nvarchar(255),
@Dob datetime
as
begin
	set nocount on;
	insert into [Video](AdvertiserId, Url, Dob)
	values(@AdvertiserId, @Url, @Dob)

	select @@IDENTITY
end
GO

DROP PROCEDURE IF EXISTS [DAH_Video_Update]
GO
create procedure [dbo].[DAH_Video_Update]
@Id bigint,
@AdvertiserId bigint,
@Url nvarchar(255),
@Dob datetime
as
begin
	set nocount on;
	update [Video]
	set AdvertiserId=@AdvertiserId, 
		Url=@Url,
		Dob = @Dob
	where id = @Id

	select @@ROWCOUNT
end
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

DROP PROCEDURE IF EXISTS [DAH_Video_GetAll]
GO
create procedure [dbo].[DAH_Video_GetAll]
as
begin
	set nocount on;
	select * from [Video], Advertiser.Email as AEMail, Advertiser.Name as AName, Advertiser.Dob as ADob
	inner join Advertiser on Advertiser.Id = Video.AdvertiseId
end
GO

DROP PROCEDURE IF EXISTS [DAH_Video_GetAll]
GO
create procedure [dbo].[DAH_Video_GetAll]
@Id bigint
as
begin
	set nocount on;
	select * from [Video], Advertiser.Email as AEMail, Advertiser.Name as AName, Advertiser.Dob as ADob
	inner join Advertiser on Advertiser.Id = Video.AdvertiseId
	where Video.Advertiser.Id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Video_GetById
GO
create procedure DAH_Video_GetById
@Id bigint
as
begin
	set nocount on;
	select * from [Video]
	where id = @Id
end
GO

DROP PROCEDURE IF EXISTS DAH_Video_GetById
GO
create procedure DAH_Video_GetById
@Id bigint
as
begin
	set nocount on;
	select * from [Video], Advertiser.Email as AEMail, Advertiser.Name as AName, Advertiser.Dob as ADob
	inner join Advertiser on Advertiser.Id = Video.AdvertiseId
	where Video.Id = @Id
end
GO
