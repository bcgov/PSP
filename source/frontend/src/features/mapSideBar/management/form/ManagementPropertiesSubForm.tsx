import { FieldArray, FormikProps } from 'formik';

import { Section } from '@/components/common/Section/Section';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';

import { PropertyForm } from '../../shared/models';
import { ManagementFormModel } from '../models/ManagementFormModel';

export interface ManagementPropertiesSubFormProps {
  formikProps: FormikProps<ManagementFormModel>;
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const ManagementPropertiesSubForm: React.FunctionComponent<ManagementPropertiesSubFormProps> = ({
  formikProps,
}) => {
  return (
    <>
      <div className="py-2">
        Select one or more properties that you want to include in this disposition. You can choose a
        location from the map, or search by other criteria.
      </div>

      <FieldArray name="fileProperties">
        {({ remove }) => (
          <Section header="Selected properties">
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

export default ManagementPropertiesSubForm;
