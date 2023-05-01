import clsx from 'classnames';
import * as Styled from 'components/common/styles';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Api_Compensation } from 'models/api/Compensation';
import { useState } from 'react';
import { MdClose } from 'react-icons/md';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

export interface CompensationRequisitionDetailViewProps {
  compensation?: Api_Compensation;
  onClose: () => void;
  loading: boolean;
  error: boolean;
  editMode: boolean;
  setEditMode: (editMode: boolean) => void;
}

export const CompensationRequisitionDetailView: React.FunctionComponent<
  React.PropsWithChildren<CompensationRequisitionDetailViewProps>
> = ({ compensation, onClose, loading, error }) => {
  const [show, setShow] = useState(true);

  let trayContent = (
    <HalfHeightDiv>
      {!!compensation?.id && (
        <b>{compensation.id}</b>
        // <ActivityForm
        //   activity={{ ...activity, id: +activity.id }}
        //   file={file}
        //   isEditable={
        //     !(
        //       activity.activityStatusTypeCode?.id === 'CANCELLED' ||
        //       activity.activityStatusTypeCode?.id === 'COMPLETE'
        //     )
        //   }
        //   editMode={editMode}
        //   setEditMode={setEditMode}
        //   onSave={onSave}
        //   onEditRelatedProperties={onEditRelatedProperties}
        //   formContent={currentFormContent}
        // />
      )}
    </HalfHeightDiv>
  );
  if (error) {
    trayContent = (
      <b>
        Failed to load Compensation requisition. Refresh the page or load another compensation
        requisition.
      </b>
    );
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
          Compensation requisition&nbsp;-&nbsp;
          {loading ? '' : compensation?.id ?? 'Unknown'}
          <Styled.CloseButton
            id="close-tray"
            icon={<MdClose size={20} />}
            title="close"
            onClick={() => {
              setShow(false);
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
