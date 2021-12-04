import styled from 'styled-components';

// common ui styling
export * from 'features/contacts/contact/create/styles';

export const CreatePersonLayout = styled.div`
  display: flex;
  flex-direction: column;
  width: 100%;
`;

export const SummaryText = styled.p`
  color: #494949;
  font-size: 1.6rem;
  text-decoration: none;
  &:before {
    content: '* ';
    color: #606060;
    font-weight: 700;
  }
`;
