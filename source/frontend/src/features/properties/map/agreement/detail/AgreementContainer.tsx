import { SideBarContext } from 'features/properties/map/context/sidebarContext';
import { useAgreementProvider } from 'hooks/repositories/useAgreemetProvider';
import * as React from 'react';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { useContext } from 'react';

import { IAgreementViewProps } from './AgreementView';

export interface IAgreementContainerProps {
  acquisitionFileId: number;
  onEdit: () => void;
  View: React.FunctionComponent<React.PropsWithChildren<IAgreementViewProps>>;
}

export const AgreementContainer: React.FunctionComponent<
  React.PropsWithChildren<IAgreementContainerProps>
> = ({ acquisitionFileId, onEdit, View }) => {
  const {
    getAcquisitionAgreements: { execute: getAgreements, response, loading },
  } = useAgreementProvider();
  const { file, fileLoading } = useContext(SideBarContext);

  const fetchAgreements = useCallback(async () => {
    if (!!acquisitionFileId) {
      return await getAgreements(acquisitionFileId);
    }
  }, [acquisitionFileId, getAgreements]);
  useEffect(() => {
    fetchAgreements();
  }, [fetchAgreements]);

  if (!!file && file?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  return !!file?.id ? (
    <>
      <View loading={loading} agreements={response || []} onEdit={onEdit}></View>
    </>
  ) : null;
};

export default AgreementContainer;
