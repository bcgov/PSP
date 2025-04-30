/*******************************************************************************
Drop the stored procedure to preserve the user that deleted the record in the 
history table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2025-Apr-30  Original version.
*******************************************************************************/

DROP PROCEDURE IF EXISTS [dbo].[PIM_DELETION_HISTORY]
GO
