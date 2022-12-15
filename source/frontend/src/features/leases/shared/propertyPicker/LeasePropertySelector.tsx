import { ModalProps } from 'components/common/GenericModal';
import MapSelectorContainer from 'components/propertySelector/MapSelectorContainer';
import { IMapProperty } from 'components/propertySelector/models';
import { ModalContext } from 'contexts/modalContext';
import { Section } from 'features/mapSideBar/tabs/Section';
import { IPropertyFilter } from 'features/properties/filter/IPropertyFilter';
import { FieldArray, FieldArrayRenderProps, FormikProps } from 'formik';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import useDeepCompareMemo from 'hooks/useDeepCompareMemo';
import { useProperties } from 'hooks/useProperties';
import { IProperty } from 'interfaces';
import { useCallback, useContext, useRef, useState } from 'react';
import { Col, Row } from 'react-bootstrap';

import { FormLeaseProperty, LeaseFormModel } from '../../models';
import SelectedPropertyHeaderRow from './selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from './selectedPropertyList/SelectedPropertyRow';

interface LeasePropertySelectorProp {
  formikProps: FormikProps<LeaseFormModel>;
}

export const LeasePropertySelector: React.FunctionComponent<
  React.PropsWithChildren<LeasePropertySelectorProp>
> = ({ formikProps }) => {
  const { values } = formikProps;

  const { getProperties } = useProperties();

  const { setModalContent, setDisplayModal } = useContext(ModalContext);
  const [propertiesToConfirm, setPropertiesToConfirm] = useState<FormLeaseProperty[]>([]);

  const [propertyIndexToRemove, setPropertyIndexToRemove] = useState<number | undefined>(undefined);

  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

  const addProperties = useCallback(
    (properties: FormLeaseProperty[]) => {
      if (arrayHelpersRef.current !== null && properties.length > 0) {
        properties.forEach(property => {
          arrayHelpersRef.current!.push(property);
        });
      }
    },
    [arrayHelpersRef],
  );

  const searchProperty = async (newProperty: IMapProperty): Promise<IProperty[] | undefined> => {
    const params: IPropertyFilter = {
      pinOrPid: (newProperty.pid || newProperty.pin || '')?.toString(),
      searchBy: 'pinOrPid',
      address: '',
    };

    const result = await getProperties.execute(params);
    return result?.items;
  };

  const confirmAdd = useCallback(() => {
    setDisplayModal(false);
    addProperties(propertiesToConfirm);
    setPropertiesToConfirm([]);
  }, [setDisplayModal, addProperties, propertiesToConfirm]);

  const cancelAdd = useCallback(() => {
    setDisplayModal(false);
    setPropertiesToConfirm([]);
  }, [setDisplayModal]);

  const addModalProps: ModalProps = useDeepCompareMemo(() => {
    return {
      title: 'Not inventory property',
      message:
        'You have selected a property not previously in the inventory. Do you want to add this property to the lease?',
      display: false,
      closeButton: false,
      okButtonText: 'Add',
      cancelButtonText: 'Cancel',
      handleOk: confirmAdd,
      handleCancel: cancelAdd,
    };
  }, [confirmAdd, cancelAdd]);

  useDeepCompareEffect(() => {
    setModalContent(addModalProps);
  }, [addModalProps]);

  const processAddedProperties = async (newProperties: IMapProperty[]) => {
    let needsWarning = false;
    const newFormProperties = [];

    for (let i = 0; i < newProperties.length; i++) {
      const property = newProperties[i];
      const formProperty = FormLeaseProperty.fromMapProperty(property);

      // Retrieve the pims id of the property if it exists
      if (formProperty.property !== undefined && formProperty.property.apiId === undefined) {
        const result = await searchProperty(property);
        if (result !== undefined && result.length > 0) {
          formProperty.property.apiId = result[0].id;
        }
      }

      newFormProperties.push(formProperty);

      if (formProperty.property?.apiId === undefined) {
        needsWarning = needsWarning || true;
      } else {
        needsWarning = needsWarning || false;
      }
    }

    if (needsWarning) {
      setPropertiesToConfirm(newFormProperties);
      setDisplayModal(true);
    } else {
      addProperties(newFormProperties);
    }
  };

  const cancelRemove = useCallback(() => {
    if (propertyIndexToRemove !== undefined) {
      setPropertyIndexToRemove(undefined);
    }
    setDisplayModal(false);
  }, [propertyIndexToRemove, setDisplayModal]);

  const confirmRemove = useCallback(() => {
    if (propertyIndexToRemove !== undefined) {
      arrayHelpersRef.current?.remove(propertyIndexToRemove);
      setPropertyIndexToRemove(undefined);
    }
    setDisplayModal(false);
  }, [propertyIndexToRemove, setDisplayModal]);

  const removeModalProps: ModalProps = useDeepCompareMemo(() => {
    return {
      title: 'Removing Property from form',
      message: 'Are you sure you want to remove this property from this lease/license?',
      display: false,
      closeButton: false,
      okButtonText: 'Remove',
      cancelButtonText: 'Cancel',
      handleOk: confirmRemove,
      handleCancel: cancelRemove,
    };
  }, [confirmRemove, cancelRemove]);

  useDeepCompareEffect(() => {
    setModalContent(removeModalProps);
  }, [removeModalProps]);

  const onRemoveClick = (index: number) => {
    setPropertyIndexToRemove(index);
    setDisplayModal(true);
  };

  return (
    <Section header="Properties to include in this file:">
      <div className="py-2">
        Select one or more properties that you want to include in this lease/license file. You can
        choose a location from the map, or search by other criteria.
      </div>

      <FieldArray
        name="properties"
        render={arrayHelpers => {
          arrayHelpersRef.current = arrayHelpers;
          return (
            <>
              <Row className="py-3 no-gutters">
                <Col>
                  <MapSelectorContainer
                    addSelectedProperties={processAddedProperties}
                    modifiedProperties={values.getPropertiesAsForm()}
                  />
                </Col>
              </Row>
              <Section header="Selected properties">
                <SelectedPropertyHeaderRow />
                {formikProps.values.properties.map((leaseProperty, index) => {
                  const property = leaseProperty?.property;
                  if (property !== undefined) {
                    return (
                      <SelectedPropertyRow
                        key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                        onRemove={() => onRemoveClick(index)}
                        nameSpace={`properties.${index}`}
                        index={index}
                        property={property}
                      />
                    );
                  }
                  return <></>;
                })}
                {formikProps.values.properties.length === 0 && <span>No Properties selected</span>}
              </Section>
            </>
          );
        }}
      />
    </Section>
  );
};

export default LeasePropertySelector;
