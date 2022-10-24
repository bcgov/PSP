import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

import { LeaseH3 } from '../detail/styles';
import * as Styled from './styles';

export interface INoteSubFormProps {}

export const NoteSubForm: React.FunctionComponent<INoteSubFormProps> = () => {
  return (
    <>
      <LeaseH3>Notes</LeaseH3>
      <Row>
        <Col>
          <Styled.LargeTextArea field="note" />
        </Col>
      </Row>
    </>
  );
};

export default NoteSubForm;
