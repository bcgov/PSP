/* -----------------------------------------------------------------------------
Populate the PIMS_DISPOSITION_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2023-Nov-21  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DISPOSITION_TYPE
GO

INSERT INTO PIMS_DISPOSITION_TYPE (DISPOSITION_TYPE_CODE, DESCRIPTION)
VALUES
  (N'OPEN',    N'Open Market Sale'),
  (N'DIRECT',  N'Direct Sale'),
  (N'CLOSURE', N'Road Closure'),
  (N'OTHER',   N'Other Transfer');
GO
