import clsx from 'classnames';
import React from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { exists } from '@/utils';

export interface ISimpleSectionHeaderProps {
  title: string;
  className?: string;
}

/**
 * Simple section header that takes a section title and optional children components.
 * If children components are supplied, they will be rendered on the right end of the header.
 * @param props customize the component by passing a title and optional class-name to modify the default component styling.
 * @returns The header component
 */
export const SimpleSectionHeader: React.FunctionComponent<
  React.PropsWithChildren<ISimpleSectionHeaderProps>
> = ({ title, className, children }) => {
  return (
    <StyledRow className={clsx('no-gutters', className)}>
      <Col xs="auto" className="d-flex justify-content-start px-2 my-1">
        {title ?? ''}
      </Col>
      {exists(children) && (
        <Col xs="auto" className="d-flex justify-content-end my-1">
          {children}
        </Col>
      )}
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
