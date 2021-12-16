-- Insert test data for insurance related to lease id = 2
INSERT INTO [dbo].[PIMS_INSURANCE]
	(
	[LEASE_ID]
	,[INSURANCE_TYPE_CODE]
	,[INSURER_ORG_ID]
	,[INSURER_CONTACT_ID]
	,[MOTI_RISK_MGMT_CONTACT_ID]
	,[BCTFA_RISK_MGMT_CONTACT_ID]
	,[INSURANCE_PAYEE_TYPE_CODE]
	,[COVERAGE_DESCRIPTION]
	,[COVERAGE_LIMIT]
	,[INSURED_VALUE]
	,[START_DATE]
	,[EXPIRY_DATE]
	,[CONCURRENCY_CONTROL_NUMBER]
	)
VALUES
	(
		2
	, 'GENERAL'
	, 1
	, 1
	, 2
	, 4
	, 'SELF'
	, 'This is a description provided for insurance in order to test. Insurance type code is general.'
	, 100000
	, 50000
	, GETDATE()
	, '2022-12-23'
	, 1
),
	(
		2
	, 'MARINE'
	, 1
	, 1
	, 2
	, 3
	, 'REPLCOST'
	, 'This is a description provided for insurance in order to test. This is of type marine.'
	, 100000
	, 50000
	, GETDATE()
	, '2022-12-23'
	, 1
),
	(
		2
	, 'VEHICLE'
	, 1
	, 2
	, 3
	, 4
	, 'REPLCOST'
	, 'This is a description provided for insurance in order to test. This is of type vehicle.'
	, 100000
	, 50000
	, GETDATE()
	, '2022-12-23'
	, 1
)
