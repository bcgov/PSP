-- =============================================
-- Author:		Ben Driver
-- Create date: 2019-11-12
-- Updates:     Doug Filteau
--              Adapted for MoTI-PIMS
--
-- Description:	Generic error handling called by RDBMS enforced audit field triggers and concurency control number checking.
-- =============================================

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


-- --------------------------------------------------------------------------------------------
-- Drop procedure pims_error_handling
-- --------------------------------------------------------------------------------------------
IF OBJECT_ID ( 'pims_error_handling', 'P' ) IS NOT NULL
  drop PROCEDURE dbo.pims_error_handling;
go
-- ............................................................................................


-- --------------------------------------------------------------------------------------------
-- Create procedure pims_error_handling
-- --------------------------------------------------------------------------------------------
CREATE PROCEDURE [dbo].[pims_error_handling] AS
  begin
    DECLARE @errmsg   nvarchar(2048),
      @severity tinyint,
      @state    tinyint,
      @errno    int,
      @proc     sysname,
      @lineno   int

    SELECT @errmsg = error_message(), @severity = error_severity(),
      @state  = error_state(), @errno = error_number(),
      @proc   = error_procedure(), @lineno = error_line()

    IF @errmsg NOT LIKE '***%'
      BEGIN
        SELECT @errmsg = '*** ' + coalesce(quotename(@proc), '<dynamic SQL>') +
          ', Line ' + ltrim(str(@lineno)) + '. Errno ' +
          ltrim(str(@errno)) + ': ' + @errmsg
      END

    RAISERROR('%s', @severity, @state, @errmsg)
  end
GO
-- ............................................................................................
