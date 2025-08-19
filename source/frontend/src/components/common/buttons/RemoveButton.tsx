import React, { CSSProperties, MouseEventHandler } from 'react';
import { ButtonProps } from 'react-bootstrap';
import { FaTrash } from 'react-icons/fa';
import { MdClose } from 'react-icons/md';
import styled, { css } from 'styled-components';

import { StyledIconButton } from './IconButton';
import { LinkButton } from './LinkButton';

interface IRemoveButtonProps extends ButtonProps {
  label?: string;
  title?: string;
  'data-testId'?: string;
  fontSize?: string;
  icon?: React.ReactNode;
  style?: CSSProperties | null;
  onRemove: MouseEventHandler;
}

export const RemoveButton: React.FunctionComponent<
  React.PropsWithChildren<IRemoveButtonProps>
> = props => {
  return (
    <StyledRemoveLinkButton
      $fontSize={props.fontSize}
      onClick={props.onRemove}
      data-testid={props['data-testId'] ?? 'remove-button'}
    >
      <MdClose size="2rem" title="remove" /> <span className="text">{props.label ?? 'Remove'}</span>
    </StyledRemoveLinkButton>
  );
};

export const RemoveIconButton: React.FunctionComponent<
  React.PropsWithChildren<IRemoveButtonProps>
> = props => {
  return (
    <StyledRemoveIconButton
      variant="primary"
      title={props.title ?? 'remove'}
      onClick={props.onRemove}
      data-testid={props['data-testId'] ?? 'remove-button'}
      style={props.style}
    >
      {props.icon ?? <FaTrash size={22} />}
    </StyledRemoveIconButton>
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
        color: #aaaaaa !important;
      }
      text-decoration: none;
      line-height: unset;
      .text {
        display: none;
      }
      svg:hover,
      svg:active,
      svg:focus {
        color: #d8292f !important;
        text-decoration: none;
        opacity: unset;
        flex-direction: row;
        .text {
          display: inline;
          line-height: 2rem;
        }
      }
    }
  }
`;
