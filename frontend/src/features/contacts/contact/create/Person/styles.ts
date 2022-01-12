import { Button } from 'components/common/form/Button';
import { Stack } from 'components/common/Stack/Stack';
import styled from 'styled-components';

// common ui styling
export * from 'features/contacts/contact/create/styles';

export const CreatePersonLayout = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
  overflow: auto;
  padding-right: 1rem;
`;

interface ISummaryText {
  variant?: string;
}

export const SummaryText = styled(Stack)<ISummaryText>`
  color: ${props => (props.variant === 'error' ? '#d8292f' : '#494949')};
  font-size: 1.6rem;
  text-decoration: none;
`;

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

export const ErrorMessage = styled(Stack).attrs({ $direction: 'row' })`
  color: #d8292f;
  font-size: 1.6rem;
  width: auto;
`;
