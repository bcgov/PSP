/* -----------------------------------------------------------------------------
Delete all data from the PIMS_DOCUMENT_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_DOCUMENT_STATUS_TYPE
GO

INSERT INTO PIMS_DOCUMENT_STATUS_TYPE (DOCUMENT_STATUS_TYPE_CODE, DESCRIPTION, IS_DISABLED, DISPLAY_ORDER)
VALUES
  (N'NONE',    N'None',         CONVERT([bit],(0)),   1),
  (N'DRAFT',   N'Draft',        CONVERT([bit],(0)),   2),
  (N'APPROVD', N'Approved',     CONVERT([bit],(0)),   3),
  (N'SIGND',   N'Signed',       CONVERT([bit],(0)),   4),
  (N'FINAL',   N'Final',        CONVERT([bit],(0)),   5),
  (N'AMENDD',  N'Amended',      CONVERT([bit],(0)),   6),
  (N'CNCLD',   N'Cancelled',    CONVERT([bit],(0)),   7),
  (N'SENT',    N'Sent',         CONVERT([bit],(1)), 999),
  (N'RGSTRD',  N'Registered',   CONVERT([bit],(1)), 999),
  (N'UNREGD',  N'Unregistered', CONVERT([bit],(1)), 999);
GO
