import React from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import styled from 'styled-components';

import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';
import { stringToNumberOrNull } from '@/utils/formUtils';

import { InlineFlexDiv } from './styles';
import TooltipWrapper from './TooltipWrapper';

type ApiDbCompatibilityEntry = {
  apiVersion: number;
  databaseVersions: number[];
};

const parseApiDbCompatibility = (rawValue?: string): ApiDbCompatibilityEntry[] => {
  if (!rawValue?.trim()) {
    return [];
  }

  try {
    const parsed = JSON.parse(rawValue) as Record<string, unknown>;
    if (!parsed || typeof parsed !== 'object' || Array.isArray(parsed)) {
      return [];
    }

    return Object.entries(parsed)
      .map(([apiVersionKey, dbVersionValues]) => {
        const apiVersionValue = stringToNumberOrNull(apiVersionKey);
        const apiVersion = Number.isFinite(apiVersionValue ?? NaN) ? apiVersionValue : null;
        const dbVersions = Array.isArray(dbVersionValues)
          ? dbVersionValues
              .map(stringToNumberOrNull)
              .filter((value): value is number => Number.isFinite(value ?? NaN))
          : [];

        if (apiVersion === null || dbVersions.length === 0) {
          return null;
        }

        return { apiVersion, databaseVersions: dbVersions } as ApiDbCompatibilityEntry;
      })
      .filter((value): value is ApiDbCompatibilityEntry => value !== null);
  } catch {
    return [];
  }
};

const getApiBuildVersion = (informationalVersion?: string): number | null => {
  if (!informationalVersion) {
    return null;
  }

  const match = informationalVersion.match(/-(\d+)(?:\.|$)|\.(\d+)$/);
  const candidate = match?.[1] ?? match?.[2] ?? informationalVersion;
  const parsed = stringToNumberOrNull(candidate);
  return Number.isFinite(parsed ?? NaN) ? parsed : null;
};

/**
 * Provides a way to display the API version information.
 * Makes an AJAX request to the API for the version information.
 * @returns ApiVersionInfo component.
 */
export const ApiVersionInfo = () => {
  const { getVersion } = useApiHealth();
  const [version, setVersion] = React.useState<IApiVersion>(null);

  const apiDbCompatibility = React.useMemo(
    () => parseApiDbCompatibility(import.meta.env.VITE_API_DB_VERSION_COMPATIBILITY),
    [],
  );

  useDeepCompareEffect(() => {
    let isActive = true;
    const get = async () => {
      const response = await getVersion();
      if (isActive && version?.informationalVersion !== response.data?.informationalVersion) {
        setVersion(response.data);
      }
    };
    get();
    return () => {
      isActive = false;
    };
  }, [getVersion]);

  const findDBVersion = (frontEndVersion: string): string => {
    // remove suffix
    const frontEndVersionClean = frontEndVersion.substring(0, frontEndVersion.lastIndexOf('.'));
    const start = frontEndVersion.lastIndexOf('-') + 1;
    const end = frontEndVersion.lastIndexOf('.');

    // Get DB Version.
    const dbVersion = frontEndVersionClean.substring(start, end);
    // Convert to 2 decimal version so it matches DB record.
    const numericVersion = parseFloat(dbVersion).toFixed(2);

    return numericVersion.toString();
  };

  const frontEndVersion = import.meta.env.VITE_PACKAGE_VERSION;
  const frontEndDBVersion = findDBVersion(frontEndVersion);

  const apiVersionMismatch = version?.informationalVersion !== frontEndVersion;
  const apiBuildVersion = getApiBuildVersion(version?.informationalVersion);
  const dbVersionValueRaw = stringToNumberOrNull(version?.dbVersion);
  const dbVersionValue = Number.isFinite(dbVersionValueRaw ?? NaN) ? dbVersionValueRaw : null;
  const frontEndDbVersionValueRaw = stringToNumberOrNull(frontEndDBVersion);
  const frontEndDbVersionValue = Number.isFinite(frontEndDbVersionValueRaw ?? NaN)
    ? frontEndDbVersionValueRaw
    : null;
  const allowedDbVersions = new Set<number>();

  if (frontEndDbVersionValue !== null) {
    allowedDbVersions.add(frontEndDbVersionValue);
  }

  if (apiBuildVersion !== null) {
    const match = apiDbCompatibility.find(entry => entry.apiVersion === apiBuildVersion);
    match?.databaseVersions.forEach(dbVersion => allowedDbVersions.add(dbVersion));
  }
  console.debug(import.meta.env.VITE_API_DB_VERSION_COMPATIBILITY);

  const dbVersionMismatch =
    dbVersionValue === null || allowedDbVersions.size === 0
      ? version?.dbVersion !== frontEndDBVersion
      : !allowedDbVersions.has(dbVersionValue);

  const versionMismatchMsg = (apiMismatch: boolean, dbMismatch: boolean): string => {
    let msg = '';
    if (apiMismatch || dbMismatch) {
      msg = msg.concat(
        `Warning: There is a version mismatch with the backend.
         API: ${version?.informationalVersion}; DB: ${version?.dbVersion}`,
      );
    }

    return msg;
  };

  return (
    <StyledContainer>
      <div className="version" data-testid="version-tag">
        {`v${frontEndVersion ?? ''}`}
      </div>

      {(apiVersionMismatch || dbVersionMismatch) && (
        <VersionMissmatchDiv data-testid="version-mismatch-warning">
          <TooltipWrapper
            tooltipId="warning"
            tooltip={versionMismatchMsg(apiVersionMismatch, dbVersionMismatch)}
            className="warning"
          >
            <AiOutlineExclamationCircle size={20} />
          </TooltipWrapper>
        </VersionMissmatchDiv>
      )}
    </StyledContainer>
  );
};

export default ApiVersionInfo;

const StyledContainer = styled.div`
  display: flex;
  gap: 10px;
  flex-grow: 1;
  flex-direction: row;
  justify-content: flex-end;
  align-items: center;
`;

export const VersionMissmatchDiv = styled(InlineFlexDiv)`
  color: ${props => props.theme.css.textWarningColor};
  background-color: ${props => props.theme.css.warningBackgroundColor};
  border-radius: 0.4rem;
  letter-spacing: 0.1rem;
  padding: 0.2rem 0.5rem;
  font-family: 'BCSans-Bold';
  font-size: 1.4rem;
  width: fit-content;
`;
