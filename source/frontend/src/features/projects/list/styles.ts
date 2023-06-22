import styled from 'styled-components';

import { InlineForm, InlineInput } from '@/components/common/form/styles';
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
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export const PageHeader = styled.h3`
  text-align: left;
`;

export const PageToolbar = styled.div`
  align-items: center;
  padding: 0;
  padding-bottom: 2rem;
`;

export const Spacer = styled.div`
  display: flex;
  flex: 1 1 auto;
`;

export const FilterBox = styled(InlineForm)`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
`;

export const LongInlineInput = styled(InlineInput)`
  flex: 3 1 auto;
  max-width: 31rem;
`;
