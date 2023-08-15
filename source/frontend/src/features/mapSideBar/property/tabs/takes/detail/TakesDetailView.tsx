import * as React from 'react';
import styled from 'styled-components';

import YesNoButtons from '@/components/common/buttons/YesNoButtons';
import EditButton from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { H2 } from '@/components/common/styles';
import AreaContainer from '@/components/measurements/AreaContainer';
import * as API from '@/constants/API';
import { Claims } from '@/constants/claims';
import { TakesStatusTypes } from '@/constants/takesStatusTypes';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { Api_PropertyFile } from '@/models/api/PropertyFile';
import { Api_Take } from '@/models/api/Take';
import { getApiPropertyName, prettyFormatDate, prettyFormatUTCDate } from '@/utils';

import { StyledBorderSection, StyledNoTabSection } from '../styles';

export interface ITakesDetailViewProps {
  takes: Api_Take[];
  allTakesCount: number;
  loading: boolean;
  fileProperty: Api_PropertyFile;
  onEdit: (edit: boolean) => void;
}

export const TakesDetailView: React.FunctionComponent<ITakesDetailViewProps> = ({
  takes,
  allTakesCount,
  fileProperty,
  loading,
  onEdit,
}) => {
  const cancelledTakes = takes?.filter(t => t.takeStatusTypeCode === TakesStatusTypes.CANCELLED);
  const nonCancelledTakes = takes?.filter(t => t.takeStatusTypeCode !== TakesStatusTypes.CANCELLED);
  const takesNotInFile = allTakesCount - (takes?.length ?? 0);

  const { getCodeById } = useLookupCodeHelpers();
  const { hasClaim } = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <StyledEditWrapper>
        {onEdit !== undefined && hasClaim(Claims.PROPERTY_EDIT) && (
          <EditButton
            title="Edit takes"
            onClick={() => {
              onEdit(true);
            }}
          />
        )}
      </StyledEditWrapper>
      <Section>
        <H2>Takes for {getApiPropertyName(fileProperty.property).value}</H2>
        <StyledBlueSection>
          <SectionField
            labelWidth="8"
            label="Takes for this property in the current file"
            tooltip="The number of takes in completed, , In-progress or cancelled state for this property in this acquisition file."
          >
            {takes?.length ?? 0}
          </SectionField>
          <SectionField
            labelWidth="8"
            label="Takes for this property in other files"
            tooltip="The number of takes in completed, In-progress or cancelled state for this property, in files other than this acquisition file. The other files can be found under the Acquisition section of the PIMS Files tab"
          >
            {takesNotInFile}
          </SectionField>
        </StyledBlueSection>
      </Section>
      {[...nonCancelledTakes, ...cancelledTakes].map((take, index) => {
        return (
          <Section key={take.id} isCollapsable initiallyExpanded data-testid={`take-${index}`}>
            <H2>Take {index + 1}</H2>
            <SectionField label="Take added on">
              {prettyFormatUTCDate(take.appCreateTimestamp)}
            </SectionField>
            <SectionField label="Take type *">
              {take.takeTypeCode ? getCodeById(API.TAKE_TYPES, take.takeTypeCode) : ''}
            </SectionField>
            <SectionField label="Take status *">
              {take.takeStatusTypeCode
                ? getCodeById(API.TAKE_STATUS_TYPES, take.takeStatusTypeCode)
                : ''}
            </SectionField>
            <SectionField label="Site contamination">
              {take.takeSiteContamTypeCode
                ? getCodeById(API.TAKE_SITE_CONTAM_TYPES, take.takeSiteContamTypeCode)
                : ''}
            </SectionField>
            <SectionField label="Description" labelWidth="12">
              {take.description}
            </SectionField>
            <StyledNoTabSection header="Area">
              <StyledBorderSection>
                <SectionField label="Is there a new right of way? *" labelWidth="8">
                  <YesNoButtons id="newRightOfWayToggle" disabled value={take.isNewRightOfWay} />
                </SectionField>
                {take.isNewRightOfWay && (
                  <SectionField label="Area" labelWidth="12">
                    <AreaContainer landArea={take.newRightOfWayArea ?? undefined} />
                  </SectionField>
                )}
              </StyledBorderSection>
              <StyledBorderSection>
                <SectionField label="Is there a Statutory Right of Way: (SRW)? *" labelWidth="8">
                  <YesNoButtons
                    id="statutoryRightOfWayToggle"
                    disabled
                    value={take.isStatutoryRightOfWay}
                  />
                </SectionField>
                {take.isStatutoryRightOfWay && (
                  <>
                    <SectionField label="Area" labelWidth="12">
                      <AreaContainer landArea={take.statutoryRightOfWayArea ?? undefined} />
                    </SectionField>
                  </>
                )}
              </StyledBorderSection>
              <StyledBorderSection>
                <SectionField
                  label="Is there Land Act-Reserve(s)/Withdrawal(s)/Notation(s)? *"
                  labelWidth="8"
                >
                  <YesNoButtons id="landActToggle" disabled value={take.isLandAct} />
                </SectionField>
                {take.isLandAct && (
                  <>
                    <SectionField label="Land Act" labelWidth="3">
                      {take.landActTypeCode
                        ? take.landActTypeCode.id + ' ' + take.landActTypeCode.description
                        : ''}
                    </SectionField>

                    <SectionField label="Area" labelWidth="12">
                      <AreaContainer landArea={take.landActArea ?? undefined} />
                    </SectionField>

                    <SectionField label="End date" labelWidth="3" contentWidth="4">
                      {prettyFormatDate(take.landActEndDt ?? undefined)}
                    </SectionField>
                  </>
                )}
              </StyledBorderSection>
              <StyledBorderSection>
                <SectionField label="Is there a License to Construct (LTC)? *" labelWidth="8">
                  <YesNoButtons
                    id="licenseToConstructToggle"
                    disabled
                    value={take.isLicenseToConstruct}
                  />
                </SectionField>
                {take.isLicenseToConstruct && (
                  <>
                    <SectionField label="Area" labelWidth="12">
                      <AreaContainer landArea={take.licenseToConstructArea ?? undefined} />
                    </SectionField>

                    <SectionField label="LTC end date" labelWidth="3" contentWidth="4">
                      {prettyFormatDate(take.ltcEndDt ?? undefined)}
                    </SectionField>
                  </>
                )}
              </StyledBorderSection>
            </StyledNoTabSection>
            <StyledNoTabSection header="Surplus">
              <StyledBorderSection>
                <SectionField label="Is there a Surplus? *" labelWidth="8">
                  <YesNoButtons id="surplusToggle" disabled value={take.isSurplus} />
                </SectionField>
                {take.isSurplus && (
                  <SectionField label="Area" labelWidth="12">
                    <AreaContainer landArea={take.surplusArea ?? undefined} />
                  </SectionField>
                )}
              </StyledBorderSection>
            </StyledNoTabSection>
          </Section>
        );
      })}
    </StyledSummarySection>
  );
};

const StyledBlueSection = styled.div`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  padding: 1rem;
`;

export default TakesDetailView;
