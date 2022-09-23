import { InlineSelect } from 'components/common/form/styles';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { yesNoUnknownOptions } from 'utils/formUtils';

import { LeaseH3 } from '../detail/styles';
import * as Styled from './styles';

interface IReferenceSubFormProps {}

const ReferenceSubForm: React.FunctionComponent<IReferenceSubFormProps> = props => {
  return (
    <>
      <Row>
        <Col>
          <LeaseH3>Reference Information</LeaseH3>
        </Col>
      </Row>
      <Row>
        <Col>
          <InlineSelect
            label="Physical lease/license exists:"
            field="hasPhysicalLicense"
            options={yesNoUnknownOptions}
            placeholder=""
          />
        </Col>
      </Row>
      <Row>
        <Col>
          <InlineSelect
            label="Digital lease/license exists:"
            field="hasDigitalLicense"
            options={yesNoUnknownOptions}
            placeholder=""
          />
        </Col>
      </Row>
      <Row>
        <Col>
          <Styled.MediumTextArea
            label="Location of documents:"
            field="documentationReference"
            tooltip="Use this space to paste in links or system paths to relevant documents"
          />
        </Col>
      </Row>
    </>
  );
};

export default ReferenceSubForm;
