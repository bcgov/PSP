import { Button } from 'components/common/buttons/Button';
import styled from 'styled-components';

export const TrayHeader = styled.div`
  font-size: 2rem;
  font-weight: bold;
  padding: 0.8rem;
  border-bottom: 0.4rem solid ${props => props.theme.css.primaryColor};
  margin-bottom: 1rem;
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
    padding: 0rem;
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
