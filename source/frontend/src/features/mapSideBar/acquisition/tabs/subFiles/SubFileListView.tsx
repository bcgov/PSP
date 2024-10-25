import { FaPlus } from 'react-icons/fa';
import { useLocation } from 'react-router-dom';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { SectionField } from '@/components/common/Section/SectionField';
import { StyledSummarySection } from '@/components/common/Section/SectionStyles';
import { SimpleSectionHeader } from '@/components/common/SimpleSectionHeader';
import { StyledSectionAddButton } from '@/components/common/styles';
import { Claims } from '@/constants';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { exists } from '@/utils';

import { SubFilesResultsTable } from './table/SubFilesResultsTable';

export interface ISubFileListViewProps {
  loading: boolean;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  subFiles: ApiGen_Concepts_AcquisitionFile[];
  onAdd: () => void;
}

export const SubFileListView: React.FunctionComponent<ISubFileListViewProps> = ({
  loading,
  acquisitionFile,
  subFiles,
  onAdd,
}) => {
  const location = useLocation();
  const { hasClaim } = useKeycloakWrapper();
  const baseLocation = location.pathname.split(`/${acquisitionFile.id}/subFiles`)[0];

  const parentAcquisitionFileNoValue =
    subFiles?.find(x => x.parentAcquisitionFileId === null).fileNo ?? '';

  return (
    <StyledSummarySection>
      <LoadingBackdrop show={loading} parentScreen={true} />
      <Section
        header={
          <SimpleSectionHeader title="Linked Files">
            {!exists(acquisitionFile.parentAcquisitionFileId) &&
              hasClaim(Claims.ACQUISITION_ADD) &&
              exists(onAdd) && (
                <StyledSectionAddButton onClick={onAdd}>
                  <FaPlus size="2rem" className="mr-2" />
                  Add Sub-interest File
                </StyledSectionAddButton>
              )}
          </SimpleSectionHeader>
        }
      >
        <SectionField label="Linked files" labelWidth="3" valueTestId="linked-files-header">
          {parentAcquisitionFileNoValue}
        </SectionField>

        {subFiles && (
          <SubFilesResultsTable
            results={subFiles}
            currentAcquisitionFileId={acquisitionFile.id}
            routeURL={baseLocation}
          />
        )}
      </Section>
    </StyledSummarySection>
  );
};

export default SubFileListView;
