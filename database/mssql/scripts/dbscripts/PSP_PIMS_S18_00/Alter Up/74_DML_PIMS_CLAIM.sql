/* -----------------------------------------------------------------------------
Insert data into the PIMS_CLAIM table.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Devin Smith  2021-Nov-18  Initial version
----------------------------------------------------------------------------- */

INSERT [dbo].[PIMS_CLAIM] ([CLAIM_ID], [CLAIM_UID], [NAME], [KEYCLOAK_ROLE_ID], [DESCRIPTION], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID]) VALUES (21, N'a9b9075b-2ea5-43b6-91df-2cc075453428', N'ROLE_PIMS_R', N'a9b9075b-2ea5-43b6-91df-2cc075453428', N'Read access to Property-related map layers', 0, 3, CAST(N'2021-11-29T02:17:49.903' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-29T07:42:25.990' AS DateTime), N'service', N'1b93f614-91da-4b32-b36e-bd2c6ebd12e2', N'', CAST(N'2021-11-29T02:17:49.903' AS DateTime), N'SEED', CAST(N'2021-11-29T07:42:25.967' AS DateTime), N'SEED')
GO
