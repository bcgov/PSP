import { Feature, Geometry } from 'geojson';

import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { TANTALIS_CrownLandTenures_Feature_Properties } from '@/models/layers/crownLand';
import { exists, formatUTCDateTime } from '@/utils';

export interface ICrownDetailsTabViewProps {
  crownFeature?: Feature<Geometry, TANTALIS_CrownLandTenures_Feature_Properties> | undefined;
}

export const CrownDetailsTabView: React.FunctionComponent<ICrownDetailsTabViewProps> = ({
  crownFeature,
}) => {
  if (!exists(crownFeature)) {
    return null;
  } else {
    return (
      <StyledSummarySection>
        <Section header="Crown Details">
          <SectionField label="Tenure stage">
            {crownFeature?.properties?.TENURE_STAGE ?? ''}
          </SectionField>
          <SectionField label="Tenure status">
            {crownFeature?.properties?.TENURE_STATUS ?? ''}
          </SectionField>
          <SectionField label="Tenure type">
            {crownFeature?.properties?.TENURE_TYPE ?? ''}
          </SectionField>
          <SectionField label="Tenure sub type">
            {crownFeature?.properties?.TENURE_SUBTYPE ?? ''}
          </SectionField>
          <SectionField label="Tenure purpose">
            {crownFeature?.properties?.TENURE_PURPOSE ?? ''}
          </SectionField>
          <SectionField label="Crown lands file">
            {crownFeature?.properties?.CROWN_LANDS_FILE ?? ''}
          </SectionField>
          <SectionField label="Tenure document">
            {crownFeature?.properties?.TENURE_DOCUMENT ?? ''}
          </SectionField>
          <SectionField label="Tenure expiry">
            {formatUTCDateTime(crownFeature?.properties?.TENURE_EXPIRY, 'MMM D, YYYY', false)}
          </SectionField>
          <SectionField label="Tenure legal description">
            {crownFeature?.properties?.TENURE_LEGAL_DESCRIPTION ?? ''}
          </SectionField>
          <SectionField label="Tenure area in hectares">
            {crownFeature?.properties?.TENURE_AREA_IN_HECTARES ?? ''}
          </SectionField>
          <SectionField label="Responsible business unit">
            {crownFeature?.properties?.RESPONSIBLE_BUSINESS_UNIT ?? ''}
          </SectionField>
          <SectionField label="Feature area square meters">
            {crownFeature?.properties?.FEATURE_AREA_SQM ?? ''}
          </SectionField>
          <SectionField label="Feature length meters">
            {crownFeature?.properties?.FEATURE_LENGTH_M ?? ''}
          </SectionField>
        </Section>
      </StyledSummarySection>
    );
  }
};

export default CrownDetailsTabView;
