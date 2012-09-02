/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2012/4/11 22:55:51                           */
/*==============================================================*/


if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('"�û�--Ⱥ��Ӧ��"') and o.name = 'FK_�û�--Ⱥ��Ӧ��_REFERENCE_�û�Ⱥ��Ϣ��')
alter table "�û�--Ⱥ��Ӧ��"
   drop constraint "FK_�û�--Ⱥ��Ӧ��_REFERENCE_�û�Ⱥ��Ϣ��"
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('�û���Ϣ��') and o.name = 'FK_�û���Ϣ��_REFERENCE_���������')
alter table �û���Ϣ��
   drop constraint FK_�û���Ϣ��_REFERENCE_���������
go

if exists (select 1
   from sys.sysreferences r join sys.sysobjects o on (o.id = r.constid and o.type = 'F')
   where r.fkeyid = object_id('�û���Ϣ��') and o.name = 'FK_�û���Ϣ��_REFERENCE_�û�--Ⱥ��Ӧ��')
alter table �û���Ϣ��
   drop constraint "FK_�û���Ϣ��_REFERENCE_�û�--Ⱥ��Ӧ��"
go

alter table ���������
   drop constraint PK_���������
go

if exists (select 1
            from  sysobjects
           where  id = object_id('���������')
            and   type = 'U')
   drop table ���������
go

if exists (select 1
            from  sysobjects
           where  id = object_id('"�û�--Ⱥ��Ӧ��"')
            and   type = 'U')
   drop table "�û�--Ⱥ��Ӧ��"
go

alter table �û���Ϣ��
   drop constraint PK_�û���Ϣ��
go

if exists (select 1
            from  sysobjects
           where  id = object_id('�û���Ϣ��')
            and   type = 'U')
   drop table �û���Ϣ��
go

alter table �û�Ⱥ��Ϣ��
   drop constraint PK_�û�Ⱥ��Ϣ��
go

if exists (select 1
            from  sysobjects
           where  id = object_id('�û�Ⱥ��Ϣ��')
            and   type = 'U')
   drop table �û�Ⱥ��Ϣ��
go

/*==============================================================*/
/* Table: ���������                                                 */
/*==============================================================*/
create table ��������� (
   QuestionID           numeric(2)           not null,
   Question             varchar(20)          not null
)
go

alter table ���������
   add constraint PK_��������� primary key (QuestionID)
go

/*==============================================================*/
/* Table: "�û�--Ⱥ��Ӧ��"                                            */
/*==============================================================*/
create table "�û�--Ⱥ��Ӧ��" (
   LoginName            varchar(50)          not null,
   GroupID              numeric              not null
)
go

/*==============================================================*/
/* Table: �û���Ϣ��                                                 */
/*==============================================================*/
create table �û���Ϣ�� (
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

alter table �û���Ϣ��
   add constraint PK_�û���Ϣ�� primary key (LoginName)
go

/*==============================================================*/
/* Table: �û�Ⱥ��Ϣ��                                                */
/*==============================================================*/
create table �û�Ⱥ��Ϣ�� (
   GroupID              numeric              not null,
   GroupName            varchar(10)          not null,
   GroupType            varchar(10)          null,
   GroupTips            varchar(100)         null,
   Notes                varchar(100)         null,
   Creater              varchar(25)          null,
   CreateDate           datetime             not null
)
go

alter table �û�Ⱥ��Ϣ��
   add constraint PK_�û�Ⱥ��Ϣ�� primary key (GroupID)
go

alter table "�û�--Ⱥ��Ӧ��"
   add constraint "FK_�û�--Ⱥ��Ӧ��_REFERENCE_�û�Ⱥ��Ϣ��" foreign key (GroupID)
      references �û�Ⱥ��Ϣ�� (GroupID)
go

alter table �û���Ϣ��
   add constraint FK_�û���Ϣ��_REFERENCE_��������� foreign key (QuestionID)
      references ��������� (QuestionID)
go

alter table �û���Ϣ��
   add constraint "FK_�û���Ϣ��_REFERENCE_�û�--Ⱥ��Ӧ��" foreign key (LoginName)
      references "�û�--Ⱥ��Ӧ��" (LoginName)
go

