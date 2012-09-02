/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2012/4/11 22:55:51                           */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('"用户--群对应表"') and o.name = 'FK_用户--群对应表_REFERENCE_用户群信息表')
alter table "用户--群对应表"
   drop constraint "FK_用户--群对应表_REFERENCE_用户群信息表"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('用户信息表') and o.name = 'FK_用户信息表_REFERENCE_密码问题表')
alter table 用户信息表
   drop constraint FK_用户信息表_REFERENCE_密码问题表
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('用户信息表') and o.name = 'FK_用户信息表_REFERENCE_用户--群对应表')
alter table 用户信息表
   drop constraint "FK_用户信息表_REFERENCE_用户--群对应表"
go

alter table 密码问题表
   drop constraint PK_密码问题表
go

if exists (select 1
            from  sysobjects
           where  id = object_id('密码问题表')
            and   type = 'U')
   drop table 密码问题表
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"用户--群对应表"')
            and   type = 'U')
   drop table "用户--群对应表"
go

alter table 用户信息表
   drop constraint PK_用户信息表
go

if exists (select 1
            from  sysobjects
           where  id = object_id('用户信息表')
            and   type = 'U')
   drop table 用户信息表
go

alter table 用户群信息表
   drop constraint PK_用户群信息表
go

if exists (select 1
            from  sysobjects
           where  id = object_id('用户群信息表')
            and   type = 'U')
   drop table 用户群信息表
go

/*==============================================================*/
/* Table: 密码问题表                                                 */
/*==============================================================*/
create table 密码问题表 (
   QuestionID           numeric(2)           not null,
   Question             varchar(20)          not null
)
go

alter table 密码问题表
   add constraint PK_密码问题表 primary key (QuestionID)
go

/*==============================================================*/
/* Table: "用户--群对应表"                                            */
/*==============================================================*/
create table "用户--群对应表" (
   LoginName            varchar(50)          not null,
   GroupID              numeric              not null
)
go

/*==============================================================*/
/* Table: 用户信息表                                                 */
/*==============================================================*/
create table 用户信息表 (
   LoginName            varchar(25)          not null,
   Password             varchar(15)          not null,
   Name                 varchar(10)          not null,
   EnglishName          varchar(25)          null,
   Sexy                 varchar(2)           null,
   BirthDay             datetime             null,
   Phone                varchar(20)          null,
   MobilPhone           varchar(20)          null,
   Mail                 varchar(50)          null,
   QQ                   numeric(15)          null,
   Weibo                varchar(50)          null,
   MSN                  varchar(50)          null,
   GraduteSchool        varchar(50)          null,
   Job                  varchar(10)          null,
   Company              varchar(50)          null,
   IDNumber             varchar(18)          null,
   Photo                image                null,
   QuestionID           numeric(2)           null,
   QuestionAnswer       varchar(20)          null
)
go

alter table 用户信息表
   add constraint PK_用户信息表 primary key (LoginName)
go

/*==============================================================*/
/* Table: 用户群信息表                                                */
/*==============================================================*/
create table 用户群信息表 (
   GroupID              numeric              not null,
   GroupName            varchar(10)          not null,
   GroupType            varchar(10)          null,
   GroupTips            varchar(100)         null,
   Notes                varchar(100)         null,
   Creater              varchar(25)          null,
   CreateDate           datetime             not null
)
go

alter table 用户群信息表
   add constraint PK_用户群信息表 primary key (GroupID)
go

alter table "用户--群对应表"
   add constraint "FK_用户--群对应表_REFERENCE_用户群信息表" foreign key (GroupID)
      references 用户群信息表 (GroupID)
go

alter table 用户信息表
   add constraint FK_用户信息表_REFERENCE_密码问题表 foreign key (QuestionID)
      references 密码问题表 (QuestionID)
go

alter table 用户信息表
   add constraint "FK_用户信息表_REFERENCE_用户--群对应表" foreign key (LoginName)
      references "用户--群对应表" (LoginName)
go

