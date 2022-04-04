import * as React from 'react';
import styled from 'styled-components';

export const PropertySelectorLayout: React.FunctionComponent = ({ children }) => {
  return (
    <StyledPropertyInfoLayout>
      <Header>
        Select one or more properties that you want to include in this research file. You can choose
        a location from the map, or search by other criteria.
      </Header>
      <Content>{children}</Content>
    </StyledPropertyInfoLayout>
  );
};

const Content = styled.div`
  position: absolute;
  grid-area: content;
  padding: 0 1.5rem;
  width: 100%;
  height: 100%;
  box-sizing: border-box;
`;

const Header = styled.div`
  grid-area: header;
  text-align: left;
  padding: 1.5rem;
`;

const StyledPropertyInfoLayout = styled.div`
  text-align: left;
  width: 100%;
  height: 65rem;
  display: grid;
  grid: 3.8rem fit-content(4.7rem) 1fr 8rem / 1fr;
  grid-template-areas:
    'title'
    'header';
  position: relative;
  box-sizing: border-box;
`;

export default PropertySelectorLayout;
