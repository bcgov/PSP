import React from 'react';
import {
  MdOutlineKeyboardDoubleArrowLeft,
  MdOutlineKeyboardDoubleArrowRight,
} from 'react-icons/md';
import styled from 'styled-components';

import { Button, ButtonProps } from '.';

interface IExpandCollapseButtonProps extends ButtonProps {
  expanded: boolean;
  toggleExpanded: () => void;
}

/**
 * PlusButton displaying a plus button, used to add new items.
 * @param param0
 */
export const ExpandCollapseButton: React.FC<
  React.PropsWithChildren<IExpandCollapseButtonProps>
> = ({ expanded, toggleExpanded, className }) => {
  return (
    <StyledExpandButton
      variant="light"
      icon={expanded ? <MdOutlineKeyboardDoubleArrowLeft /> : <MdOutlineKeyboardDoubleArrowRight />}
      title={expanded ? 'collapse' : 'expand'}
      onClick={() => toggleExpanded()}
      className={className}
    ></StyledExpandButton>
  );
};

const StyledExpandButton = styled(Button)`
  &.btn.btn-light.Button {
    padding: 0;
    border: 0.1rem solid ${props => props.theme.bcTokens.typographyColorDisabled};
    background-color: white;
    color: ${props => props.theme.bcTokens.typographyColorSecondary};
    &:focus,
    &:active {
      background-color: white;
      color: ${props => props.theme.bcTokens.typographyColorSecondary};
    }
  }
  &.btn,
  .Button__icon,
  svg {
    max-width: 2.4rem;
    min-width: 2.4rem;
    max-height: 2.4rem;
    min-height: 2.4rem;
    font-size: 1.4rem;
  }
`;
