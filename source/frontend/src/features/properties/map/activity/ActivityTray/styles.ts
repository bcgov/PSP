import { Button } from 'components/common/buttons/Button';
import styled from 'styled-components';

export const TrayHeader = styled.div`
  font-size: 2rem;
  font-weight: bold;
  padding: 1rem;
  background-color: ${props => props.theme.css.primaryColor};
  color: white;
  position: sticky;
`;

export const ActivityTrayPage = styled.div`
  padding: 1.5rem;
`;

export const CloseButton = styled(Button)`
  &#close-tray {
    float: right;
    padding: 0rem;
    cursor: pointer;
    fill: ${props => props.theme.css.textColor};
    &:hover {
      fill: ${props => props.theme.css.secondaryVariantColor};
    }
  }
`;

export const ActivityTray = styled.div`
  width: 100%;
  height: 100%;
  overflow: auto;
  border-radius: 1rem;
  text-align: left;
  transition: transform 0.5s ease-in-out;
`;
