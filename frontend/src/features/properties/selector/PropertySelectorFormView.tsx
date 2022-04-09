import * as Styled from 'components/common/styles';
import { StyledFormSection } from 'features/mapSideBar/tabs/SectionStyles';
import { FieldArray, Formik } from 'formik';
import { IProperty } from 'interfaces';
import { noop } from 'lodash';
import * as React from 'react';

import MapClickMonitor from './components/MapClickMonitor';
import SelectedPropertyHeaderRow from './components/SelectedPropertyHeaderRow';
import SelectedPropertyRow from './components/SelectedPropertyRow';
import PropertySelectorSubForm from './PropertySelectorSubForm';

export interface IPropertySelectorModel {
  properties: IProperty[];
}

export interface IPropertySelectorFormViewProps {
  onClickDraftMarker: () => void;
  onClickAway: () => void;
  selecting?: boolean;
  properties?: IProperty[];
}

const PropertySelectorFormView: React.FunctionComponent<IPropertySelectorFormViewProps> = ({
  onClickDraftMarker,
  onClickAway,
  properties,
}) => {
  const initial: IPropertySelectorModel = { properties: properties ?? [] };
  return (
    <Formik initialValues={initial} onSubmit={noop}>
      {({ values }) => (
        <>
          <StyledFormSection>
            <Styled.H3>Select a property</Styled.H3>
            <PropertySelectorSubForm
              onClickDraftMarker={onClickDraftMarker}
              onClickAway={onClickAway}
              nameSpace={`properties.${values.properties?.length - 1}`}
            />
          </StyledFormSection>
          <StyledFormSection>
            <Styled.H3>Selected properties</Styled.H3>
            <FieldArray name="properties">
              {({ push, remove }) => (
                <>
                  <SelectedPropertyHeaderRow />
                  <MapClickMonitor addProperty={push} />
                  {values.properties.map((property, index, properties) => (
                    <SelectedPropertyRow
                      key={`property.${property.latitude}-${property.longitude}-${property.pid}`}
                      onRemove={() => remove(index)}
                      nameSpace={`properties.${index}`}
                      index={index}
                    />
                  ))}
                </>
              )}
            </FieldArray>
          </StyledFormSection>
        </>
      )}
    </Formik>
  );
};

export default PropertySelectorFormView;
