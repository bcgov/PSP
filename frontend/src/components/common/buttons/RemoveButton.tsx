import React from 'react';
import { MdClose } from 'react-icons/md';
import styled from 'styled-components';

import { LinkButton } from './LinkButton';

interface IRemoveButtonProps {
  onRemove: () => void;
}

export const RemoveButton: React.FunctionComponent<IRemoveButtonProps> = ({ onRemove }) => {
  return (
    <StyledLinkButton onClick={onRemove}>
      <MdClose size="2rem" title="remove" /> <span className="text">Remove</span>
    </StyledLinkButton>
  );
};

const StyledLinkButton = styled(LinkButton)`
  &&.btn {
    color: #aaaaaa;
    text-decoration: none;
    line-height: unset;
    .text {
      display: none;
    }
    &:hover,
    &:active,
    &:focus {
      color: #d8292f;
      text-decoration: none;
      opacity: unset;
      display: flex;
      flex-direction: row;
      .text {
        display: inline;
        line-height: 2rem;
      }
    }
  }
`;
