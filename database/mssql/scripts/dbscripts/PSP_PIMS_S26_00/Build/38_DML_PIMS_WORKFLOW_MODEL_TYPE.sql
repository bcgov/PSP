/* -----------------------------------------------------------------------------
Delete all data from the PIMS_WORKFLOW_MODEL_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_WORKFLOW_MODEL_TYPE
GO

INSERT INTO PIMS_WORKFLOW_MODEL_TYPE (WORKFLOW_MODEL_TYPE_CODE, DESCRIPTION)
VALUES
  (N'ACQUIRE', N'Property Acquisition'),
  (N'DISPOSE', N'Property Disposition'),
  (N'EVALUATE', N'Property Valuation');