/* -----------------------------------------------------------------------------
Delete all data from the PIMS_CONTACT_METHOD_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_CONTACT_METHOD_TYPE
GO

INSERT INTO PIMS_CONTACT_METHOD_TYPE (CONTACT_METHOD_TYPE_CODE, DESCRIPTION)
VALUES
  (N'FAX', N'Facsimile machine'),
  (N'PERSMOBIL', N'Personal mobile phone'),
  (N'PERSPHONE', N'Personal phone'),
  (N'WORKMOBIL', N'Work mobile phone'),
  (N'WORKPHONE', N'Work phone'),
  (N'PERSEMAIL', N'Personal email'),
  (N'WORKEMAIL', N'Work email');