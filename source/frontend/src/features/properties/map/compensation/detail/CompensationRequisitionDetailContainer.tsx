import { Api_Compensation } from 'models/api/Compensation';

import { CompensationRequisitionDetailViewProps } from './CompensationRequisitionDetailView';

export interface CompensationRequisitionDetailContainerProps {
  compensation: Api_Compensation;
  clientConstant: string;
  gstConstant: number;
  loading: boolean;
  setEditMode: (editMode: boolean) => void;
  View: React.FunctionComponent<React.PropsWithChildren<CompensationRequisitionDetailViewProps>>;
}

export const CompensationRequisitionDetailContainer: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailContainerProps>
> = ({ compensation, setEditMode, View, clientConstant, gstConstant, loading }) => {
  return compensation ? (
    <View
      compensation={compensation}
      setEditMode={setEditMode}
      clientConstant={clientConstant}
      gstConstant={gstConstant}
      loading={loading}
    ></View>
  ) : null;
};
