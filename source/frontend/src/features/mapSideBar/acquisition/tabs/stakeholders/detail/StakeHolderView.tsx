import EditButton from '@/components/common/EditButton';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledEditWrapper, StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { Claims } from '@/constants/index';
import { StyledNoData } from '@/features/documents/commonStyles';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_InterestHolder } from '@/models/api/generated/ApiGen_Concepts_InterestHolder';

import AcquisitionFileStatusUpdateSolver from '../../fileDetails/detail/AcquisitionFileStatusUpdateSolver';
import PropertyInterestHoldersViewTable from './PropertyInterestHoldersViewTable';
import StakeholderOrganizer from './stakeholderOrganizer';

export interface IStakeHolderViewProps {
  loading: boolean;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  interestHolders: ApiGen_Concepts_InterestHolder[] | undefined;
  onEdit: () => void;
}

export const StakeHolderView: React.FunctionComponent<IStakeHolderViewProps> = ({
  loading,
  acquisitionFile,
  interestHolders,
  onEdit,
}) => {
  const keycloak = useKeycloakWrapper();

  const legacyStakeHolders = acquisitionFile.legacyStakeholders ?? [];

  const organizer = new StakeholderOrganizer(acquisitionFile, interestHolders);

  const statusSolver = new AcquisitionFileStatusUpdateSolver(acquisitionFile.fileStatusTypeCode);

  const groupedInterestProperties = organizer.getInterestProperties();
  const groupedNonInterestProperties = organizer.getNonInterestProperties();

  return (
    <>
      <StyledSummarySection>
        <LoadingBackdrop show={loading} parentScreen={true} />
        <StyledEditWrapper className="mr-3 my-1">
          {keycloak.hasClaim(Claims.ACQUISITION_EDIT) && statusSolver.canEditStakeholders() ? (
            <EditButton title="Edit Interests" onClick={onEdit} />
          ) : null}
        </StyledEditWrapper>
        <Section isCollapsable initiallyExpanded header="Interests">
          {groupedInterestProperties.length === 0 && legacyStakeHolders.length === 0 && (
            <StyledNoData>
              <p>There are no interest holders associated with this file.</p>
              <p> To add an interest holder, click the edit button.</p>
            </StyledNoData>
          )}
          {groupedInterestProperties.map((interestPropertyGroup, index) => (
            <PropertyInterestHoldersViewTable
              key={`interest-holder-section-${index}`}
              propertyInterestHolders={interestPropertyGroup}
            />
          ))}
          {legacyStakeHolders.length > 0 && (
            <>
              <hr />
              <SectionField
                label="Legacy interest holders"
                tooltip="This is read-only field to display legacy information"
                labelWidth="4"
                contentWidth="8"
                valueTestId="acq-file-legacy-stakeholders"
              >
                {legacyStakeHolders.map((stakeholder, index) => (
                  <div key={index}>
                    <label key={index}>{stakeholder}</label>
                    {index < legacyStakeHolders.length - 1 && <br />}
                  </div>
                ))}
              </SectionField>
            </>
          )}
        </Section>
      </StyledSummarySection>
      <StyledSummarySection>
        <Section isCollapsable initiallyExpanded header="Non-interest Payees">
          {groupedNonInterestProperties.length === 0 && (
            <StyledNoData>
              <p>There are no non-interest payees associated with this file.</p>
              <p> To add a non-interest payee, click the edit button.</p>
            </StyledNoData>
          )}
          {groupedNonInterestProperties.map((interestPropertyGroup, index) => (
            <PropertyInterestHoldersViewTable
              key={`non-interest-payee-section-${index}`}
              propertyInterestHolders={interestPropertyGroup}
            />
          ))}
        </Section>
      </StyledSummarySection>
    </>
  );
};

export default StakeHolderView;
