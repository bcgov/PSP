import React, { useState } from 'react';
import { Col, Collapse, Row } from 'react-bootstrap';

import {
  ArrowDropDownIcon,
  ArrowDropUpIcon,
  StyledFormSection,
  StyledSectionHeader,
} from './SectionStyles';

interface SectionProps {
  header: React.ReactNode;
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

      <Collapse in={!isCollapsable || !isCollapsed}>
        <div>{children}</div>
      </Collapse>
    </StyledFormSection>
  );
};
