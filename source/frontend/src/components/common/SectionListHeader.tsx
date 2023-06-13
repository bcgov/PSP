import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import { ImFileText2 } from 'react-icons/im';
import styled from 'styled-components';

import { StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

export interface ISectionListHeaderProps {
  title: string;
  addButtonText?: string;
  addButtonIcon?: 'add' | null;
  onAdd?: () => void;
  claims: Claims[];
}

export const SectionListHeader: React.FunctionComponent<
  React.PropsWithChildren<ISectionListHeaderProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const onClick = () => props.onAdd && props.onAdd();

  let icon;
  switch (props.addButtonIcon) {
    case 'add': {
      icon = <FaPlus size="2rem" />;
      break;
    }
    default: {
      icon = <ImFileText2 size="2rem" />;
      break;
    }
  }

  return (
    <StyledRow className="no-gutters">
      <Col xs="auto" className="px-2 my-1">
        {props.title}
      </Col>
      <Col xs="auto" className="my-1">
        {hasClaim(props.claims) && (
          <StyledSectionAddButton onClick={onClick}>
            {icon}
            &nbsp;{props.addButtonText ?? 'Add'}
          </StyledSectionAddButton>
        )}
      </Col>
    </StyledRow>
  );
};

const StyledRow = styled(Row)`
  justify-content: space-between;
  align-items: end;
  min-height: 4.5rem;
  .btn {
    margin: 0;
  }
`;
