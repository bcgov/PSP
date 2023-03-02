/* -----------------------------------------------------------------------------
Insert data into the PIMS_ACTIVITY_TEMPLATE table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Doug Filteau  2022-Sep-30  Initial version
Doug Filteau  2022-Oct-31  Added 'File Document'
----------------------------------------------------------------------------- */

DELETE
FROM   PIMS_ACTIVITY_TEMPLATE
WHERE  ACTIVITY_TEMPLATE_TYPE_CODE IN (N'GENLTR', N'NOTENTRY', N'CONDENTRY', N'RECNEGOT', N'CONSULT', N'RECTAKES', N'OFFAGREE', N'COMPREQ');
GO
