import { FaPlus } from 'react-icons/fa';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists } from '@/utils';

export interface ISubFileListViewProps {
  loading: boolean;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  onAdd: () => void;
}

export const SubFileListView: React.FunctionComponent<ISubFileListViewProps> = ({
  loading,
  onAdd,
}) => {
  const { hasClaim } = useKeycloakWrapper();

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />

      <Section
        header={
          <SimpleSectionHeader title="Linked Files">
            {hasClaim(Claims.ACQUISITION_ADD) && exists(onAdd) && (
              <StyledSectionAddButton onClick={onAdd}>
                <FaPlus size="2rem" className="mr-2" />
                Add Sub-interest File
              </StyledSectionAddButton>
            )}
          </SimpleSectionHeader>
        }
      ></Section>
    </StyledSummarySection>
  );
};

export default SubFileListView;
