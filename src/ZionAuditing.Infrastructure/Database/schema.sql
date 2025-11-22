-- Create AuditLogs table
CREATE TABLE AuditLogs (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    EntityName NVARCHAR(255) NOT NULL,
    EntityId UNIQUEIDENTIFIER NOT NULL,
    UserId NVARCHAR(450) NOT NULL,
    Action NVARCHAR(50) NOT NULL,
    OldValues NVARCHAR(MAX) NULL,
    NewValues NVARCHAR(MAX) NULL,
    Timestamp DATETIME2 NOT NULL,
    IpAddress NVARCHAR(45) NULL,
    UserAgent NVARCHAR(500) NULL,

    INDEX IX_AuditLogs_EntityId (EntityId),
    INDEX IX_AuditLogs_UserId (UserId),
    INDEX IX_AuditLogs_Timestamp (Timestamp DESC),
    INDEX IX_AuditLogs_EntityName (EntityName)
);

-- Create AuditEntries table
CREATE TABLE AuditEntries (
    Id UNIQUEIDENTIFIER PRIMARY KEY,
    AuditLogId UNIQUEIDENTIFIER NOT NULL,
    PropertyName NVARCHAR(255) NOT NULL,
    OldValue NVARCHAR(MAX) NULL,
    NewValue NVARCHAR(MAX) NULL,

    CONSTRAINT FK_AuditEntries_AuditLogs
        FOREIGN KEY (AuditLogId)
        REFERENCES AuditLogs(Id)
        ON DELETE CASCADE,

    INDEX IX_AuditEntries_AuditLogId (AuditLogId)
);
