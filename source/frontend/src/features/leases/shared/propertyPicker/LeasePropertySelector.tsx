import { ModalProps } from 'components/common/GenericModal';
import MapSelectorContainer from 'components/propertySelector/MapSelectorContainer';
import { IMapProperty } from 'components/propertySelector/models';
import { ModalContext } from 'contexts/modalContext';
import { Section } from 'features/mapSideBar/tabs/Section';
import { IPropertyFilter } from 'features/properties/filter/IPropertyFilter';
import { FieldArray, FieldArrayRenderProps, FormikProps } from 'formik';
import useDeepCompareEffect from 'hooks/useDeepCompareEffect';
import { useProperties } from 'hooks/useProperties';
import { IProperty } from 'interfaces';
import { useCallback, useContext, useMemo, useRef, useState } from 'react';
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
  const [propertyToAdd, setPropertyToAdd] = useState<FormLeaseProperty | undefined>(undefined);

  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

  const addProperty = useCallback(() => {
    if (arrayHelpersRef.current !== null && propertyToAdd !== undefined) {
      arrayHelpersRef.current.push(propertyToAdd);

      setPropertyToAdd(undefined);
    }
  }, [setPropertyToAdd, arrayHelpersRef, propertyToAdd]);

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
    addProperty();
  }, [setDisplayModal, addProperty]);

  const cancelAdd = useCallback(() => {
    setDisplayModal(false);
    setPropertyToAdd(undefined);
  }, [setDisplayModal]);

  const customModalProps: ModalProps = useMemo(() => {
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
    setModalContent(customModalProps);
  }, [customModalProps]);

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
                    onSelectedProperty={async (newProperty: IMapProperty) => {
                      const formProperty = FormLeaseProperty.fromMapProperty(newProperty);

                      // Retrieve the pims id of the property if it exists
                      if (
                        formProperty.property !== undefined &&
                        formProperty.property.apiId === undefined
                      ) {
                        const result = await searchProperty(newProperty);
                        if (result?.length === 1) {
                          formProperty.property.apiId = result[0].id;
                        }
                      }

                      setPropertyToAdd(formProperty);

                      if (formProperty.property?.apiId === undefined) {
                        setDisplayModal(true);
                      } else {
                        addProperty();
                      }
                    }}
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
                        onRemove={() => arrayHelpers.remove(index)}
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
