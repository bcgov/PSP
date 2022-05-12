/* -----------------------------------------------------------------------------
Delete all data from the PIMS_PROJECT_RISK_TYPE table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Jul-16  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_PROJECT_RISK_TYPE
GO

INSERT INTO PIMS_PROJECT_RISK_TYPE (PROJECT_RISK_TYPE_CODE, DESCRIPTION)
VALUES
  (N'GREEN', N'90-100% of the property value'),
  (N'YELLOW', N'50% of the property value'),
  (N'RED', N'0% of the property value');