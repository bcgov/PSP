import styled from 'styled-components';

export const StyledSectionSubheader = styled.div`
  font-weight: bold;
  color: ${props => props.theme.bcTokens.surfaceColorPrimaryButtonDefault};
  border-bottom: 0.2rem ${props => props.theme.bcTokens.surfaceColorPrimaryButtonDefault} solid;
  margin-bottom: 2rem;
`;
