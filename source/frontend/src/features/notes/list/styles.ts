import styled from 'styled-components';

import { Scrollable as ScrollableBase } from '@/components/common/Scrollable/Scrollable';

export const ListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
`;

export const Scrollable = styled(ScrollableBase)`
  width: 100%;
`;

export const PageHeader = styled.h3`
  text-align: left;
  font-family: BcSans-Bold;
  font-size: 2.6rem;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: 0.2rem solid ${props => props.theme.css.primaryLightColor};
`;

export const PageToolbar = styled.div`
  align-items: center;
  padding: 0;
  padding-bottom: 2rem;
`;
