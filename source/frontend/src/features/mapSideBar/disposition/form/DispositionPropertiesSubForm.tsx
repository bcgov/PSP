import { FieldArray, FormikProps } from 'formik';
import { geoJSON } from 'leaflet';
import { Col, Row } from 'react-bootstrap';
import { PiCornersOut } from 'react-icons/pi';

import { LinkButton } from '@/components/common/buttons/LinkButton';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Section } from '@/components/common/Section/Section';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { exists, latLngLiteralToGeometry } from '@/utils';

import { PropertyForm } from '../../shared/models';
import { DispositionFormModel } from '../models/DispositionFormModel';

export interface DispositionPropertiesSubFormProps {
  formikProps: FormikProps<DispositionFormModel>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const DispositionPropertiesSubForm: React.FunctionComponent<DispositionPropertiesSubFormProps> = ({
  formikProps,
}) => {
  const mapMachine = useMapStateMachine();

  const fitBoundaries = () => {
    const fileProperties = formikProps.values.fileProperties;

    if (exists(fileProperties)) {
      const locations = fileProperties.map(
        p => p?.polygon ?? latLngLiteralToGeometry(p?.fileLocation),
      );
      const bounds = geoJSON(locations).getBounds();

      mapMachine.requestFlyToBounds(bounds);
    }
  };

  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this disposition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="fileProperties">
        {({ remove }) => (
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
            {formikProps.values.fileProperties.map((property, index) => (
              <SelectedPropertyRow
                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                onRemove={() => remove(index)}
                nameSpace={`fileProperties.${index}`}
                index={index}
                property={property.toFeatureDataset()}
              />
            ))}
            {formikProps.values.fileProperties.length === 0 && <span>No Properties selected</span>}
          </Section>
        )}
      </FieldArray>
    </>
  );
};

export default DispositionPropertiesSubForm;
