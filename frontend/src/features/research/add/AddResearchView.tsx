import { Scrollable as ScrollableBase } from 'components/common/Scrollable/Scrollable';
import * as React from 'react';
import styled from 'styled-components';

import AddResearchForm from './AddResearchForm';

export interface IAddResearchViewProps {}

export const AddResearchView: React.FunctionComponent = () => {
  return (
    <StyledListPage>
      <StyledScrollable>
        <AddResearchForm />
      </StyledScrollable>
    </StyledListPage>
  );
};

const StyledListPage = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  width: 100%;
  font-size: 14px;
  gap: 2.5rem;
  padding: 0;
`;

const StyledScrollable = styled(ScrollableBase)`
  padding: 1.6rem 3.2rem;
  width: 100%;
`;

export default AddResearchView;
