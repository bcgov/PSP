import React from 'react';
import { Col, Row } from 'react-bootstrap';

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
    <Row>
      <Col md={6}>
        <Input field={withNameSpace(namespace, 'value')} />
      </Col>
      <Col md={4} className="pl-0">
        <Select
          field={withNameSpace(namespace, 'contactMethodTypeCode')}
          options={emailTypes}
          placeholder="Select type..."
        />
      </Col>
      <Col md={2} className="pl-0 pt-2">
        {onRemove && (
          <Stack justifyContent="flex-start" className="h-100">
            <RemoveButton fontSize="1.3rem" onRemove={onRemove}></RemoveButton>
          </Stack>
        )}
      </Col>
    </Row>
  );
};
