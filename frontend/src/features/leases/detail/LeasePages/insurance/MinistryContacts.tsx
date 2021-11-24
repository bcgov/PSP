import { IInsurance } from 'interfaces';
import { Col, Row } from 'react-bootstrap';

import { LabelCol, SubTitle } from './styles';

interface IContactsProps {
  insurance: IInsurance;
}

interface ContactsView {
  riskManagement: string;
  ministryOfFinance: string;
}

const MinistryContacts: React.FunctionComponent<IContactsProps> = ({ insurance }) => {
  const columnWidth = 7;
  const model: ContactsView = {
    riskManagement: insurance.motiRiskManagementContact?.fullName || 'N.A',
    ministryOfFinance: insurance.bctfaRiskManagementContact?.fullName || 'N.A',
  };
  return (
    <Row>
      <Col>
        <SubTitle>Ministry Contacts</SubTitle>
        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>MOTI Risk Management:</LabelCol>
              <Col>{model.riskManagement}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Ministry of Finance:</LabelCol>
              <Col>{model.ministryOfFinance}</Col>
            </Row>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default MinistryContacts;
