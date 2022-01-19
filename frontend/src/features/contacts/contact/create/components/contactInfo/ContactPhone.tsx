import { Input, Select } from 'components/common/form';
import { Stack } from 'components/common/Stack/Stack';
import * as Styled from 'features/contacts/contact/create/styles';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';
import { withNameSpace } from 'utils/formUtils';

import useContactInfoHelpers from './useContactInfoHelpers';

// re-export helper hooks
export { useContactInfoHelpers };

export interface IContactPhone {
  namespace?: string;
  onRemove?: () => void;
}

/**
 * Form fields to capture phone contact information for this Person.
 * @param {IContactPhone} param0
 */
export const ContactPhone: React.FunctionComponent<IContactPhone> = ({ namespace, onRemove }) => {
  const { phoneTypes } = useContactInfoHelpers();

  return (
    <>
      <Row>
        <Col md={6}>
          <Input field={withNameSpace(namespace, 'value')} label="Phone" />
        </Col>
        <Col md={4}>
          <Select
            label="Phone type"
            field={withNameSpace(namespace, 'contactMethodTypeCode')}
            options={phoneTypes}
            placeholder="Select..."
          />
        </Col>
        <Col md={2} style={{ paddingLeft: 0, paddingBottom: '2rem' }}>
          {onRemove && (
            <Stack justifyContent="flex-end" className="h-100">
              <Styled.RemoveButton onClick={onRemove}>
                <MdClose size="2rem" /> <span className="text">Remove</span>
              </Styled.RemoveButton>
            </Stack>
          )}
        </Col>
      </Row>
    </>
  );
};
