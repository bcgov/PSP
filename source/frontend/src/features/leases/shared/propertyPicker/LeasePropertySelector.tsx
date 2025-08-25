import { FieldArray, FieldArrayRenderProps, FormikProps } from 'formik';
import { geoJSON } from 'leaflet';
import { useCallback, useContext, useRef } from 'react';
import { Col, Row } from 'react-bootstrap';
import { PiCornersOut } from 'react-icons/pi';

import { LinkButton } from '@/components/common/buttons';
import { ModalProps } from '@/components/common/GenericModal';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Section } from '@/components/common/Section/Section';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { ModalContext } from '@/contexts/modalContext';
import { exists, latLngLiteralToGeometry } from '@/utils';

import { LeaseFormModel } from '../../models';
import SelectedPropertyHeaderRow from './selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from './selectedPropertyList/SelectedPropertyRow';

interface LeasePropertySelectorProp {
  formikProps: FormikProps<LeaseFormModel>;
}

export const LeasePropertySelector: React.FunctionComponent<LeasePropertySelectorProp> = ({
  formikProps,
}) => {
  const { values } = formikProps;
  const { setModalContent, setDisplayModal } = useContext(ModalContext);

  const mapMachine = useMapStateMachine();

  const arrayHelpersRef = useRef<FieldArrayRenderProps | null>(null);

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
              {formikProps.values.properties.length === 0 && <span>No Properties selected</span>}
            </Section>
          );
        }}
      />
    </Section>
  );
};

export default LeasePropertySelector;
