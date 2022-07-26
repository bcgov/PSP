import React, { useState } from 'react';
import { Col, Collapse, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { ArrowDropDownIcon, ArrowDropUpIcon } from './SectionStyles';

interface SectionProps {
  header?: React.ReactNode;
  isCollapsable?: boolean;
  initiallyExpanded?: boolean;
}

export const Section: React.FC<SectionProps> = ({
  header,
  children,
  isCollapsable,
  initiallyExpanded,
}) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(!initiallyExpanded && true);
  return (
    <StyledFormSection>
      {header && (
        <StyledSectionHeader>
          <Row>
            <Col>{header}</Col>
            <Col xs="1">
              {isCollapsable && isCollapsed && (
                <ArrowDropDownIcon
                  onClick={() => {
                    setIsCollapsed(!isCollapsed);
                  }}
                />
              )}
              {isCollapsable && !isCollapsed && (
                <ArrowDropUpIcon
                  onClick={() => {
                    setIsCollapsed(!isCollapsed);
                  }}
                />
              )}
            </Col>
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

const StyledFormSection = styled.div`
  margin: 1.5rem;
  padding: 1.5rem;
  background-color: white;
  text-align: left;
`;
