import { FieldArray, useFormikContext } from 'formik';
import { useEffect } from 'react';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { Section } from '@/components/common/Section/Section';
import SelectedPropertyHeaderRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyHeaderRow';
import SelectedPropertyRow from '@/components/propertySelector/selectedPropertyList/SelectedPropertyRow';

import { PropertyForm } from '../../shared/models';
import { ResearchForm } from './models';

export interface IResearchPropertiesProps {
  confirmBeforeAdd: (propertyForm: PropertyForm) => Promise<boolean>;
}

const ResearchProperties: React.FC<IResearchPropertiesProps> = () => {
  const { values } = useFormikContext<ResearchForm>();

  const { setEditPropertiesMode } = useMapStateMachine();

  useEffect(() => {
    setEditPropertiesMode(true);
  }, [setEditPropertiesMode]);

  useEffect(() => {
    // Set the map state machine to edit properties mode so that the map selector knows what mode it is in.
    setEditPropertiesMode(true);
    return () => {
      setEditPropertiesMode(false);
    };
  }, [setEditPropertiesMode]);

  return (
    <Section header="Properties to include in this file:">
      <div className="py-2">
        Select one or more properties that you want to include in this research file. You can choose
        a location from the map, or search by other criteria.
      </div>

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
