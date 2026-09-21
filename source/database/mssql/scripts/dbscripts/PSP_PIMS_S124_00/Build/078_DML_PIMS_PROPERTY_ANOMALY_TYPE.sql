/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_ANOMALY_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
Doug Filteau  2025-Oct-08  Added NOACCESS
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_ANOMALY_TYPE
GO

INSERT INTO PIMS_PROPERTY_ANOMALY_TYPE (PROPERTY_ANOMALY_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACCESS',     N'Access'),
  (N'BIZLOSS',    N'Potential for business loss claims'),
  (N'EFCLAUSE',   N'E&F clause'),
  (N'BLDGLIENS',  N'Building liens'),
  (N'DISTURB',    N'Disturbance'),
  (N'DUPTITLE',   N'Duplicate title'),
  (N'ASSGNRENT',  N'Assignment of rent'),
  (N'MORTSECINT', N'Mortgage/security interests'),
  (N'CHRGCROWN',  N'Charge to the Crown'),
  (N'CERTPNDLIT', N'Certification of pending litigation'),
  (N'CHGHOLDGEN', N'Charge holders in general'),
  (N'LGLNOT',     N'Legal notations'),
  (N'OTHER',      N'Other'),
  (N'NOACCESS',   N'No access');
GO

-- --------------------------------------------------------------
-- Update the display order.
-- --------------------------------------------------------------
UPDATE biz
SET    biz.DISPLAY_ORDER              = seq.ROW_NUM
     , biz.CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1
FROM   PIMS_PROPERTY_ANOMALY_TYPE biz JOIN
       (SELECT PROPERTY_ANOMALY_TYPE_CODE
             , ROW_NUMBER() OVER (ORDER BY DESCRIPTION) AS ROW_NUM
        FROM   PIMS_PROPERTY_ANOMALY_TYPE) seq  ON seq.PROPERTY_ANOMALY_TYPE_CODE = biz.PROPERTY_ANOMALY_TYPE_CODE
GO
