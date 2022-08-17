import clsx from 'classnames';
import LoadingBackdrop from 'components/maps/leaflet/LoadingBackdrop/LoadingBackdrop';
import { Api_Activity } from 'models/api/Activity';
import { useState } from 'react';
import { MdClose } from 'react-icons/md';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

import * as Styled from './styles';

interface IActivityTrayProps {
  activityId?: number;
  activity?: Api_Activity;
  onClose: () => void;
  loading: boolean;
  error: boolean;
}

export const ActivityTray = ({
  activityId,
  activity,
  onClose,
  error,
  loading,
}: IActivityTrayProps) => {
  const [show, setShow] = useState(true);

  return (
    <ReactVisibilitySensor
      onChange={isVisible => {
        !isVisible && setShow(true);
      }}
    >
      <Styled.ActivityTray className={clsx({ show: show })} data-testid="activity-tray">
        <Styled.TrayHeader>
          Activity&nbsp;-&nbsp;
          {loading
            ? ''
            : activity?.activityTemplate?.activityTemplateTypeCode?.description ?? 'Unknown'}
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
        <Styled.ActivityTrayPage>
          {error ? (
            <b>Failed to load activity. Refresh the page or load another activity.</b>
          ) : loading ? (
            <LoadingBackdrop parentScreen show={loading} />
          ) : (
            <HalfHeightDiv></HalfHeightDiv>
          )}
        </Styled.ActivityTrayPage>
      </Styled.ActivityTray>
    </ReactVisibilitySensor>
  );
};

const HalfHeightDiv = styled.div`
  flex-direction: column;
  display: flex;
  height: 50%;
`;
