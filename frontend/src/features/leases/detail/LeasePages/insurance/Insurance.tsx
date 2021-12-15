import { FormSection } from 'components/common/form/styles';
import { getIn, useFormikContext } from 'formik';
import { IInsurance, ILease } from 'interfaces';
import { Col, Row } from 'react-bootstrap';

import Policy from './Policy';

const Insurance: React.FunctionComponent = () => {
  const { values } = useFormikContext<ILease>();
  const insuranceList: IInsurance[] = getIn(values, 'insurances') ?? [];
  return (
    <>
      {insuranceList.map((insurance: IInsurance, index: number) => (
        <div>
          <FormSection key={index + insurance.id}>
            <h2 data-testid="insurance-title">{insurance.insuranceType.description}</h2>
            <br />
            <Row>
              <Col>
                <Policy insurance={insurance} />
              </Col>
            </Row>
          </FormSection>
          <br />
        </div>
      ))}
    </>
  );
};

export default Insurance;
