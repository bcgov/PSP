/*******************************************************************************
Stored procedure to remove all objects in the provided schema.  The schema will 
be dropped unless it's a standard schema such as 'dbo'.

Parameter     Description
------------  -----------------------------------------------------------------
prmSchemaNm   The name of the database schema to be removed.
prmDebugMode  If false (default), the generated SQL is executed, otherwise the SQL 
              is printed.              
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Oct-27  Original version.
*******************************************************************************/

DROP PROCEDURE IF EXISTS [dbo].[PIM_DROP_SCHEMA]
GO

CREATE PROCEDURE [dbo].[PIM_DROP_SCHEMA] @prmSchemaNm  nvarchar(128),
                                         @prmDebugMode bit = 0
AS
BEGIN
  BEGIN TRANSACTION;
  SET XACT_ABORT ON;
  SET NOCOUNT OFF;
  
  DECLARE @sql NVARCHAR(MAX) = '';
  
  BEGIN TRY  
    -- Drop Foreign Key Constraints
    SELECT @sql += 'ALTER TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_object_id)) + '.' + QUOTENAME(OBJECT_NAME(parent_object_id)) + ' DROP CONSTRAINT ' + QUOTENAME(name) + ';' + CHAR(13) + CHAR(10)
    FROM   sys.foreign_keys
    WHERE  OBJECT_SCHEMA_NAME(parent_object_id) = @prmSchemaNm;
    
    -- Drop Triggers
    SELECT @sql += 'DROP TRIGGER ' + QUOTENAME(OBJECT_SCHEMA_NAME(parent_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13) + CHAR(10)
    FROM   sys.triggers
    WHERE  OBJECT_SCHEMA_NAME(parent_id) = @prmSchemaNm;
    
    -- Drop Views
    SELECT @sql += 'DROP VIEW ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13) + CHAR(10)
    FROM   sys.views
    WHERE  OBJECT_SCHEMA_NAME(object_id) = @prmSchemaNm;
    
    -- Drop Stored Procedures and Functions
    SELECT @sql += 'DROP ' + CASE WHEN type = 'P' THEN 'PROCEDURE' WHEN type IN ('FN', 'IF', 'TF') THEN 'FUNCTION' END + ' ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13) + CHAR(10)
    FROM   sys.objects
    WHERE  OBJECT_SCHEMA_NAME(object_id) = @prmSchemaNm AND type IN ('P', 'FN', 'IF', 'TF');
    
    -- Drop Tables
    SELECT @sql += 'DROP TABLE ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13) + CHAR(10)
    FROM   sys.tables
    WHERE  OBJECT_SCHEMA_NAME(object_id) = @prmSchemaNm;
    
    -- Drop Sequences
    SELECT @sql += 'DROP SEQUENCE ' + QUOTENAME(OBJECT_SCHEMA_NAME(object_id)) + '.' + QUOTENAME(name) + ';' + CHAR(13) + CHAR(10)
    FROM   sys.sequences
    WHERE  OBJECT_SCHEMA_NAME(object_id) = @prmSchemaNm;
    
    -- Drop the Schema if not 'dbo'
    IF @prmSchemaNm <> 'dbo'
      SET @sql += 'DROP SCHEMA ' + QUOTENAME(@prmSchemaNm) + ';' + CHAR(13) + CHAR(10);
    
    -- If not in debug mode, execute the query.  Otherwise display the values of 
    -- the internal variables. 
    IF @prmDebugMode = 0
      BEGIN
      EXEC sp_executesql @sql;
      END
    ELSE
      BEGIN
      PRINT '@sql = ' + @sql;
      END;
    
    COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF (XACT_STATE()) = -1
      ROLLBACK TRANSACTION;
  END CATCH
END;
