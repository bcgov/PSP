-- Insert security deposit return test data for lease id = 2
INSERT INTO [dbo].[PIMS_SECURITY_DEPOSIT_RETURN]
	(
	[LEASE_ID]
	,[SECURITY_DEPOSIT_TYPE_CODE]
	,[TERMINATION_DATE]
	,[DEPOSIT_TOTAL]
	,[CLAIMS_AGAINST]
	,[RETURN_AMOUNT]
	,[RETURN_DATE]
	,[CHEQUE_NUMBER]
	,[PAYEE_NAME]
	,[PAYEE_ADDRESS]
	,[CONCURRENCY_CONTROL_NUMBER]
	)
VALUES
	(
		2
	, 'PET'
	, GETDATE()
	, 1000
	, 500
	, 250
	, GETDATE()
	, '1234'
	, 'Chester Tester'
	, '1234 Fake St.'
	, 1
  ),
	(
		2
	, 'SECURITY'
	, GETDATE()
	, 1230
	, 500
	, 270
	, GETDATE()
	, '1234'
	, 'Les McTester'
	, '1234 Faker St.'
	, 1
  )
