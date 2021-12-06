/* -----------------------------------------------------------------------------
Delete all data from the PIMS_TENANT table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . 
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2021-Aug-24  Initial version
----------------------------------------------------------------------------- */

DELETE FROM PIMS_TENANT
GO

INSERT INTO dbo.PIMS_TENANT (CODE, NAME, SETTINGS) 
VALUES (N'MOTI', N'Ministry of Transportation & Infrastructure', N'{ "HelpDeskEmail": "pims@gov.bc.ca" }');