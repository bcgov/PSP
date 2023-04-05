import { SideBarContext } from 'features/properties/map/context/sidebarContext';
import { useAgreementProvider } from 'hooks/repositories/useAgreemetProvider';
import { Api_Agreement } from 'models/api/Agreement';
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

  const test_greemenmt: Api_Agreement = {
    agreementId: 123,
    acquisitionFileId: 123,
    agreementType: { description: 'Total' },
    agreementDate: '2023-04-05T19:06:46.533',
    agreementStatus: false,
    completionDate: '2023-04-05T19:06:46.533',
    terminationDate: '2023-04-05T19:06:46.533',
    commencementDate: '2023-04-05T19:06:46.533',
    depositAmount: 1568.15,
    noLaterThanDays: 15,
    purchasePrice: 1000,
    legalSurveyPlanNum: '123456789',
    offerDate: '2023-04-05T19:06:46.533',
    expiryDateTime: '2023-04-05T19:06:46.533',
    signedDate: '2023-04-05T19:06:46.533',
    inspectionDate: '2023-04-05T19:06:46.533',
  };

  return !!file?.id ? (
    <>
      <View loading={loading} agreements={response || []} onEdit={onEdit}></View>
    </>
  ) : null;
};

export default AgreementContainer;
