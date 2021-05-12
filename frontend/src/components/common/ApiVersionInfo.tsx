import React from 'react';
import { IApiVersion, useApiHealth } from 'hooks/pims-api';

/**
 * Provides a way to display the API version information.
 * Makes an AJAX request to the API for the version information.
 * @returns ApiVersionInfo component.
 */
export const ApiVersionInfo = () => {
  const { getVersion } = useApiHealth();
  const [version, setVersion] = React.useState<IApiVersion>();

  React.useEffect(() => {
    const get = async () => {
      const response = await getVersion();
      setVersion(response.data);
    };
    get();
  }, [getVersion, setVersion]);

  return (
    <div className="version" data-testid="version">
      {version?.version ? `v${version.version ?? ''}` : 'api unavailable'}
    </div>
  );
};

export default ApiVersionInfo;
