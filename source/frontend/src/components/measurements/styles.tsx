import styled from 'styled-components';

export const StyledGreenCol = styled.div`
  max-width: 25rem;
  padding: 1rem;
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  border: 1px solid ${props => props.theme.bcTokens.iconColorSuccess};
  border-radius: 0.5rem;
`;

export const StyledGreenGrey = styled.div`
  max-width: 25rem;
  padding: 1rem;
  background-color: ${props => props.theme.css.highlightBackgroundColor};
  border: 1px solid ${props => props.theme.bcTokens.surfaceColorBackgroundDarkBlue};
  border-radius: 0.5rem;
`;

export const StyledGreenBlue = styled.div`
  max-width: 25rem;
  padding: 1rem;
  background-color: ${props => props.theme.css.filterBoxColor};
  border: 1px solid ${props => props.theme.bcTokens.surfaceColorBackgroundDarkBlue};
  border-radius: 0.5rem;
`;

export const StyledInput = styled.input`
  max-width: 9rem;
`;
