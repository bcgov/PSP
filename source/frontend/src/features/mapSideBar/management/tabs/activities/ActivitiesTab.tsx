import { FaPlus } from 'react-icons/fa';

import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import usePathGenerator from '@/features/mapSideBar/shared/sidebarPathGenerator';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { isValidId } from '@/utils';

import ManagementStatusUpdateSolver from '../fileDetails/detail/ManagementStatusUpdateSolver';
import AdHocFileActivitiesSummaryContainer from './list/AdHocSummaryActivitiesContainer';
import AdHocSummaryActivitiesView from './list/AdHocSummaryActivitiesListView';
import ManagementFileActivitiesListContainer from './list/ManagementFileActivitiesListContainer';
import ManagementFileActivitiesListView from './list/ManagementFileActivitiesListView';

export interface IActivitiesTabProps {
  managementFile: ApiGen_Concepts_ManagementFile;
}

export const ActivitiesTab: React.FunctionComponent<IActivitiesTabProps> = ({ managementFile }) => {
  const { hasClaim } = useKeycloakWrapper();
  const pathGenerator = usePathGenerator();

  const onAdd = () => {
    if (isValidId(managementFile.id)) {
      pathGenerator.addDetail('management', managementFile.id, 'activities');
    }
  };

  const statusSolver = new ManagementStatusUpdateSolver(managementFile);

  return (
    <StyledSummarySection>
      <Section
        header={
          <SimpleSectionHeader title="Activities List">
            {hasClaim(Claims.MANAGEMENT_EDIT) && statusSolver.canEditActivities() && (
              <StyledSectionAddButton onClick={onAdd} data-testid="add-activity-button">
                <FaPlus size="2rem" className="mr-2" />
                Add an Activity
              </StyledSectionAddButton>
            )}
          </SimpleSectionHeader>
        }
      >
        <p>
          <strong>Activity documentation:</strong> You can attach a document after creating the
          activity. Create, then edit and attach a file if needed.
        </p>
        <ManagementFileActivitiesListContainer
          View={ManagementFileActivitiesListView}
          managementFileId={managementFile.id}
          statusSolver={statusSolver}
        ></ManagementFileActivitiesListContainer>
      </Section>
      <AdHocFileActivitiesSummaryContainer
        View={AdHocSummaryActivitiesView}
        managementFileId={managementFile.id}
      ></AdHocFileActivitiesSummaryContainer>
    </StyledSummarySection>
  );
};

export default ActivitiesTab;
