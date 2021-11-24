-- Insert security deposit data of the three different SEC_DEP_HOLDER_TYPE_CODE
INSERT INTO [PIMS_TST].[dbo].[PIMS_SECURITY_DEPOSIT]
    ([LEASE_ID], [SEC_DEP_HOLDER_TYPE_CODE], [SECURITY_DEPOSIT_TYPE_CODE], [DESCRIPTION], [AMOUNT_PAID], [TOTAL_AMOUNT], [DEPOSIT_DATE], [ANNUAL_INTEREST_RATE], [CONCURRENCY_CONTROL_NUMBER])
VALUES
    (
        2
	, 'MINISTRY'
	, 'PET'
	, 'This is a test description'
	, 1111
	, 2000
	, GETDATE()
	, 12
	, 1
  ),
    (
        3
      , 'OTHER'
      , 'SECURITY'
      , 'This is a test description.'
      , 10000
      , 20000
      , GETDATE()
      , 13
      , 1
  ),
    (
        4
      , 'PROPMGR'
      , 'PET'
      , 5000
      , 10000
      , GETDATE()
      , 10
      , 1
  )
