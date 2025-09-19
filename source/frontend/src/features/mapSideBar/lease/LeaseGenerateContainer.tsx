import { useCallback, useMemo, useState } from 'react';

import { ApiGen_CodeTypes_FormTypes } from '@/models/api/generated/ApiGen_CodeTypes_FormTypes';
import { ApiGen_CodeTypes_LeaseLicenceTypes } from '@/models/api/generated/ApiGen_CodeTypes_LeaseLicenceTypes';
import { ApiGen_Concepts_Lease } from '@/models/api/generated/ApiGen_Concepts_Lease';

import { IGenerateFormViewProps } from '../acquisition/common/GenerateForm/GenerateFormView';
import GenerateItemView from '../acquisition/common/GenerateForm/GenerateItemView';
import { useGenerateLicenceOfOccupation } from '../acquisition/common/GenerateForm/hooks/useGenerateLicenceOfOccupation';

export interface ILeaseGenerateContainerProps {
  lease: ApiGen_Concepts_Lease;
  View: React.FunctionComponent<React.PropsWithChildren<IGenerateFormViewProps>>;
}

const LeaseGenerateContainer: React.FunctionComponent<
  React.PropsWithChildren<ILeaseGenerateContainerProps>
> = ({ lease, View }) => {
  const [isGenerating, setIsGenerating] = useState(false);
  const generateLicenceOfOccupation = useGenerateLicenceOfOccupation();

  const onGenerate = useCallback(
    (lease: ApiGen_Concepts_Lease) => {
      setIsGenerating(true);
      generateLicenceOfOccupation(lease).finally(() => setIsGenerating(false));
    },
    [generateLicenceOfOccupation],
  );

  const handleGenerateClick = () => {
    onGenerate(lease);
  };

  const canGenerateH1005a = useMemo(
    () => lease?.type.id === ApiGen_CodeTypes_LeaseLicenceTypes.LOOBCTFA,
    [lease?.type.id],
  );

  const canGenerateH1005 = useMemo(
    () => lease?.type.id === ApiGen_CodeTypes_LeaseLicenceTypes.LIPPUBHWY,
    [lease?.type.id],
  );

  const hasForms = useMemo(
    () => canGenerateH1005a || canGenerateH1005,
    [canGenerateH1005, canGenerateH1005a],
  );

  if (!hasForms) {
    return <></>;
  }

  return (
    <View isLoading={isGenerating}>
      {canGenerateH1005a && (
        <GenerateItemView
          formType={ApiGen_CodeTypes_FormTypes.H1005A}
          label="Generate H-1005(a)"
          onGenerate={handleGenerateClick}
        />
      )}
      {canGenerateH1005 && (
        <GenerateItemView
          formType={ApiGen_CodeTypes_FormTypes.H1005}
          label="Generate H-1005"
          onGenerate={handleGenerateClick}
        />
      )}
    </View>
  );
};

export default LeaseGenerateContainer;
