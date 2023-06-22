import styled from 'styled-components';

import { Scrollable as ScrollableBase } from '@/components/common/Scrollable/Scrollable';

export const StyledContainer = styled.div`
  padding: 1rem;
`;

export const ListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  gap: 2.5rem;
  padding: 0;
`;

export const Scrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export const PageHeader = styled.h3`
  text-align: left;
`;
