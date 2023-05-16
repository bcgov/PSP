import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_Compensation } from 'models/api/Compensation';

import { useGenerateH120 } from '../../acquisition/common/GenerateForm/hooks/useGenerateH120';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: Api_Compensation;
  acquisitionFile: Api_AcquisitionFile;
  clientConstant: string;
  gstConstant: number;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailContainerProps>
> = ({
  compensation,
  setEditMode,
  View,
  clientConstant,
  acquisitionFile,
  gstConstant,
  loading,
}) => {
  const onGenerate = useGenerateH120();
  return compensation ? (
    <View
      compensation={compensation}
      acqFileProduct={acquisitionFile.product}
      acqFileProject={acquisitionFile.project}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      gstConstant={gstConstant}
      loading={loading}
      onGenerate={onGenerate}
    ></View>
  ) : null;
};
