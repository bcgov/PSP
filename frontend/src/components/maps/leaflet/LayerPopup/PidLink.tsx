import { IProperty } from 'interfaces';
import * as React from 'react';
import Button from 'react-bootstrap/Button';
import { storeProperty } from 'store/slices/properties';
import { useAppDispatch } from 'store/store';
import { pidFormatter } from 'utils';

/**
 * Component to display the PID as a link if the layer provides a PID.
 * The link will load the property information into the redux store.
 * @param param0 Component properties.
 * @param param0.data Leaflet layer attribute values.
 * @returns
 */
export const PidLink = ({ data }: { data: any }) => {
  const dispatch = useAppDispatch();
  // a reference to the internal Leaflet map instance (this is NOT a react-leaflet class but the underlying leaflet map)

  const clickPid = () => {
    dispatch(
      storeProperty({
        pid: data.PID,
        pin: data.PIN,
        latitude: 48.43289,
        longitude: -123.49443,
        address: { municipality: data.MUNICIPALITY },
      } as IProperty),
    );
  };
  return !!data.PID ? (
    <Button variant="link" size="sm" onClick={clickPid} style={{ minHeight: '0px' }}>
      {pidFormatter(data.PID)}
    </Button>
  ) : null;
};
