PRINT 'Add Tenants'

INSERT INTO dbo.[Tenants] (
  [Code]
  , [Name]
  , [Settings]
) VALUES (
  'MOTI'
  , 'Ministry of Transportation & Infrastructure'
  , '{ "HelpDeskEmail": "pims@gov.bc.ca" }'
)

