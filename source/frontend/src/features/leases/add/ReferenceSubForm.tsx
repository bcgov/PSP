import { InlineYesNoSelect } from 'components/common/form/styles';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseH3 } from '../detail/styles';
import * as Styled from './styles';

interface IReferenceSubFormProps {}

const ReferenceSubForm: React.FunctionComponent<
  React.PropsWithChildren<IReferenceSubFormProps>
> = props => {
  return (
    <>
      <Row>
        <Col>
          <LeaseH3>Reference Information</LeaseH3>
        </Col>
      </Row>
      <Row>
        <Col>
          <Styled.LargeInlineInput label="LIS #:" field="tfaFileNumber" />
        </Col>
      </Row>
      <Row>
        <Col>
          <Styled.LargeInlineInput label="PS #:" field="psFileNo" />
        </Col>
      </Row>
      <Row>
        <Col>
          <InlineYesNoSelect label="Physical lease/license exists:" field="hasPhysicalLicense" />
        </Col>
      </Row>
      <Row>
        <Col>
          <InlineYesNoSelect label="Digital lease/license exists:" field="hasDigitalLicense" />
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
