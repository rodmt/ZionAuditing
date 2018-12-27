-- Copyright © 2018 Zion Software Solutions, LLC. All Rights Reserved.
-- 
-- Unpublished copyright. This material contains proprietary information
-- that shall be used or copied only within Zion Software Solutions, 
-- except with written permission of Zion Software Solutions.

use [master];
go


if exists (select 1 from [sys].[databases] where [name] = N'ZionAuditing')
begin
	print formatmessage(N'%4s...dropping database [ZionAuditing]', N'');
	alter database [ZionAuditing] set single_user with rollback immediate;
	drop database [ZionAuditing];
end;
go

print formatmessage(N'%4s...creating database [ZionAuditing]', N'');
go

declare @za_data_file sysname = concat(cast(serverproperty(N'InstanceDefaultDataPath') as sysname), N'ZionAuditing.mdf');
declare @za_user_data_file sysname = concat(cast(serverproperty(N'InstanceDefaultDataPath') as sysname), N'ZionAuditing_UserData.ndf');
declare @za_log_file sysname = concat(cast(serverproperty(N'InstanceDefaultLogPath') as sysname), N'ZionAuditing_log.ldf');

declare @db_create_script nvarchar(1000) = N'
	create database [ZionAuditing]
	on primary
	(
		name = ZA_Primary
		, filename = ' + quotename(@za_data_file, N'''') + N'
		, size = 10GB
		, maxsize = unlimited
		, filegrowth = 5GB
	)
	, filegroup userdata default
	(
		name = ZA_UserData
		, filename = ' + quotename(@za_user_data_file, N'''') + N'
		, size = 2GB
		, maxsize = unlimited
		, filegrowth = 64MB
	)
	log on
	(
		name = ZA_Log
		, filename = ' + quotename(@za_log_file, N'''') + N'
		, size = 100MB
		, maxsize = unlimited
		, filegrowth = 64MB
	);';

exec(@db_create_script)
go

alter authorization on database::ZionAuditing to sqladmin;
go

use [ZionAuditing];
go

alter database current collate Latin1_General_100_CI_AS;
alter database current set memory_optimized_elevate_to_snapshot on;
alter database current set recovery simple; --full;
alter database current set auto_update_statistics_async on;
alter database current set allow_snapshot_isolation on;
go

print formatmessage(N'%4s...setting query store on database [ZionAuditing]', N'');
go

alter database current
set query_store
(
	operation_mode = read_write
	, cleanup_policy = (stale_query_threshold_days = 30)
	, data_flush_interval_seconds = 3000
	, max_storage_size_mb = 2048
	, interval_length_minutes = 15
	, size_based_cleanup_mode = auto
	, query_capture_mode = auto
	, max_plans_per_query = 1000
);
go

print formatmessage(N'%4s...configuring database for memory-optimized tables', N'');
go

alter database current add filegroup [ZA_mod] contains memory_optimized_data;
go

declare @za_platform_dir_path sysname = concat(cast(serverproperty(N'InstanceDefaultDataPath') as sysname), N'ZionAuditing_dir');
declare @mod_add_file_script nvarchar(1000) = concat(N'alter database current add file (name = [ZionAuditing_dir], filename = N''', @za_platform_dir_path, N''') to filegroup [ZA_mod];');
exec(@mod_add_file_script);
go

print formatmessage(N'%4s...Adding new role and users to roles.', N'');
go

if database_principal_id('db_executor') is null
begin
	print formatmessage(N'%4s...creating [db_executor] role', N'');
	create role db_executor;
	grant execute to db_executor;
end
go

alter login sqladmin enable;
go


print formatmessage(N'%4s...creating schemas', N'');
go

create schema [seq] authorization dbo;
go

exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Holds sequences used by all tables in the application', @level0type = N'schema', @level0name = [seq];
go

create schema [aud] authorization dbo;
go

exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'SQL objects used by the application.', @level0type = N'schema', @level0name = [aud];
go

create schema [int] authorization dbo;
go

exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Objects needed for ETL integration.', @level0type = N'schema', @level0name = [int];
go

create schema [app] authorization dbo;
go

exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Application related items.', @level0type = N'schema', @level0name = [app];
go

--grant alter on schema::int to [us\us-svcpmteapprouter];
--grant alter on schema:: int to [us\us-svcciplatformprod];
--go

print formatmessage(N'%4s...creating sequences', N'');
go

create sequence [seq].[AccountId] as int start with 1;
create sequence [seq].[PaymentTypeId] as int start with 1;
create sequence [seq].[PayeeId] as int start with 1;
create sequence [seq].[ExpenseId] as int start with 1;
create sequence [seq].[UserId] as int start with 1;
--create sequence [seq].[WorkshopPotentialDateId] as int start with 1;
--create sequence [seq].[StageId] as int start with 1;
--create sequence [seq].[WorkshopTypeId] as int start with 1;
--create sequence [seq].[SurveyDataTypeId] as int start with 1;
--create sequence [seq].[SurveyCommentId] as int start with 1;
--create sequence [seq].[NotificationTemplateId] as int start with 1;
--create sequence [seq].[SurveyCommentCategoryId] as int start with 1;
--create sequence [seq].[WorkshopTopicId] as int start with 1;
--create sequence [seq].[ConceptToolId] as int start with 1;
--create sequence [seq].[SpecialistTypeId] as int start with 1;
--create sequence [seq].[TouchpointId] as int start with 1;
--create sequence [seq].[ContainerId] as int start with 1;
--create sequence [seq].[ContainerEntryId] as int start with 1;
--create sequence [seq].[AttendeeTypeId] as int start with 1;
--create sequence [seq].[PostWorkshopFormId] as int start with 1;
--create sequence [seq].[WhiteGloveInteractionId] as int start with 1;
--go

print formatmessage(N'%4s...creating table [dbo].[__Version]', N'');
go

create table [dbo].[__Version]
(
	[Major] int not null
	, [Minor] int not null
	, [Build] int not null
	, [Revision] int not null
	, [AppliedOn] datetimeoffset(7) not null
		constraint [DF_Dbo__Version] default(sysdatetimeoffset())		
	, [Comment] nvarchar(max) null
	, constraint [PK_Dbo__Version] primary key clustered ([Major], [Minor], [Build], [Revision]) with (ignore_dup_key = off)	
) on [primary];
go

print formatmessage(N'%4s...creating table [app].[User]', N'');
go

create table [app].[User]
(
	[Id] int not null
		constraint [DF_App_User] default(next value for [seq].[UserId])
		constraint [PK_App_User] primary key clustered ([Id]) with (ignore_dup_key = off)
	, [FirstName] nvarchar(128) not null
	, [LastName] nvarchar(128) not null
	, [EmailAddress] nvarchar(255) not null
	, [EmployeeId] bigint not null
	, [SystemName] nvarchar(100) not null
	, [Domain] nvarchar(20) not null
	, [Enabled] bit not null
	, [ValidFrom] datetime2(7) generated always as row start
    , [ValidTo] datetime2(7) generated always as row end
    , period for system_time([ValidFrom], [ValidTo])
) on [primary]
with
(
	system_versioning = on (history_table = [app].[UserArchive])
);
go

alter index [IX_UserArchive] ON [app].[UserArchive] rebuild partition = all with (data_compression = none);
go

create index [IX_Aud_User_Enabled] on [app].[User]([Enabled]);
go

print formatmessage(N'%4s...creating table [aud].[Account]', N'');
go

create table [aud].[Account] 
(
	[Id] int not null
		constraint [DF_Aud_Account_Id] default(next value for [seq].[AccountId])
		constraint [PK_Aud_Account] primary key clustered ([Id]) with (ignore_dup_key = off)
	, [Name] nvarchar(100) not null
	, [Flags] int not null constraint [DF_Aud_Account_Flags] default(0x0)
	--, constraint [UQ0_Account] unique nonclustered ([Number]) with (ignore_dup_key = off)
	, constraint [CK_Aud_Aud_Account] check([Flags] >= 0x0)
) on [primary];
go

-- Add the extended properties
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Contains a record or statement of financial expenditure or receipts relating to a particular period or purpose.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = null, @level2name = null;
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'An identification number for an account.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'column', @level2name = [Id];
--exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Default constaint creates a unique value of type uniqueidentifier.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'constraint', @level2name = [DF0_Account];
--exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Number of an account.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'column', @level2name = [Number];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Name of an account.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'column', @level2name = [Name];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Flags of the account.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'column', @level2name = [Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Default constraint value of 0x0.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'constraint', @level2name = [DF_Aud_Account_Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Primary key (clustered) constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'constraint', @level2name = [PK_Aud_Account];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Clustered index created by the primary key constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'index', @level2name = [PK_Aud_Account];
--exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Unique (nonclustered) constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'constraint', @level2name = [UQ0_Account];
--exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Nonclustered index created by the unique constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'index', @level2name = [UQ0_Account];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Check constraint [AccountFlags] >= 0x0.  Since the flags are signed integers, we will enforce a value of >= 0 for the flags.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Account], @level2type = N'constraint', @level2name = [CK_Aud_Aud_Account];
go

print formatmessage(N'%4s...creating table [app].[PaymentType]', N'');
go

create table [app].[PaymentType] 
(
	[Id] int not null
		constraint [DF_App_PaymentType_Id] default(next value for [seq].[PaymentTypeId])
		constraint [PK_App_PaymentType] primary key clustered ([Id]) with (ignore_dup_key = off)
	, [Name] nvarchar(100) not null
	, [Flags] int not null constraint [DF_App_PaymentType_Flags] default(0x0)
	, constraint [CK_App_App_PaymentType] check([Flags] >= 0x0)
) on [primary];
go

exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'Contains the payment types', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = null, @level2name = null;
exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'An identification number for an account.', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = N'column', @level2name = [Id];
exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'Name of an account.', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = N'column', @level2name = [Name];
exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'Flags of the account.', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = N'column', @level2name = [Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'Default constraint value of 0x0.', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = N'constraint', @level2name = [DF_App_PaymentType_Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'Primary key (clustered) constraint.', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = N'constraint', @level2name = [PK_App_PaymentType];
exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'Clustered index created by the primary key constraint.', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = N'index', @level2name = [PK_App_PaymentType];
exec [sys].[sp_addextendedproperty] @name = N'ZionAppiting_Description', @value = N'Check constraint [AccountFlags] >= 0x0.  Since the flags are signed integers, we will enforce a value of >= 0 for the flags.', @level0type = N'schema', @level0name = [app], @level1type = N'table', @level1name = [PaymentType], @level2type = N'constraint', @level2name = [CK_App_App_PaymentType];
go

print formatmessage(N'%4s...creating table [aud].[Payee]', N'');
go

create table [aud].[Payee] 
(
	[Id] int not null
		constraint [DF_Aud_Payee_Id] default(next value for [seq].[PayeeId])
		constraint [PK_Aud_Payee] primary key clustered ([Id]) with (ignore_dup_key = off)
	, [Name] nvarchar(100) not null
	, [Flags] int not null constraint [DF_Aud_Payee_Flags] default(0x0)
	, constraint [CK_Aud_Payee_Flags] check([Flags] >= 0x0)
) on [primary];
go

exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Contains the payees', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = null, @level2name = null;
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'An identification number for a payee.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = N'column', @level2name = [Id];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Name of a payee.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = N'column', @level2name = [Name];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Flags of the payee.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = N'column', @level2name = [Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Default constraint value of 0x0.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = N'constraint', @level2name = [DF_Aud_Payee_Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Primary key (clustered) constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = N'constraint', @level2name = [PK_Aud_Payee];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Clustered index created by the primary key constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = N'index', @level2name = [PK_Aud_Payee];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Check constraint [AccountFlags] >= 0x0.  Since the flags are signed integers, we will enforce a value of >= 0 for the flags.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Payee], @level2type = N'constraint', @level2name = [CK_Aud_Payee_Flags];
go

print formatmessage(N'%4s...creating table [aud].[Expense]', N'');
go

create table [aud].[Expense] 
(
	[Id] int not null
		constraint [DF_Aud_Expense_Id]
			default(next value for [seq].[ExpenseId])
		constraint [PK_Aud_Expense]
			primary key clustered ([Id]) with (ignore_dup_key = off)
	, [AccountId] int not null
		constraint [FK_Aud_Expense_Aud_Account_AccountId] 
			foreign key ([AccountId]) references [aud].[Account]([Id]) not for replication	
	, [RequestedBy] nvarchar(100) not null
	, [Amount] numeric(8,2) not null
	, [PayeeId] int not null
		constraint [FK_Aud_Expense_Aud_Payee_PayeeId] 
			foreign key ([PayeeId]) references [aud].[Payee]([Id]) not for replication
	, [PaymentTypeId] int not null
		constraint [FK_Aud_Expense_App_PaymentType_PaymentTypeId] 
			foreign key ([PaymentTypeId]) references [app].[PaymentType]([Id]) not for replication
	, [Description] nvarchar(255) not null
	, [Flags] int not null
		constraint [DF_Aud_Expense_Flags]
			default(0x0)
		constraint [CK_Aud_Expense_Flags]
			check([Flags] >= 0x0)
	, [FiledAt] datetimeoffset(7) not null constraint [DF_Aud_Expense_FiledAt] default(sysdatetimeoffset())
	, [CreatedBy] int not null
		constraint [FK_Aud_Expense_App_User_CreatedBy] foreign key ([CreatedBy]) references [app].[User]([Id]) not for replication
	, [CreatedAt] datetimeoffset(7) not null
		constraint [DF_Aud_Expense_CreatedAt] default(sysdatetimeoffset())
	, [ModifiedBy] int null
		constraint [FK_Aud_Expense_App_User_ModifiedBy] foreign key ([ModifiedBy]) references [app].[User]([Id]) not for replication
	, [ModifiedAt] datetimeoffset(7) null
	, [ValidFrom] datetime2(7) generated always as row start
    , [ValidTo] datetime2(7) generated always as row end
    , period for system_time([ValidFrom], [ValidTo])
) on [primary]
with
(
	system_versioning = on (history_table = [aud].[ExpenseArchive])
);
go

alter index [IX_ExpenseArchive] ON [aud].[ExpenseArchive] rebuild partition = all with (data_compression = none);
go

create index [IX_Aud_Expense_AccountId] on [aud].[Expense]([AccountId]);
create index [IX_Aud_Expense_PayeeId] on [aud].[Expense]([PayeeId]);
create index [IX_Aud_Expense_PaymentTypeId] on [aud].[Expense]([PaymentTypeId]);
create index [IX_Aud_Expense_Flags] on [aud].[Expense]([Flags]);
create index [IX_Aud_Expense_CreatedBy] on [aud].[Expense]([CreatedBy]);
create index [IX_Aud_Expense_ModifiedBy] on [aud].[Expense]([ModifiedBy]);
go

-- Add the extended properties
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Contains the money spent on something.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = null, @level2name = null;
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'An identification number for an account.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = N'column', @level2name = [Id];
--exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Default constaint creates a unique value of type uniqueidentifier.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = N'constraint', @level2name = [DF0_Expense];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Flags of the expenses.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = N'column', @level2name = [Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Default constraint value of 0x0.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = N'constraint', @level2name = [DF_Aud_Expense_Flags];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Primary key (clustered) constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = N'constraint', @level2name = [PK_Aud_Expense];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Clustered index created by the primary key constraint.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = N'index', @level2name = [PK_Aud_Expense];
exec [sys].[sp_addextendedproperty] @name = N'ZionAuditing_Description', @value = N'Check constraint [ExpenseFlags] >= 0x0.  Since the flags are signed integers, we will enforce a value of >= 0 for the flags.', @level0type = N'schema', @level0name = [aud], @level1type = N'table', @level1name = [Expense], @level2type = N'constraint', @level2name = [CK_Aud_Expense_Flags];
go

set nocount on;
go

insert [aud].[Account]([Id], [Name]) values (1000, N'Groceries'), (2000, N'Dining'), (3000, N'Fuel'), (4000, N'Car'), (5000, N'Other'), (6000, N'Home');
insert [app].[PaymentType]([Name]) values (N'credit'), (N'check');
go