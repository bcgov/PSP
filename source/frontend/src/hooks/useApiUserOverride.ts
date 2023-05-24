import { uniq } from 'lodash';
import { UserOverrideCode } from 'models/api/UserOverrideCode';
import { useCallback, useEffect, useRef, useState } from 'react';
import { useAxiosErrorHandlerWithConfirmation } from 'utils';

import { useModalContext } from './useModalContext';

export interface IUserOverrideModalState {
  previousUserOverrideCodes: UserOverrideCode[];
  userOverrideCode: UserOverrideCode | null;
  message: string | null;
}

export const useApiUserOverride = <
  FunctionType extends (userOverrideCodes: UserOverrideCode[]) => Promise<any>,
>() => {
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
        setState({
          previousUserOverrideCodes: [...state.previousUserOverrideCodes],
          userOverrideCode: userOverrideCode,
          message: message,
        });
        setDisplayModal(true);
      }
    },
    [setDisplayModal, state.previousUserOverrideCodes],
  );

  const handleOverrideError = useAxiosErrorHandlerWithConfirmation(needsUserAction);

  const apiCallWithOverride = useCallback(
    async (apiFunction: FunctionType, userOverrideCodes: UserOverrideCode[] = []) => {
      try {
        var response = await apiFunction(uniq(userOverrideCodes));
        setDisplayModal(false);
        return response;
      } catch (e) {
        handleOverrideError(e);
      } finally {
        overridenApiFunction.current = apiFunction;
      }
      return Promise.resolve(undefined);
    },
    [handleOverrideError, setDisplayModal],
  );

  useEffect(() => {
    setModalContent({
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
      okButtonText: 'Acknowledge & Continue',
      okButtonVariant: 'warning',
      cancelButtonText: 'Cancel',
    });
  }, [apiCallWithOverride, overridenApiFunction, setDisplayModal, setModalContent, state]);

  return apiCallWithOverride;
};

export default useApiUserOverride;
