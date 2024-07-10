import { useEffect, useState } from 'react';
import Nav from 'react-bootstrap/Nav';
import { FaExternalLinkAlt, FaQuestionCircle } from 'react-icons/fa';
import styled from 'styled-components';

import { H3 } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { useModalContext } from '@/hooks/useModalContext';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import useIsMounted from '@/hooks/util/useIsMounted';
import { useAppSelector } from '@/store/hooks';
import { useTenants } from '@/store/slices/tenants';
import { useTenant } from '@/tenants/useTenant';
import { exists } from '@/utils';

import IMailMessage from '../components/IMailMessage';
import HelpModalContentContainer from './HelpModalContentContainer';

/**
 * A help icon that displays the Help Modal when clicked. Does not display unless the user is authenticated.
 * @param props
 */
export function HelpContainer() {
  const keycloak = useKeycloakWrapper();
  const { setModalContent, setDisplayModal, modalProps } = useModalContext();
  const isMounted = useIsMounted();

  // this is the tenant info that gets injected into the frontend via a config-map in Openshift
  const tenant = useTenant();
  const { pimsTrainingResourceUrl } = tenant;

  // this is the tenant info that is fetched via web request to /api/tenants endpoint (and is then cached in Redux)
  const { getSettings } = useTenants();
  const [mailto, setMailto] = useState<IMailMessage | undefined>(undefined);
  const tenantsState = useAppSelector(state => state.tenants);
  const config = tenantsState?.config;
  const helpDeskEmail = exists(config?.settings?.helpDeskEmail)
    ? config!.settings!.helpDeskEmail
    : null;

  useEffect(() => {
    const update = async () => {
      if (!exists(config)) {
        await getSettings(); // TODO: PSP-4402 Determine why the HelpModal is being created two times, which results in two requests to the API.
      }
    };
    update();
  }, [getSettings, config]);

  useDeepCompareEffect(() => {
    if (isMounted()) {
      setModalContent({
        ...modalProps,
        okButtonHref:
          exists(helpDeskEmail) && exists(mailto)
            ? `mailto:${helpDeskEmail}?subject=${mailto?.subject}&body=${mailto?.body}`
            : undefined,
      });
    }
  }, [mailto, isMounted, setModalContent, modalProps, helpDeskEmail]);

  return keycloak.obj.authenticated ? (
    <Nav.Item>
      <TooltipWrapper tooltipId="help-tooltip" tooltip="Ask for Help">
        <StyledContainer
          onClick={() => {
            setModalContent({
              draggable: true,
              variant: 'info',
              cancelButtonText: 'No',
              okButtonText: 'Yes',
              title: 'Help Desk',
              headerIcon: <FaQuestionCircle size={22} />,
              message: (
                <>
                  <H3Styled>Get started with PIMS</H3Styled>
                  <p>
                    This overview has useful tools that will support you to start using the
                    application. You can also watch the video demos.
                  </p>
                  <LinkStyled target="_blank" href={pimsTrainingResourceUrl}>
                    PIMS Resources <FaExternalLinkAlt />
                  </LinkStyled>
                  <hr />
                  <HelpModalContentContainer setMailto={setMailto} />
                  <StyledConfirmationText>
                    Do you want to proceed and send the email?
                  </StyledConfirmationText>
                </>
              ),
              handleOk: () => setDisplayModal(false),
              handleOkDisabled: !exists(helpDeskEmail),
              okButtonHref:
                exists(helpDeskEmail) && exists(mailto)
                  ? `mailto:${helpDeskEmail}?subject=${mailto?.subject}&body=${mailto?.body}`
                  : undefined,
              handleCancel: () => setDisplayModal(false),
            });
            setDisplayModal(true);
          }}
        >
          <StyledHelpIcon size="24px" />
          <label>Help</label>
        </StyledContainer>
      </TooltipWrapper>
    </Nav.Item>
  ) : null;
}

const StyledHelpIcon = styled(FaQuestionCircle)`
  cursor: pointer;
`;

const StyledContainer = styled.div`
  display: flex;
  align-items: center;
  flex-direction: row;
  label {
    margin-left: 1rem;
    margin-bottom: 0;
    &:hover {
      cursor: pointer;
    }
  }
`;

const H3Styled = styled(H3)`
  border: none;
  margin-bottom: 16px;
`;

const LinkStyled = styled.a`
  margin-bottom: 16px;
  display: flex;
  align-items: center;
  gap: 0.5rem;
`;

const StyledConfirmationText = styled.p`
  margin-top: 24px;
`;

export default HelpContainer;
