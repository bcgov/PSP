import styled from 'styled-components';

export const SideNavBar = styled.div`
  align-items: center;
  grid-area: iconbar;
  background-color: ${props => props.theme.css.primaryColor};
  transition: 0.5s width;
  width: 3rem;
  &.expanded {
    width: 8rem;
  }
  svg {
    fill: white;
  }
  .chevron {
    margin-top: auto;
    align-self: flex-end;
    cursor: pointer;
  }
`;

export const AppGridContainer = styled.div`
  padding: 0;
  text-align: center;
  grid: ${props => props.theme.css.headerHeight} auto ${props => props.theme.css.footerHeight} / min-content 1fr;
  height: 100vh;
  width: 100vw;
  display: grid;
  overflow-y: hidden;
  grid-template-areas:
    'header header'
    'iconbar content'
    'footer footer';
`;

export const EmptyAppGridContainer = styled.div`
  grid: ${props => props.theme.css.headerHeight} auto ${props => props.theme.css.footerHeight} / 1fr;
  height: 100vh;
  width: 100vw;
  display: grid;
  overflow-y: hidden;
  grid-template-areas:
    'header'
    'content'
    'footer';
`;
