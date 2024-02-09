import { AxiosError } from 'axios';
import { uniq } from 'lodash';
import { useCallback, useRef } from 'react';

import { RemoveSelfContractorContent } from '@/features/mapSideBar/acquisition/tabs/fileDetails/update/UpdateAcquisitionContainer';
import { IApiError } from '@/interfaces/IApiError';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandlerWithConfirmation } from '@/utils';

import { useModalContext } from './useModalContext';

export interface IUserOverrideModalState {
  previousUserOverrideCodes: UserOverrideCode[];
  userOverrideCode: UserOverrideCode | null;
  message: string | null;
}

export const useApiUserOverride = <
  FunctionType extends (userOverrideCodes: UserOverrideCode[]) => Promise<any>,
>(
  genericErrorMessage: string,
) => {
  const overridenApiFunction = useRef<FunctionType | null>(null);
  const errorHandlerRef = useRef<((e: AxiosError<IApiError>) => void) | null>(null);
  const { setModalContent, setDisplayModal } = useModalContext();

  const needsUserAction = useCallback(
    (
      userOverrideCode: UserOverrideCode | null,
      message: string | null,
      previousUserOverrideCodes: UserOverrideCode[],
    ) => {
      if (userOverrideCode) {
        showUserOverrideModal({
          previousUserOverrideCodes: [...previousUserOverrideCodes],
          userOverrideCode: userOverrideCode,
          message: message,
        });
      }
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [],
  );

  const handleOverrideError = useAxiosErrorHandlerWithConfirmation(
    needsUserAction,
    genericErrorMessage,
  );

  const apiCallWithOverride = useCallback(
    async (
      apiFunction: FunctionType,
      userOverrideCodes: UserOverrideCode[] = [],
      handleError?: (e: AxiosError<IApiError>) => void,
    ) => {
      try {
        var response = await apiFunction(uniq(userOverrideCodes));
        setDisplayModal(false);
        return response;
      } catch (e) {
        handleOverrideError(e, handleError, userOverrideCodes);
      } finally {
        overridenApiFunction.current = apiFunction;
        errorHandlerRef.current = handleError ?? null;
      }
      return Promise.resolve(undefined);
    },
    [handleOverrideError, setDisplayModal],
  );

  const showUserOverrideModal = useCallback(
    (modalState: IUserOverrideModalState) => {
      if (modalState?.userOverrideCode) {
        switch (modalState.userOverrideCode) {
          case UserOverrideCode.CONTRACTOR_SELFREMOVED:
            setModalContent({
              title: 'Note',
              variant: 'info',
              message: RemoveSelfContractorContent(),
              handleOk: async () => {
                setDisplayModal(false);
              },
              okButtonText: 'Close',
            });
            break;
          default: {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: modalState?.message,
              handleOk: async () => {
                if (modalState?.userOverrideCode && overridenApiFunction.current) {
                  const overrideCode = modalState.userOverrideCode;
                  await apiCallWithOverride(
                    overridenApiFunction.current,
                    [...modalState.previousUserOverrideCodes, overrideCode],
                    errorHandlerRef.current ?? undefined,
                  );
                }
              },
              handleCancel: () => {
                setDisplayModal(false);
              },
              okButtonText: 'Yes',
              okButtonVariant: 'warning',
              cancelButtonText: 'No',
            });
          }
        }
        setDisplayModal(true);
      }
    },
    [apiCallWithOverride, overridenApiFunction, setDisplayModal, setModalContent],
  );

  return apiCallWithOverride;
};

export default useApiUserOverride;
