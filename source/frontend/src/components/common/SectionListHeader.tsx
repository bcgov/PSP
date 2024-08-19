import clsx from 'classnames';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';

export interface ISectionListHeaderProps {
  title: string;
  addButtonText?: string;
  addButtonIcon?: JSX.Element;
  onAdd?: () => void;
  claims: Claims[];
  'data-testId'?: string;
  className?: string;
}

export const SectionListHeader: React.FunctionComponent<
  React.PropsWithChildren<ISectionListHeaderProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const onClick = () => props.onAdd && props.onAdd();

  return (
    <StyledRow className={clsx('no-gutters', props.className)}>
      <Col xs="auto" className="px-2 my-1">
        {props.title}
      </Col>
      <Col xs="auto" className="my-1">
        {hasClaim(props.claims) && props.onAdd && (
          <StyledSectionAddButton onClick={onClick} data-testid={props['data-testId']}>
            {props.addButtonIcon}
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
