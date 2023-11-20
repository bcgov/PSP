import { FormikProps } from 'formik';
import * as React from 'react';
import { useCallback, useRef, useState } from 'react';
import { useHistory, useLocation, useRouteMatch } from 'react-router-dom';

import { useQuery } from '@/hooks/use-query';
import { Api_LastUpdatedBy } from '@/models/api/File';

import { IMapSidebarPopoutRouterProps } from '../context/popoutContext';
import { IMapContentViewProps } from './MapContentView';
import { ISidebarFooterProps } from './SidebarFooter';

interface IMapContentContainerProps {
  View: React.FunctionComponent<React.PropsWithChildren<IMapContentViewProps>>;
  Header: React.FunctionComponent<React.PropsWithChildren<IHeaderContainerProps>>;
  Footer: React.FunctionComponent<ISidebarFooterProps>;
  PopupRouter: React.FunctionComponent<React.PropsWithChildren<IMapSidebarPopoutRouterProps>>;
  onClose: (() => void) | undefined;
  title: React.ReactNode;
  icon: React.ReactNode | React.FunctionComponent<React.PropsWithChildren<unknown>>;
}

export interface IHeaderContainerProps {
  lastUpdatedBy: Api_LastUpdatedBy | null;
  onClose: (() => void) | undefined;
}

export interface IBodyContainerProps {
  formikRef: React.RefObject<FormikProps<any>>;
}

export interface IFooterContainerProps {
  formikRef: React.RefObject<FormikProps<any>>;
  onSave: () => Promise<void>;
  onCancel: () => void;
}

export const MapContentContainer: React.FunctionComponent<
  React.PropsWithChildren<IMapContentContainerProps>
> = ({ View, Header, Footer, PopupRouter, title, icon, onClose, ...props }) => {
  const formikRef = useRef<FormikProps<any>>(null);
  const [isValid, setIsValid] = useState<boolean>(true);
  const [showConfirmModal, setShowConfirmModal] = useState<boolean>(false);

  const location = useLocation();
  const history = useHistory();
  const match = useRouteMatch();
  const query = useQuery();
  const isEditing = query.get('edit') === 'true';

  const setIsEditing = (value: boolean) => {
    if (value) {
      query.set('edit', value.toString());
    } else {
      query.delete('edit');
    }
    history.push({ search: query.toString() });
  };

  const handleSaveClick = async () => {
    await formikRef?.current?.validateForm();
    if (!formikRef?.current?.isValid) {
      setIsValid(false);
    } else {
      setIsValid(true);
    }

    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      formikRef.current?.submitForm();
    }
  };

  const handleCancelClick = () => {
    if (formikRef !== undefined) {
      if (formikRef.current?.dirty) {
        setShowConfirmModal(true);
      } else {
        handleCancelConfirm();
      }
    } else {
      handleCancelConfirm();
    }
  };

  const handleCancelConfirm = () => {
    if (formikRef !== undefined) {
      formikRef.current?.resetForm();
    }
    setShowConfirmModal(false);
    setIsEditing(false);
  };

  const close = useCallback(() => onClose && onClose(), [onClose]);

  return (
    <View
      title={title}
      icon={icon}
      Header={Header}
      Footer={Footer}
      formikRef={formikRef}
      isEditing={isEditing}
      onCancel={handleCancelClick}
      onClose={close}
      onSave={handleSaveClick}
      isFormValid={isValid}
    >
      {props.children}
    </View>
  );
};

export default MapContentContainer;
