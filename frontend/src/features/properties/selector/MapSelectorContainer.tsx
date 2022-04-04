import { MapCursors, PropertyPopUpContext } from 'components/maps/providers/PropertyPopUpProvider';
import { FormikProps } from 'formik';
import { IProperty } from 'interfaces';
import * as React from 'react';

import { PropertySelectorTabsView } from '../../mapSideBar/tabs/PropertySelectorTabsView';
import PropertySelectorFormView, { IPropertySelectorModel } from './PropertySelectorFormView';

export interface IMapSelectorContainerProps {
  formikRef: React.MutableRefObject<FormikProps<IPropertySelectorModel>>;
  properties?: IProperty[];
}

export const MapSelectorContainer: React.FunctionComponent<IMapSelectorContainerProps> = ({
  formikRef,
  properties,
}) => {
  const { setCursor, cursor } = React.useContext(PropertyPopUpContext);
  return (
    <>
      <PropertySelectorTabsView
        MapSelectorView={
          <PropertySelectorFormView
            onClickDraftMarker={() => setCursor(MapCursors.DRAFT)}
            onClickAway={() => {
              setCursor(undefined);
            }}
            selecting={cursor === MapCursors.DRAFT}
            formikRef={formikRef}
            properties={properties}
          />
        }
        ListSelectorView={<></>}
      ></PropertySelectorTabsView>
    </>
  );
};

export default MapSelectorContainer;
