/* -----------------------------------------------------------------------------
Delete all data from the PIMS_ROLE_CLAIM table and repopulate.
. . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .
Author        Date         Comment
------------  -----------  -----------------------------------------------------
Devin Smith  2021-Nov-18  Initial version
----------------------------------------------------------------------------- */

DECLARE @admin BIGINT;
DECLARE @rem BIGINT;
DECLARE @rea BIGINT;
SELECT @admin = ROLE_ID FROM PIMS_ROLE where NAME = 'System Administrator';
SELECT @rem = ROLE_ID FROM PIMS_ROLE where NAME = 'Real Estate Manager';
SELECT @rea = ROLE_ID FROM PIMS_ROLE where NAME = 'Real Estate Analyst';

/** Admin - all access **/
INSERT [dbo].[PIMS_ROLE_CLAIM] ([ROLE_ID], [CLAIM_ID], [IS_DISABLED], [CONCURRENCY_CONTROL_NUMBER], [APP_CREATE_TIMESTAMP], [APP_CREATE_USERID], [APP_CREATE_USER_GUID], [APP_CREATE_USER_DIRECTORY], [APP_LAST_UPDATE_TIMESTAMP], [APP_LAST_UPDATE_USERID], [APP_LAST_UPDATE_USER_GUID], [APP_LAST_UPDATE_USER_DIRECTORY], [DB_CREATE_TIMESTAMP], [DB_CREATE_USERID], [DB_LAST_UPDATE_TIMESTAMP], [DB_LAST_UPDATE_USERID]) 
VALUES(@admin, 1, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),
(@admin, 2, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),
(@admin, 3, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 4, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 5, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 6, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 7, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 8, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 9, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 10, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 11, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 12, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@admin, 13, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 14, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 15, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 16, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 17, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 18, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 19, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 20, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@admin, 21, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

/** Real Estate Manager - read only access **/
(@rem, 8, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rem, 13, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rem, 17, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

(@rem, 21, 0, 1, CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', NULL, N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED', CAST(N'2021-11-10T02:17:49.920' AS DateTime), N'SEED'),

/** Real Estate Analyst - limited read/write access **/
(@rea, 8, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 9, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 10, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 13, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 14, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 15, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 17, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 18, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 19, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED'),

(@rea, 21, 0, 1, CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'migration', NULL, N'', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED', CAST(N'2021-10-13T04:10:54.770' AS DateTime), N'SEED')
GO