select DISTINCT prcntrct.PID_NO
     , cnprpose.DESCRIPTION as "Contract Purpose Type"
     , tenurety.DESCRIPTION as "Property Tenure Type"
     , prcntrct.PROPERTY_FILE_NUMBER
     , prcntrct.PROPERTY_FILE_NUMBER_SUFFIX
     , prcntrct.PIN
     , contract.PROPERTY_CONTRACT_ID    
     , contract.PROPERTY_EMPLOYEE_ID
     , contract.DISTRICT_NUMBER
     , contract.REGION_NUMBER
     , contract.NAME
     , contract.PROPERTY_CONTRACT_TYPE
     , contract.STATUS_CODE
     , contract.COMMENCEMENT_DATE
     , contract.INITIAL_PAYMENT_DATE
     , contract.EXPECTED_EXPIRATION_DATE
     , contract.CONTACT_NAME
     , contract.CONTACT_PHONE
     , contract.TERMINATION_DATE
     , contract.LESSOR_FILE_NUMBER
     , contract.RENT_PHONE_NO
     , contract.RENT_AMT
     , contract.RENT_NAME
     , contract.TENANT_LIABILITY_INSURANCE_AMT
     , contract.CMPRHNSV_GNRL_LBLTY_INSRNC_AMT
     , contract.INSURANCE_EXPIRY_DATE
     , contract.SECURITY_DEPOSIT_AMT
     , contract.SECURITY_DEPOSIT_DATE
     , contract.SECURITY_DEPOSIT_RETURN_DATE
     , contract.SECURITY_DEPOSIT_RETURN_AMT
     , contract.PAYMENT_FREQUENCY_UNIT
     , contract.PROPERTY_CONTRACT_PURPOSE_CODE
     , TRANSLATE(contract.MEMO_TEXT,    chr(9)||chr(10)||chr(11)||chr(13), '  ') as MEMO_TEXT
     , TRANSLATE(contract.PURPOSE_TEXT, chr(9)||chr(10)||chr(11)||chr(13), '  ') as PURPOSE_TEXT
from   app_pam.PAM_PROPERTY_CONTRACT         contract join
       app_pam.PAM_PROPERTY_CONT_PROPERTY    prcntrct on prcntrct.PROPERTY_CONTRACT_ID           = contract.PROPERTY_CONTRACT_ID           join
       app_pam.PAM_PROPERTY    property on prcntrct.PROPERTY_CONTRACT_ID                         = contract.PROPERTY_CONTRACT_ID           left outer join
       app_pam.PAM_PROPERTY_TENURE_TYPE      tenurety on tenurety.PROPERTY_TENURE_TYPE_CODE      = prcntrct.PROPERTY_TENURE_TYPE_CODE      left outer join
       app_pam.PAM_PROPERTY_CONT_PURPOSE_TYP cnprpose on cnprpose.PROPERTY_CONT_PURPOSE_TYP_CODE = contract.PROPERTY_CONTRACT_PURPOSE_CODE
where  tenurety.PROPERTY_TENURE_TYPE_CODE IN ('TM', 'TT', 'RW', 'PL')