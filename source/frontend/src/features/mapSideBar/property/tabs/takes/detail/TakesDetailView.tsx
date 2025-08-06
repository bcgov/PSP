import { Col, Row } from 'react-bootstrap';
import { FaPlus } from 'react-icons/fa';
import styled from 'styled-components';

import { RemoveIconButton } from '@/components/common/buttons';
import EditButton from '@/components/common/buttons/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SectionListHeader } from '@/components/common/SectionListHeader';
import { H2 } from '@/components/common/styles';
import TooltipIcon from '@/components/common/TooltipIcon';
import AreaContainer from '@/components/measurements/AreaContainer';
import { Claims, Roles } from '@/constants';
import * as API from '@/constants/API';
import { isAcquisitionFile } from '@/features/mapSideBar/acquisition/add/models';
import { cannotEditMessage } from '@/features/mapSideBar/acquisition/common/constants';
import AcquisitionFileStatusUpdateSolver from '@/features/mapSideBar/acquisition/tabs/fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { getDeleteModalProps, useModalContext } from '@/hooks/useModalContext';
import { ApiGen_CodeTypes_AcquisitionTakeStatusTypes } from '@/models/api/generated/ApiGen_CodeTypes_AcquisitionTakeStatusTypes';
import { ApiGen_CodeTypes_LandActTypes } from '@/models/api/generated/ApiGen_CodeTypes_LandActTypes';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { ApiGen_Concepts_Take } from '@/models/api/generated/ApiGen_Concepts_Take';
import { getApiPropertyName, prettyFormatDate, prettyFormatUTCDate } from '@/utils';
import { booleanToYesNoUnknownString } from '@/utils/formUtils';

import { StyledBorderSection, StyledNoTabSection } from '../styles';

export interface ITakesDetailViewProps {
  takes: ApiGen_Concepts_Take[];
  allTakesCount: number;
  loading: boolean;
  fileProperty: ApiGen_Concepts_FileProperty;
  onEdit: (takeId: number) => void;
  onAdd: () => void;
  onDelete: (takeId: number) => void;
}

