/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ACTIVITY_TEMPLATE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_ACTIVITY_TEMPLATE
GO

INSERT INTO PIMS_ACTIVITY_TEMPLATE (ACTIVITY_TEMPLATE_TYPE_CODE, IS_DISABLED, CONCURRENCY_CONTROL_NUMBER)
VALUES
  (N'GENERAL',   CONVERT([bit],(0)), 1),
  (N'SURVEY',    CONVERT([bit],(1)), 1),
  (N'SITEVIS',   CONVERT([bit],(0)), 1),
  (N'GENLTR',    CONVERT([bit],(0)), 1),
  (N'FILEDOC',   CONVERT([bit],(1)), 1),
  (N'NOTENTRY',  CONVERT([bit],(0)), 1),
  (N'CONDENTRY', CONVERT([bit],(0)), 1),
  (N'RECNEGOT',  CONVERT([bit],(0)), 1),
  (N'CONSULT',   CONVERT([bit],(0)), 1),
  (N'RECTAKES',  CONVERT([bit],(0)), 1),
  (N'OFFAGREE',  CONVERT([bit],(0)), 1),
  (N'COMPREQ',   CONVERT([bit],(0)), 1);
GO
