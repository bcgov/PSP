select payment.*
from   app_pam.PAM_PROPERTY                  property                                                                                      join
       app_pam.PAM_PROPERTY_CONT_PROPERTY    prcntrct on prcntrct.PROPERTY_ID                    = property.PROPERTY_ID                    join
       app_pam.PAM_PROPERTY_CONTRACT         contract on contract.PROPERTY_CONTRACT_ID           = prcntrct.PROPERTY_CONTRACT_ID           join
       app_pam.PAM_PROPERTY_TENURE_TYPE      tenurety on tenurety.PROPERTY_TENURE_TYPE_CODE      = prcntrct.PROPERTY_TENURE_TYPE_CODE      join
       app_pam.PAM_PROPERTY_CONT_PURPOSE_TYP cnprpose on cnprpose.PROPERTY_CONT_PURPOSE_TYP_CODE = contract.PROPERTY_CONTRACT_PURPOSE_CODE join
       app_pam.PAM_RCVBL_PRPTY_CONT_TRX      payment  on payment.PROPERTY_CONTRACT_ID            = contract.PROPERTY_CONTRACT_ID
where  tenurety.PROPERTY_TENURE_TYPE_CODE IN ('TM', 'TT', 'RW', 'PL');