/*******************************************************************************
Stored procedure to preserve the user that deleted the record in the history 
table.

Parameter     Description
------------  -----------------------------------------------------------------
prmUserID     The user ID of the user that deleted the record.
prmHstSchema  The name of the database schema of the history table.
prmBizSchema  The name of the database schema of the business table.
prmBizTblNm   The name of the business table that is being operated upon.  This 
              table must have a corresponding history table.
prmPKValue    The value of the primary key of the record in the business table 
              that is being deleted. 

. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2024-Sep-26  Original version.
Doug Filteau  2025-Mar-25  Further enhancement and debugging capability.
Doug Filteau  2025-Mar-28  Add row cloning to preserve original UserID.
*******************************************************************************/

CREATE OR ALTER PROCEDURE [dbo].[PIM_DELETION_HISTORY] @prmUserID    nvarchar(30),
                                                       @prmHstSchema nvarchar(128) = 'dbo', 
                                                       @prmBizSchema nvarchar(128) = 'dbo', 
                                                       @prmBizTblNm  nvarchar(128), 
                                                       @prmPKValue   bigint,
                                                       @modeDebug    bit = 0
AS
BEGIN
  BEGIN TRANSACTION;
  SET XACT_ABORT ON;
  SET NOCOUNT OFF;
  
  DECLARE @HstPKCol  nvarchar(128); -- Column name of the primary key of the history table.
  DECLARE @BizPKCol  nvarchar(128); -- Column name of the primary key of the business table.
  DECLARE @HstTblNm  nvarchar(128); -- Derived name of the history table.
  DECLARE @ColList   nvarchar(max); -- Non-PK column list of the history table.
  DECLARE @InsList   nvarchar(max); -- Non-PK column list of the values to be 
                                    -- instered into the history table.
  DECLARE @HstPKValu bigint;        -- Value of the newly cloned history row.
  DECLARE @RowCount  integer;       -- Count of rows with specified PK value.
  DECLARE @Msg       nvarchar(50);  -- Generic error message.
  DECLARE @qry       nvarchar(max); -- Dynamic query string.
  
  BEGIN TRY
    -- Initialize the name of the history table.
    IF @prmHstSchema = N'dbo'
      SET @HstTblNm = @prmBizTblNm + '_HIST'
    ELSE
      SET @HstTblNm = @prmBizTblNm;
      
    -- Determine the name of the primary key column in the history table.
    SET @HstPKCol = dbo.PIM_GET_PK_COLUMN (@prmHstSchema, @HstTblNm);
    
    -- Determine the name of the primary key column in the business table.
    SET @BizPKCol = dbo.PIM_GET_PK_COLUMN (@prmBizSchema, @prmBizTblNm);
        
    -- Delete the specified record.
    SET @qry = 'DELETE FROM [' + @prmBizSchema + '].[' + @prmBizTblNm + '] WHERE ' + @BizPKCol + ' = ' + CONVERT(nvarchar, @prmPKValue);
    
    -- If not in debug mode, execute the query.  Otherwise display the values of 
    -- the internal variables. 
    IF @modeDebug = 1
      BEGIN
      PRINT '@qry = ' + @qry;
      END
    ELSE
      BEGIN
      EXEC sp_executesql @qry;
      SELECT @RowCount = @@ROWCOUNT;
      -- If RowCount = 0, the record was not deleted.  Stop execution of the 
      -- stored procedure and return to the caller.
      IF @RowCount = 0
        BEGIN
        SET @Msg = 'Record for ' + @prmPKValue + 'in table ' + @prmBizSchema + '.' + @prmBizTblNm + + ' not deleted.';
        RAISERROR (@Msg, 0, 1) WITH NOWAIT;
        ROLLBACK TRANSACTION;
        RETURN;
        END;
      END;
    
    -- Obtain the list of columns for the history record  
    SET @ColList = [dbo].[PIM_GET_COLUMN_LIST] (@prmHstSchema, @HstTblNm, @HstPKCol);
    
    -- Modify the column list to alter the columns specific to deletion.
    SET @InsList = STUFF(@ColList, CHARINDEX('EFFECTIVE_DATE_HIST, END_DATE_HIST, ', @ColList), LEN('EFFECTIVE_DATE_HIST, END_DATE_HIST, '), 'getutcdate(), getutcdate(), ');  -- Current dates for Effective/End dates
    SET @InsList = STUFF(@InsList, CHARINDEX('APP_LAST_UPDATE_USERID',               @InsList), LEN('APP_LAST_UPDATE_USERID'),               QUOTENAME(@prmUserID, ''''));     -- Userid requestiong the deletion.
    SET @InsList = STUFF(@InsList, CHARINDEX('APP_LAST_UPDATE_USER_DIRECTORY',       @InsList), LEN('APP_LAST_UPDATE_USER_DIRECTORY'),       QUOTENAME('DELETED',  ''''));     -- Indicates this is a deleted row.

    -- If in debug mode, display the values of the internal variables. 
    IF @modeDebug = 1
      BEGIN
      PRINT '@ColList = ' + @ColList;
      PRINT '@InsList = ' + @InsList;  
      END;
    
    -- Clone most recent history row for the specified business PK and populate 
    -- the END_DATE_HIST, APP_LAST_UPDATE_USERID, and APP_LAST_UPDATE_USER_DIRECTORY 
    -- columns of the newly created history row.
    SET @qry =        'WITH cte_Latest_Hist_Row (HIST_ID, BIZ_ID, RANK) AS ';
    SET @qry = @qry + '(SELECT ' + @HstPKCol + ', ' + @BizPKCol + ', RANK() OVER (PARTITION BY ' + @BizPKCol + ' ORDER BY END_DATE_HIST DESC) AS RANK FROM ' + @prmHstSchema + '.' + @HstTblNm + ') ';
    SET @qry = @qry + 'INSERT INTO ' + @prmHstSchema + '.' + @HstTblNm + ' (' + @ColList + ') ';
    SET @qry = @qry + 'SELECT ' + @InsList + ' ';
    SET @qry = @qry + 'FROM ' + @prmHstSchema + '.' + @HstTblNm + ' hst JOIN cte_Latest_Hist_Row cte ON ';
    SET @qry = @qry + 'cte.HIST_ID = hst.' + @HstPKCol + ' ';
    SET @qry = @qry + 'WHERE cte.RANK = 1 AND cte.BIZ_ID ' + ' = ' + CONVERT(nvarchar, @prmPKValue);
    
    -- If not in debug mode, execute the query.  Otherwise display the values of 
    -- the internal variables. 
    IF @modeDebug = 0
      BEGIN
      EXEC sp_executesql @qry;
      END
    ELSE
      BEGIN
      PRINT '@qry = ' + @qry;
      END;
    
    COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF (XACT_STATE()) = -1
      ROLLBACK TRANSACTION;
  END CATCH
END;
GO
