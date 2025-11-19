import { useCallback, useMemo, useState } from 'react';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import PropertySelectorModal from '@/features/mapSideBar/research/PropertySelectorModal';
import { useAcquisitionProvider } from '@/hooks/repositories/useAcquisitionProvider';
import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';
import { exists } from '@/utils';

import GenerateItemView from './GenerateItemView';

export interface IGenerateIntakeContainerProps {
  acquisitionFileId: number;
  onGenerate: (selectedProperties: ApiGen_Concepts_FileProperty[]) => void;
}

const GenerateIntakeContainer: React.FunctionComponent<
  React.PropsWithChildren<IGenerateIntakeContainerProps>
> = ({ acquisitionFileId, onGenerate }) => {
  const [acquisitionFile, setAcquisitionFile] = useState<ApiGen_Concepts_AcquisitionFile>(null);

  const [isPropertyModalOpen, openPropertyModal, closePropertyModal] = useModalManagement();

  const {
    getAcquisitionFile: { execute: getAcquisitionFile, loading: getAcquisitionFileLoading },
    getAcquisitionProperties: {
      execute: getAcquisitionProperties,
      loading: getAcquisitionPropertiesLoading,
    },
  } = useAcquisitionProvider();

  const featchAcquisitionTeam = useCallback(async () => {
    const file = await getAcquisitionFile(acquisitionFileId);
    if (exists(file)) {
      const properties = await getAcquisitionProperties(acquisitionFileId);
      file.fileProperties = properties ?? [];
    }
    setAcquisitionFile(file);
  }, [acquisitionFileId, getAcquisitionFile, getAcquisitionProperties]);

  const handleGenerateClick = useCallback(() => {
    featchAcquisitionTeam();
    openPropertyModal();
  }, [featchAcquisitionTeam, openPropertyModal]);

  const handleSelectedProperties = (selectedProperties: ApiGen_Concepts_FileProperty[]) => {
    onGenerate(selectedProperties);
    closePropertyModal();
  };

  const isLoading = useMemo(
    () => getAcquisitionFileLoading || getAcquisitionPropertiesLoading,
    [getAcquisitionFileLoading, getAcquisitionPropertiesLoading],
  );

  return (
    <>
      <LoadingBackdrop show={isLoading} />
      <GenerateItemView
        label="Generate Intake"
        formType={ApiGen_CodeTypes_FormTypes.FORM1}
        onGenerate={handleGenerateClick}
      />
      <PropertySelectorModal
        isOpened={isPropertyModalOpen}
        availiableProperties={acquisitionFile?.fileProperties ?? []}
        onSelectOk={handleSelectedProperties}
        onCancelClick={closePropertyModal}
        isSingleSelect
      />
    </>
  );
};

export default GenerateIntakeContainer;
