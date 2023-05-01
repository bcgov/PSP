import { SideBarContext } from 'features/properties/map/context/sidebarContext';
import { useAgreementProvider } from 'hooks/repositories/useAgreementProvider';
import * as React from 'react';
import { useEffect } from 'react';
import { useCallback } from 'react';
import { useContext } from 'react';

import { useGenerateAgreement } from '../../acquisition/common/GenerateForm/hooks/useGenerateAgreement';
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
  const generateAgreement = useGenerateAgreement();

  const { file, fileLoading } = useContext(SideBarContext);
  if (!!file && file?.id === undefined && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  const fetchAgreements = useCallback(async () => {
    return await getAgreements(acquisitionFileId);
  }, [acquisitionFileId, getAgreements]);

  useEffect(() => {
    fetchAgreements();
  }, [fetchAgreements]);

  return !!file?.id ? (
    <>
      <View
        loading={loading}
        agreements={response || []}
        onEdit={onEdit}
        onGenerate={generateAgreement}
      ></View>
    </>
  ) : null;
};

export default AgreementContainer;
