import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

export interface IAssociationHeaderProps {
  icon: React.ReactNode;
  title: string;
  count?: number;
}

const AssociationHeader: React.FunctionComponent<
  React.PropsWithChildren<IAssociationHeaderProps>
> = props => {
  return (
    <Row className="no-gutters">
      <Col xs="auto">{props.icon}</Col>
      <Col xs="auto" className="px-2 my-1">
        {props.title}
      </Col>
      <Col xs="auto" className="my-1">
        <StyledIconWrapper>{props.count || 0}</StyledIconWrapper>
      </Col>
    </Row>
  );
};

export default AssociationHeader;

const StyledIconWrapper = styled.div`
  color: white;
  background-color: ${props => props.theme.css.iconLightColor};

  font-size: 1.5rem;

  border-radius: 50%;

  width: 2.5rem;
  height: 2.5rem;
  padding: 1rem;

  display: flex;
  justify-content: center;
  align-items: center;
`;
