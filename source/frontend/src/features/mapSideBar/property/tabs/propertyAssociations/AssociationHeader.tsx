import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { StyledIconWrapper } from '@/components/common/styles';

export interface IAssociationHeaderProps {
  icon: React.ReactNode;
  title: string;
  count?: number;
}

const AssociationHeader: React.FunctionComponent<
  React.PropsWithChildren<IAssociationHeaderProps>
> = props => {
  return (
    <StyledAssociationHeaderWrapper>
      <Row className="no-gutters">
        <Col xs="auto">{props.icon}</Col>
        <Col xs="auto" className="px-2 my-1">
          {props.title}
        </Col>
        <Col xs="auto" className="my-1">
          <StyledIconWrapper>{props.count || 0}</StyledIconWrapper>
        </Col>
      </Row>
    </StyledAssociationHeaderWrapper>
  );
};

export default AssociationHeader;

const StyledAssociationHeaderWrapper = styled.div`
  color: ${props => props.theme.bcTokens.iconsColorSecondary};
`;
