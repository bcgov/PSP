import styled from 'styled-components';

export const TrayHeader = styled.h3`
  font-size: 2rem;
  padding: 0.8rem;
  border-bottom: 0.4rem solid ${props => props.theme.css.primaryColor};
  border-top-left-radius: 1rem;
  border-top-right-radius: 1rem;
  margin-bottom: 3.2rem;
  background-color: ${props => props.theme.css.primaryColor};
  color: white;
  font-family: '' BCSans ',Fallback,sans-serif';
`;

//place high z-index on grandparent so that negative z-index tray still draws over map(but under navbar parent).
export const ZIndexWrapper = styled.div`
  z-index: 1000;
  position: relative;
`;

export const ActivityTrayPage = styled.div`
  display: flex;
  overflow-y: auto;
  flex-direction: column;
  height: 100%;
  a {
    font-size: 1.7rem;
  }
`;

export const CloseButton = styled.div`
  &#close-tray {
    float: right;
    cursor: pointer;
    fill: ${props => props.theme.css.textColor};
    &:hover {
      fill: ${props => props.theme.css.secondaryVariantColor};
    }
  }
`;

export const ActivityTray = styled.div`
  height: 100%;
  width: 100%
  overflow-y: hidden;
  background-color: white;
  z-index: 10000;
  border-radius: 1rem;
  text-align: left;
  transition: transform 0.5s ease-in-out;
  box-shadow: 0.3rem 0 0.4rem rgba(0, 0, 0, 0.2);
`;
