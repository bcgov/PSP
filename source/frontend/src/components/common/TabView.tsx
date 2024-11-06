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
    padding: 0.1rem 1.3rem;
    margin-bottom: 0.1rem;
    margin-left: 0.1rem;

    &:hover {
      color: ${props => props.theme.css.highlightBackgroundColor};
      background-color: ${props => props.theme.css.primaryHoverActionColor};
      border-color: transparent;
      text-decoration: underline;
      border-radius: 0.4rem;
      text-shadow: 0.1rem 0 0 currentColor;
    }
    &.active {
      border-radius: 0.4rem;
      color: ${props => props.theme.css.highlightBackgroundColor};
      background-color: ${props => props.theme.css.primaryActiveActionColor};
      border-color: transparent;
      text-shadow: 0.1rem 0 0 currentColor;
    }
  }
`;

export default TabView;
