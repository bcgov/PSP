import { Form, TextArea } from 'components/common/form';
import * as Styled from 'features/leases/detail/styles';
import * as React from 'react';
import { withNameSpace } from 'utils/formUtils';

export interface ITenantNotesProps {
  nameSpace?: string;
  disabled?: boolean;
}

/**
 * Displays all notes directly associated with this lease tenant.
 * @param {ITenantNotesProps} param0
 */
export const TenantNotes: React.FunctionComponent<ITenantNotesProps> = ({
  nameSpace,
  disabled,
}) => {
  return (
    <>
      <Styled.FormGrid>
        <Form.Label>Notes:</Form.Label>
        <TextArea
          innerClassName="notes"
          disabled={disabled}
          style={{ marginLeft: '1rem' }}
          field={withNameSpace(nameSpace, '')}
        />
      </Styled.FormGrid>
    </>
  );
};

export default TenantNotes;
