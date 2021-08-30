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
  (N'MAILADDR', N'Mailing Address'),
  (N'INSURE', N'Proof of Insurance Address'),
  (N'RENTAL', N'Rental Payment Address'),
  (N'PROPNOTIFY', N'Property Notification Address'),
  (N'PHYSADDR', N'Physical Address');