/* -----------------------------------------------------------------------------
Delete all data from the PIMS_SURPLUS_DECLARATION_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_SURPLUS_DECLARATION_TYPE
GO

INSERT INTO PIMS_SURPLUS_DECLARATION_TYPE (SURPLUS_DECLARATION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'YES',     N'Yes'),
  (N'NO',      N'No'),
  (N'EXPIRED', N'Expired'),
  (N'UNKNOWN', N'Unknown');