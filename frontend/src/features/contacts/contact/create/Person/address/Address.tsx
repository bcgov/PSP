import { Input } from 'components/common/form';
import * as Styled from 'features/contacts/contact/create/styles';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';

export interface IAddressProps {}

/**
 * Displays comments directly associated with this Individual Contact.
 * @param {IAddressProps} param0
 */
export const Address: React.FunctionComponent<IAddressProps> = () => {
  return (
    <>
      <Row>
        <Col md={8}>
          <Input field="streetAddress1" label="Address (line 1)" />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input field="countryId" label="Country" />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input field="municipality" label="City" />
        </Col>
      </Row>
      <Row>
        <Col md={4}>
          <Input field="provinceId" label="Province" />
        </Col>
      </Row>
    </>
  );
};

export default Address;
