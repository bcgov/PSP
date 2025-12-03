import { FormikProps } from 'formik';
import { useCallback, useContext, useMemo, useRef } from 'react';

import { ModalProps } from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { Section } from '@/components/common/Section/Section';
import { ModalContext } from '@/contexts/modalContext';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import PropertiesListContainer from '@/features/mapSideBar/shared/update/properties/PropertiesListContainer';
import { usePropertyFormSyncronizer } from '@/hooks/usePropertyFormSyncronizer';

import { LeaseFormModel } from '../../models';

interface LeasePropertySelectorProp {
  formikProps: FormikProps<LeaseFormModel>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

export const LeasePropertySelector: React.FunctionComponent<LeasePropertySelectorProp> = ({
  formikProps,
  confirmBeforeAdd,
}) => {
  const { values } = formikProps;
  const localRef = useRef<FormikProps<LeaseFormModel>>(null);

  const { setModalContent, setDisplayModal } = useContext(ModalContext);

  const { isLoading } = usePropertyFormSyncronizer(localRef, 'properties');
  const formProperties = useMemo(() => values.properties.map(x => x.property), [values.properties]);

  const cancelRemove = useCallback(() => {
    setDisplayModal(false);
  }, [setDisplayModal]);

  const getRemoveModalProps = useCallback<(removeCallback: () => void) => ModalProps>(
    (removeCallback: () => void) => {
      return {
        variant: 'info',
        title: 'Removing Property from Lease/Licence',
        message: 'Are you sure you want to remove this property from this lease/licence?',
        display: false,
        okButtonText: 'Remove',
        cancelButtonText: 'Cancel',
        handleOk: () => removeCallback(),
        handleCancel: cancelRemove,
      };
    },
    [cancelRemove],
  );

  const onRemoveClick = useCallback(
    (_: number, removeCallback: () => void) => {
      setModalContent(getRemoveModalProps(removeCallback));
      setDisplayModal(true);
    },
    [getRemoveModalProps, setDisplayModal, setModalContent],
  );

  return (
    <Section header="Properties to include in this file:">
      <div className="py-2">
        Select one or more properties that you want to include in this lease/licence file. You can
        choose a location from the map, or search by other criteria.
      </div>
      <LoadingBackdrop show={isLoading} />
      <PropertiesListContainer
        properties={formProperties}
        verifyCanRemove={onRemoveClick}
        needsConfirmationBeforeAdd={confirmBeforeAdd}
      />
    </Section>
  );
};

export default LeasePropertySelector;
