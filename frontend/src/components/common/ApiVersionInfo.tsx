import React from 'react';
import { IApiVersion, useApiHealth } from 'hooks/pims-api';

/**
 * Provides a way to display the API version information.
 * Makes an AJAX request to the API for the version information.
 * @returns ApiVersionInfo component.
 */
export const ApiVersionInfo = () => {
  const api = useApiHealth();
  const [version, setVersion] = React.useState<IApiVersion>();

  React.useEffect(() => {
    api.getVersion().then(response => {
      setVersion(response.data);
      return response.data;
    });
  }, [api]);

  return <div className="version">v{version?.version}</div>;
};

export default ApiVersionInfo;
