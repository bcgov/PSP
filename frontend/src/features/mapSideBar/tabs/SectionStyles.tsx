import styled from 'styled-components';

export const StyledScrollable = styled.div`
  overflow-y: auto;
`;

export const StyledFormSection = styled.div`
  margin: 1.5rem;
  padding: 1.5rem;
  background-color: white;
  text-align: left;
`;
export const StyledSectionHeader = styled.h2`
  font-weight: bold;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: 0.2rem ${props => props.theme.css.primaryColor} solid;
  margin-bottom: 2rem;
`;
