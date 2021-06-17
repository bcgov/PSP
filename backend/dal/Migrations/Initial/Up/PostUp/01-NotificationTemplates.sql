PRINT N'Adding [PIMS_NOTIFICATION_TEMPLATE]'

INSERT INTO dbo.[PIMS_NOTIFICATION_TEMPLATE] (
    [NOTIFICATION_TEMPLATE_ID]
    , [NAME]
    , [DESCRIPTION]
    , [IS_DISABLED]
    , [TO]
    , [CC]
    , [BCC]
    , [AUDIENCE]
    , [ENCODING]
    , [BODY_TYPE]
    , [PRIORITY]
    , [SUBJECT]
    , [BODY]
    , [TAG]
) VALUES (
    1
    , 'Access Request'
    , 'A new authenticated user has requested access.'
    , 0
    , ''
    , ''
    , ''
    , 'Default'
    , 'Utf8'
    , 'Html'
    , 'High'
    , 'PIMS - Access Request'
    , '
<html><head><title>@Model.Environment.Title</title></head>
<body><p>Dear Administrator,</p><p>@Model.AccessRequest.User.FirstName @Model.AccessRequest.User.LastName has submitted an access request to <a href="@Model.Environment.Uri">PIMS</a>.</p><p>Signin and review their request.</p></body></html>'
    , 'Access Request'
)

-- Update sequence so that it works with the latest data.
ALTER SEQUENCE dbo.[PIMS_NOTIFICATION_TEMPLATE_ID_SEQ]
RESTART WITH 2

