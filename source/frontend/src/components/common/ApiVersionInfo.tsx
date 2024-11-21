import React from 'react';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import styled from 'styled-components';

import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';

import { InlineFlexDiv } from './styles';
import TooltipWrapper from './TooltipWrapper';

/**
 * Provides a way to display the API version information.
 * Makes an AJAX request to the API for the version information.
 * @returns ApiVersionInfo component.
 */
export const ApiVersionInfo = () => {
  const { getVersion } = useApiHealth();
  const [version, setVersion] = React.useState<IApiVersion>(null);

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

  const apiVersionMissmatch = version?.informationalVersion !== frontEndVersion;
  const dbVersionMissmatch = version?.dbVersion !== frontEndDBVersion;

  const versionMissmatchMsg = (apiMissmatch: boolean, dbMissmatch: boolean): string => {
    let msg = '';
    if (apiMissmatch || dbMissmatch) {
      msg = msg.concat(
        `Warning: There is a version missmatch with the backend.
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

      {(apiVersionMissmatch || dbVersionMissmatch) && (
        <VersionMissmatchDiv data-testid="version-missmatch-warning">
          <TooltipWrapper
            tooltipId="warning"
            tooltip={versionMissmatchMsg(apiVersionMissmatch, dbVersionMissmatch)}
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
  text-transform: uppercase;
  color: ${props => props.theme.css.textWarningColor};
  background-color: ${props => props.theme.css.warningBackgroundColor};
  border-radius: 0.4rem;
  letter-spacing: 0.1rem;
  padding: 0.2rem 0.5rem;
  font-family: 'BCSans-Bold';
  font-size: 1.4rem;
  width: fit-content;
`;
