import styled from 'styled-components';

import { Section } from '../Section';

export const StyledNoTabSection = styled(Section)`
  &.form-section {
    margin-left: 0;
    margin-right: 0;
    padding-left: 0;
    padding-right: 0;
  }
`;

export const StyledBorderSection = styled(Section)`
  border: 0.1rem solid lightgrey;
  border-radius: 0.5rem;
`;

export const StyledSummarySection = styled.div`
  background-color: ${props => props.theme.css.filterBackgroundColor};
`;

export const StyledEditWrapper = styled.div`
  color: ${props => props.theme.css.primary};
  text-align: right;
`;
