/*------------------------------------------------------------------------------
Drop SQL Server function to return the list of non-PK columns in the specified 
table.

Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Apr-30  Initial version.
------------------------------------------------------------------------------*/

-- Drop the stored procedure if it already exists.
DROP FUNCTION IF EXISTS [dbo].[PIM_GET_COLUMN_LIST]
GO
