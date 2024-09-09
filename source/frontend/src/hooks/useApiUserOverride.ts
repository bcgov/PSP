import { AxiosError } from 'axios';
import { uniq } from 'lodash';
import { useCallback, useRef } from 'react';

import { ModalContent } from '@/components/common/GenericModal';
import { RemoveSelfContractorContent } from '@/features/mapSideBar/acquisition/tabs/fileDetails/update/UpdateAcquisitionContainer';
import { IApiError } from '@/interfaces/IApiError';
import { UserOverrideCode } from '@/models/api/UserOverrideCode';
import { useAxiosErrorHandlerWithConfirmation } from '@/utils';

import { useModalContext } from './useModalContext';

export type CustomModalOptions = Pick<ModalContent, 'variant' | 'title'>;

export interface IApiUserOverrideOptions {
  /** Optional arguments to control the look-and-feel of the UserOverride modal. The key is the user-override code to target with the custom values. */
  modalOptions: Map<UserOverrideCode, CustomModalOptions>;
}

export interface IUserOverrideModalState extends CustomModalOptions {
  previousUserOverrideCodes: UserOverrideCode[];
  userOverrideCode: UserOverrideCode | null;
  message: string | null;
}

export const useApiUserOverride = <
  FunctionType extends (userOverrideCodes: UserOverrideCode[]) => Promise<any>,
>(
  genericErrorMessage: string,
  options?: IApiUserOverrideOptions,
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
        // allow the caller of this hook to customize the look and feel of the user override modal.
        const customModalProps = options?.modalOptions?.get(userOverrideCode) ?? {
          variant: 'warning',
          title: 'User Override Required',
        };

        showUserOverrideModal({
          previousUserOverrideCodes: [...previousUserOverrideCodes],
          userOverrideCode: userOverrideCode,
          message: message,
          variant: customModalProps.variant,
          title: customModalProps.title,
        });
      }
    },
    // eslint-disable-next-line react-hooks/exhaustive-deps
    [options],
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
              variant: modalState?.variant ?? 'warning',
              title: modalState?.title ?? 'User Override Required',
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
