import { FieldArray, FormikProps } from 'formik';

import { Section } from '@/components/common/Section/Section';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';

import { PropertyForm } from '../../shared/models';
import { AcquisitionForm } from './models';

export interface IAcquisitionPropertiesProps {
  formikProps: FormikProps<AcquisitionForm>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

export const AcquisitionPropertiesSubForm: React.FunctionComponent<IAcquisitionPropertiesProps> = ({
  formikProps,
}) => {
  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this acquisition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="properties">
        {({ remove }) => (
          <Section header="Selected Properties">
            <SelectedPropertyHeaderRow />
            {formikProps.values.properties.map((property, index) => (
              <SelectedPropertyRow
                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                onRemove={() => remove(index)}
                nameSpace={`properties.${index}`}
                index={index}
                property={property.toFeatureDataset()}
              />
            ))}
            {formikProps.values.properties.length === 0 && <span>No Properties selected</span>}
          </Section>
        )}
      </FieldArray>
    </>
  );
};

export default AcquisitionPropertiesSubForm;
