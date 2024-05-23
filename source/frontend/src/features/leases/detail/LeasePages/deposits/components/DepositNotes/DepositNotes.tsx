import { getIn, useFormikContext } from 'formik';
import { FunctionComponent, PropsWithChildren, useState } from 'react';
import { FaPencilAlt } from 'react-icons/fa';
import styled from 'styled-components';

import { TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { Claims } from '@/constants/index';
import SaveCancelButtons from '@/features/leases/SaveCancelButtons';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

export interface IDepositNotesProps {
  disabled?: boolean;
  onSave: (notes: string) => Promise<void>;
  onEdit: () => void;
  onCancel: () => void;
}

/**
 * Displays all deposit notes directly associated with this lease.
 * @param {IDepositNotesProps} param0
 */
export const DepositNotes: FunctionComponent<PropsWithChildren<IDepositNotesProps>> = ({
  disabled,
  onEdit,
  onSave,
  onCancel,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const formikProps = useFormikContext();
  const notes = getIn(formikProps.values, 'returnNotes');
  const [collapsed, setCollapsed] = useState<boolean>(true);
  return (
    <Section
      isCollapsable={collapsed}
      initiallyExpanded={false}
      header={
        <>
          <span>Deposit Notes</span>
          {hasClaim(Claims.LEASE_EDIT) && disabled && (
            <EditButton
              data-testid="edit-notes"
              className="ml-2"
              onClick={() => {
                onEdit();
                setCollapsed(false);
              }}
            />
          )}
        </>
      }
    >
      <TextArea rows={5} disabled={disabled} field="returnNotes" />
      {hasClaim(Claims.LEASE_EDIT) && !disabled && (
        <StyledButtons
          onCancel={() => {
            onCancel();
            setCollapsed(true);
          }}
          onSaveOverride={() => onSave(notes)}
          formikProps={formikProps}
        />
      )}
    </Section>
  );
};

const EditButton = styled(FaPencilAlt)`
  &:hover {
    cursor: pointer;
  }
`;

const StyledButtons = styled(SaveCancelButtons)`
  background-color: transparent;
`;

export default DepositNotes;
