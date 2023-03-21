/* -----------------------------------------------------------------------------
Populate the missing code values in the PIMS_TAKE_SITE_CONTAM_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Jan-30  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TAKE_SITE_CONTAM_TYPE
GO

INSERT INTO PIMS_TAKE_SITE_CONTAM_TYPE (TAKE_SITE_CONTAM_TYPE_CODE, DESCRIPTION)
VALUES
  (N'UNK', N'Unknown'),
  (N'YES', N'Yes'),
  (N'NO',  N'No');
GO
