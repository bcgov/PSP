import axios, { AxiosError } from 'axios';
import { useCallback, useEffect, useState } from 'react';

import IHealthcheckResponse from '@/hooks/pims-api/interfaces/IHealthcheckResponse';
import ISystemCheck from '@/hooks/pims-api/interfaces/ISystemCheck';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';
import useKeycloakWrapper from '@/hooks/useKeycloakWrapper';
import { IApiError } from '@/interfaces/IApiError';
import HealthCheckStyled from '@/layouts/Healthcheck';
import { HealthcheckMessagesTypesEnum } from '@/layouts/models/HealthcheckMessagesTypes';
import { useTenant } from '@/tenants/useTenant';

import { IHealthCheckIssue, IHealthCheckViewProps } from './HealthcheckView';

export interface IHealthcheckContainerProps {
  systemDegraded: boolean;
  updateHealthcheckResult: (systemDegraded: boolean) => void;
  View: React.FunctionComponent<IHealthCheckViewProps>;
}

export const HealthcheckContainer: React.FunctionComponent<IHealthcheckContainerProps> = ({
  systemDegraded,
  updateHealthcheckResult,
  View,
}) => {
  const [systemChecked, setSystemChecked] = useState<boolean>(null);
  const [healthCheckIssues, setHealthCheckIssues] = useState<IHealthCheckIssue[]>([]);

  const { pimsHealthcheckMessages } = useTenant();
  const { getLive, getSystemCheck } = useApiHealth();
  const keycloak = useKeycloakWrapper();

  const checkExternalSystemStatus = useCallback(
    (response: IHealthcheckResponse, service: HealthcheckMessagesTypesEnum) => {
      if (response && response.status !== 'Healthy') {
        setHealthCheckIssues([
          ...healthCheckIssues,
          {
            key: service,
            msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum[service]],
          },
        ]);
      }
    },
    [healthCheckIssues, pimsHealthcheckMessages],
  );

  const checkAllSystemsHealth = useCallback(
    (response: ISystemCheck) => {
      checkExternalSystemStatus(
        response.entries?.Geoserver,
        HealthcheckMessagesTypesEnum.GEOSERVER,
      );

      checkExternalSystemStatus(
        response.entries?.PmbcExternalApi,
        HealthcheckMessagesTypesEnum.PMBC,
      );

      checkExternalSystemStatus(response.entries?.Mayan, HealthcheckMessagesTypesEnum.MAYAN);

      checkExternalSystemStatus(response.entries?.Ltsa, HealthcheckMessagesTypesEnum.LTSA);

      checkExternalSystemStatus(response.entries?.Geocoder, HealthcheckMessagesTypesEnum.GEOCODER);

      checkExternalSystemStatus(response.entries?.Cdogs, HealthcheckMessagesTypesEnum.CDOGS);
    },
    [checkExternalSystemStatus],
  );

  const handleError = useCallback(
    async (axiosError: AxiosError<IApiError>): Promise<void> => {
      const systemIssues: IHealthCheckIssue[] = [];
      let systemDegradedCheck = false;

      // 500 - API NOT Responding
      if (axiosError?.response?.status === 500) {
        systemIssues.push({
          key: HealthcheckMessagesTypesEnum.PIMS_API,
          msg: pimsHealthcheckMessages[
            HealthcheckMessagesTypesEnum[HealthcheckMessagesTypesEnum.PIMS_API]
          ],
        });

        setHealthCheckIssues(systemIssues);
        systemDegradedCheck = true;
      }

      // 503 - API responding service not available
      if (axiosError?.response?.status === 503) {
        const data = axiosError?.response?.data as unknown as ISystemCheck;

        checkAllSystemsHealth(data);
        systemDegradedCheck = true;
      }

      updateHealthcheckResult(systemDegradedCheck);
    },
    [checkAllSystemsHealth, pimsHealthcheckMessages, updateHealthcheckResult],
  );

  const fetchSystemCheckInformation = useCallback(async () => {
    const systemIssues: IHealthCheckIssue[] = [];
    let systemDegradedCheck = false;
    try {
      const pimsApi = await getLive();
      if (pimsApi.data.status !== 'Healthy') {
        systemIssues.push({
          key: HealthcheckMessagesTypesEnum.PIMS_API,
          msg: pimsHealthcheckMessages[
            HealthcheckMessagesTypesEnum[HealthcheckMessagesTypesEnum.PIMS_API]
          ],
        });

        setHealthCheckIssues(systemIssues);
        systemDegradedCheck = true;
      }

      const systemCheck = await getSystemCheck();
      if (systemCheck.data.status !== 'Healthy') {
        checkAllSystemsHealth(systemCheck.data);
        systemDegradedCheck = true;
      }

      updateHealthcheckResult(systemDegradedCheck);
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        handleError(axiosError);
      }
    } finally {
      setSystemChecked(true);
    }
  }, [
    checkAllSystemsHealth,
    getLive,
    getSystemCheck,
    handleError,
    pimsHealthcheckMessages,
    updateHealthcheckResult,
  ]);

  useEffect(() => {
    if (systemChecked == null && keycloak.obj.authenticated) {
      fetchSystemCheckInformation();
    }
  }, [fetchSystemCheckInformation, keycloak.obj.authenticated, systemChecked, systemDegraded]);

  return systemChecked ? (
    <HealthCheckStyled>
      <View systemDegraded={systemDegraded} systemChecks={healthCheckIssues}></View>
    </HealthCheckStyled>
  ) : null;
};
