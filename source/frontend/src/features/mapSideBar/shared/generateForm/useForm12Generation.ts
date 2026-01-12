import { useCallback } from 'react';

import { useModalManagement } from '@/hooks/useModalManagement';
import { ApiGen_Concepts_FileProperty } from '@/models/api/generated/ApiGen_Concepts_FileProperty';

export function useForm12Generation(
  generateForm12: (properties: any[]) => Promise<any>,
  setIsGenerating: React.Dispatch<React.SetStateAction<boolean>>,
) {
  const [isPropertyModalOpen, openPropertyModal, closePropertyModal] = useModalManagement();

  const onGenerate = useCallback(
    (properties: ApiGen_Concepts_FileProperty[]) => {
      setIsGenerating(true);
      generateForm12(properties.map(fp => fp.property)).finally(() => setIsGenerating(false));
    },
    [generateForm12, setIsGenerating],
  );

  const handleGenerateForm12Click = () => {
    openPropertyModal();
  };

  const handleSelectedProperties = (selectedProperties: ApiGen_Concepts_FileProperty[]) => {
    onGenerate(selectedProperties);
    closePropertyModal();
  };

  return {
    isPropertyModalOpen,
    openPropertyModal,
    closePropertyModal,
    handleGenerateForm12Click,
    handleSelectedProperties,
  };
}
