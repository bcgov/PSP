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
  (N'PL',       N'Payable Contract',   CONVERT([bit],(1))),
  (N'TM',       N'Titled Land - MoTI', CONVERT([bit],(1))),
  (N'CL',       N'Crown Land Reserve', CONVERT([bit],(1))),
  (N'TT',       N'Titled Land - TFA',  CONVERT([bit],(1))),
  (N'RW',       N'Right of Way',       CONVERT([bit],(1))),
  (N'CR',       N'Closed Road',        CONVERT([bit],(1))),
  (N'HWYROAD',  N'Highway/Road',       CONVERT([bit],(0))),
  (N'ADJLAND',  N'Adjacent Land',      CONVERT([bit],(0))),
  (N'CLOSEDRD', N'Closed Road',        CONVERT([bit],(0))),
  (N'UNKNOWN',  N'Unknown',            CONVERT([bit],(0)));
GO
