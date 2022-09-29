import { IApiVersion, useApiHealth } from 'hooks/pims-api';
import React from 'react';

/**
 * Provides a way to display the API version information.
 * Makes an AJAX request to the API for the version information.
 * @returns ApiVersionInfo component.
 */
export const ApiVersionInfo = () => {
  const { getVersion } = useApiHealth();
  const [version, setVersion] = React.useState<IApiVersion>();

  React.useEffect(() => {
    let isActive = true;
    const get = async () => {
      const response = await getVersion();
      if (isActive) {
        setVersion(response.data);
      }
    };
    get();
    return () => {
      isActive = false;
    };
  }, [getVersion, setVersion]);

  return (
    <div className="version" data-testid="version">
      {version?.informationalVersion ? `v${version.informationalVersion ?? ''}` : 'api unavailable'}
    </div>
  );
};

export default ApiVersionInfo;
