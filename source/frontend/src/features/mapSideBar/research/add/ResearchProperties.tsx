import { FieldArray, useFormikContext } from 'formik';

import { Section } from '@/components/common/Section/Section';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';
import { useEditPropertiesMode } from '@/hooks/useEditPropertiesMode';

import { PropertyForm } from '../../shared/models';
import { AddPropertiesGuide } from './AddPropertiesGuide';
import { ResearchForm } from './models';

export interface IResearchPropertiesProps {
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const ResearchProperties: React.FC<IResearchPropertiesProps> = () => {
  const { values } = useFormikContext<ResearchForm>();

  useEditPropertiesMode();

  return (
    <Section header="Properties to include in this file:">
      <AddPropertiesGuide />
      <FieldArray name="properties">
        {({ remove }) => (
          <Section header="Selected properties">
            <SelectedPropertyHeaderRow />
            {values.properties.map((property, index) => (
              <SelectedPropertyRow
                key={`property.${property.latitude}-${property.longitude}-${property.pid}-${property.apiId}`}
                onRemove={() => remove(index)}
                nameSpace={`properties.${index}`}
                index={index}
                property={property.toFeatureDataset()}
              />
            ))}
            {values.properties.length === 0 && <span>No Properties selected</span>}
          </Section>
        )}
      </FieldArray>
    </Section>
  );
};

export default ResearchProperties;
