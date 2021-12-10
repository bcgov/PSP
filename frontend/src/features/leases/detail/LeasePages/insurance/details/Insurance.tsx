import { FormSection } from 'components/common/form/styles';
import { IInsurance } from 'interfaces';
import { Col, Row } from 'react-bootstrap';

import Policy from './Policy';

export interface InsuranceDetailsViewProps {
  insuranceList: IInsurance[];
}

const InsuranceDetailsView: React.FunctionComponent<InsuranceDetailsViewProps> = ({
  insuranceList,
}) => {
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

export default InsuranceDetailsView;
