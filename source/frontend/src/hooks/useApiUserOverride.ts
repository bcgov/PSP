import { AxiosError } from 'axios';
import { uniq } from 'lodash';
import { useCallback, useEffect, useRef, useState } from 'react';

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
  const [state, setState] = useState<IUserOverrideModalState>({
    previousUserOverrideCodes: [],
    userOverrideCode: null,
    message: null,
  });
  const overridenApiFunction = useRef<FunctionType | null>(null);
  const { setModalContent, setDisplayModal } = useModalContext();

  const needsUserAction = useCallback(
    (userOverrideCode: UserOverrideCode | null, message: string | null) => {
      if (userOverrideCode) {
        setState(oldState => {
          return {
            previousUserOverrideCodes: [...oldState.previousUserOverrideCodes],
            userOverrideCode: userOverrideCode,
            message: message,
          };
        });
        setDisplayModal(true);
      }
    },
    [setDisplayModal],
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
        const response = await apiFunction(uniq(userOverrideCodes));
        setDisplayModal(false);
        return response;
      } catch (e) {
        handleOverrideError(e, handleError);
      } finally {
        overridenApiFunction.current = apiFunction;
      }
      return Promise.resolve(undefined);
    },
    [handleOverrideError, setDisplayModal],
  );

  useEffect(() => {
    if (state?.userOverrideCode) {
      switch (state.userOverrideCode) {
        case UserOverrideCode.CONTRACTOR_SELFREMOVED:
          setModalContent({
            title: 'Note',
            variant: 'info',
            message: RemoveSelfContractorContent(),
            handleOk: async () => {
              setState({
                previousUserOverrideCodes: [...state.previousUserOverrideCodes],
                userOverrideCode: null,
                message: null,
              });
              setDisplayModal(false);
            },
            okButtonText: 'Close',
          });
          break;
        default: {
          setModalContent({
            variant: 'warning',
            title: 'User Override Required',
            message: state?.message,
            handleOk: async () => {
              if (state?.userOverrideCode && overridenApiFunction.current) {
                setState({
                  ...state,
                  previousUserOverrideCodes: [
                    ...state.previousUserOverrideCodes,
                    state?.userOverrideCode,
                  ],
                });
                await apiCallWithOverride(overridenApiFunction.current, [
                  ...state.previousUserOverrideCodes,
                  state?.userOverrideCode,
                ]);
              }
            },
            handleCancel: () => {
              setState({
                previousUserOverrideCodes: [...state.previousUserOverrideCodes],
                userOverrideCode: null,
                message: null,
              });
              setDisplayModal(false);
            },
            okButtonText: 'Yes',
            okButtonVariant: 'warning',
            cancelButtonText: 'No',
          });
        }
      }
    }
  }, [
    apiCallWithOverride,
    overridenApiFunction,
    setDisplayModal,
    setModalContent,
    state,
    state.previousUserOverrideCodes,
  ]);

  return apiCallWithOverride;
};

export default useApiUserOverride;
