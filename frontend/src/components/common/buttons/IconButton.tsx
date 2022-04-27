import styled from 'styled-components';

import { Button } from './Button';

export const IconButton = styled(Button)`
  &&.btn {
    background-color: unset;
    border: 0;
    :hover,
    :active {
      background-color: unset;
    }
    svg {
      transition: all 0.3s ease-out;
    }
    svg:hover {
      transition: all 0.3s ease-in;
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
