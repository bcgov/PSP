import './Legend.scss';

import React from 'react';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image';
import Row from 'react-bootstrap/Row';

// import LandRed from '@/assets/images/pin/land-reg.png';
// import LandInfo from '@/assets/images/pin/marker-info-orange.png';
// import LandOtherInterest from '@/assets/images/pin/other-interest.png';

// import LandPoI from '../../../../../assets/images/pins/land-poi.png';

export const Legend = () => {
  const keys = React.useMemo(() => {
    return [
      {
        pin: require('@/assets/images/pins/land-reg.png'),
        label: 'Core Inventory',
      },
      {
        pin: '../../../../../assets/images/pins/land-poi.png',
        label: 'Property of Interest',
      },
      {
        pin: '../../../../../assets/images/pins/land-poi.png',
        label: 'Other Interest',
      },
      {
        pin: require('@/assets/images/pins/disposed.png'),
        label: 'Disposed',
      },
      {
        pin: require('@/assets/images/pins/marker-info-orange.png'),
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
