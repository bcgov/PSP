/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ADDRESS_USAGE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ADDRESS_USAGE_TYPE
GO

INSERT INTO PIMS_ADDRESS_USAGE_TYPE (ADDRESS_USAGE_TYPE_CODE, DESCRIPTION)
VALUES
  (N'MAILADDR', N'Mailing address'),
  (N'INSURE', N'Proof of insurance address'),
  (N'RENTAL', N'Rental payment address'),
  (N'PROPNOTIFY', N'Property notification address'),
  (N'PHYSADDR', N'Physical address'),
  (N'UNKNOWN', N'Unknown address type');