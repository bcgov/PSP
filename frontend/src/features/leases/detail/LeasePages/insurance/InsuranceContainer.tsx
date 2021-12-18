import { INSURANCE_TYPES } from 'constants/API';
import { getIn, useFormikContext } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IInsurance, ILease } from 'interfaces';
import { useState } from 'react';
import { Button } from 'react-bootstrap';

import InsuranceDetailsView from './details/Insurance';
import InsuranceEditContainer from './edit/EditContainer';

const InsuranceContainer: React.FunctionComponent = () => {
  const context = useFormikContext<ILease>();
  const leaseId: number = getIn(context.values, 'id') || -1;
  const insuranceList: IInsurance[] = getIn(context.values, 'insurances') ?? [];

  const lookupCodes = useLookupCodeHelpers();
  const insuranceTypes = lookupCodes.getByType(INSURANCE_TYPES).sort((a, b) => {
    return (a.displayOrder || 0) - (b.displayOrder || 0);
  });

  const [isEditing, setEditing] = useState(false);
  return (
    <>
      {!isEditing && (
        <>
          <Button variant="link" onClick={() => setEditing(!isEditing)}>
            Edit Insurances
          </Button>
          <InsuranceDetailsView insuranceList={insuranceList} insuranceTypes={insuranceTypes} />
        </>
      )}
      {isEditing && (
        <InsuranceEditContainer
          leaseId={leaseId}
          insuranceList={insuranceList}
          insuranceTypes={insuranceTypes}
          onSuccess={() => {
            context.submitForm();
            setEditing(false);
          }}
          onCancel={() => {
            setEditing(false);
            context.resetForm();
          }}
        />
      )}
    </>
  );
};

export default InsuranceContainer;
