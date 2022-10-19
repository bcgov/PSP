import {
  ISelectedPropertyContext,
  SelectedPropertyContextProvider,
} from 'components/maps/providers/SelectedPropertyContext';
import { Formik } from 'formik';
import { noop } from 'lodash';
import { act } from 'react-dom/test-utils';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';
import { render, RenderOptions, userEvent, waitFor } from 'utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { ResearchForm } from './models';
import ResearchProperties from './ResearchProperties';

const mockStore = configureMockStore([thunk]);

const testForm = new ResearchForm();
testForm.name = 'Test name';
testForm.properties = [
  PropertyForm.fromMapProperty({ pid: '123-456-789' }),
  PropertyForm.fromMapProperty({ pin: '1111222' }),
];

const store = mockStore({});
const setDraftProperties = jest.fn();

describe('ResearchProperties component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      initialForm: ResearchForm;
    } & Partial<ISelectedPropertyContext>,
  ) => {
    // render component under test
    const component = render(
      <SelectedPropertyContextProvider values={{ setDraftProperties: setDraftProperties }}>
        <Formik initialValues={renderOptions.initialForm} onSubmit={noop}>
          <ResearchProperties />
        </Formik>
      </SelectedPropertyContextProvider>,
      {
        ...renderOptions,
        store: store,
      },
    );

    return {
      store,
      component,
    };
  };

  afterEach(() => {
    jest.resetAllMocks();
    setDraftProperties.mockReset();
  });

  it('renders as expected when provided no properties', () => {
    const { component } = setup({ initialForm: testForm });
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', async () => {
    const {
      component: { getByText },
    } = await setup({ initialForm: testForm });

    await waitFor(async () => {
      expect(setDraftProperties).toHaveBeenCalledWith([]);
    });
    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('properties can be removed', async () => {
    const {
      component: { getAllByTitle, queryByText },
    } = await setup({ initialForm: testForm });
    const pidRow = getAllByTitle('remove')[0];

    await act(async () => {
      userEvent.click(pidRow);
      await waitFor(async () => {
        expect(setDraftProperties).toHaveBeenCalledWith([]);
      });
    });

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('properties are prefixed by svg with incrementing id', async () => {
    const {
      component: { getByTitle },
    } = await setup({ initialForm: testForm });

    await waitFor(async () => {
      expect(setDraftProperties).toHaveBeenCalledWith([]);
    });
    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });

  it('properties with lat/lng are synchronized', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    await setup({ initialForm: formWithProperties });

    await act(async () => {
      await waitFor(async () => {
        expect(setDraftProperties).toHaveBeenCalledWith([
          {
            geometry: { coordinates: [2, 1], type: 'Point' },
            properties: { id: 0, name: 'New Parcel' },
            type: 'Feature',
          },
        ]);
      });
    });
  });

  it('multiple properties with lat/lng are synchronized', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    formWithProperties.properties[1].latitude = 3;
    formWithProperties.properties[1].longitude = 4;

    await setup({ initialForm: formWithProperties });

    await act(async () => {
      await waitFor(async () => {
        expect(setDraftProperties).toHaveBeenCalledWith([
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
        ]);
      });
    });
  });
});
