import { TabsProps } from 'react-bootstrap';
import { Tabs as BsTabs } from 'react-bootstrap';
import styled from 'styled-components';

const TabView: React.FunctionComponent<
  React.PropsWithChildren<TabsProps & { className?: string }>
> = ({ children, className, ...props }) => {
  return (
    <StyledTabWrapper className={className}>
      <StyledTabs {...props}>{children}</StyledTabs>
    </StyledTabWrapper>
  );
};

const StyledTabWrapper = styled.div`
  .tab-content {
    .tab-pane {
      position: relative;
    }
    border-radius: 0 0.4rem 0.4rem 0.4rem;
    overflow-y: auto;
    background-color: ${props => props.theme.css.highlightBackgroundColor};
  }
  height: 100%;
`;

const StyledTabs = styled(BsTabs)`
  background-color: white;
  color: ${props => props.theme.css.linkColor};
  font-size: 1.4rem;
  border-color: transparent;
  .nav-tabs {
    height: auto;
  }
  .nav-item {
    color: ${props => props.theme.css.linkColor};
    min-width: 5rem;
    padding: 0.1rem 1.2rem;

    &:hover {
      color: ${props => props.theme.css.linkHoverColor};
      border-color: transparent;
      text-decoration: underline;
    }
    &.active {
      background-color: ${props => props.theme.css.highlightBackgroundColor};
      font-family: 'BCSans-Bold';
      color: ${props => props.theme.css.linkHoverColor};
      border-color: transparent;
    }
  }
`;

export default TabView;
