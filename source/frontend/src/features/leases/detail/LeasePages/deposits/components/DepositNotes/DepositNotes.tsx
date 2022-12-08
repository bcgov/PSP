import { TextArea } from 'components/common/form';
import { Claims } from 'constants/index';
import SaveCancelButtons from 'features/leases/SaveCancelButtons';
import { getIn, useFormikContext } from 'formik';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import { ILease } from 'interfaces';
import * as React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPencilAlt } from 'react-icons/fa';
import styled from 'styled-components';

import * as Styled from '../../styles';

export interface IDepositNotesProps {
  disabled?: boolean;
  onSave: (notes: string) => Promise<ILease | undefined>;
  onEdit: () => void;
  onCancel: () => void;
}

/**
 * Displays all deposit notes directly associated with this lease.
 * @param {IDepositNotesProps} param0
 */
export const DepositNotes: React.FunctionComponent<React.PropsWithChildren<IDepositNotesProps>> = ({
  disabled,
  onEdit,
  onSave,
  onCancel,
}) => {
  const { hasClaim } = useKeycloakWrapper();
  const formikProps = useFormikContext();
  const notes = getIn(formikProps.values, 'returnNotes');
  return (
    <>
      <Row>
        <Col>
          <Styled.FormDescriptionLabel>Deposit Notes</Styled.FormDescriptionLabel>
          {hasClaim(Claims.LEASE_EDIT) && disabled && (
            <EditButton data-testid="edit-notes" className="ml-2" onClick={() => onEdit()} />
          )}
        </Col>
      </Row>
      <Row>
        <Col>
          <TextArea rows={5} disabled={disabled} field="returnNotes" />
        </Col>
      </Row>
      <Row>
        <Col>
          {hasClaim(Claims.LEASE_EDIT) && !disabled && (
            <StyledButtons
              onCancel={onCancel}
              onSaveOverride={() => onSave(notes)}
              formikProps={formikProps}
            />
          )}
        </Col>
      </Row>
    </>
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
