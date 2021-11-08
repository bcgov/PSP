import { FormSection } from 'components/common/form/styles';
import { getIn, useFormikContext } from 'formik';
import { IInsurance, ILease } from 'interfaces';
import { Col, Row } from 'react-bootstrap';

import Insurer from './Insurer';
import MinistryContacts from './MinistryContacts';
import Policy from './Policy';

const Insurance: React.FunctionComponent = () => {
  const { values } = useFormikContext<ILease>();
  const insuranceList: IInsurance[] = getIn(values, 'insurances') ?? [];
  return (
    <>
      {insuranceList.map((insurance: IInsurance, index: number) => (
        <div>
          <FormSection key={index + insurance.id}>
            <h2>{insurance.insuranceType.description || ''}</h2>
            <br />
            <Row>
              <Col>
                <Insurer insurance={insurance} />
              </Col>
              <Col>
                <MinistryContacts insurance={insurance} />
              </Col>
            </Row>
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
