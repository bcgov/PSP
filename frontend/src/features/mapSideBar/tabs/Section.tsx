import { StyledFormSection, StyledSectionHeader } from 'features/mapSideBar/tabs/SectionStyles';
import React from 'react';

export const Section: React.FC<{ header: string }> = ({ header, children }) => {
  return (
    <StyledFormSection>
      <StyledSectionHeader>{header}</StyledSectionHeader>
      {children}
    </StyledFormSection>
  );
};
