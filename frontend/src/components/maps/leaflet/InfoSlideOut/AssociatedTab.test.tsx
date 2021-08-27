import 'leaflet';
import 'leaflet/dist/leaflet.css';

import { render } from '@testing-library/react';
import { createMemoryHistory } from 'history';
import { IProperty } from 'interfaces';
import { mockProperties } from 'mocks/filterDataMock';
import * as React from 'react';
import { Provider } from 'react-redux';
import { Router } from 'react-router-dom';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import AssociatedBuildingsList from './AssociatedBuildingsList';
import AssociatedParcelsList from './AssociatedParcelsList';

const history = createMemoryHistory();
const mockStore = configureMockStore([thunk]);
const store = mockStore({});

const AsscParcelsTab = (parcels: IProperty[]) => {
  return (
    <Provider store={store}>
      <Router history={history}>
        <AssociatedParcelsList parcels={parcels} />
      </Router>
    </Provider>
  );
};

const AsscBuildingsTab = (buildings: IProperty[]) => {
  return (
    <Provider store={store}>
      <Router history={history}>
        <AssociatedBuildingsList buildings={buildings} />
      </Router>
    </Provider>
  );
};

describe('Associated Buildings/Parcels view', () => {
  it('Associated buildings list renders correctly', () => {
    const { container } = render(AsscBuildingsTab(mockProperties));
    expect(container.firstChild).toMatchSnapshot();
  });

  it('Associated buildings list shows building name', () => {
    const { getByText } = render(AsscBuildingsTab(mockProperties));
    expect(getByText('test name')).toBeVisible();
  });

  it('Add associated building link does not appear if no permission', () => {
    const { queryByText } = render(AsscBuildingsTab(mockProperties));
    expect(queryByText('Add a new Building')).toBeNull();
  });

  it('Associated parcels list renders correctly', () => {
    const { container } = render(AsscParcelsTab(mockProperties));
    expect(container.firstChild).toMatchSnapshot();
  });

  it('Associated parcels list shows parcel PID', () => {
    const { getByText } = render(AsscParcelsTab(mockProperties));
    expect(getByText('000-000-000')).toBeVisible();
  });
});
