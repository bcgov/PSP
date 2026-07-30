import React from 'react';
import styled from 'styled-components';

import IApiVersion from '@/hooks/pims-api/interfaces/IApiVersion';
import { useApiHealth } from '@/hooks/pims-api/useApiHealth';
import useDeepCompareEffect from '@/hooks/util/useDeepCompareEffect';

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

  const frontEndVersion = import.meta.env.VITE_PACKAGE_VERSION;

  return (
    <StyledContainer>
      <div className="version" data-testid="version-tag">
        {`v${frontEndVersion}`}
      </div>
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
