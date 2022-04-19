import React from 'react';

import { StyledFormSection, StyledSectionHeader } from './SectionStyles';

export const Section: React.FC<{ header: string }> = ({ header, children }) => {
  return (
    <StyledFormSection>
      <StyledSectionHeader>{header}</StyledSectionHeader>
      {children}
    </StyledFormSection>
  );
};
