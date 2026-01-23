import { Location } from 'history';
import { useEffect, useState } from 'react';
import { Prompt } from 'react-router-dom';

import { getCancelModalProps, useModalContext } from '@/hooks/useModalContext';

interface Props {
  when?: boolean | undefined;
  navigate: (path: string) => void;
  shouldBlockNavigation: (location: Location) => boolean;
  showModal?: boolean;
}

const ConfirmNavigation = ({ when, navigate, shouldBlockNavigation, showModal = true }: Props) => {
  const { setDisplayModal, setModalContent } = useModalContext();
  const [lastLocation, setLastLocation] = useState<Location | null>(null);
  const [confirmedNavigation, setConfirmedNavigation] = useState(false);

  const handleConfirmNavigationClick = () => {
    setDisplayModal(false);
    setConfirmedNavigation(true);
  };

  const handleBlockedNavigation = (nextLocation: Location): boolean => {
    if (!confirmedNavigation && shouldBlockNavigation(nextLocation)) {
      if (showModal) {
        setModalContent({
          ...getCancelModalProps(),
          handleOk: () => handleConfirmNavigationClick(),
        });
        setDisplayModal(true);
      }
      setLastLocation(nextLocation);
      return false;
    }
    return true;
  };

  useEffect(() => {
    if (confirmedNavigation && lastLocation) {
      // Navigate to the previous blocked location with your navigate function
      navigate(lastLocation.pathname);
      // Reset state after navigation so future navigation can be blocked again
      setConfirmedNavigation(false);
      setLastLocation(null);
    }
  }, [confirmedNavigation, lastLocation, navigate, setConfirmedNavigation, setLastLocation]);

  return <Prompt when={when} message={handleBlockedNavigation} />;
};
export default ConfirmNavigation;
