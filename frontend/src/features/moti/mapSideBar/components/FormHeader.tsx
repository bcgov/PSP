import styled from 'styled-components';

/**
 * Styled Component representing the header for each of the tabbed forms.
 */
export const FormHeader = styled.h4`
  color: ${props => props.theme.css.textColor};
  background-color: ${props => props.theme.css.filterBackgroundColor};
  text-align: left;
`;
