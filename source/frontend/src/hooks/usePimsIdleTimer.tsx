import { useKeycloak } from '@react-keycloak/web';
import { useModalContext } from 'hooks/useModalContext';
import { useIdleTimer } from 'react-idle-timer';
import { useTenant } from 'tenants';

const usePimsIdleTimer = () => {
  const { keycloak } = useKeycloak();
  const { idlePromptTimeout, idleTimeout } = useTenant();

  // when we are idle, log out of the application.
  const onIdle = () => {
    keycloak.logout();
  };

  // when the prompt timer expires, display the idle prompt.
  const onPrompt = () => {
    setDisplayModal(true);
  };

  const onActive = () => {
    console.log('onActive');
    idleProps.start();
  };

  const onAction = () => {
    console.log('onEvent');
    idleProps.start();
  };

  // a custom styled pims prompt that resets the active timer if ok is clicked or immediately triggers the idle action if canceled
  const { setDisplayModal } = useModalContext({
    title: 'Still Working?',
    message: 'You have been idle for some time. Would you like to remain logged in?',
    okButtonText: 'Keep working',
    cancelButtonText: 'Log out',
    handleOk: () => {
      idleProps.activate();
      setDisplayModal(false);
    },
    handleCancel: onIdle,
  });

  const idleProps = useIdleTimer({
    onIdle,
    onPrompt,
    onActive,
    onAction,
    eventsThrottle: 500,
    timeout: 1000 * 60 * idleTimeout, // how long before the prompt is displayed
    promptTimeout: 1000 * 60 * idlePromptTimeout, // how long after the prompt is displayed to be auto logged out
  });

  return idleProps;
};

export default usePimsIdleTimer;
