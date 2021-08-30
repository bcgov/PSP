import * as React from 'react';

/**
 * Component to display the PID as a link if the layer provides a PID.
 * The link will load the property information into the redux store.
 * // TODO: This doesn't currently work, but at some point in time it should extract information from the layer and place it into the redux property store.
 * @param param0 Component properties.
 * @param param0.data Leaflet layer attribute values.
 * @returns
 */
export const PidLink = ({ data }: { data: any }) => {
  return !!data.PID ? <>{data.PID}</> : null;
};
