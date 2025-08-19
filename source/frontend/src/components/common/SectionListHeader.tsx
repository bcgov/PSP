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
  'title-data-testId'?: string;
  'button-data-testId'?: string;
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
      <Col
        xs="auto"
        className="align-items-end primary-column"
        data-testid={props['title-data-testId']}
      >
        {props.title}
      </Col>

      {hasClaim(props.claims) && exists(props.onButtonAction) && props.isAddEnabled !== false && (
        <Col xs="auto" className=" my-1 secondary-column">
          <StyledSectionAddButton
            onClick={onClickButtonAction}
            data-testid={props['button-data-testId']}
          >
            {props.addButtonIcon}
            &nbsp;{props.addButtonText ?? 'Add'}
          </StyledSectionAddButton>
        </Col>
      )}
      {!props.isAddEnabled && exists(props.cannotAddComponent) && (
        <Col xs="auto" className="my-1 secondary-column">
          <span style={{ float: 'right' }}>{props.cannotAddComponent}</span>
        </Col>
      )}
    </StyledRow>
  );
};

const StyledRow = styled(Row)`
  display: flex;
  justify-content: space-between;
  align-items: end;
  min-height: 4.5rem;
  .btn {
    margin: 0;
  }

  .primary-column {
    flex: 1 1 0;
  }

  .secondary-column {
    flex: 0.5 1 0;
  }
`;
