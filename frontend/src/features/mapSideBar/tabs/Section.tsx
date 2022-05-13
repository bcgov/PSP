import React, { useState } from 'react';
import { Collapse } from 'react-bootstrap';

import {
  ArrowDropDownIcon,
  ArrowDropUpIcon,
  StyledFormSection,
  StyledSectionHeader,
} from './SectionStyles';

interface SectionProps {
  icon?: string;
  header: string;
  isCollapsable?: boolean;
  initiallyExpanded?: boolean;
}

export const Section: React.FC<SectionProps> = ({
  icon,
  header,
  children,
  isCollapsable,
  initiallyExpanded,
}) => {
  const [isCollapsed, setIsCollapsed] = useState<boolean>(!initiallyExpanded && true);
  return (
    <StyledFormSection>
      <StyledSectionHeader>
        {icon}
        {header}

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
      </StyledSectionHeader>

      <Collapse in={!isCollapsable || !isCollapsed}>
        <div>{children}</div>
      </Collapse>
    </StyledFormSection>
  );
};
