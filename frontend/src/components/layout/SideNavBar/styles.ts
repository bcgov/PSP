import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

export const TrayHeader = styled.h3`
  font-size: 32px;
  padding-bottom: 0.5rem;
  border-bottom: 4px solid ${props => props.theme.css.primaryColor};
  margin-bottom: 2rem;
`;

export const SideNavBar = styled.div`
  height: calc(
    100vh - ${props => props.theme.css.headerHeight} - ${props => props.theme.css.footerHeight}
  );
  overflow-y: auto;
  display: flex;
  flex-direction: column;
  position: relative;
  align-items: center;
  grid-area: iconbar;
  background-color: ${props => props.theme.css.primaryColor};
  transition: 0.5s width;
  width: 3.8rem;
  &.expanded {
    width: 10rem;
  }
  svg {
    fill: white;
  }
  .chevron {
    margin-top: auto;
    align-self: flex-end;
    cursor: pointer;
    flex-shrink: 0;
  }
`;

//place high z-index on grandparent so that negative z-index tray still draws over map(but under navbar parent).
export const ZIndexWrapper = styled.div`
  z-index: 1000;
  position: relative;
`;

export const SideTrayPage = styled.div`
  display: flex;
  flex-direction: column;
  a {
    font-size: 17px;
  }
`;

export const CloseButton = styled(FaWindowClose)`
  &#close-tray {
    float: right;
    cursor: pointer;
    fill: ${props => props.theme.css.textColor};
    &:hover {
      fill: ${props => props.theme.css.secondaryVariantColor};
    }
  }
`;

export const SideTray = styled.div`
  height: 100%;
  overflow-y: auto;
  position: absolute;
  top: 0;
  right: 0;
  background-color: white;
  z-index: -1;
  width: 40rem;
  padding: 0.5rem 1rem;
  text-align: left;
  transition: transform 0.5s ease-in-out;
  box-shadow: 3px 0 4px rgba(0, 0, 0, 0.2);
  &.show {
    transform: translateX(40rem);
  }
  @media (max-width: 765px) {
    width: 20rem;
    &.show {
      transform: translateX(20rem);
    }
  }
`;
