PRINT N'Add [PIMS_TENANT]'

INSERT INTO dbo.[PIMS_TENANT] (
  [CODE]
  , [NAME]
  , [SETTINGS]
) VALUES (
  'MOTI'
  , 'Ministry of Transportation & Infrastructure'
  , '{ "HelpDeskEmail": "pims@gov.bc.ca" }'
)