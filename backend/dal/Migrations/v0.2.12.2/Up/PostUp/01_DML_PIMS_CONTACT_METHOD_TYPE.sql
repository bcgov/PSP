/* -----------------------------------------------------------------------------
Delete all data from the PIMS_CONTACT_METHOD_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

INSERT INTO PIMS_CONTACT_METHOD_TYPE (CONTACT_METHOD_TYPE_CODE, DESCRIPTION)
VALUES
  (N'PERSMOBIL', N'Personal mobile phone'),
  (N'WORKMOBIL', N'Work mobile phone')