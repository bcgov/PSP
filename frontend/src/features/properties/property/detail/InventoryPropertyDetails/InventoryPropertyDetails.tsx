import { SectionField } from 'features/mapSideBar/tabs/SectionField';
import {
  StyledFormSection,
  StyledScrollable,
  StyledSectionHeader,
} from 'features/mapSideBar/tabs/SectionStyles';
import React from 'react';

interface IInventoryPropertyDetailsProps {}

/**
 * Provides basic property information, as displayed under "Property Details" tab on the Property Information slide-out
 * @returns the rendered property details panel
 */
export const InventoryPropertyDetails: React.FC<IInventoryPropertyDetailsProps> = () => {
  return (
    <StyledScrollable>
      <Section title="Property attributes">
        <SectionField label="MOTI region"></SectionField>
        <SectionField label="Highways district"></SectionField>
        <SectionField label="Electoral district"></SectionField>
        <SectionField label="Agricultural Land Reserve"></SectionField>
        <SectionField label="Land parcel type"></SectionField>
        <SectionField label="Municipal zoning"></SectionField>
        <SectionField label="Anomalies"></SectionField>
      </Section>

      <Section title="Tenure Status">
        <SectionField label="Tenure status"></SectionField>
        <SectionField label="Provincial Public Hwy"></SectionField>
        <SectionField label="Highway / Road"></SectionField>
        <SectionField label="Adjacent land"></SectionField>
      </Section>

      <Section title="First Nations Information">
        <SectionField label="Band name"></SectionField>
        <SectionField label="Reserve name"></SectionField>
      </Section>

      <Section title="Area"></Section>

      <Section title="Notes"></Section>
    </StyledScrollable>
  );
};

export const Section: React.FC<{ title: string }> = ({ title, children }) => {
  return (
    <StyledFormSection>
      <StyledSectionHeader>{title}</StyledSectionHeader>
      {children}
    </StyledFormSection>
  );
};
