import styled from 'styled-components';

import { Button } from './Button';

export const StyledIconButton = styled(Button)`
  &&.btn {
    background-color: unset;
    border: none;
    :hover,
    :focus,
    :active {
      background-color: unset;
      outline: none;
      box-shadow: none;
    }
    svg {
      transition: all 0.3s ease-out;
    }
    svg:hover {
      transition: all 0.3s ease-in;
    }

    &.btn-primary {
      svg {
        color: ${({ theme, disabled }) =>
          disabled ? theme.css.disabledColor : theme.css.primaryColor};
      }
      svg:hover {
        color: ${({ theme, disabled }) =>
          disabled ? theme.css.disabledColor : theme.css.primaryColor};
      }
    }
    &.btn-light {
      svg {
        color: ${({ theme }) => theme.css.slideOutBlue};
      }
      svg:hover {
        color: ${({ theme }) => theme.css.dangerColor};
      }
    }
    &.btn-info {
      svg {
        color: ${({ theme }) => theme.css.slideOutBlue};
      }
      svg:hover {
        color: ${({ theme }) => theme.css.activeColor};
      }
    }
  }
`;
