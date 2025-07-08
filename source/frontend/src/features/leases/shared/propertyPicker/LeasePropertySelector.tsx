import { FieldArray, FieldArrayRenderProps, FormikProps } from 'formik';
import { geoJSON, LatLngLiteral } from 'leaflet';
import isNumber from 'lodash/isNumber';
import { useCallback, useContext, useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import { PiCornersOut } from 'react-icons/pi';

import { LinkButton } from '@/components/common/buttons';
import { ModalProps } from '@/components/common/GenericModal';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import MapSelectorContainer from '@/components/propertySelector/MapSelectorContainer';
import { IMapProperty } from '@/components/propertySelector/models';
import { ModalContext } from '@/contexts/modalContext';
import { AddressForm } from '@/features/mapSideBar/shared/models';
import { IPropertyFilter } from '@/features/properties/filter/IPropertyFilter';
import { useBcaAddress } from '@/features/properties/map/hooks/useBcaAddress';
import { useProperties } from '@/hooks/repositories/useProperties';
import { ApiGen_Concepts_PropertyView } from '@/models/api/generated/ApiGen_Concepts_PropertyView';
import {
  exists,
  isLatLngInFeatureSetBoundary,
  isValidId,
  isValidString,
  latLngLiteralToGeometry,
} from '@/utils';

import { FormLeaseProperty, LeaseFormModel } from '../../models';
import SelectedPropertyHeaderRow from './selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from './selectedPropertyList/SelectedPropertyRow';

interface LeasePropertySelectorProp {
  formikProps: FormikProps<LeaseFormModel>;
}

export const LeasePropertySelector: React.FunctionComponent<LeasePropertySelectorProp> = ({
  formikProps,
}) => {
  const { values, setFieldValue } = formikProps;
  const { getPropertiesFromView: getProperties } = useProperties();
  const { setModalContent, setDisplayModal } = useContext(ModalContext);
  const { getPrimaryAddressByPid, bcaLoading } = useBcaAddress();

  const mapMachine = useMapStateMachine();

  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

  const addProperties = useCallback(
    (properties: FormLeaseProperty[]) => {
      if (arrayHelpersRef.current !== null && properties.length > 0) {
        properties.forEach((leaseProperty, index) => {
          const property = leaseProperty?.property;
          if (
            values.properties?.length === 0 &&
            index === 0 &&
            property !== undefined &&
            property.regionName !== 'Cannot determine'
          ) {
            setFieldValue('regionId', property.region ? property.region.toString() : '');
          }

          arrayHelpersRef.current && arrayHelpersRef.current.push(leaseProperty);
        });
      }
    },
    [arrayHelpersRef, setFieldValue, values.properties?.length],
  );

  const searchProperty = async (
    newProperty: IMapProperty,
  ): Promise<ApiGen_Concepts_PropertyView[] | undefined> => {
    const params: IPropertyFilter = {
      pid: exists(newProperty.pid) ? newProperty.pid.toString() : '',
      pin: exists(newProperty.pin) ? newProperty.pin.toString() : '',
      searchBy: exists(newProperty.pid) ? 'pid' : 'pin',
      address: '',
      page: undefined,
      quantity: undefined,
      latitude: undefined,
      longitude: undefined,
      historical: '',
      ownership: '',
      coordinates: null,
      name: '',
      section: '',
      township: '',
      range: '',
      district: '',
    };

    const result = await getProperties.execute(params);
    return result?.items ?? undefined;
  };

  const fitBoundaries = () => {
    const fileProperties = values.properties;

    if (exists(fileProperties)) {
      const locations = fileProperties
        .map(p => p?.property?.polygon ?? latLngLiteralToGeometry(p?.property?.fileLocation))
        .filter(exists);
      const bounds = geoJSON(locations).getBounds();

      if (exists(bounds) && bounds.isValid()) {
        mapMachine.requestFlyToBounds(bounds);
      }
    }
  };

  const confirmAdd = useCallback(
    (propertiesToConfirm: FormLeaseProperty[]) => {
      setDisplayModal(false);
      addProperties(propertiesToConfirm);
    },
    [setDisplayModal, addProperties],
  );

  const cancelAdd = useCallback(() => {
    setDisplayModal(false);
  }, [setDisplayModal]);

  const getAddModalProps = useCallback<(properties: FormLeaseProperty[]) => ModalProps>(
    (properties: FormLeaseProperty[]) => {
      return {
        variant: 'info',
        title: 'Not inventory property',
        message:
          'You have selected a property not previously in the inventory. Do you want to add this property to the lease?',
        display: false,
        okButtonText: 'Add',
        cancelButtonText: 'Cancel',
        handleOk: () => confirmAdd(properties),
        handleCancel: cancelAdd,
      };
    },
    [confirmAdd, cancelAdd],
  );

  const processAddedProperties = async (newProperties: SelectedFeatureDataset[]) => {
    let needsWarning = false;
    const newFormProperties: FormLeaseProperty[] = [];

    await newProperties.reduce(async (promise, property) => {
      return promise.then(async () => {
        const formProperty = FormLeaseProperty.fromFeatureDataset(property);

        const bcaSummary = formProperty?.property?.pid
          ? await getPrimaryAddressByPid(formProperty?.property?.pid, 30000)
          : undefined;

        // Retrieve the pims id of the property if it exists
        if (exists(formProperty.property) && !isValidId(formProperty.property.apiId)) {
          needsWarning = true;

          formProperty.property.address = bcaSummary?.address
            ? AddressForm.fromBcaAddress(bcaSummary?.address)
            : undefined;

          if (
            isValidString(formProperty?.property?.pid) ||
            isValidString(formProperty?.property?.pin)
          ) {
            const result = await searchProperty(formProperty.property.toMapProperty());
            if (result !== undefined && result.length > 0) {
              formProperty.property.apiId = result[0].id;
            }
          }
        }

        newFormProperties.push(formProperty);
      });
    }, Promise.resolve());

    if (needsWarning) {
      setModalContent(getAddModalProps(newFormProperties));
      setDisplayModal(true);
    } else {
      addProperties(newFormProperties);
    }
  };

  const cancelRemove = useCallback(() => {
    setDisplayModal(false);
  }, [setDisplayModal]);

  const confirmRemove = useCallback(
    (indexToRemove: number) => {
      if (indexToRemove !== undefined) {
        arrayHelpersRef.current?.remove(indexToRemove);
      }
      setDisplayModal(false);
    },
    [setDisplayModal],
  );

  const getRemoveModalProps = useCallback<(index: number) => ModalProps>(
    (index: number) => {
      return {
        variant: 'info',
        title: 'Removing Property from Lease/Licence',
        message: 'Are you sure you want to remove this property from this lease/licence?',
        display: false,
        okButtonText: 'Remove',
        cancelButtonText: 'Cancel',
        handleOk: () => confirmRemove(index),
        handleCancel: cancelRemove,
      };
    },
    [confirmRemove, cancelRemove],
  );

  const onRemoveClick = useCallback(
    (index: number) => {
      setModalContent(getRemoveModalProps(index));
      setDisplayModal(true);
    },
    [getRemoveModalProps, setDisplayModal, setModalContent],
  );

  return (
    <>
      <LoadingBackdrop show={bcaLoading} />
      <Section header="Properties to include in this file:">
        <div className="py-2">
          Select one or more properties that you want to include in this lease/licence file. You can
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
                      repositionSelectedProperty={(
                        featureset: SelectedFeatureDataset,
                        latLng: LatLngLiteral,
                        index: number | null,
                      ) => {
                        // As long as the marker is repositioned within the boundary of the originally selected property simply reposition the marker without further notification.
                        if (
                          isNumber(index) &&
                          index >= 0 &&
                          isLatLngInFeatureSetBoundary(latLng, featureset)
                        ) {
                          const formProperty = formikProps.values.properties[index];
                          const updatedFormProperty =
                            FormLeaseProperty.fromFormLeaseProperty(formProperty);
                          updatedFormProperty.property.fileLocation = latLng;

                          // Find property within formik values and reposition it based on incoming file marker position
                          arrayHelpers.replace(index, updatedFormProperty);
                        }
                      }}
                      modifiedProperties={LeaseFormModel.getPropertiesAsForm(values).map(p =>
                        p.toFeatureDataset(),
                      )}
                    />
                  </Col>
                </Row>
                <Section
                  header={
                    <Row>
                      <Col xs="11">Selected Properties</Col>
                      <Col>
                        <TooltipWrapper
                          tooltip="Fit map to the file properties"
                          tooltipId="property-selector-tooltip"
                        >
                          <LinkButton title="Fit boundaries button" onClick={fitBoundaries}>
                            <PiCornersOut size={18} className="mr-2" />
                          </LinkButton>
                        </TooltipWrapper>
                      </Col>
                    </Row>
                  }
                >
                  <SelectedPropertyHeaderRow />
                  {formikProps.values.properties.map((leaseProperty, index) => {
                    const property = leaseProperty?.property;
                    if (property !== undefined) {
                      return (
                        <SelectedPropertyRow
                          formikProps={formikProps}
                          key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                          onRemove={() => onRemoveClick(index)}
                          nameSpace={`properties.${index}`}
                          index={index}
                          property={property.toFeatureDataset()}
                          showSeparator={index < formikProps.values.properties.length - 1}
                        />
                      );
                    }
                    return <></>;
                  })}
                  {formikProps.values.properties.length === 0 && (
                    <span>No Properties selected</span>
                  )}
                </Section>
              </>
            );
          }}
        />
      </Section>
    </>
  );
};

export default LeasePropertySelector;
