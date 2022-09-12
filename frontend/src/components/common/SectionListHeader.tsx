import { StyledSectionAddButton } from 'components/common/styles';
import { Claims } from 'constants/index';
import { useKeycloakWrapper } from 'hooks/useKeycloakWrapper';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { ImFileText2 } from 'react-icons/im';

export interface ISectionListHeaderProps {
  title: string;
  addButtonText?: string;
  onAdd?: () => void;
  claims: Claims[];
}

export const SectionListHeader: React.FunctionComponent<ISectionListHeaderProps> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const onClick = () => props.onAdd && props.onAdd();

  return (
    <Row className="no-gutters justify-content-between">
      <Col xs="auto" className="px-2 my-1">
        {props.title}
      </Col>
      <Col xs="auto" className="my-1">
        {hasClaim(props.claims) && (
          <StyledSectionAddButton onClick={onClick}>
            <ImFileText2 size="2rem" />
            &nbsp;{props.addButtonText ?? 'Add'}
          </StyledSectionAddButton>
        )}
      </Col>
    </Row>
  );
};
