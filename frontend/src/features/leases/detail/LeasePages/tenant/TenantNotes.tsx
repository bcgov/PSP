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
      <Styled.FormDescriptionLabel>Notes</Styled.FormDescriptionLabel>
      <Styled.TenantNotes disabled={disabled} field={withNameSpace(nameSpace, '')} />
    </>
  );
};

export default TenantNotes;
