import { FieldArray, FieldArrayRenderProps, FormikProps } from 'formik';
import noop from 'lodash/noop';
import { useCallback, useContext, useEffect, useMemo, useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import styled from 'styled-components';

import { Button } from '@/components/common/buttons';
import { ModalProps } from '@/components/common/GenericModal';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { SelectedFeatureDataset } from '@/components/common/mapFSM/useLocationFeatureLoader';
import { Section } from '@/components/common/Section/Section';
import { ZoomIconType, ZoomToLocation } from '@/components/maps/ZoomToLocation';
import { ModalContext } from '@/contexts/modalContext';
import { PropertyForm } from '@/features/mapSideBar/shared/models';
import AddPropertiesGuide from '@/features/mapSideBar/shared/update/properties/AddPropertiesGuide';
import { useFeatureDatasetsWithAddresses } from '@/hooks/useFeatureDatasetsWithAddresses';
import { exists, firstOrNull } from '@/utils';
import { addPropertiesToCurrentFile } from '@/utils/propertyUtils';

import { LeaseFormModel } from '../../models';
import SelectedPropertyHeaderRow from './selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from './selectedPropertyList/SelectedPropertyRow';

interface LeasePropertySelectorProp {
  formikProps: FormikProps<LeaseFormModel>;
}

export const LeasePropertySelector: React.FunctionComponent<LeasePropertySelectorProp> = ({
  formikProps,
}) => {
  const localRef = useRef<FormikProps<LeaseFormModel>>(null);

  const { setModalContent, setDisplayModal } = useContext(ModalContext);

  const { selectedFeatures, processCreation, mapLocationFeatureDataset, prepareForCreation } =
    useMapStateMachine();

  const { featuresWithAddresses } = useFeatureDatasetsWithAddresses(selectedFeatures ?? []);

  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

  const selectedFeatureDataset = useMemo<SelectedFeatureDataset>(() => {
    return {
      selectingComponentId: mapLocationFeatureDataset?.selectingComponentId ?? null,
      location: mapLocationFeatureDataset?.location,
      fileLocation: mapLocationFeatureDataset?.fileLocation ?? null,
      parcelFeature: firstOrNull(mapLocationFeatureDataset?.parcelFeatures),
      pimsFeature: firstOrNull(mapLocationFeatureDataset?.pimsFeatures),
      regionFeature: mapLocationFeatureDataset?.regionFeature ?? null,
      districtFeature: mapLocationFeatureDataset?.districtFeature ?? null,
      municipalityFeature: firstOrNull(mapLocationFeatureDataset?.municipalityFeatures),
      isActive: true,
      displayOrder: 0,
    };
  }, [
    mapLocationFeatureDataset?.selectingComponentId,
    mapLocationFeatureDataset?.location,
    mapLocationFeatureDataset?.fileLocation,
    mapLocationFeatureDataset?.parcelFeatures,
    mapLocationFeatureDataset?.pimsFeatures,
    mapLocationFeatureDataset?.regionFeature,
    mapLocationFeatureDataset?.districtFeature,
    mapLocationFeatureDataset?.municipalityFeatures,
  ]);

  // Convert SelectedFeatureDataset to PropertyForm
  const propertyForms = useMemo(
    () =>
      featuresWithAddresses.map(obj => {
        const property = PropertyForm.fromFeatureDataset(obj.feature);
        if (exists(obj.address)) {
          property.address = obj.address;
        }
        return property;
      }),
    [featuresWithAddresses],
  );

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

  const handleAddToSelection = useCallback(() => {
    prepareForCreation([selectedFeatureDataset]);
  }, [prepareForCreation, selectedFeatureDataset]);

  useEffect(() => {
    if (exists(localRef.current) && propertyForms.length > 0) {
      addPropertiesToCurrentFile(localRef, 'properties', propertyForms, noop);
      processCreation();
    }
  }, [localRef, processCreation, propertyForms]);

  return (
    <Section header="Properties to include in this file:">
      <div className="py-2">
        Select one or more properties that you want to include in this lease/licence file. You can
        choose a location from the map, or search by other criteria.
      </div>

      <AddPropertiesGuide />
      {exists(selectedFeatureDataset?.parcelFeature) && (
        <StyledButtonWrapper>
          <Button onClick={handleAddToSelection}>Add selected property</Button>
        </StyledButtonWrapper>
      )}

      <FieldArray
        name="properties"
        render={arrayHelpers => {
          arrayHelpersRef.current = arrayHelpers;
          return (
            <Section
              header={
                <Row>
                  <Col xs="11">Selected Properties</Col>
                  <Col>
                    <ZoomToLocation
                      icon={ZoomIconType.area}
                      formProperties={formikProps?.values?.properties?.map(lf => lf?.property)}
                    />
                  </Col>
                </Row>
              }
            >
              <SelectedPropertyHeaderRow />
              {formikProps.values.properties.map((leaseProperty, index) => {
                const property = leaseProperty?.property;
                if (exists(property)) {
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
              {formikProps.values.properties.length === 0 && <span>No Properties selected</span>}
            </Section>
          );
        }}
      />
    </Section>
  );
};

export default LeasePropertySelector;

const StyledButtonWrapper = styled.div`
  margin: 0 1.6rem;
  padding-left: 1.6rem;
  text-align: left;
  text-underline-offset: 2px;

  button {
    font-size: 14px;
  }
`;
