import clsx from 'classnames';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants/index';
import { useKeycloakWrapper } from '@/hooks/useKeycloakWrapper';
import { exists } from '@/utils';

export interface ISectionListHeaderProps {
  title: string;
  addButtonText?: string;
  addButtonIcon?: JSX.Element;
  cannotAddComponent?: JSX.Element;
  onButtonAction?: () => void;
  claims: Claims[];
  'data-testId'?: string;
  className?: string;
  isAddEnabled?: boolean;
}

export const SectionListHeader: React.FunctionComponent<
  React.PropsWithChildren<ISectionListHeaderProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const onClickButtonAction = () => props.onButtonAction && props.onButtonAction();

  return (
    <StyledRow className={clsx('no-gutters', props.className)}>
      <Col xs="auto" className="align-items-end">
        {props.title}
      </Col>
      <Col xs="auto" className="my-1">
        {hasClaim(props.claims) && exists(props.onButtonAction) && props.isAddEnabled !== false && (
          <StyledSectionAddButton onClick={onClickButtonAction} data-testid={props['data-testId']}>
            {props.addButtonIcon}
            &nbsp;{props.addButtonText ?? 'Add'}
          </StyledSectionAddButton>
        )}
        {!props.isAddEnabled && exists(props.cannotAddComponent) && (
          <span style={{ float: 'right' }}>{props.cannotAddComponent}</span>
        )}
      </Col>
    </StyledRow>
  );
};

const StyledRow = styled(Row)`
  display: flex;
  grid-template-columns: 1fr 0.5fr;
  justify-content: space-between;
  align-items: end;
  min-height: 4.5rem;
  .btn {
    margin: 0;
  }
`;
