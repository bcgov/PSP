import { FormSection } from 'components/common/form/styles';
import { IInsurance } from 'interfaces';
import { Col, Row } from 'react-bootstrap';
import { ILookupCode } from 'store/slices/lookupCodes';

import Policy from './Policy';

export interface InsuranceDetailsViewProps {
  insuranceList: IInsurance[];
  insuranceTypes: ILookupCode[];
}

const InsuranceDetailsView: React.FunctionComponent<InsuranceDetailsViewProps> = ({
  insuranceList,
  insuranceTypes,
}) => {
  return (
    <>
      {insuranceList
        .sort((a, b) => {
          return (
            insuranceTypes.findIndex(i => i.id === a.insuranceType.id) -
            insuranceTypes.findIndex(i => i.id === b.insuranceType.id)
          );
        })
        .map((insurance: IInsurance, index: number) => (
          <div key={index + insurance.id}>
            <FormSection>
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
