import { Col, Row } from 'react-bootstrap';

import { Input, Select } from '@/components/common/form';
import { withNameSpace } from '@/utils/formUtils';

import { DmsDirection } from './models';

export interface ICoordinateSearchProps {
  field: string;
}

export const CoordinateSearchForm: React.FunctionComponent<ICoordinateSearchProps> = ({
  field,
}) => {
  return (
    <Row className="d-flex align-items-center">
      <Col>Lat:</Col>
      <Col>
        <Input field={withNameSpace(field, 'latitude.degrees')}></Input>
      </Col>
      <Col>
        <Input field={withNameSpace(field, 'latitude.minutes')}></Input>
      </Col>
      <Col>
        <Input field={withNameSpace(field, 'latitude.seconds')}></Input>
      </Col>
      <Col>
        <Select
          field={withNameSpace(field, 'latitude.direction')}
          options={[
            { label: 'N', value: DmsDirection.N },
            { label: 'S', value: DmsDirection.S },
          ]}
        ></Select>
      </Col>
      <Col>Long:</Col>
      <Col>
        <Input field={withNameSpace(field, 'longitude.degrees')}></Input>
      </Col>
      <Col>
        <Input field={withNameSpace(field, 'longitude.minutes')}></Input>
      </Col>
      <Col>
        <Input field={withNameSpace(field, 'longitude.seconds')}></Input>
      </Col>
      <Col>
        <Select
          field={withNameSpace(field, 'longitude.direction')}
          options={[
            { label: 'W', value: DmsDirection.W },
            { label: 'E', value: DmsDirection.E },
          ]}
        ></Select>
      </Col>
    </Row>
  );
};
