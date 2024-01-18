import React from 'react';
import { Col, Row } from 'react-bootstrap';

import { RemoveButton } from '@/components/common/buttons';
import { Input, Select } from '@/components/common/form';
import { withNameSpace } from '@/utils/formUtils';

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
export const ContactPhone: React.FunctionComponent<React.PropsWithChildren<IContactPhone>> = ({
  namespace,
  onRemove,
}) => {
  const { phoneTypes } = useContactInfoHelpers();

  return (
    <Row>
      <Col md={6}>
        <Input field={withNameSpace(namespace, 'value')} />
      </Col>
      <Col md={4} className="pl-0">
        <Select
          field={withNameSpace(namespace, 'contactMethodTypeCode')}
          options={phoneTypes}
          placeholder="Select type..."
        />
      </Col>
      <Col md={2} className="pl-0 pt-2">
        {onRemove && <RemoveButton fontSize="1.3rem" onRemove={onRemove} />}
      </Col>
    </Row>
  );
};
