import { Col } from 'react-bootstrap';
import { FaWindowClose } from 'react-icons/fa';
import styled from 'styled-components';

import { H1 } from '@/components/common/styles';

export const TrayHeader = styled(H1)`
  display: flex;
  align-items: end;
  border-bottom: 0;
  margin-bottom: 0.2rem;
`;

export const SideNavBar = styled.div`
  height: 100%;
  overflow-y: auto;
  overflow-x: hidden;
  padding-top: 1.5rem;
  display: flex;
  flex-direction: column;
  position: relative;
  align-items: center;
  grid-area: iconbar;
  background-color: ${props => props.theme.tenant.colour};
  transition: 0.5s width;
  width: 6rem;
  label {
    color: white;
    min-width: 8rem;
  }
  &.expanded {
    width: 16rem;
  }
  .nav-item svg {
    min-width: 2.4rem;
  }
  .to-bottom {
    margin-top: auto;
    margin-bottom: 1.6rem;
  }
  .nav-link {
    padding: 0.8rem 1.6rem;
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
  height: 100%;
  a {
    font-size: 1.7rem;
  }
`;

export const CloseButton = styled(FaWindowClose)`
  &#close-tray {
    float: right;
    cursor: pointer;
    margin-top: 0.5rem;
    fill: ${props => props.theme.bcTokens.typographyColorSecondary};
    &:hover {
      fill: ${props => props.theme.css.activeActionColor};
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
  width: 64rem;
  padding: 0.8rem 1.6rem;
  text-align: left;
  transition: transform 0.3s ease-in-out;
  box-shadow: 0.3rem 0 0.4rem rgba(0, 0, 0, 0.2);
  &.show {
    transform: translateX(64rem);
  }
  @media (max-width: 1225px) {
    width: 32rem;
    &.show {
      transform: translateX(32rem);
    }
  }
`;

export const HalfHeightDiv = styled.div`
  flex-direction: column;
  display: flex;
  height: 50%;
`;

export const ExportH3 = styled.h3`
  font-family: 'BcSans-Bold';
  font-size: 2rem;
  margin-bottom: 1rem;
  text-align: left;
  padding: 1rem 0 0.5rem 0;
  color: ${props => props.theme.bcTokens.typographyColorSecondary};
  border-bottom: solid 0.3rem ${props => props.theme.css.headerBorderColor};
  display: flex;
  align-items: flex-end;
`;

export const Content = styled.div`
  width: 100%;
  height: 100%;
  display: flex;
  flex-direction: column;
  margin-top: 2.4rem;
`;

export const ButtonBar = styled(Col)`
  gap: 1.5rem;
  align-items: center;
`;
