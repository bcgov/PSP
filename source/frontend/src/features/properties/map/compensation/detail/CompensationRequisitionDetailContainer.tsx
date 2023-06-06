import { Api_AcquisitionFile } from 'models/api/AcquisitionFile';
import { Api_CompensationRequisition } from 'models/api/CompensationRequisition';

import { useGenerateH120 } from '../../acquisition/common/GenerateForm/hooks/useGenerateH120';
import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: Api_CompensationRequisition;
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
      acqFileProject={acquisitionFile.project}
      acqFileProduct={acquisitionFile.product}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      gstConstant={gstConstant}
      loading={loading}
      onGenerate={onGenerate}
    ></View>
  ) : null;
};
