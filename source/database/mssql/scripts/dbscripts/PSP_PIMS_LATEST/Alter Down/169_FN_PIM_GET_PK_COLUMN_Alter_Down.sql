/*------------------------------------------------------------------------------
Drop SQL Server function to return the primary key column of the specified table

Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Apr-30  Initial version.
------------------------------------------------------------------------------*/

-- Drop the stored procedure if it already exists.
DROP FUNCTION IF EXISTS [dbo].[PIM_GET_PK_COLUMN]
GO
