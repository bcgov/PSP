/* -----------------------------------------------------------------------------
Insert data into the PIMS_ACTIVITY_TEMPLATE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Sep-30  Initial version
Doug Filteau  2022-Oct-31  Added 'File Document'
----------------------------------------------------------------------------- */

INSERT INTO PIMS_ACTIVITY_TEMPLATE (ACTIVITY_TEMPLATE_TYPE_CODE, IS_DISABLED, CONCURRENCY_CONTROL_NUMBER)
VALUES
  (N'GENLTR',    CONVERT([bit],(0)), 1),
  (N'NOTENTRY',  CONVERT([bit],(0)), 1),
  (N'CONDENTRY', CONVERT([bit],(0)), 1),
  (N'RECNEGOT',  CONVERT([bit],(0)), 1),
  (N'CONSULT',   CONVERT([bit],(0)), 1),
  (N'RECTAKES',  CONVERT([bit],(0)), 1),
  (N'OFFAGREE',  CONVERT([bit],(0)), 1),
  (N'COMPREQ',   CONVERT([bit],(0)), 1);
GO
