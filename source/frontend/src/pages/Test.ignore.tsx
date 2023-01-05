import { ENVIRONMENT } from 'constants/environment';
import * as React from 'react';
import Button from 'react-bootstrap/Button';
import ButtonGroup from 'react-bootstrap/ButtonGroup';
import Container from 'react-bootstrap/Container';
import { useDispatch } from 'react-redux';
import { AnyAction } from 'redux';
import { ThunkDispatch } from 'redux-thunk';
import { RootState } from 'store/store';

import download from '../utils/download';

/**
 * Test provides a testing page for various things.
 */
const Test = () => {
  const dispatch: ThunkDispatch<RootState, unknown, AnyAction> = useDispatch();

  const fetch = (accept: 'csv' | 'excel') =>
    dispatch(
      download({
        url: ENVIRONMENT.apiUrl + '/reports/properties?classificationId=1',
        fileName: `pims-inventory.${accept === 'csv' ? 'csv' : 'xlsx'}`,
        actionType: 'properties-report',
        headers: {
          Accept: accept === 'csv' ? 'text/csv' : 'application/vnd.ms-excel',
        },
      }),
    );

  return (
    <Container>
      <h3>Property Reports</h3>
      <hr />
      <ButtonGroup>
        <Button onClick={() => fetch('csv')}>Download CSV</Button>
        <Button onClick={() => fetch('excel')}>Download Excel</Button>
      </ButtonGroup>
    </Container>
  );
};

export default Test;
