import './Legend.scss';

import React from 'react';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image';
import Row from 'react-bootstrap/Row';

import disposedIcon from '@/assets/images/pins/disposed.png';
import landPoiIcon from '@/assets/images/pins/land-poi.png';
import landRegIcon from '@/assets/images/pins/land-reg.png';
import infoIcon from '@/assets/images/pins/marker-info-orange.png';
import otherInterestIcon from '@/assets/images/pins/other-interest.png';
import retiredIcon from '@/assets/images/pins/retired.png';

export const Legend = () => {
  const keys = React.useMemo(() => {
    return [
      {
        pin: landRegIcon,
        label: 'Core Inventory',
      },
      {
        pin: landPoiIcon,
        label: 'Property of Interest',
      },
      {
        pin: otherInterestIcon,
        label: 'Other Interest',
      },
      {
        pin: disposedIcon,
        label: 'Disposed',
      },
      {
        pin: retiredIcon,
        label: 'Retired (Subdivided/consolidated)',
      },
      {
        pin: infoIcon,
        label: 'Search result (not in inventory)',
      },
    ];
  }, []);

  return (
    <Card className="legend-control">
      <Card.Header>Marker Legend</Card.Header>

      <Card.Body>
        {keys.map((item, index) => {
          return (
            <Row key={index}>
              <Col xs={2}>
                <Image height={25} src={item.pin} />
              </Col>
              <Col className="label">{item.label}</Col>
            </Row>
          );
        })}
      </Card.Body>
    </Card>
  );
};
