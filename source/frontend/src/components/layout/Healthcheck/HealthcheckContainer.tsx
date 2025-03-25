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
  updateHealthcheckResult: (checkResult: boolean) => void;
  View: React.FunctionComponent<IHealthCheckViewProps>;
}

export const HealthcheckContainer: React.FunctionComponent<IHealthcheckContainerProps> = ({
  systemDegraded,
  updateHealthcheckResult,
  View,
}) => {
  const [systemChecked, setSystemChecked] = useState<boolean>(null);
  const [healthCheckIssues, setHealthCheckIssues] = useState<IHealthCheckIssue[]>(null);

  const { pimsHealthcheckMessages } = useTenant();
  const { getLive, getSystemCheck } = useApiHealth();
  const keycloak = useKeycloakWrapper();

  const checkExternalSystemStatus = useCallback(
    (
      response: IHealthcheckResponse,
      service: HealthcheckMessagesTypesEnum,
      systemIssues: IHealthCheckIssue[],
    ) => {
      if (response && response.status !== 'Healthy') {
        systemIssues.push({
          key: service,
          msg: pimsHealthcheckMessages[HealthcheckMessagesTypesEnum[service]],
        });
      }
    },
    [pimsHealthcheckMessages],
  );

  const handleError = useCallback(
    async (axiosError: AxiosError<IApiError>): Promise<void> => {
      const systemIssues: IHealthCheckIssue[] = [];

      // 500 - API NOT Responding
      if (axiosError?.response?.status === 500) {
        systemIssues.push({
          key: HealthcheckMessagesTypesEnum.PIMS_API,
          msg: pimsHealthcheckMessages[
            HealthcheckMessagesTypesEnum[HealthcheckMessagesTypesEnum.PIMS_API]
          ],
        });
        updateHealthcheckResult(true);
      }

      // 503 - API responding service not available
      if (axiosError?.response?.status === 503) {
        const data = axiosError?.response?.data as unknown as ISystemCheck;

        checkExternalSystemStatus(
          data.entries?.Geoserver,
          HealthcheckMessagesTypesEnum.GEOSERVER,
          systemIssues,
        );

        checkExternalSystemStatus(
          data.entries?.PmbcExternalApi,
          HealthcheckMessagesTypesEnum.PMBC,
          systemIssues,
        );

        checkExternalSystemStatus(
          data.entries?.Mayan,
          HealthcheckMessagesTypesEnum.MAYAN,
          systemIssues,
        );

        checkExternalSystemStatus(
          data.entries?.Ltsa,
          HealthcheckMessagesTypesEnum.LTSA,
          systemIssues,
        );

        checkExternalSystemStatus(
          data.entries?.Geocoder,
          HealthcheckMessagesTypesEnum.GEOCODER,
          systemIssues,
        );

        checkExternalSystemStatus(
          data.entries?.Cdogs,
          HealthcheckMessagesTypesEnum.CDOGS,
          systemIssues,
        );

        setHealthCheckIssues(systemIssues);
        updateHealthcheckResult(true);
      }
    },
    [checkExternalSystemStatus, pimsHealthcheckMessages, updateHealthcheckResult],
  );

  const fetchSystemCheckInformation = useCallback(async () => {
    const systemIssues: IHealthCheckIssue[] = [];
    let systemDegraded = false;
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
        systemDegraded = true;
      }

      const systemCheck = await getSystemCheck();
      if (systemCheck.data.status !== 'Healthy') {
        systemDegraded = true;
      }

      updateHealthcheckResult(systemDegraded);
    } catch (e) {
      if (axios.isAxiosError(e)) {
        const axiosError = e as AxiosError<IApiError>;
        handleError(axiosError);
      }
    } finally {
      setSystemChecked(true);
    }
  }, [getLive, getSystemCheck, handleError, pimsHealthcheckMessages, updateHealthcheckResult]);

  useEffect(() => {
    if (systemChecked == null && keycloak.obj.authenticated) {
      fetchSystemCheckInformation();
    }
  }, [fetchSystemCheckInformation, keycloak.obj.authenticated, systemChecked, systemDegraded]);

  return systemChecked && systemDegraded ? (
    <HealthCheckStyled>
      <View systemChecks={healthCheckIssues}></View>
    </HealthCheckStyled>
  ) : null;
};
