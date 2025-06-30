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
  onAdd?: () => void;
  onGenerate?: () => void;
  claims?: Claims[];
  'data-testId'?: string;
  className?: string;
  isAddEnabled?: boolean;
}

export const SectionListHeader: React.FunctionComponent<
  React.PropsWithChildren<ISectionListHeaderProps>
> = props => {
  const { hasClaim } = useKeycloakWrapper();
  const onClickAdd = () => props.onAdd && props.onAdd();
  const onClickGenerate = () => props.onGenerate && props.onGenerate();

  return (
    <StyledRow className={clsx('no-gutters', props.className)}>
      <Col xs="auto" className="align-items-end">
        {props.title}
      </Col>
      <Col xs="auto" className="my-1">
        {hasClaim(props.claims) && exists(props.onAdd) && props.isAddEnabled !== false && (
          <StyledSectionAddButton onClick={onClickAdd} data-testid={props['data-testId']}>
            {props.addButtonIcon}
            &nbsp;{props.addButtonText ?? 'Add'}
          </StyledSectionAddButton>
        )}
        {exists(props.onGenerate) && props.isAddEnabled !== false && (
          <StyledSectionAddButton
            onClick={onClickGenerate}
            data-testid={props['data-testId']}
            title="Download File"
          >
            {props.addButtonIcon}
            &nbsp;{props.addButtonText ?? 'Generate'}
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
  display: grid;
  grid-template-columns: 1fr 0.5fr;
  justify-content: space-between;
  align-items: end;
  min-height: 4.5rem;
  .btn {
    margin: 0;
  }
`;
