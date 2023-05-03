import clsx from 'classnames';
import * as Styled from 'components/common/styles';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Api_Compensation } from 'models/api/Compensation';
import { useState } from 'react';
import React from 'react';
import { MdClose } from 'react-icons/md';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

import UpdateCompensationRequisitionForm from './CompensationRequisitionForm';
import { CompensationRequisitionDetailContainer } from './detail/CompensationRequisitionDetailContainer';
import CompensationRequisitionDetailView from './detail/CompensationRequisitionDetailView';
import UpdateCompensationRequisitionContainer from './update/UpdateCompensationRequisitionContainer';

export interface CompensationRequisitionTrayViewProps {
  compensation?: Api_Compensation;
  clientConstant: string;
  gstConstant: number | undefined;
  onClose: () => void;
  loading: boolean;
  error: boolean;
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
}

export const CompensationRequisitionTrayView: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionTrayViewProps>
> = ({
  compensation,
  clientConstant,
  gstConstant,
  editMode,
  setEditMode,
  loading,
  error,
  onClose,
}) => {
  const [show, setShow] = useState(true);

  let detailViewContent =
    !editMode && compensation ? (
      <HalfHeightDiv>
        {!!compensation?.id && (
          <CompensationRequisitionDetailContainer
            compensation={compensation}
            View={CompensationRequisitionDetailView}
            clientConstant={clientConstant}
            gstConstant={gstConstant ?? 0}
            loading={false}
            setEditMode={setEditMode}
          ></CompensationRequisitionDetailContainer>
        )}
      </HalfHeightDiv>
    ) : undefined;

  let updateViewConent =
    editMode && compensation ? (
      <HalfHeightDiv>
        <UpdateCompensationRequisitionContainer
          compensation={compensation}
          formikRef={React.createRef()}
          onSuccess={() => {
            setShow(false);
            setEditMode(false);
            onClose();
          }}
          View={UpdateCompensationRequisitionForm}
        ></UpdateCompensationRequisitionContainer>
      </HalfHeightDiv>
    ) : undefined;

  let trayContent = editMode ? updateViewConent : detailViewContent;

  if (error) {
    trayContent = (
      <b>
        Failed to load Compensation requisition. Refresh the page or load another compensation
        requisition.
      </b>
    );
  } else if (!gstConstant) {
    trayContent = <b>Failed to load Gst Constant</b>;
  } else if (loading) {
    trayContent = <LoadingBackdrop parentScreen show={loading} />;
  }

  return (
    <ReactVisibilitySensor
      onChange={(isVisible: boolean) => {
        !isVisible && setShow(true);
      }}
    >
      <Styled.PopupTray className={clsx({ show: show })}>
        <Styled.TrayHeader>
          {editMode ? 'Edit ' : ''}Compensation Requisition (H120)
          <Styled.CloseButton
            id="close-tray"
            icon={<MdClose size={20} />}
            title="close"
            onClick={() => {
              setShow(false);
              setEditMode(false);
              onClose();
            }}
          ></Styled.CloseButton>
        </Styled.TrayHeader>
        <Styled.ActivityTrayPage>{trayContent}</Styled.ActivityTrayPage>
      </Styled.PopupTray>
    </ReactVisibilitySensor>
  );
};

const HalfHeightDiv = styled.div`
  flex-direction: column;
  display: flex;
  height: 50%;
`;
