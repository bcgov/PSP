import { Button } from 'components/common/form';
import styled from 'styled-components';

export const RemoveButton = styled(Button).attrs({ variant: 'link' })`
  && {
    color: #aaaaaa;
    text-decoration: none;
    line-height: unset;
    .text {
      display: none;
    }
    &:hover {
      color: #d8292f;
      text-decoration: none;
      .text {
        display: inline;
      }
    }
  }
`;
