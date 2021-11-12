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
    contactName: insurance.insurerContact.fullName || 'N.A',
    mobilePhone: insurance.insurerContact.mobile,
    landlinePhone: insurance.insurerContact.landline,
    email: insurance.insurerContact.email || 'N.A',
  };
  return (
    <Row>
      <Col>
        <SubTitle>Insurer</SubTitle>
        <Row>
          <Col>
            <Row>
              <LabelCol xs={columnWidth}>Organization:</LabelCol>
              <Col data-testid="organization">{model.organization}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Contact name:</LabelCol>
              <Col data-testid="contact-name">{model.contactName}</Col>
            </Row>
            <Row>
              <LabelCol xs={columnWidth}>Phone number:</LabelCol>
              <Col data-testid="col-phone">
                {model.mobilePhone !== undefined && (
                  <div className="mb-2">mobile: {model.mobilePhone}</div>
                )}
                {model.landlinePhone !== undefined && (
                  <div className="mb-2">landline: {model.landlinePhone}</div>
                )}
                {model.mobilePhone === undefined && model.landlinePhone === undefined && (
                  <div className="mb-2">N.A</div>
                )}
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
