import clsx from 'classnames';
import React, { useState } from 'react';
import { Col, Collapse, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ArrowDropDownIcon, ArrowDropUpIcon } from './SectionStyles';

interface SectionProps {
  header?: React.ReactNode;
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
  children,
  title,
  isCollapsable,
  initiallyExpanded,
  noPadding,
  className,
  ...rest
}) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(!initiallyExpanded && true);
  return (
    <StyledFormSection
      className={clsx('form-section', className)}
      data-testid={rest['data-testid']}
      noPadding={noPadding}
    >
      {header && (
        <StyledSectionHeader>
          <Row className="no-gutters">
            <Col>{header}</Col>
            {isCollapsable && (
              <Col xs="1">
                {isCollapsed && (
                  <ArrowDropDownIcon
                    title={`expand-${title ?? 'section'}`}
                    onClick={(event: React.MouseEvent<SVGElement>) => {
                      event.preventDefault();
                      setIsCollapsed(!isCollapsed);
                    }}
                  />
                )}
                {!isCollapsed && (
                  <ArrowDropUpIcon
                    title={`collapse-${title ?? 'section'}`}
                    onClick={(event: React.MouseEvent<SVGElement>) => {
                      event.preventDefault();
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

const StyledSectionHeader = styled.h2`
  font-weight: bold;
  color: ${props => props.theme.css.primaryColor};
  border-bottom: 0.2rem ${props => props.theme.css.primaryColor} solid;
  margin-bottom: 2rem;
`;

const StyledFormSection = styled.div<{ noPadding?: boolean }>`
  margin: ${props => (props.noPadding === true ? '' : '1.5rem')};
  padding: ${props => (props.noPadding === true ? '' : '1.5rem')};
  background-color: white;
  text-align: left;
  border-radius: 0.5rem;
`;
