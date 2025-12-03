/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_STATUS_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_ANOMALY_TYPE
GO

INSERT INTO PIMS_PROPERTY_ANOMALY_TYPE (PROPERTY_ANOMALY_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'ACCESS',     N'Access',                              CONVERT([bit],(0))),
  (N'BIZLOSS',    N'Potential for business loss claims',  CONVERT([bit],(0))),
  (N'EFCLAUSE',   N'E&F clause',                          CONVERT([bit],(0))),
  (N'BLDGLIENS',  N'Building liens',                      CONVERT([bit],(0))),
  (N'DISTURB',    N'Disturbance',                         CONVERT([bit],(0))),
  (N'DUPTITLE',   N'Duplicate title',                     CONVERT([bit],(0))),
  (N'ASSGNRENT',  N'Assignment of rent',                  CONVERT([bit],(0))),
  (N'MORTSECINT', N'Mortgage/security interests',         CONVERT([bit],(0))),
  (N'CHRGCROWN',  N'Charge to the Crown',                 CONVERT([bit],(0))),
  (N'CERTPNDLIT', N'Certification of pending litigation', CONVERT([bit],(0))),
  (N'CHGHOLDGEN', N'Charge holders in general',           CONVERT([bit],(0))),
  (N'LGLNOT',     N'Legal notations',                     CONVERT([bit],(0))),
  (N'OTHER',      N'Other',                               CONVERT([bit],(0)));
GO
