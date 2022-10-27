import { UpdatePropertyDetailsContainer } from 'features/mapSideBar/tabs/propertyDetails/update/UpdatePropertyDetailsContainer';
import { FormikProps } from 'formik';
import * as React from 'react';

import ComposedProperty from './ComposedProperty';
import PropertyContainer from './PropertyContainer';

export interface IPropertyViewSelectorProps {
  isEditMode: boolean;
  setEditMode: (isEditing: boolean) => void;
  onSuccess: () => void;
  composedProperty: ComposedProperty;
}

const PropertyViewSelector = React.forwardRef<FormikProps<any>, IPropertyViewSelectorProps>(
  (props, formikRef) => {
    if (props.isEditMode && !!props?.composedProperty?.apiWrapper?.response?.id) {
      return (
        <UpdatePropertyDetailsContainer
          id={props.composedProperty?.apiWrapper?.response?.id}
          onSuccess={props.onSuccess}
          ref={formikRef}
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
  },
);

export default PropertyViewSelector;
