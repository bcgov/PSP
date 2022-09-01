import clsx from 'classnames';
import { useState } from 'react';
import { MdClose } from 'react-icons/md';
import ReactVisibilitySensor from 'react-visibility-sensor';
import styled from 'styled-components';

import * as Styled from './styles';

interface IActivityTrayProps {
  activityId: number;
  onClose: () => void;
}

export const ActivityTray = (props: IActivityTrayProps) => {
  const { activityId, onClose } = props;
  const [show, setShow] = useState(true);

  return (
    <ReactVisibilitySensor
      onChange={isVisible => {
        !isVisible && setShow(true);
      }}
    >
      <Styled.ActivityTray className={clsx({ show: show })} data-testid="activity-tray">
        <Styled.TrayHeader>
          Activity: General
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
        Opened activity {activityId}
        <Styled.ActivityTrayPage>
          <HalfHeightDiv></HalfHeightDiv>
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
