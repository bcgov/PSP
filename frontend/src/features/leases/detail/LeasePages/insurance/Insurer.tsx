import { IInsurance } from 'interfaces';
import { Col, Row } from 'react-bootstrap';

import { LabelCol, SubTitle } from './styles';

interface InsurerProps {
  insurance: IInsurance;
}

interface InsurerView {
  organization: string;
  contactName: string;
  mobilePhone?: string;
  landlinePhone?: string;
  email: string;
}

const Insurer: React.FunctionComponent<InsurerProps> = ({ insurance }) => {
  const columnWidth = 5;
  const model: InsurerView = {
    organization: insurance.insurerOrganization.name,
    contactName:
      insurance.insurerContact.fullName ||
      insurance.insurerContact.surname ||
      insurance.insurerContact.firstName ||
      insurance.insurerContact.middleNames ||
      '',
    mobilePhone: insurance.insurerContact.mobile || '',
    landlinePhone: insurance.insurerContact.landline || '',
    email: insurance.insurerContact.email || '',
  };
  return (
    <Row>
      <Col>
        <SubTitle>Insurer</SubTitle>
        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>Organization:</LabelCol>
              <Col>{model.organization}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Contact name:</LabelCol>
              <Col>{model.contactName}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Phone number:</LabelCol>
              <Col>
                <div className="mb-2">mobile: {model.mobilePhone}</div>
                <div className="mb-2">landline: {model.landlinePhone}</div>
              </Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Email:</LabelCol>
              <Col>{model.email}</Col>
            </Row>
          </Col>
        </Row>
      </Col>
    </Row>
  );
};

export default Insurer;
