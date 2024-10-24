/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACQUISITION_FILE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACQUISITION_FILE_STATUS_TYPE
GO

INSERT INTO PIMS_ACQUISITION_FILE_STATUS_TYPE (ACQUISITION_FILE_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'ACTIVE', N'Active',    CONVERT([bit],(0))),
  (N'CANCEL', N'Cancelled', CONVERT([bit],(0))),
  (N'CLOSED', N'Closed',    CONVERT([bit],(1))),
  (N'ARCHIV', N'Archived',  CONVERT([bit],(0))),
  (N'DRAFT',  N'Draft',     CONVERT([bit],(0))),
  (N'HOLD',   N'Hold',      CONVERT([bit],(0))),
  (N'COMPLT', N'Complete',  CONVERT([bit],(0)));
GO
