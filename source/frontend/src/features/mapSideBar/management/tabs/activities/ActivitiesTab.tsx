import { FaPlus } from 'react-icons/fa';
import { useHistory, useRouteMatch } from 'react-router-dom';

import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_ManagementFile } from '@/models/api/generated/ApiGen_Concepts_ManagementFile';
import { isValidId } from '@/utils';

export interface IActivitiesTabProps {
  managementFile: ApiGen_Concepts_ManagementFile;
}

export const ActivitiesTab: React.FunctionComponent<IActivitiesTabProps> = ({ managementFile }) => {
  const history = useHistory();
  const match = useRouteMatch();
  const { hasClaim } = useKeycloakWrapper();

  const onAdd = () => {
    if (isValidId(managementFile?.id)) {
      history.push(`${match.url}/new`);
    }
  };

  return (
    <StyledSummarySection>
      <Section
        header={
          <SimpleSectionHeader title="Activity List">
            {hasClaim(Claims.MANAGEMENT_EDIT) && (
              <StyledSectionAddButton onClick={onAdd}>
                <FaPlus size="2rem" className="mr-2" />
                Add Activity
              </StyledSectionAddButton>
            )}
          </SimpleSectionHeader>
        }
      >
        <p>
          <strong>Activity documentation:</strong> You can attach a document after creating the
          activity. Create, then edit and attach a file if needed.
        </p>
      </Section>
    </StyledSummarySection>
  );
};

export default ActivitiesTab;
