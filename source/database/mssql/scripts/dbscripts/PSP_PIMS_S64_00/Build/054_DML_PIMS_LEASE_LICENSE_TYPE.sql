/* -----------------------------------------------------------------------------
Delete all data from the PIMS_LEASE_LICENSE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_LEASE_LICENSE_TYPE
GO

INSERT INTO PIMS_LEASE_LICENSE_TYPE (LEASE_LICENSE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'LSREG',     N'Lease - Registered',                CONVERT([bit],(0))),
  (N'LSUNREG',   N'Lease - Unregistered',              CONVERT([bit],(0))),
  (N'LSGRND',    N'Ground Lease',                      CONVERT([bit],(0))),
  (N'LIOCCTTLD', N'License of Occupation (titled)',    CONVERT([bit],(0))),
  (N'LIOCCUSE',  N'License of Occupation (use)',       CONVERT([bit],(0))),
  (N'LIOCCACCS', N'License of Occupation (access)',    CONVERT([bit],(0))),
  (N'LIOCCUTIL', N'License of Occupation (utilities)', CONVERT([bit],(0))),
  (N'LICONSTRC', N'License to Construct',              CONVERT([bit],(1))),
  (N'LIPPUBHWY', N'License of Prov Public Highway',    CONVERT([bit],(0))),
  (N'RESLNDTEN', N'Residential Tenancy Act',           CONVERT([bit],(0))),
  (N'LIMOTIPRJ', N'MOTI Project Use License',          CONVERT([bit],(0))),
  (N'MANUFHOME', N'Manufactured Home Act',             CONVERT([bit],(0))),
  (N'ROADXING',  N'Road Crossing',                     CONVERT([bit],(0))),
  (N'OTHER',     N'Other',                             CONVERT([bit],(0)));
GO
