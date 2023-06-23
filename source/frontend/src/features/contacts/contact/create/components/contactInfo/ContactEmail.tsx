import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdClose } from 'react-icons/md';

import { RemoveButton } from '@/components/common/buttons';
import { Input, Select } from '@/components/common/form';
import { Stack } from '@/components/common/Stack/Stack';
import { withNameSpace } from '@/utils/formUtils';

import useContactInfoHelpers from './useContactInfoHelpers';

// re-export helper hooks
export { useContactInfoHelpers };

export interface IContactEmail {
  namespace?: string;
  onRemove?: () => void;
}

/**
 * Form fields to capture email contact information for this Person.
 * @param {IContactEmail} param0
 */
export const ContactEmail: React.FunctionComponent<React.PropsWithChildren<IContactEmail>> = ({
  namespace,
  onRemove,
}) => {
  const { emailTypes } = useContactInfoHelpers();

  return (
    <>
      <Row>
        <Col md={6}>
          <Input field={withNameSpace(namespace, 'value')} label="Email" />
        </Col>
        <Col md={4}>
          <Select
            label="Email type"
            field={withNameSpace(namespace, 'contactMethodTypeCode')}
            options={emailTypes}
            placeholder="Select..."
          />
        </Col>
        <Col md={2} style={{ paddingLeft: 0, paddingTop: '3rem' }}>
          {onRemove && (
            <Stack justifyContent="flex-start" className="h-100">
              <RemoveButton onRemove={onRemove}>
                <MdClose size="2rem" /> <span className="text">Remove</span>
              </RemoveButton>
            </Stack>
          )}
        </Col>
      </Row>
    </>
  );
};
