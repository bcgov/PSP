/* -----------------------------------------------------------------------------
Insert additional data into the PIMS_SECURITY_DEPOSIT_TYPE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

INSERT INTO PIMS_SECURITY_DEPOSIT_TYPE (SECURITY_DEPOSIT_TYPE_CODE, DESCRIPTION)
VALUES
  (N'OTHER', N'Other deposit');