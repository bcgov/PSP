import YesNoButtons from 'components/common/buttons/YesNoButtons';
import EditButton from 'components/common/EditButton';
import { H2, StyledDivider } from 'components/common/styles';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import AreaContainer from 'components/measurements/AreaContainer';
import * as API from 'constants/API';
import { Claims } from 'constants/claims';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import useLookupCodeHelpers from 'hooks/useLookupCodeHelpers';
import { Api_PropertyFile } from 'models/api/PropertyFile';
import { Api_Take } from 'models/api/Take';
import * as React from 'react';
import styled from 'styled-components';
import { formatApiDateTime, prettyFormatDate } from 'utils';
import { getApiPropertyName } from 'utils/mapPropertyUtils';

import { Section } from '../../Section';
import { SectionField } from '../../SectionField';
import {
  StyledBorderSection,
  StyledEditWrapper,
  StyledNoTabSection,
  StyledSummarySection,
} from '../styles';

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
  const cancelledTakes = takes?.filter(t => t.takeStatusTypeCode === 'CANCELLED');
  const nonCancelledTakes = takes?.filter(t => t.takeStatusTypeCode !== 'CANCELLED');

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
          <SectionField labelWidth="6" label="Total takes for this property">
            {allTakesCount ?? 0}
          </SectionField>
          <i>Count is inclusive of all files</i>
          <SectionField
            labelWidth="6"
            label="Total cancelled takes for this property"
            className="mt-3"
          >
            {cancelledTakes?.length ?? 0}
          </SectionField>
          <i>Count is inclusive of all files</i>
          <StyledDivider />
          <p>
            There are <b>{takes?.length ?? 0} take(s)</b> for this property on this file.
          </p>
        </StyledBlueSection>
      </Section>
      {[...nonCancelledTakes, ...cancelledTakes].map((take, index) => (
        <Section key={take.id} isCollapsable initiallyExpanded data-testid={`take-${index}`}>
          <H2>Take {index + 1}</H2>
          <SectionField label="Take added on">
            {prettyFormatDate(formatApiDateTime(take.appCreateTimestamp))}
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
              <SectionField label="Is there a Section 16? *" labelWidth="8">
                <YesNoButtons id="section16Toggle" disabled value={take.isSection16} />
              </SectionField>
              {take.isSection16 && (
                <>
                  <SectionField label="Area" labelWidth="12">
                    <AreaContainer landArea={take.section16Area ?? undefined} />
                  </SectionField>
                  {take.section16EndDt && (
                    <SectionField label="Section 16 end date">
                      {prettyFormatDate(take.section16EndDt ?? undefined)}
                    </SectionField>
                  )}
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
                  {take.ltcEndDt && (
                    <SectionField label="LTC end date">
                      {prettyFormatDate(take.ltcEndDt ?? undefined)}
                    </SectionField>
                  )}
                </>
              )}
            </StyledBorderSection>
          </StyledNoTabSection>
          <StyledNoTabSection header="Surplus">
            <StyledBorderSection>
              <SectionField label="Is there a Surplus? *" labelWidth="8">
                <YesNoButtons
                  id="surplusSeveranceToggle"
                  disabled
                  value={take.isSurplusSeverance}
                />
              </SectionField>
              {take.isSurplusSeverance && (
                <SectionField label="Area" labelWidth="12">
                  <AreaContainer landArea={take.surplusSeveranceArea ?? undefined} />
                </SectionField>
              )}
            </StyledBorderSection>
          </StyledNoTabSection>
        </Section>
      ))}
    </StyledSummarySection>
  );
};

const StyledBlueSection = styled.div`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  padding: 0.5rem;
`;

export default TakesDetailView;
