import './Legend.scss';

import React from 'react';
import Card from 'react-bootstrap/Card';
import Col from 'react-bootstrap/Col';
import Image from 'react-bootstrap/Image';
import Row from 'react-bootstrap/Row';

export const Legend = () => {
  const keys = React.useMemo(() => {
    return [
      {
        pin: require('assets/images/pins/land-reg.png').default,
        label: 'Parcel',
      },
      {
        pin: require('assets/images/pins/building-reg.png').default,
        label: 'Building',
      },
      {
        pin: require('assets/images/pins/subdiv-reg.png').default,
        label: 'Proposed Subdivision',
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
