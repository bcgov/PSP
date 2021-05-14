import React from 'react';
import noop from 'lodash/noop';
import { BuildingForm } from '.';
import { Provider } from 'react-redux';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { Router } from 'react-router-dom';
import { createMemoryHistory } from 'history';
import { useKeycloak } from '@react-keycloak/web';
import * as API from 'constants/API';
import * as reducerTypes from 'constants/reducerTypes';
import { fireEvent, render, wait, act } from '@testing-library/react';
import { fillInput } from 'utils/testUtils';
import { useApi, PimsAPI, IGeocoderResponse } from 'hooks/useApi';
import { ILookupCode } from 'store/slices/lookupCodes';

const mockStore = configureMockStore([thunk]);
const history = createMemoryHistory();

const lCodes = {
  lookupCodes: [
    { name: 'agencyVal', id: '1', isDisabled: false, type: API.AGENCY_CODE_SET_NAME },
    { name: 'disabledAgency', id: '2', isDisabled: true, type: API.AGENCY_CODE_SET_NAME },
    { name: 'roleVal', id: '1', isDisabled: false, type: API.ROLE_CODE_SET_NAME },
    { name: 'disabledRole', id: '2', isDisabled: true, type: API.ROLE_CODE_SET_NAME },
  ] as ILookupCode[],
};

const store = mockStore({
  [reducerTypes.LOOKUP_CODE]: lCodes,
  [reducerTypes.PROPERTIES]: { parcels: [], draftParcels: [] },
});

jest.mock('@react-keycloak/web');
(useKeycloak as jest.Mock).mockReturnValue({
  keycloak: {
    userInfo: {
      agencies: ['1'],
      roles: ['admin-properties'],
    },
    subject: 'test',
  },
});
jest.mock('hooks/useApi');
((useApi as unknown) as jest.Mock<Partial<PimsAPI>>).mockReturnValue({
  searchAddress: async () => {
    return [
      {
        siteId: '1',
        fullAddress: '12345 fake st.',
        address1: '4321 fake st.',
        administrativeArea: 'Victoria',
        provinceCode: 'BC',
        latitude: 1,
        longitude: 2,
        score: 100,
      },
    ] as IGeocoderResponse[];
  },
});
jest.mock('lodash/debounce', () => jest.fn(fn => fn));

const getBuildingForm = (disabled: boolean) => {
  return (
    <Provider store={store}>
      <Router history={history}>
        <BuildingForm
          setBuildingToAssociateLand={noop}
          goToAssociatedLand={noop}
          setMovingPinNameSpace={noop}
          nameSpace="building"
          disabled={disabled}
        />
      </Router>
    </Provider>
  );
};

const buildingForm = (
  <Provider store={store}>
    <Router history={history}>
      <BuildingForm
        setBuildingToAssociateLand={noop}
        goToAssociatedLand={noop}
        setMovingPinNameSpace={noop}
        nameSpace="data"
      />
    </Router>
  </Provider>
);

describe('Building Form', () => {
  it('component renders correctly', () => {
    const { container } = render(buildingForm);
    expect(container.firstChild).toMatchSnapshot();
  });

  it('displays identification page on initial load', () => {
    const { getByText } = render(buildingForm);
    expect(getByText(/building information/i)).toBeInTheDocument();
  });

  it('updates form content based on geocoder response', async () => {
    const { getByText, getByDisplayValue, container } = render(buildingForm);
    const input = container.querySelector(`input[name="data.address.line1"]`);
    await wait(() => {
      fireEvent.change(input!, {
        target: {
          value: '123 fake st.',
        },
      });
    });
    const suggestion = getByText('12345 fake st.');
    expect(suggestion).toBeVisible();

    suggestion.click();
    expect(getByDisplayValue('Victoria')).toBeVisible();
    expect(getByDisplayValue('4321 fake st.')).toBeVisible();
  });

  it('displays a modal if geocoder selection to overwrite existing data.', async () => {
    const { getByText, getByDisplayValue, container } = render(buildingForm);
    const address = container.querySelector(`input[name="data.address.line1"]`);
    await fillInput(container, 'data.latitude', '12.29');

    await wait(() => {
      fireEvent.change(address!, {
        target: {
          value: '123 fake st.',
        },
      });
    });
    act(() => {
      const suggestion = getByText('12345 fake st.');
      expect(suggestion).toBeVisible();
      suggestion.click();
    });
    const updateButton = getByText('Update');
    updateButton.click();
    expect(getByDisplayValue('Victoria')).toBeVisible();
    expect(getByDisplayValue('4321 fake st.')).toBeVisible();
    expect(container.querySelector(`input[name="data.latitude"]`)).toHaveValue(1);
  });

  it('displays a modal if geocoder selection would overwrite existing data and allows cancel', async () => {
    const { getByText, container } = render(buildingForm);
    const address = container.querySelector(`input[name="data.address.line1"]`);
    await fillInput(container, 'data.latitude', '12.29');

    await wait(() => {
      fireEvent.change(address!, {
        target: {
          value: '123 fake st.',
        },
      });
    });
    act(() => {
      const suggestion = getByText('12345 fake st.');
      expect(suggestion).toBeVisible();
      suggestion.click();
    });

    const updateButton = getByText('Cancel');
    updateButton.click();
    expect(container.querySelector(`input[name="data.latitude"]`)).toHaveValue(12.29);
  });

  it('building form goes to corresponding steps', async () => {
    const { getByText } = render(getBuildingForm(true));
    await wait(() => {
      fireEvent.click(getByText(/continue/i));
    });
    expect(getByText(/Net Usable Area/i)).toBeInTheDocument();
    await wait(() => {
      fireEvent.click(getByText(/Continue/i));
    });
    expect(getByText('Building Valuation', { exact: true })).toBeInTheDocument();
    await wait(() => {
      fireEvent.click(getByText(/Continue/i));
    });
    expect(getByText(/Review your building info/i)).toBeInTheDocument();
  });

  it('building review has appropriate subforms', async () => {
    const { getByText } = render(getBuildingForm(true));
    await wait(() => {
      fireEvent.click(getByText(/Review/i));
    });
    expect(getByText(/Building Identification/i)).toBeInTheDocument();
    expect(getByText(/Net Usable Area/i)).toBeInTheDocument();
    expect(getByText(/Net Book Value/i)).toBeInTheDocument();
  });
});
