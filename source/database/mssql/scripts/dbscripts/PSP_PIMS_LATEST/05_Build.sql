/* ---------------------------------------------------------------------- */
/* Script generated with: DeZign for Databases 14.5.1                     */
/* Target DBMS:           MS SQL Server 2017                              */
/* Project file:          Temporal_Test.dez                               */
/* Project name:          Temporal_Test                                   */
/* Author:                                                                */
/* Script type:           Database creation script                        */
/* Created on:            2024-10-23 15:51                                */
/* ---------------------------------------------------------------------- */

CREATE TABLE [TST] (
    [PK] BIGINT NOT NULL,
    [NUM] INTEGER NOT NULL,
    [DESCRIPTION] NVARCHAR(40),
    [HUBBA_HUBBA] NVARCHAR(max),
    [HUBBA_BUBBA] VARBINARY(max),
    [ROW_VERSION] rowversion NOT NULL,
    CONSTRAINT [PK_TST] PRIMARY KEY CLUSTERED ([PK])
)
GO

CREATE TABLE [Child] (
    [PK] BIGINT NOT NULL,
    [TST_PK] BIGINT NOT NULL,
    [NUM] INTEGER,
    [DESCRIPTION] VARCHAR(40),
    [ROW_VERSION] rowversion NOT NULL,
    CONSTRAINT [PK_Child] PRIMARY KEY CLUSTERED ([PK])
)
GO

ALTER TABLE [Child] ADD CONSTRAINT [TST_Child] 
    FOREIGN KEY ([TST_PK]) REFERENCES [TST] ([PK]) ON DELETE CASCADE
GO
