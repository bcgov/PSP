import { FormikProps } from 'formik';
import { useEffect, useState } from 'react';

import { PropertyForm } from '@/features/mapSideBar/shared/models';
import { useModalContext } from '@/hooks/useModalContext';
import { exists } from '@/utils';

export interface IUseAddFileConfirmationProps<T> {
  formikRef: React.RefObject<FormikProps<T>>;
  confirmBeforeAdd: (property: PropertyForm) => Promise<boolean>;
  fieldName: string;
  properties: PropertyForm[];
  message: React.ReactNode;
}

export function useAddFileConfirmation<T>({
  formikRef,
  confirmBeforeAdd,
  fieldName,
  properties,
  message,
}: IUseAddFileConfirmationProps<T>) {
  const { setModalContent, setDisplayModal } = useModalContext();
  const [needsUserConfirmation, setNeedsUserConfirmation] = useState<boolean>(true);

  useEffect(() => {
    const runAsync = async () => {
      if (exists(properties) && exists(formikRef.current) && needsUserConfirmation) {
        if (properties.length > 0) {
          // Check all properties for confirmation
          const needsConfirmation = await Promise.all(
            properties.map(formProperty => confirmBeforeAdd(formProperty)),
          );
          if (needsConfirmation.some(confirm => confirm)) {
            setModalContent({
              variant: 'warning',
              title: 'User Override Required',
              message: message,
              okButtonText: 'Yes',
              cancelButtonText: 'No',
              handleOk: () => {
                // allow the properties to be added to the file being created
                formikRef.current?.resetForm();
                formikRef.current?.setFieldValue(fieldName, properties);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
              handleCancel: () => {
                // clear out the properties array as the user did not agree to the popup
                formikRef.current?.resetForm();
                formikRef.current?.setFieldValue(fieldName, []);
                setDisplayModal(false);
                // show the user confirmation modal only once when creating a file
                setNeedsUserConfirmation(false);
              },
            });
            setDisplayModal(true);
          }
        }
      }
    };

    runAsync();
  }, [
    confirmBeforeAdd,
    fieldName,
    formikRef,
    message,
    needsUserConfirmation,
    properties,
    setDisplayModal,
    setModalContent,
  ]);
}
