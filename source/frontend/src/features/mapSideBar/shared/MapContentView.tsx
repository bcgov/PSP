import { FormikProps } from 'formik';
import * as React from 'react';
import { useContext } from 'react';

import { SideBarContext } from '../context/sidebarContext';
import MapSideBarLayout from '../layout/MapSideBarLayout';
import { IHeaderContainerProps } from './MapContentContainer';
import { ISidebarFooterProps } from './SidebarFooter';

export interface IMapContentViewProps {
  title: React.ReactNode;
  Header: React.FunctionComponent<React.PropsWithChildren<IHeaderContainerProps>>;
  Footer: React.FunctionComponent<React.PropsWithChildren<ISidebarFooterProps>>;
  icon: React.ReactNode | React.FunctionComponent<React.PropsWithChildren<unknown>>;
  isEditing: boolean;
  formikRef: React.RefObject<FormikProps<any>>;
  isFormValid: boolean;
  onClose: (() => void) | undefined;
  onSave: () => Promise<void>;
  onCancel: () => void;
}

export const MapContentView: React.FunctionComponent<IMapContentViewProps> = ({
  Header,
  Footer,
  title,
  icon,
  isEditing,
  formikRef,
  isFormValid,
  onClose,
  onCancel,
  onSave,
}) => {
  const { file, lastUpdatedBy } = useContext(SideBarContext);

  return (
    <MapSideBarLayout
      title={title}
      icon={icon}
      header={Header && <Header lastUpdatedBy={lastUpdatedBy} onClose={onClose} />}
      footer={
        isEditing && (
          <Footer onCancel={onCancel} onSave={onSave} displayRequiredFieldError={isFormValid} />
        )
      }
    ></MapSideBarLayout>
  );
};

export default MapContentView;
