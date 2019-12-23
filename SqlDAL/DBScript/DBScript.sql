USE [RestMediaServer]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*********/
/*  USER */
/*********/
create procedure [dbo].[DAH_User_Delete]
@Id int
as
begin
	set nocount on;
	delete from [User]
	where Id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
create procedure [dbo].[DAH_User_GetAll]
as
begin
	set nocount on;
	select * from [User]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE procedure [dbo].[DAH_User_Insert]
@Email nvarchar(50),
@Alias nvarchar(50),
@Dob datetime,
@IsActive bit,
@IsEmailConfirmed bit

as
begin
	set nocount on;
	insert into [User](Email, Alias, Dob, IsActive)
	values(@Email, @Alias, @Dob, @IsActive)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[DAH_User_Scalar]
as
begin
	set nocount on;
	select COUNT(Id) as Id from [User]
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

create procedure [dbo].[DAH_User_Update]
@Id int,
@Email nvarchar(50),
@Alias nvarchar(50),
@Dob datetime,
@IsActive bit
as
begin
	set nocount on;
	update [User]
	set Email=@Eamil, 
		Alias=@Alias, 
		Dob = @Dob,
		IsActive = @osActive
	where id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Email] [nvarchar](255) NOT NULL,
	[Alias] [nvarchar](50) NOT NULL,
	[Dob] [datetime] NULL,
	[IsActive] [datetime] NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

create procedure DAH_User_GetById
@Id int
as
begin
	set nocount on;
	select * from [User]
	where id = @Id
end
GO


/*********/
/* TOPIC */
/*********/
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
/* Opinion */
/***********/
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

CREATE procedure [dbo].[DAH_Opinion_Insert]
@Description nvarchar(255),
@Dob datetime

as
begin
	set nocount on;
	insert into [Opinion](Description, Dob)
	values(@Description, @Dob)

	select @@IDENTITY
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
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

create procedure [dbo].[DAH_Opinion_Update]
@Id int,
@Description nvarchar(255),
@Dob datetime

as
begin
	set nocount on;
	update [Opinion]
	set Description=@Description, 
		Dob = @Dob
	where id = @Id
end
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Opinion](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Description] [nvarchar](255) NOT NULL,
	[Dob] [datetime] NULL

 CONSTRAINT [PK_Opinion] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

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

