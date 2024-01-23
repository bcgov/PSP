import * as React from 'react';
import styled from 'styled-components';

import YesNoButtons from '@/components/common/buttons/YesNoButtons';
import EditButton from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { H2 } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import AreaContainer from '@/components/measurements/AreaContainer';
import * as API from '@/constants/API';
import { Claims } from '@/constants/claims';
import Roles from '@/constants/roles';
import { TakesStatusTypes } from '@/constants/takesStatusTypes';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import { isAcquisitionFile } from '@/features/mapSideBar/acquisition/tabs/agreement/update/models';
import StatusUpdateSolver from '@/features/mapSideBar/acquisition/tabs/fileDetails/detail/statusUpdateSolver';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { getApiPropertyName, prettyFormatDate, prettyFormatUTCDate } from '@/utils';

import { StyledBorderSection, StyledNoTabSection } from '../styles';

export interface ITakesDetailViewProps {
  takes: ApiGen_Concepts_Take[];
  allTakesCount: number;
  loading: boolean;
  fileProperty: ApiGen_Concepts_FileProperty;
  onEdit: (edit: boolean) => void;
}

export const TakesDetailView: React.FunctionComponent<ITakesDetailViewProps> = ({
  takes,
  allTakesCount,
  fileProperty,
  loading,
  onEdit,
}) => {
  const cancelledTakes = takes?.filter(
    t => t.takeStatusTypeCode?.id === TakesStatusTypes.CANCELLED,
  );
  const nonCancelledTakes = takes?.filter(
    t => t.takeStatusTypeCode?.id !== TakesStatusTypes.CANCELLED,
  );
  const takesNotInFile = allTakesCount - (takes?.length ?? 0);

  const { getCodeById } = useLookupCodeHelpers();
  const { hasClaim, hasRole } = useKeycloakWrapper();

  const file = fileProperty.file;

  const statusSolver = new StatusUpdateSolver(isAcquisitionFile(file) ? file : null);

  const canEditDetails = () => {
    if (hasRole(Roles.SYSTEM_ADMINISTRATOR) || statusSolver.canEditDetails()) {
      return true;
    }
    return false;
  };

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <StyledEditWrapper>
        {onEdit !== undefined && hasClaim(Claims.PROPERTY_EDIT) && canEditDetails() ? (
          <EditButton
            title="Edit takes"
            onClick={() => {
              onEdit(true);
            }}
          />
        ) : null}
        {!canEditDetails() && (
          <TooltipIcon
            toolTipId={`${fileProperty?.fileId || 0}-summary-cannot-edit-tooltip`}
            toolTip={cannotEditMessage}
          />
        )}
      </StyledEditWrapper>
      <Section>
        <H2>Takes for {getApiPropertyName(fileProperty.property).value}</H2>
        <StyledBlueSection>
          <SectionField
            labelWidth="8"
            label="Takes for this property in the current file"
            tooltip="The number of takes in completed, In-progress or cancelled state for this property in this acquisition file."
          >
            {takes?.length ?? 0}
          </SectionField>
          <SectionField
            labelWidth="8"
            label="Takes for this property in other files"
            tooltip="The number of takes in completed, In-progress or cancelled state for this property, in files other than this acquisition file. The other files can be found under the Acquisition section of the PIMS Files tab"
            valueTestId="takes-in-other-files"
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
              {take.takeTypeCode?.id ? getCodeById(API.TAKE_TYPES, take.takeTypeCode.id) : ''}
            </SectionField>
            <SectionField label="Take status *">
              {take.takeStatusTypeCode?.id
                ? getCodeById(API.TAKE_STATUS_TYPES, take.takeStatusTypeCode.id)
                : ''}
            </SectionField>
            <SectionField label="Site contamination">
              {take.takeSiteContamTypeCode?.id
                ? getCodeById(API.TAKE_SITE_CONTAM_TYPES, take.takeSiteContamTypeCode.id)
                : ''}
            </SectionField>
            <SectionField label="Description" labelWidth="12">
              {take.description}
            </SectionField>
            <StyledNoTabSection header="Area">
              <StyledBorderSection>
                <SectionField
                  label="Is there a new highway dedication? *"
                  labelWidth="8"
                  tooltip="The term new highway dedication includes municipal road or provincial public highway."
                >
                  <YesNoButtons
                    id="newRightOfWayToggle"
                    disabled
                    value={take.isNewHighwayDedication ?? undefined}
                  />
                </SectionField>
                {take.isNewHighwayDedication && (
                  <SectionField label="Area" labelWidth="12">
                    <AreaContainer landArea={take.newHighwayDedicationArea ?? undefined} />
                  </SectionField>
                )}
                <SectionField
                  label="Is this being acquired for MoTI inventory? *"
                  labelWidth="8"
                  tooltip="The property will be added to inventory."
                  className="pt-4"
                >
                  <YesNoButtons
                    id="addPropertyToggle"
                    disabled
                    value={take.isAcquiredForInventory ?? undefined}
                  />
                </SectionField>
              </StyledBorderSection>
              <StyledBorderSection>
                <SectionField
                  label="Is there a new registered interest in land (SRW, Easement or Covenant)? *"
                  labelWidth="8"
                >
                  <YesNoButtons
                    id="newInterestInSrwToggle"
                    disabled
                    value={take.isNewInterestInSrw ?? undefined}
                  />
                </SectionField>
                {take.isNewInterestInSrw && (
                  <>
                    <SectionField label="Area" labelWidth="12">
                      <AreaContainer landArea={take.statutoryRightOfWayArea ?? undefined} />
                    </SectionField>

                    <SectionField label="SRW end date" labelWidth="3" contentWidth="4">
                      {prettyFormatDate(take.srwEndDt ?? undefined)}
                    </SectionField>
                  </>
                )}
              </StyledBorderSection>
              <StyledBorderSection>
                <SectionField label="Is a there a new Land Act tenure? *" labelWidth="8">
                  <YesNoButtons
                    id="landActToggle"
                    disabled
                    value={take.isNewLandAct ?? undefined}
                  />
                </SectionField>
                {take.isNewLandAct && (
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
                <SectionField
                  label="Is there a new License for Construction Access (TLCA/LTC)? *"
                  labelWidth="8"
                >
                  <YesNoButtons
                    id="licenseToConstructToggle"
                    disabled
                    value={take.isNewLicenseToConstruct ?? undefined}
                  />
                </SectionField>
                {take.isNewLicenseToConstruct && (
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
                  <YesNoButtons
                    id="surplusToggle"
                    disabled
                    value={take.isThereSurplus ?? undefined}
                  />
                </SectionField>
                {take.isThereSurplus && (
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
