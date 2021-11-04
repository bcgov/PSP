import { FormSection } from 'components/common/form/styles';
import { Col, Row } from 'react-bootstrap';

import Insurer from './Insurer';
import MinistryContacts from './MinistryContacts';
import Policy from './Policy';

const Insurance: React.FunctionComponent = () => {
  return (
    <FormSection>
      <p>Commercial General Liability (CGL)</p>
      <Row>
        <Col>
          <Insurer />
        </Col>
        <Col>
          <MinistryContacts />
        </Col>
      </Row>
      <Row>
        <Col>
          <Policy />
        </Col>
      </Row>
    </FormSection>
  );
};

export default Insurance;
