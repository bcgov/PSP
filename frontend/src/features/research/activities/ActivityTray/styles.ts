import { Button } from 'components/common/buttons/Button';
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

export const CloseButton = styled(Button)`
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
  border-radius: 1rem;
  text-align: left;
  transition: transform 0.5s ease-in-out;
`;
