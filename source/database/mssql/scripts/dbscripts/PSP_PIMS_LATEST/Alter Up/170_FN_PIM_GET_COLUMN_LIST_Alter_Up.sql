/*------------------------------------------------------------------------------
SQL Server function to return the list of non-PK columns in the specified table.

Input Parameters
----------------
SchemaName: Database schema of the history table of interest.
TableName:  Name of the history table of interest.

Output Parameters
-----------------
ColumnLst: The comma-separated list of non-PK columns for the specified table.

Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Apr-04  Initial version.
------------------------------------------------------------------------------*/

-- Drop the stored procedure if it already exists.
DROP FUNCTION IF EXISTS [dbo].[PIM_GET_COLUMN_LIST]
GO

CREATE FUNCTION [dbo].[PIM_GET_COLUMN_LIST] (@prmSchemaName nvarchar(128) = 'dbo', @prmTableName nvarchar(128), @prmPKColName NVARCHAR(128))
RETURNS nvarchar(max)
AS
  BEGIN
  DECLARE @ColumnLst nvarchar(max);
  
  SELECT   @ColumnLst = COALESCE(@ColumnLst + ', ', '') + COLUMN_NAME
  FROM     information_schema.columns
  WHERE    TABLE_SCHEMA = @prmSchemaName
       AND TABLE_NAME   = @prmTableName
       AND COLUMN_NAME <> @prmPKColName
  ORDER BY ORDINAL_POSITION
  
  RETURN @ColumnLst
  END
GO
