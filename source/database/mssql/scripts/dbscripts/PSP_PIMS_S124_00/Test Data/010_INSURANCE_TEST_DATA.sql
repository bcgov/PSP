-- Insert test data for insurance related to lease id = 2
INSERT INTO [dbo].[PIMS_INSURANCE]
	(
	[LEASE_ID]
	,[INSURANCE_TYPE_CODE]
	,[COVERAGE_DESCRIPTION]
	,[COVERAGE_LIMIT]
	,[EXPIRY_DATE]
	,[CONCURRENCY_CONTROL_NUMBER]
	)
VALUES
	(
		2
	, 'GENERAL'
	, 'This is a description provided for insurance in order to test. Insurance type code is general.'
	, 100000
	, '2022-12-23'
	, 1
),
	(
		2
	, 'MARINE'
	, 'This is a description provided for insurance in order to test. This is of type marine.'
	, 100000
	, '2022-12-23'
	, 1
),
	(
		2
	, 'VEHICLE'
	, 'This is a description provided for insurance in order to test. This is of type vehicle.'
	, 100000
	, '2022-12-23'
	, 1
)
