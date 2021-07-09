import { Button } from 'components/common/form';
import * as React from 'react';
import styled from 'styled-components';

const ButtonWrapper = styled.div`
  display: flex;
  justify-content: flex-end;
  .btn {
    margin: 1rem;
    margin-right: 0;
    min-height: 34px;
    padding: 4px 12px;
  }
`;

interface IInventoryFormButtonsProps {
  /** function to be called when the cancel button is clicked */
  onCancel: () => void;
  /** function to be called when the save button is clicked */
  onSubmit: () => void;
  /** disable the buttons */
  disabled?: boolean;
}

/**
 * Cancel and Submit buttons to control MOTI inventory forms.
 * @param {IInventoryFormButtonsProps} param0
 */
export const InventoryFormButtons: React.FunctionComponent<IInventoryFormButtonsProps> = ({
  onCancel,
  onSubmit,
  disabled,
}) => {
  return (
    <ButtonWrapper>
      <Button variant="secondary" className="narrow" onClick={onCancel} disabled={disabled}>
        Cancel
      </Button>
      <Button className="narrow" onClick={onSubmit} disabled={disabled}>
        Save
      </Button>
    </ButtonWrapper>
  );
};

export default InventoryFormButtons;
