/* -----------------------------------------------------------------------------
Disable existing codes in the PIMS_PROPERTY_TENURE_TYPE table and add new codes.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Mar-08  Initial version
----------------------------------------------------------------------------- */

BEGIN TRANSACTION;

-- Disable the existing code values
UPDATE PIMS_PROPERTY_TENURE_TYPE
SET    IS_DISABLED                = CONVERT([bit],(1))
     , CONCURRENCY_CONTROL_NUMBER = CONCURRENCY_CONTROL_NUMBER + 1;

IF @@ROWCOUNT = 0
  BEGIN
  INSERT INTO PIMS_PROPERTY_TENURE_TYPE (PROPERTY_TENURE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
    VALUES
	  (N'PL',       N'Payable Contract',   CONVERT([bit],(1))),
	  (N'TM',       N'Titled Land - MoTI', CONVERT([bit],(1))),
	  (N'CL',       N'Crown Land Reserve', CONVERT([bit],(1))),
	  (N'TT',       N'Titled Land - TFA',  CONVERT([bit],(1))),
	  (N'RW',       N'Right of Way',       CONVERT([bit],(1))),
	  (N'CR',       N'Closed Road',        CONVERT([bit],(1)));
  END

-- Insert the new code values
INSERT INTO PIMS_PROPERTY_TENURE_TYPE (PROPERTY_TENURE_TYPE_CODE, DESCRIPTION, IS_DISABLED)
  VALUES
    (N'HWYROAD',  N'Highway/Road',       CONVERT([bit],(0))),
	(N'ADJLAND',  N'Adjacent Land',      CONVERT([bit],(0))),
	(N'CLOSEDRD', N'Closed Road',        CONVERT([bit],(0))),
	(N'UNKNOWN',  N'Unknown',            CONVERT([bit],(0)));

COMMIT TRANSACTION;
