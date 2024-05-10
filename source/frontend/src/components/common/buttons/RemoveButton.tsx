import React from 'react';
import { MdClose } from 'react-icons/md';
import styled, { css } from 'styled-components';

import { StyledIconButton } from './IconButton';
import { LinkButton } from './LinkButton';

interface IRemoveButtonProps {
  label?: string;
  dataTestId?: string | null;
  fontSize?: string;
  onRemove: () => void;
}

export const RemoveButton: React.FunctionComponent<React.PropsWithChildren<IRemoveButtonProps>> = ({
  label,
  dataTestId,
  fontSize,
  onRemove,
}) => {
  return (
    <StyledRemoveLinkButton $fontSize={fontSize} onClick={onRemove}>
      <MdClose data-testid={dataTestId ?? 'remove-button'} size="2rem" title="remove" />{' '}
      <span className="text">{label ?? 'Remove'}</span>
    </StyledRemoveLinkButton>
  );
};

// Support font-size override for remove buttons
export const StyledRemoveLinkButton = styled(LinkButton)<{ $fontSize?: string }>`
  &&.btn {
    ${props =>
      props.$fontSize &&
      css`
        font-size: ${props.$fontSize};
      `}
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

export const StyledRemoveIconButton = styled(StyledIconButton)`
  &&.btn {
    &.btn-primary {
      svg {
        color: ${({ theme }) => theme.bcTokens.iconsColorDisabled};
      }
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
  }
`;
