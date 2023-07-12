import Row from 'react-bootstrap/Row';
import { Link } from 'react-router-dom';
import styled from 'styled-components';

export const LayerPopupTitle = styled.h5`
  font-weight: bold;
  font-size: 1.6rem;
  color: ${props => props.theme.css.headingTextColor};

  line-height: 2.4rem;
`;

export const MenuRow = styled(Row)`
  text-align: center;
  padding-bottom: 1rem;
`;

export const StyledLink = styled(Link)`
  padding: 0 0.4rem;
`;
