import * as React from 'react';
import { TabsProps } from 'react-bootstrap';
import styled from 'styled-components';

import * as Styled from '@/components/common/styles';

const TabView: React.FunctionComponent<
  React.PropsWithChildren<TabsProps & { className?: string }>
> = ({ children, className, ...props }) => {
  return (
    <StyledTabWrapper className={className}>
      <Styled.Tabs {...props}>{children}</Styled.Tabs>
    </StyledTabWrapper>
  );
};

const StyledTabWrapper = styled.div`
  .tab-content {
    .tab-pane {
      position: relative;
    }
    border-radius: 0 0.4rem 0.4rem 0.4rem;
    height: calc(100% - 2.4rem); // substract nav height
    overflow-y: auto;
    background-color: ${props => props.theme.css.filterBackgroundColor};
  }
  height: 100%;
`;

export default TabView;
