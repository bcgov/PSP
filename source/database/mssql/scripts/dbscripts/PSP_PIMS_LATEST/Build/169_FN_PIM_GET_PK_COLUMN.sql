/*------------------------------------------------------------------------------
SQL Server function to return the primary key column of the specified table

Input Parameters
----------------
SchemaName: Database schema of the history table of interest.
TableName:  Name of the history table of interest.

Output Parameters
-----------------
PKColNm: The name of the primary key column for the table.

Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Feb-27  Initial version.
------------------------------------------------------------------------------*/

-- Drop the stored procedure if it already exists.
DROP FUNCTION IF EXISTS [dbo].[PIM_GET_PK_COLUMN]
GO

CREATE FUNCTION [dbo].[PIM_GET_PK_COLUMN] (@SchemaName nvarchar(128), @TableName nvarchar(128))
RETURNS nvarchar(128)
AS
  BEGIN
  DECLARE @Result   nvarchar(128)

  -- Retrieve the name of the primary key column from the specified table.
  SET @Result = (SELECT COLUMN_NAME
                 FROM   information_schema.key_column_usage
                 WHERE  OBJECTPROPERTY(OBJECT_ID(CONSTRAINT_SCHEMA + '.' + QUOTENAME(CONSTRAINT_NAME)), 'IsPrimaryKey') = 1
                    AND TABLE_SCHEMA = @SchemaName
                    AND TABLE_NAME   = @TableName)

  RETURN @Result 
  END
GO
