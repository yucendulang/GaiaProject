/*
Navicat SQL Server Data Transfer

Source Server         : gaiaproject2.database.chinacloudapi.cn
Source Server Version : 120000
Source Host           : gaiaproject2.database.chinacloudapi.cn:1433
Source Database       : gaiaproject2
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 120000
File Encoding         : 65001

Date: 2017-12-28 14:03:28
*/


-- ----------------------------
-- Table structure for __EFMigrationsHistory
-- ----------------------------
DROP TABLE [dbo].[__EFMigrationsHistory]
GO
CREATE TABLE [dbo].[__EFMigrationsHistory] (
[MigrationId] nvarchar(150) NOT NULL ,
[ProductVersion] nvarchar(32) NOT NULL ,
PRIMARY KEY ([MigrationId])
)


GO

-- ----------------------------
-- Indexes structure for table __EFMigrationsHistory
-- ----------------------------

-- ----------------------------
-- Table structure for AspNetRoleClaims
-- ----------------------------
DROP TABLE [dbo].[AspNetRoleClaims]
GO
CREATE TABLE [dbo].[AspNetRoleClaims] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ClaimType] nvarchar(MAX) NULL ,
[ClaimValue] nvarchar(MAX) NULL ,
[RoleId] nvarchar(450) NOT NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table AspNetRoleClaims
-- ----------------------------
CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [dbo].[AspNetRoleClaims]
([RoleId] ASC) 
GO

-- ----------------------------
-- Table structure for AspNetRoles
-- ----------------------------
DROP TABLE [dbo].[AspNetRoles]
GO
CREATE TABLE [dbo].[AspNetRoles] (
[Id] nvarchar(450) NOT NULL ,
[ConcurrencyStamp] nvarchar(MAX) NULL ,
[Name] nvarchar(256) NULL ,
[NormalizedName] nvarchar(256) NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUserClaims
-- ----------------------------
DROP TABLE [dbo].[AspNetUserClaims]
GO
CREATE TABLE [dbo].[AspNetUserClaims] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ClaimType] nvarchar(MAX) NULL ,
[ClaimValue] nvarchar(MAX) NULL ,
[UserId] nvarchar(450) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUserLogins
-- ----------------------------
DROP TABLE [dbo].[AspNetUserLogins]
GO
CREATE TABLE [dbo].[AspNetUserLogins] (
[LoginProvider] nvarchar(450) NOT NULL ,
[ProviderKey] nvarchar(450) NOT NULL ,
[ProviderDisplayName] nvarchar(MAX) NULL ,
[UserId] nvarchar(450) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUserRoles
-- ----------------------------
DROP TABLE [dbo].[AspNetUserRoles]
GO
CREATE TABLE [dbo].[AspNetUserRoles] (
[UserId] nvarchar(450) NOT NULL ,
[RoleId] nvarchar(450) NOT NULL 
)


GO

-- ----------------------------
-- Table structure for AspNetUsers
-- ----------------------------
DROP TABLE [dbo].[AspNetUsers]
GO
CREATE TABLE [dbo].[AspNetUsers] (
[Id] nvarchar(450) NOT NULL ,
[AccessFailedCount] int NOT NULL ,
[ConcurrencyStamp] nvarchar(MAX) NULL ,
[Email] nvarchar(256) NULL ,
[EmailConfirmed] bit NOT NULL ,
[LockoutEnabled] bit NOT NULL ,
[LockoutEnd] datetimeoffset(7) NULL ,
[NormalizedEmail] nvarchar(256) NULL ,
[NormalizedUserName] nvarchar(256) NULL ,
[PasswordHash] nvarchar(MAX) NULL ,
[PhoneNumber] nvarchar(MAX) NULL ,
[PhoneNumberConfirmed] bit NOT NULL ,
[SecurityStamp] nvarchar(MAX) NULL ,
[TwoFactorEnabled] bit NOT NULL ,
[UserName] nvarchar(256) NULL ,
[groupid] int NULL DEFAULT ((0)) 
)


GO

-- ----------------------------
-- Table structure for AspNetUserTokens
-- ----------------------------
DROP TABLE [dbo].[AspNetUserTokens]
GO
CREATE TABLE [dbo].[AspNetUserTokens] (
[UserId] nvarchar(450) NOT NULL ,
[LoginProvider] nvarchar(450) NOT NULL ,
[Name] nvarchar(450) NOT NULL ,
[Value] nvarchar(MAX) NULL 
)


GO

-- ----------------------------
-- Table structure for GameFactionModel
-- ----------------------------
DROP TABLE [dbo].[GameFactionModel]
GO
CREATE TABLE [dbo].[GameFactionModel] (
[Id] int NOT NULL IDENTITY(1,1) ,
[FactionChineseName] nvarchar(20) NULL ,
[FactionName] nvarchar(20) NULL ,
[gameinfo_id] int NOT NULL ,
[kjPostion] nvarchar(20) NULL ,
[numberBuild] nvarchar(20) NULL ,
[numberFst1] int NOT NULL ,
[numberFst2] int NOT NULL ,
[rank] int NOT NULL ,
[scoreFst1] int NOT NULL ,
[scoreFst2] int NOT NULL ,
[scoreKj] int NOT NULL ,
[scorePw] int NOT NULL ,
[scoreRound] nvarchar(20) NULL ,
[scoreTotal] int NOT NULL ,
[userid] nvarchar(20) NULL ,
[username] nvarchar(20) NULL ,
[gameinfo_name] nvarchar(20) NULL ,
[UserCount] int NULL ,
[scoreLuo] int NULL ,
[scoreDifference] int NULL ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table GameFactionModel
-- ----------------------------

-- ----------------------------
-- Table structure for GameInfoModel
-- ----------------------------
DROP TABLE [dbo].[GameInfoModel]
GO
CREATE TABLE [dbo].[GameInfoModel] (
[Id] int NOT NULL IDENTITY(1,1) ,
[ATTList] nvarchar(50) NULL ,
[FSTList] nvarchar(30) NULL ,
[GameStatus] int NOT NULL ,
[RBTList] nvarchar(50) NULL ,
[RSTList] nvarchar(50) NULL ,
[STT3List] nvarchar(30) NULL ,
[STT6List] nvarchar(50) NULL ,
[UserCount] int NOT NULL ,
[loginfo] varchar(MAX) NULL ,
[name] nvarchar(20) NULL ,
[version] int NOT NULL ,
[endtime] smalldatetime NULL ,
[scoreFaction] nvarchar(100) NULL ,
[IsTestGame] int NULL ,
[userlist] varchar(50) NULL ,
[MapSelction] varchar(20) NULL ,
[starttime] smalldatetime NULL ,
[username] varchar(20) NULL ,
[round] int NULL DEFAULT ((0)) ,
[IsRandomOrder] bit NULL DEFAULT ((1)) ,
[IsAllowLook] bit NULL DEFAULT ((1)) ,
PRIMARY KEY ([Id])
)


GO

-- ----------------------------
-- Indexes structure for table GameInfoModel
-- ----------------------------

-- ----------------------------
-- Table structure for UserFriend
-- ----------------------------
DROP TABLE [dbo].[UserFriend]
GO
CREATE TABLE [dbo].[UserFriend] (
[Id] int NOT NULL IDENTITY(1,1) ,
[Remark] nvarchar(50) NULL ,
[UserId] nvarchar(50) NULL ,
[UserIdTo] nvarchar(50) NULL ,
[UserName] nvarchar(50) NULL ,
[UserNameTo] nvarchar(50) NULL ,
[Type] int NULL DEFAULT ((1)) 
)


GO
