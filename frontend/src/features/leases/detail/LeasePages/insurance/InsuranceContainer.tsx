import { FormSection } from 'components/common/form/styles';
import { INSURANCE_TYPES, LEASE_PROGRAM_TYPES } from 'constants/API';
import { getIn, useFormikContext } from 'formik';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { IInsurance, ILease } from 'interfaces';
import { useState } from 'react';
import { Button, Col, Row } from 'react-bootstrap';

import InsuranceDetailsView from './details/Insurance';
import InsuranceEditView from './edit/Edit';

const InsuranceContainer: React.FunctionComponent = () => {
  const { values } = useFormikContext<ILease>();
  const insuranceList: IInsurance[] = getIn(values, 'insurances') ?? [];

  const lookupCodes = useLookupCodeHelpers();
  const insuranceTypes = lookupCodes.getByType(INSURANCE_TYPES);

  const [isEditing, setEditing] = useState(false);
  return (
    <>
      asdas
      <Button variant="link" onClick={() => setEditing(!isEditing)}>
        {isEditing ? 'We are editing' : `We are not editing`}
      </Button>
      {!isEditing && <InsuranceDetailsView insuranceList={insuranceList} />}
      {isEditing && (
        <InsuranceEditView insuranceList={insuranceList} insuranceTypes={insuranceTypes} />
      )}
    </>
  );
};

export default InsuranceContainer;
