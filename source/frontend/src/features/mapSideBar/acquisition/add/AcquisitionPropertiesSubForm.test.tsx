import { Formik } from 'formik';
import { noop } from 'lodash';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { render, RenderOptions, userEvent, waitFor } from '@/utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { AcquisitionPropertiesSubForm } from './AcquisitionPropertiesSubForm';
import { AcquisitionForm } from './models';

const mockStore = configureMockStore([thunk]);
jest.mock('@react-keycloak/web');

const customSetFilePropertyLocations = jest.fn();

jest.mock('@/components/common/mapFSM/MapStateMachineContext');

describe('AcquisitionProperties component', () => {
  // render component under test
  const setup = (props: { initialForm: AcquisitionForm }, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <>
        <Formik initialValues={props.initialForm} onSubmit={noop}>
          {formikProps => <AcquisitionPropertiesSubForm formikProps={formikProps} />}
        </Formik>
      </>,
      {
        ...renderOptions,
        store: mockStore({}),
        claims: [],
      },
    );

    return { ...utils };
  };

  let testForm: AcquisitionForm;

  beforeEach(() => {
    testForm = new AcquisitionForm();
    testForm.fileName = 'Test name';
    testForm.properties = [
      PropertyForm.fromMapProperty({ pid: '123-456-789' }),
      PropertyForm.fromMapProperty({ pin: '1111222' }),
    ];
    (useMapStateMachine as jest.Mock).mockImplementation(() => {
      return { ...mapMachineBaseMock, setFilePropertyLocations: customSetFilePropertyLocations };
    });
  });

  afterEach(() => {
    jest.clearAllMocks();
    customSetFilePropertyLocations.mockReset();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders list of properties', async () => {
    const { getByText } = setup({ initialForm: testForm });

    await waitFor(() => {
      expect(customSetFilePropertyLocations).toHaveBeenCalledWith([
        { lat: 0, lng: 0 },
        { lat: 0, lng: 0 },
      ]);
    });

    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked', async () => {
    const { getAllByTitle, queryByText } = setup({ initialForm: testForm });
    const pidRow = getAllByTitle('remove')[0];
    userEvent.click(pidRow);

    await waitFor(() => {
      expect(customSetFilePropertyLocations).toHaveBeenCalledWith([{ lat: 0, lng: 0 }]);
    });

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('should display properties with svg prefixed with incrementing id', async () => {
    const { getByTitle } = setup({ initialForm: testForm });

    await waitFor(() => {
      expect(customSetFilePropertyLocations).toHaveBeenCalledWith([
        { lat: 0, lng: 0 },
        { lat: 0, lng: 0 },
      ]);
    });

    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });

  it('should synchronize a single property with provided lat/lng', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    setup({ initialForm: formWithProperties });

    await waitFor(() => {
      expect(customSetFilePropertyLocations).toHaveBeenCalledWith([
        { lat: 1, lng: 2 },
        { lat: 0, lng: 0 },
      ]);
    });
  });

  it('should synchronize multiple properties with provided lat/lng', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    formWithProperties.properties[1].latitude = 3;
    formWithProperties.properties[1].longitude = 4;

    setup({ initialForm: formWithProperties });

    await waitFor(() => {
      expect(customSetFilePropertyLocations).toHaveBeenCalledWith([
        { lat: 1, lng: 2 },
        { lat: 3, lng: 4 },
      ]);
    });
  });
});
