import * as Styled from 'features/contacts/contact/create/styles';
import * as React from 'react';
import { MdClose } from 'react-icons/md';

import { Stack } from '../Stack/Stack';

interface IRemoveButtonProps {
  onRemove: () => void;
}

export const RemoveButton: React.FunctionComponent<IRemoveButtonProps> = ({ onRemove }) => {
  return (
    <Stack justifyContent="flex-start" className="h-100">
      <Styled.RemoveButton onClick={onRemove}>
        <MdClose size="2rem" title="remove" /> <span className="text">Remove</span>
      </Styled.RemoveButton>
    </Stack>
  );
};

export default RemoveButton;
