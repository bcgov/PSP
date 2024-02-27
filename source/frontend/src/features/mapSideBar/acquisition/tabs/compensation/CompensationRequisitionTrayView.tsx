import clsx from 'classnames';
import React from 'react';
import { MdClose } from 'react-icons/md';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import * as Styled from '@/components/common/styles';
import { ApiGen_Concepts_AcquisitionFile } from '@/models/api/generated/ApiGen_Concepts_AcquisitionFile';
import { ApiGen_Concepts_CompensationRequisition } from '@/models/api/generated/ApiGen_Concepts_CompensationRequisition';

import { CompensationRequisitionDetailContainer } from './detail/CompensationRequisitionDetailContainer';
import CompensationRequisitionDetailView from './detail/CompensationRequisitionDetailView';
import UpdateCompensationRequisitionContainer from './update/UpdateCompensationRequisitionContainer';
import UpdateCompensationRequisitionForm from './update/UpdateCompensationRequisitionForm';

export interface CompensationRequisitionTrayViewProps {
  compensation?: ApiGen_Concepts_CompensationRequisition;
  acquisitionFile: ApiGen_Concepts_AcquisitionFile;
  clientConstant: string;
  gstConstant: number | undefined;
  onClose: () => void;
  loading: boolean;
  error: boolean;
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
  show: boolean;
  setShow: (show: boolean) => void;
  onUpdate: () => void;
}

export const CompensationRequisitionTrayView: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionTrayViewProps>
> = ({
  compensation,
  acquisitionFile,
  clientConstant,
  gstConstant,
  editMode,
  setEditMode,
  show,
  setShow,
  loading,
  error,
  onClose,
  onUpdate,
}) => {
  const detailViewContent =
    !editMode && compensation ? (
      <HalfHeightDiv>
        {!!compensation?.id && acquisitionFile && (
          <CompensationRequisitionDetailContainer
            compensation={compensation}
            acquisitionFile={acquisitionFile}
            View={CompensationRequisitionDetailView}
            clientConstant={clientConstant}
            loading={loading}
            setEditMode={setEditMode}
          ></CompensationRequisitionDetailContainer>
        )}
      </HalfHeightDiv>
    ) : undefined;

  const updateViewConent =
    editMode && compensation ? (
      <HalfHeightDiv>
        <UpdateCompensationRequisitionContainer
          compensation={compensation}
          acquisitionFile={acquisitionFile}
          onSuccess={() => {
            setEditMode(false);
            onUpdate();
          }}
          onCancel={() => {
            setEditMode(false);
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
        <Styled.TrayContent>{trayContent}</Styled.TrayContent>
      </Styled.PopupTray>
    </ReactVisibilitySensor>
  );
};

const HalfHeightDiv = styled.div`
  flex-direction: column;
  display: flex;
  height: 50%;
`;
