import * as Styled from 'components/common/styles';
import * as React from 'react';
import { TabsProps } from 'react-bootstrap';
import styled from 'styled-components';

const TabView: React.FunctionComponent<TabsProps> = ({ children, ...props }) => {
  return (
    <StyledTabWrapper>
      <Styled.Tabs {...props}>{children}</Styled.Tabs>
    </StyledTabWrapper>
  );
};

const StyledTabWrapper = styled.div`
  .nav-tabs {
    height: 2.4rem;
  }
  .tab-content {
    .tab-pane {
      position: relative;
    }
    border-radius: 0 0.4rem 0.4rem 0.4rem;
    height: calc(100% - 2.4rem);
    overflow-y: auto;
    overflow-x: hidden;
    background-color: ${props => props.theme.css.filterBackgroundColor};
  }
  height: 100%;
`;

export default TabView;