export const TakesDetailView: React.FunctionComponent<ITakesDetailViewProps> = ({
  takes,
  allTakesCount,
  fileProperty,
  loading,
  onEdit,
  onAdd,
  onDelete,
}) => {
  const cancelledTakes = takes?.filter(
    t => t.takeStatusTypeCode?.id === ApiGen_CodeTypes_AcquisitionTakeStatusTypes.CANCELLED,
  );
  const nonCancelledTakes = takes?.filter(
    t => t.takeStatusTypeCode?.id !== ApiGen_CodeTypes_AcquisitionTakeStatusTypes.CANCELLED,
  );
  const takesNotInFile = allTakesCount - (takes?.length ?? 0);

  const { getCodeById } = useLookupCodeHelpers();
  const { hasClaim, hasRole } = useKeycloakWrapper();
  const { setModalContent, setDisplayModal } = useModalContext();

  const file = fileProperty.file;

  const statusSolver = new AcquisitionFileStatusUpdateSolver(
    isAcquisitionFile(file) ? file.fileStatusTypeCode : null,
  );

  const canEditTakes = (take: ApiGen_Concepts_Take) => {
    if (
      statusSolver.canEditTakes() &&
      ((take.takeStatusTypeCode.id !==
        ApiGen_CodeTypes_AcquisitionTakeStatusTypes.COMPLETE.toString() &&
        hasClaim(Claims.ACQUISITION_EDIT)) ||
        hasRole(Roles.SYSTEM_ADMINISTRATOR))
    ) {
      return true;
    }
    return false;
  };

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <Section>
        <H2>Takes for {getApiPropertyName(fileProperty.property).value}</H2>
        <StyledBlueSection>
          <SectionField
            labelWidth={{ xs: 8 }}
            label="Takes for this property in the current file"
            tooltip="The number of takes in completed, In-progress or cancelled state for this property in this acquisition file"
          >
            {takes?.length ?? 0}
          </SectionField>
          <SectionField
            labelWidth={{ xs: 8 }}
            label="Takes for this property in other files"
            tooltip="The number of takes in completed, In-progress or cancelled state for this property, in files other than this acquisition file. The other files can be found under the Acquisition section of the PIMS Files tab"
            valueTestId="takes-in-other-files"
          >
            {takesNotInFile}
          </SectionField>
        </StyledBlueSection>
      </Section>
      <Section>
        <H2 className="mb-8">
          <SectionListHeader
            title="Takes"
            claims={[Claims.PROPERTY_EDIT]}
            addButtonIcon={<FaPlus />}
            addButtonText="Add Take"
            onButtonAction={onAdd}
            cannotAddComponent={
              <TooltipIcon toolTipId={`takes-cannot-add-tooltip`} toolTip={cannotEditMessage} />
            }
            isAddEnabled={statusSolver?.canEditTakes()}
          />
        </H2>
        {[...nonCancelledTakes, ...cancelledTakes].map((take, index) => {
          return (
            <Section
              noPadding
              isCollapsable
              initiallyExpanded
              data-testid={`take-${index}`}
              key={`takes-${index}`}
              header={
                <Row>
                  <Col md="10">Take {index + 1}</Col>
                  <Col md="2" className="d-flex align-items-center justify-content-end">
                    {onEdit !== undefined && canEditTakes(take) ? (
                      <EditButton title="Edit take" onClick={() => onEdit(take.id)} />
                    ) : null}
                    {!canEditTakes(take) && (
                      <TooltipIcon
                        toolTipId={`${fileProperty?.fileId || 0}-summary-cannot-edit-tooltip`}
                        toolTip="Retired records are referenced for historical purposes only and cannot be edited or deleted. If the take has been added in error, contact your system administrator to re-open the file, which will allow take deletion"
                      />
                    )}

                    {canEditTakes(take) && (
                      <RemoveIconButton
                        title="Remove take"
                        onRemove={() => {
                          setModalContent({
                            ...getDeleteModalProps(),
                            handleOk: () => {
                              onDelete(take.id);
                              setDisplayModal(false);
                            },
                          });
                          setDisplayModal(true);
                        }}
                      />
                    )}
                  </Col>
                </Row>
              }
            >
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
              {take.completionDt && (
                <SectionField label="Completion date *">
                  {prettyFormatDate(take.completionDt)}
                </SectionField>
              )}
              <SectionField label="Site contamination">
                {take.takeSiteContamTypeCode?.id
                  ? getCodeById(API.TAKE_SITE_CONTAM_TYPES, take.takeSiteContamTypeCode.id)
                  : ''}
              </SectionField>
              <SectionField label="Description" labelWidth={{ xs: 12 }}>
                {take.description}
              </SectionField>
              <StyledNoTabSection header="Area">
                <StyledBorderSection>
                  <SectionField
                    label="Is there a new highway dedication? *"
                    labelWidth={{ xs: 9 }}
                    tooltip="The term new highway dedication includes municipal road or provincial public highway"
                  >
                    {booleanToYesNoUnknownString(take.isNewHighwayDedication ?? undefined)}
                  </SectionField>
                  {take.isNewHighwayDedication && (
                    <SectionField label="Area" labelWidth={{ xs: 12 }}>
                      <AreaContainer landArea={take.newHighwayDedicationArea ?? undefined} />
                    </SectionField>
                  )}
                  <SectionField
                    label="Is this being acquired for MOTT inventory? *"
                    labelWidth={{ xs: 9 }}
                    tooltip="The property will be added to inventory"
                    className="pt-4"
                  >
                    {booleanToYesNoUnknownString(take.isAcquiredForInventory ?? undefined)}
                  </SectionField>
                </StyledBorderSection>
                <StyledBorderSection>
                  <SectionField
                    label="Is there a new registered interest in land (SRW, Easement or Covenant)? *"
                    labelWidth={{ xs: 9 }}
                  >
                    {booleanToYesNoUnknownString(take.isNewInterestInSrw ?? undefined)}
                  </SectionField>
                  {take.isNewInterestInSrw && (
                    <>
                      <SectionField label="Area" labelWidth={{ xs: 12 }}>
                        <AreaContainer landArea={take.statutoryRightOfWayArea ?? undefined} />
                      </SectionField>

                      <SectionField
                        label="SRW end date"
                        labelWidth={{ xs: 3 }}
                        contentWidth={{ xs: 4 }}
                      >
                        {prettyFormatDate(take.srwEndDt ?? undefined)}
                      </SectionField>
                    </>
                  )}
                </StyledBorderSection>
                <StyledBorderSection>
                  <SectionField label="Is there a new Land Act tenure? *" labelWidth={{ xs: 9 }}>
                    {booleanToYesNoUnknownString(take.isNewLandAct ?? undefined)}
                  </SectionField>
                  {take.isNewLandAct && (
                    <>
                      <SectionField label="Land Act" labelWidth={{ xs: 3 }}>
                        {take.landActTypeCode
                          ? take.landActTypeCode.id + ' ' + take.landActTypeCode.description
                          : ''}
                      </SectionField>

                      <SectionField label="Area" labelWidth={{ xs: 12 }}>
                        <AreaContainer landArea={take.landActArea ?? undefined} />
                      </SectionField>

                      {![
                        ApiGen_CodeTypes_LandActTypes.TRANSFER_OF_ADMIN_AND_CONTROL.toString(),
                        ApiGen_CodeTypes_LandActTypes.CROWN_GRANT.toString(),
                      ].includes(take.landActTypeCode.id) && (
                        <SectionField
                          label="End date"
                          labelWidth={{ xs: 3 }}
                          contentWidth={{ xs: 4 }}
                        >
                          {prettyFormatDate(take.landActEndDt ?? undefined)}
                        </SectionField>
                      )}
                    </>
                  )}
                </StyledBorderSection>
                <StyledBorderSection>
                  <SectionField
                    label="Is there a new Licence for Construction Access (TLCA/LTC)? *"
                    labelWidth={{ xs: 9 }}
                  >
                    {booleanToYesNoUnknownString(take.isNewLicenseToConstruct ?? undefined)}
                  </SectionField>
                  {take.isNewLicenseToConstruct && (
                    <>
                      <SectionField label="Area" labelWidth={{ xs: 12 }}>
                        <AreaContainer landArea={take.licenseToConstructArea ?? undefined} />
                      </SectionField>

                      <SectionField
                        label="LTC end date"
                        labelWidth={{ xs: 3 }}
                        contentWidth={{ xs: 4 }}
                      >
                        {prettyFormatDate(take.ltcEndDt ?? undefined)}
                      </SectionField>
                    </>
                  )}
                </StyledBorderSection>
                <StyledBorderSection>
                  <SectionField label="Is there a Lease (Payable)? *" labelWidth={{ xs: 9 }}>
                    {booleanToYesNoUnknownString(take.isLeasePayable ?? undefined)}
                  </SectionField>
                  {take.isLeasePayable && (
                    <>
                      <SectionField label="Area" labelWidth={{ xs: 12 }}>
                        <AreaContainer landArea={take.leasePayableArea ?? undefined} />
                      </SectionField>

                      <SectionField
                        label="End date"
                        labelWidth={{ xs: 3 }}
                        contentWidth={{ xs: 4 }}
                      >
                        {prettyFormatDate(take.leasePayableEndDt ?? undefined)}
                      </SectionField>
                    </>
                  )}
                </StyledBorderSection>
              </StyledNoTabSection>
              <StyledNoTabSection header="Surplus">
                <StyledBorderSection>
                  <SectionField label="Is there a Surplus? *" labelWidth={{ xs: 9 }}>
                    {booleanToYesNoUnknownString(take.isThereSurplus ?? undefined)}
                  </SectionField>
                  {take.isThereSurplus && (
                    <SectionField label="Area" labelWidth={{ xs: 12 }}>
                      <AreaContainer landArea={take.surplusArea ?? undefined} />
                    </SectionField>
                  )}
                </StyledBorderSection>
              </StyledNoTabSection>
            </Section>
          );
        })}
      </Section>
    </StyledSummarySection>
  );
};

const StyledBlueSection = styled.div`
  background-color: ${({ theme }) => theme.css.filterBoxColor};
  border-radius: 0.5rem;
  padding: 1rem;
`;

export default TakesDetailView;
