import axios, { AxiosError } from 'axios';
import React, { useCallback, useEffect, useState } from 'react';
import { ErrorBoundary } from 'react-error-boundary';
import LoadingBar from 'react-redux-loading-bar';

import ErrorModal from '@/components/common/ErrorModal';
import { Footer, Header } from '@/components/layout';
import HealthcheckView, {
  IHealthCheckIssue,
} from '@/components/layout/Healthcheck/HealthcheckView';
import ISystemCheck from '@/hooks/pims-api/interfaces/ISystemCheck';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { IApiError } from '@/interfaces/IApiError';
import { useTenant } from '@/tenants/useTenant';

import FooterStyled from './Footer';
import HeaderStyled from './Header';
import HealthCheckStyled from './Healthcheck';
import { HealthcheckMessagesTypesEnum } from './models/HealthcheckMessagesTypes';
import * as Styled from './styles';

const PublicLayout: React.FC<React.PropsWithChildren<React.HTMLAttributes<HTMLElement>>> = ({
  children,
  ...rest
}) => {
  const [systemChecked, setSystemChecked] = useState<boolean>(null);
  const [systemDegraded, setSystemDegraded] = useState<boolean>(false);
  const [healthCheckIssues, setHealthCheckIssues] = useState<IHealthCheckIssue[]>(null);

  const { pimsHealthcheckMessages } = useTenant();
  const { getLive, getSystemCheck } = useApiHealth();
  const keycloak = useKeycloakWrapper();

  const fetchSystemCheckInformation = useCallback(async () => {
    const systemIssues: IHealthCheckIssue[] = [];
    try {
      const pimsApi = await getLive();
      if (pimsApi.data.status !== 'Healthy') {
        systemIssues.push({
          key: HealthcheckMessagesTypesEnum.PIMS_API,
          msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.PIMS_API],
        });
        setSystemDegraded(true);
      }

      const systemCheck = await getSystemCheck();
      if (systemCheck.data.status === 'Healthy') {
        return;
      }

      setHealthCheckIssues(systemIssues);
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        // 500 - API NOT Responding
        if (axiosError?.response?.status === 500) {
          setSystemDegraded(true);
          systemIssues.push({
            key: HealthcheckMessagesTypesEnum.PIMS_API,
            msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.PIMS_API],
          });
        }

        // 503 - API responding service not available
        if (axiosError?.response?.status === 503) {
          setSystemDegraded(true);

          const data = axiosError?.response?.data as unknown as ISystemCheck;
          if (data.entries?.Geoserver !== null && data.entries.Geoserver?.status !== 'Healthy') {
            systemIssues.push({
              key: HealthcheckMessagesTypesEnum.GEOSERVER,
              msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.GEOSERVER],
            });
          }

          if (
            data.entries?.PmbcExternalApi !== null &&
            data.entries.PmbcExternalApi?.status !== 'Healthy'
          ) {
            systemIssues.push({
              key: HealthcheckMessagesTypesEnum.PMBC,
              msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.PMBC],
            });
          }

          if (data.entries?.Mayan !== null && data.entries.Mayan?.status !== 'Healthy') {
            systemIssues.push({
              key: HealthcheckMessagesTypesEnum.MAYAN,
              msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.MAYAN],
            });
          }

          if (data.entries?.Ltsa !== null && data.entries.Ltsa?.status !== 'Healthy') {
            systemIssues.push({
              key: HealthcheckMessagesTypesEnum.LTSA,
              msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.LTSA],
            });
          }

          if (data.entries?.Geocoder !== null && data.entries.Geocoder?.status !== 'Healthy') {
            systemIssues.push({
              key: HealthcheckMessagesTypesEnum.GEOCODER,
              msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.GEOCODER],
            });
          }

          if (data.entries?.Cdogs !== null && data.entries.Cdogs?.status !== 'Healthy') {
            systemIssues.push({
              key: HealthcheckMessagesTypesEnum.CDOGS,
              msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum.CDOGS],
            });
          }

          setHealthCheckIssues(systemIssues);
        }
      }
    } finally {
      setSystemChecked(true);
    }
  }, [getLive, getSystemCheck, pimsHealthcheckMessages]);

  useEffect(() => {
    if (systemChecked == null && keycloak.obj.authenticated) {
      fetchSystemCheckInformation();
    }
  }, [fetchSystemCheckInformation, keycloak.obj.authenticated, systemChecked]);

  return systemDegraded && systemChecked ? (
    <>
      <LoadingBar style={{ zIndex: 9999, backgroundColor: '#fcba19', height: '.3rem' }} />
      <Styled.AppGridContainerWithHealth className="App" {...rest}>
        <HealthCheckStyled>
          <HealthcheckView systemChecks={healthCheckIssues}></HealthcheckView>
        </HealthCheckStyled>
        <HeaderStyled>
          <Header />
        </HeaderStyled>
        <ErrorBoundary FallbackComponent={ErrorModal}>{children}</ErrorBoundary>
        <FooterStyled>
          <Footer />
        </FooterStyled>
      </Styled.AppGridContainerWithHealth>
    </>
  ) : (
    <>
      <LoadingBar style={{ zIndex: 9999, backgroundColor: '#fcba19', height: '.3rem' }} />
      <Styled.AppGridContainer className="App" {...rest}>
        <HeaderStyled>
          <Header />
        </HeaderStyled>
        <ErrorBoundary FallbackComponent={ErrorModal}>{children}</ErrorBoundary>
        <FooterStyled>
          <Footer />
        </FooterStyled>
      </Styled.AppGridContainer>
    </>
  );
};

export default PublicLayout;
