import { Col } from 'react-bootstrap';
import styled from 'styled-components';

export const LabelCol = styled(Col)`
  border-right: solid 1px ${props => props.theme.css.primaryColor};
  padding-bottom: 1rem;
`;

export const SubTitle = styled.h2`
  color: ${props => props.theme.css.primaryColor};
`;
