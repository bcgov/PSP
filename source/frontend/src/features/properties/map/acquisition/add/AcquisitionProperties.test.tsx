import {
  IMapStateContext,
  MapStateActionTypes,
  MapStateContextProvider,
} from 'components/maps/providers/MapStateContext';
import { Formik } from 'formik';
import { noop } from 'lodash';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { AcquisitionProperties } from './AcquisitionProperties';
import { AcquisitionForm } from './models';

const mockStore = configureMockStore([thunk]);

const setDraftProperties = jest.fn();

describe('AcquisitionProperties component', () => {
  // render component under test
  const setup = (
    props: { initialForm: AcquisitionForm } & Partial<IMapStateContext>,
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <MapStateContextProvider values={{ setState: setDraftProperties }}>
        <Formik initialValues={props.initialForm} onSubmit={noop}>
          {formikProps => <AcquisitionProperties formikProps={formikProps} />}
        </Formik>
      </MapStateContextProvider>,
      {
        ...renderOptions,
        store: mockStore({}),
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
  });

  afterEach(() => {
    jest.clearAllMocks();
    setDraftProperties.mockReset();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ initialForm: testForm });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders list of properties', async () => {
    const { getByText } = setup({ initialForm: testForm });

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith({
        type: MapStateActionTypes.DRAFT_PROPERTIES,
        draftProperties: [],
      });
    });

    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked', async () => {
    const { getAllByTitle, queryByText } = setup({ initialForm: testForm });
    const pidRow = getAllByTitle('remove')[0];
    userEvent.click(pidRow);

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith({
        type: MapStateActionTypes.DRAFT_PROPERTIES,
        draftProperties: [],
      });
    });

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('should display properties with svg prefixed with incrementing id', async () => {
    const { getByTitle } = setup({ initialForm: testForm });

    await waitFor(() => {
      expect(setDraftProperties).toHaveBeenCalledWith({
        type: MapStateActionTypes.DRAFT_PROPERTIES,
        draftProperties: [],
      });
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
      expect(setDraftProperties).toHaveBeenCalledWith({
        type: MapStateActionTypes.DRAFT_PROPERTIES,
        draftProperties: [
          {
            geometry: { coordinates: [2, 1], type: 'Point' },
            properties: { id: 0, name: 'New Parcel' },
            type: 'Feature',
          },
        ],
      });
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
      expect(setDraftProperties).toHaveBeenCalledWith({
        type: MapStateActionTypes.DRAFT_PROPERTIES,
        draftProperties: [
          {
            geometry: { coordinates: [2, 1], type: 'Point' },
            properties: { id: 0, name: 'New Parcel' },
            type: 'Feature',
          },
          {
            geometry: { coordinates: [4, 3], type: 'Point' },
            properties: { id: 0, name: 'New Parcel' },
            type: 'Feature',
          },
        ],
      });
    });
  });
});
