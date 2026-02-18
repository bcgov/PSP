import styled from 'styled-components';

export const StyledSectionSubheader = styled.div`
  font-weight: bold;
  color: ${props => props.theme.css.headerTextColor};
  border-bottom: 0.2rem ${props => props.theme.css.headerBorderColor} solid;
  margin-bottom: 2rem;
`;
