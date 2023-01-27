import { fireEvent, render, waitFor } from '@testing-library/react';
import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { useFormikContext } from 'formik';
import { IGeocoderResponse } from 'hooks/useApi';
import TestCommonWrapper from 'utils/TestCommonWrapper';

import { GeocoderAutoComplete } from './GeocoderAutoComplete';

const mockAxios = new MockAdapter(axios);
jest.mock('formik');

afterEach(() => {
  mockAxios.reset();
});

(useFormikContext as jest.Mock).mockReturnValue({
  handleBlur: jest.fn(),
});

const mockGeocoderOptions: IGeocoderResponse[] = [
  {
    fullAddress: '1234 Fake St',
    siteId: '1',
    address1: '1234 Fake St',
    administrativeArea: 'Test Town',
    provinceCode: 'BC',
    latitude: 1,
    longitude: 1,
    score: 60,
  },
  {
    fullAddress: '5521 Test St',
    siteId: '1',
    address1: '5521 Test St',
    administrativeArea: 'Test Town',
    provinceCode: 'BC',
    latitude: 2,
    longitude: 2,
    score: 70,
  },
];

it('renders correctly...', () => {
  const { asFragment } = render(
    <TestCommonWrapper>
      <GeocoderAutoComplete field="test" />
    </TestCommonWrapper>,
  );
  expect(asFragment()).toMatchSnapshot();
});

it('displays tooltip when required...', () => {
  const { getByTestId } = render(
    <TestCommonWrapper>
      <GeocoderAutoComplete field="test" tooltip="this is my tooltip" />
    </TestCommonWrapper>,
  );
  expect(getByTestId('tooltip-icon')).toBeInTheDocument();
});

it('gets appropriate tooltip from props...', async () => {
  const toolTipString = 'This is my tooltip.';
  const { getByTestId, getByText } = render(
    <TestCommonWrapper>
      <GeocoderAutoComplete field="test" tooltip={toolTipString} />
    </TestCommonWrapper>,
  );
  const toolTip = getByTestId('tooltip-icon');
  await waitFor(() => {
    fireEvent.mouseOver(toolTip);
  });
  expect(getByText(toolTipString)).toBeInTheDocument();
});

xit('renders options while user types...', async () => {
  mockAxios.onGet().reply(200, mockGeocoderOptions);
  const { container, getByText } = render(
    <TestCommonWrapper>
      <GeocoderAutoComplete field="test" />
    </TestCommonWrapper>,
  );
  const input = container.querySelector('input[name="test"]');
  await waitFor(() => {
    fireEvent.change(input!, { target: { value: '5521 ' } });
  });
  const option = getByText('5521 Test St');
  expect(option).toBeInTheDocument();
});
