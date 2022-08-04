import { UpdatePropertyDetailsContainer } from 'features/mapSideBar/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import { FormikProps } from 'formik';
import * as React from 'react';

import ComposedProperty from './ComposedProperty';
import PropertyContainer from './PropertyContainer';

export interface IPropertyViewSelectorProps {
  isEditMode: boolean;
  setEditMode: (isEditing: boolean) => void;
  setFormikRef: (ref: React.RefObject<FormikProps<any>> | undefined) => void;
  onSuccess: () => void;
  composedProperty: ComposedProperty;
}

const PropertyViewSelector: React.FunctionComponent<IPropertyViewSelectorProps> = props => {
  if (props.isEditMode && !!props?.composedProperty?.apiProperty?.id) {
    return (
      <UpdatePropertyDetailsContainer
        id={props.composedProperty.apiProperty.id}
        onSuccess={props.onSuccess}
        setFormikRef={props.setFormikRef}
      />
    );
  } else {
    return (
      <PropertyContainer
        composedProperty={props.composedProperty}
        setEditMode={props.setEditMode}
      />
    );
  }
};

export default PropertyViewSelector;
