import { getIn, useFormikContext } from 'formik';
import { FunctionComponent, PropsWithChildren, useState } from 'react';
import { FaEdit } from 'react-icons/fa';
import styled from 'styled-components';

import { TextArea } from '@/components/common/form';
import { Section } from '@/components/common/Section/Section';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import TooltipIcon from '@/components/common/TooltipIcon';
import { Claims } from '@/constants/index';
import SaveCancelButtons from '@/features/leases/SaveCancelButtons';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';

export interface IDepositNotesProps {
  disabled?: boolean;
  isFileFinalStatus: boolean;
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
  isFileFinalStatus,
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
        hasClaim(Claims.LEASE_EDIT) && disabled ? (
          <SectionListHeader
            claims={[Claims.LEASE_EDIT]}
            title="Deposit Comments"
            addButtonText="Edit Comments"
            addButtonIcon={<FaEdit size={'2rem'} />}
            button-data-testId="edit-comments"
            onButtonAction={() => {
              onEdit();
              setCollapsed(false);
            }}
            cannotAddComponent={
              <TooltipIcon
                toolTipId={`deposit-notes-cannot-edit-tooltip`}
                toolTip={cannotEditMessage}
              />
            }
            isAddEnabled={!isFileFinalStatus}
          />
        ) : (
          <span>Deposit Comments</span>
        )
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

const StyledButtons = styled(SaveCancelButtons)`
  background-color: transparent;
`;

export default DepositNotes;
