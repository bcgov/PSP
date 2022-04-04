import { Button } from 'components/common/form';
import GenericModal, { ModalSize } from 'components/common/GenericModal';
import { FormikProps } from 'formik';
import { IProperty } from 'interfaces';
import * as React from 'react';
import { useRef } from 'react';
import { useDispatch } from 'react-redux';
import { storeDraftProperties } from 'store/slices/properties';
import { useAppSelector } from 'store/store';
import styled from 'styled-components';

import PropertySelectorLayout from './layout/PropertySelectorLayout';
import MapSelectorContainer from './MapSelectorContainer';
import { IPropertySelectorModel } from './PropertySelectorFormView';

export interface IMapSelectorModalContainerProps {
  modalButtonText?: string;
  selectButtonText?: string;
  cancelButtonText?: string;
  setSelectedProperties: (properties: IProperty[]) => void;
  selectedProperties?: IProperty[];
}

export const MapSelectorModalContainer: React.FunctionComponent<IMapSelectorModalContainerProps> = ({
  modalButtonText,
  selectButtonText,
  cancelButtonText,
  setSelectedProperties,
  selectedProperties,
}) => {
  const [display, setDisplay] = React.useState(false);
  const draftProperties = useAppSelector(state => state?.properties?.draftProperties ?? []);
  const formikRef = useRef<FormikProps<IPropertySelectorModel>>() as React.MutableRefObject<
    FormikProps<IPropertySelectorModel>
  >;
  const dispatch = useDispatch();
  return (
    <>
      <Button onClick={() => setDisplay(true)}>{modalButtonText ?? 'Select Properties'}</Button>
      <StyledPopupWindow
        modalSize={ModalSize.XLARGE}
        display={display}
        setDisplay={setDisplay}
        title="Property Selection"
        message={
          <PropertySelectorLayout>
            <MapSelectorContainer formikRef={formikRef} properties={selectedProperties} />
          </PropertySelectorLayout>
        }
        handleCancel={() => {
          setDisplay(false);
          setSelectedProperties([]);
          dispatch(storeDraftProperties([]));
        }}
        handleOk={() => {
          setDisplay(false);
          setSelectedProperties(formikRef?.current?.values?.properties ?? []);
          dispatch(storeDraftProperties(draftProperties));
        }}
        okButtonText={selectButtonText ?? 'Add to File'}
        cancelButtonText={cancelButtonText ?? 'Cancel'}
        closeButton
        backdrop={false}
        asPopup={true}
      ></StyledPopupWindow>
    </>
  );
};

const StyledPopupWindow = styled(GenericModal)`
  top: 8rem;
  left: 7rem;
  width: 90rem;
  height: 78rem;
`;

export default MapSelectorModalContainer;
