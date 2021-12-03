import { Form, Input } from 'components/common/form';
import { FormSection } from 'components/common/form/styles';
import { useFormikContext } from 'formik';
import { IFormPerson } from 'interfaces/IFormPerson';
import { Col, Row } from 'react-bootstrap';

import CommentNotes from './comments/CommentNotes';
import * as Styled from './styles';

export interface IPersonProps {}

export const Person: React.FunctionComponent<IPersonProps> = props => {
  const { values } = useFormikContext<IFormPerson>();
  // const improvements: ILeaseImprovement[] = getIn(values, 'improvements') ?? [];

  return (
    <Styled.CreatePersonLayout>
      <FormSection>
        <Row>
          <Col md={4}>
            <Form.Group>
              <Form.Label>First Name *</Form.Label>
              <Input field="firstName" />
            </Form.Group>
          </Col>
          <Col md={3}>
            <Form.Group>
              <Form.Label>Middle</Form.Label>
              <Input field="firstName" />
            </Form.Group>
          </Col>
          <Col>
            <Form.Group>
              <Form.Label>Last Name *</Form.Label>
              <Input field="surname" />
            </Form.Group>
          </Col>
        </Row>
        <Row>
          <Col md={7}>
            <Form.Group>
              <Form.Label>Preferred Name</Form.Label>
              <Input field="preferredName" />
            </Form.Group>
          </Col>
          <Col></Col>
        </Row>
      </FormSection>

      <FormSection>
        <CommentNotes />
      </FormSection>
    </Styled.CreatePersonLayout>
  );
};

export default Person;
