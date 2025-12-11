import clsx from 'classnames';
import React, { useState } from 'react';
import { Col, Collapse, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { exists } from '@/utils';

import { ArrowDropDownIcon, ArrowDropUpIcon } from './SectionStyles';

interface SectionProps {
  header?: React.ReactNode;
  isStyledHeader?: boolean;
  isCollapsable?: boolean;
  initiallyExpanded?: boolean;
  noPadding?: boolean;
}

export const Section: React.FC<
  React.PropsWithChildren<SectionProps & React.HTMLAttributes<HTMLDivElement>> & {
    'data-testid'?: string;
  }
> = ({
  header,
  isStyledHeader,
  children,
  title,
  isCollapsable,
  initiallyExpanded,
  noPadding,
  className,
  ...rest
}) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(!(initiallyExpanded === true));
  return (
    <StyledFormSection
      className={clsx('form-section', className)}
      data-testid={rest['data-testid']}
      noPadding={noPadding}
    >
      {exists(header) && (
        <StyledSectionHeader isStyledHeader={isStyledHeader}>
          <Row className="no-gutters">
            <Col className="align-content-end">{header}</Col>
            {isCollapsable && (
              <Col xs="auto" className="pl-2 d-flex align-items-end">
                {isCollapsed && (
                  <ArrowDropDownIcon
                    title={`expand-${title ?? 'section'}`}
                    onClick={(event: React.MouseEvent<SVGElement>) => {
                      event.preventDefault();
                      event.stopPropagation();
                      setIsCollapsed(!isCollapsed);
                    }}
                  />
                )}
                {!isCollapsed && (
                  <ArrowDropUpIcon
                    title={`collapse-${title ?? 'section'}`}
                    onClick={(event: React.MouseEvent<SVGElement>) => {
                      event.preventDefault();
                      event.stopPropagation();
                      setIsCollapsed(!isCollapsed);
                    }}
                  />
                )}
              </Col>
            )}
          </Row>
        </StyledSectionHeader>
      )}

      <Collapse in={!isCollapsable || !isCollapsed}>
        <div>{children}</div>
      </Collapse>
    </StyledFormSection>
  );
};

export const StyledSectionHeader = styled.h2<{ isStyledHeader?: boolean }>`
  font-size: ${props => (props.isStyledHeader === true ? '1.0em' : '')};
  font-weight: ${props => (props.isStyledHeader === true ? '' : 'bold')};
  color: ${props => props.theme.css.headerTextColor};
  border-bottom: 0.2rem ${props => props.theme.css.headerBorderColor} solid;
  margin-bottom: 2.4rem;
`;

const StyledFormSection = styled.div<{ noPadding?: boolean }>`
  margin: ${props => (props.noPadding === true ? '' : '1.6rem')};
  padding: ${props => (props.noPadding === true ? '' : '1.6rem')};
  background-color: white;
  text-align: left;
  border-radius: 0.5rem;
`;
