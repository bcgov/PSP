import * as React from 'react';
import { useCallback, useContext, useEffect } from 'react';

import { SideBarContext } from '@/features/mapSideBar/context/sidebarContext';
import { useAgreementProvider } from '@/hooks/repositories/useAgreementProvider';
import { isValidId } from '@/utils';

import { useGenerateAgreement } from '../../../common/GenerateForm/hooks/useGenerateAgreement';
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
  if (!isValidId(file?.id) && fileLoading === false) {
    throw new Error('Unable to determine id of current file.');
  }

  const fetchAgreements = useCallback(async () => {
    return await getAgreements(acquisitionFileId);
  }, [acquisitionFileId, getAgreements]);

  useEffect(() => {
    fetchAgreements();
  }, [fetchAgreements]);

  return file?.id ? (
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
