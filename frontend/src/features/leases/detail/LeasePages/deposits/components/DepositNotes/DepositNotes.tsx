import { TextArea } from 'components/common/form';
import * as React from 'react';

import * as Styled from '../../styles';

export interface IDepositNotesProps {
  disabled?: boolean;
}

/**
 * Displays all deposit notes directly associated with this lease.
 * @param {IDepositNotesProps} param0
 */
export const DepositNotes: React.FunctionComponent<IDepositNotesProps> = ({ disabled }) => {
  return (
    <>
      <Styled.FormDescriptionLabel>Deposit Notes</Styled.FormDescriptionLabel>
      <TextArea rows={5} disabled={disabled} field="returnNotes" />
    </>
  );
};

export default DepositNotes;
