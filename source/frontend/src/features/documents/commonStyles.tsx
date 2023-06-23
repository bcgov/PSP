import styled from 'styled-components';

import { Scrollable } from '@/components/common/Scrollable/Scrollable';

export const StyledGreySection = styled.div`
  padding: 1rem;
  background-color: ${({ theme }) => theme.css.filterBackgroundColor};
`;

export const StyledH2 = styled.h2`
  font-weight: 700;
  color: ${props => props.theme.css.primaryColor};
`;

export const StyledH3 = styled.h3`
  font-weight: 700;
  font-size: 1.7rem;
  margin-bottom: 1rem;
  text-align: left;
  padding-top: 1rem;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: solid 0.1rem ${props => props.theme.css.primaryColor};
`;

export const StyledHeader = styled.div`
  text-align: left !important;
  display: flex;
`;
export const StyledScrollable = styled(Scrollable)`
  overflow-x: hidden;
  max-height: 50rem;
`;

export const StyledNoData = styled.div`
  text-align: center;
  font-style: italic;
`;
