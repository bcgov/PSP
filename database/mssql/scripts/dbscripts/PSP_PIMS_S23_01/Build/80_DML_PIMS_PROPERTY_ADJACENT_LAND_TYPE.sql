/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_ADJACENT_LAND_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_ADJACENT_LAND_TYPE
GO

INSERT INTO PIMS_PROPERTY_ADJACENT_LAND_TYPE (PROPERTY_ADJACENT_LAND_TYPE_CODE, DESCRIPTION)
VALUES
  (N'MOLBCTFA',  N'Ministry owned and leased - BCTFA'),
  (N'MOLHMQ',    N'Ministry owned and leased - HMQ'),
  (N'MONLBCTFA', N'Ministry owned not leases - BCTFA'),
  (N'MONLHMQ',   N'Ministry owned not leases - HMQ'),
  (N'LANDACTR',  N'Land Act Reserve (Section 16/17/12/14)'),
  (N'INDIANR',   N'Indian Reserve (IR)'),
  (N'PRIVATE',   N'Private (Fee Simple)'),
  (N'STATROW',   N'Statutory Right of Way (SRW)'),
  (N'SPECUSE',   N'Special Use Permit (SUP)'),
  (N'CROWN',     N'Crown');
GO
