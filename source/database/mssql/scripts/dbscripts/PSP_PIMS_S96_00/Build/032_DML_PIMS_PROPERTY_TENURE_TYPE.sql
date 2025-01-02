/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROPERTY_TENURE_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-09  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROPERTY_TENURE_TYPE
GO

INSERT INTO PIMS_PROPERTY_TENURE_TYPE (PROPERTY_TENURE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
VALUES
  (N'PL',        N'Payable Contract',                      CONVERT([bit],(1))),
  (N'TM',        N'Titled Land - MoTI',                    CONVERT([bit],(1))),
  (N'CL',        N'Crown Land Reserve',                    CONVERT([bit],(1))),
  (N'TT',        N'Titled Land - TFA',                     CONVERT([bit],(1))),
  (N'RW',        N'Right of Way',                          CONVERT([bit],(1))),
  (N'CR',        N'Closed Road',                           CONVERT([bit],(1))),
  (N'HWYROAD',   N'Highway/Road',                          CONVERT([bit],(0))),
  (N'ADJLAND',   N'Adjacent Land',                         CONVERT([bit],(1))),
  (N'CLOSEDRD',  N'Closed Road',                           CONVERT([bit],(0))),
  (N'UNKNOWN',   N'Unknown',                               CONVERT([bit],(0))),
  (N'FSMOTI',    N'Fee Simple - MoTI',                     CONVERT([bit],(0))),
  (N'FSBCTFA',   N'Fee Simple - BCTFA',                    CONVERT([bit],(0))),
  (N'FSPRIVAT',  N'Fee Simple - Private',                  CONVERT([bit],(0))),
  (N'FSCROWN',   N'Fee Simple - Crown (Non-MoTI)',         CONVERT([bit],(0))),
  (N'NSRWMOTI',  N'Non-SRW Interests - MoTI',              CONVERT([bit],(0))),
  (N'NSRWBCTFA', N'Non-SRW Interests - BCTFA',             CONVERT([bit],(0))),
  (N'SRWMOTI',   N'Statutory Right of Way (SRW) - MoTI',   CONVERT([bit],(0))),
  (N'SRWBCTFA',  N'Statutory Right of Way (SRW) - BCTFA',  CONVERT([bit],(0))),
  (N'SRWOTHER',  N'Statutory Right of Way (SRW) - Other',  CONVERT([bit],(0))),
  (N'LNDACTR',   N'Land Act Reserve (Section 16/17/12/14', CONVERT([bit],(0))),
  (N'SPECUPMT',  N'Special Use Permit (SUP)',              CONVERT([bit],(0))),
  (N'IRESERVE',  N'Indian Reserve (IR)',                   CONVERT([bit],(0))),
  (N'UNCRWNLMD', N'Unsurveyed Crown Land',                 CONVERT([bit],(0))),
  (N'LEASELIC',  N'Leased/Licensed',                       CONVERT([bit],(0)));
GO
