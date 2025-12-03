import { Formik } from 'formik';
import noop from 'lodash/noop';
import { act } from 'react-dom/test-utils';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { IMapStateMachineContext } from '@/components/common/mapFSM/MapStateMachineContext';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import { render, RenderOptions, userEvent } from '@/utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { ResearchForm } from './models';
import ResearchProperties from './ResearchProperties';

const mockStore = configureMockStore([thunk]);

let testForm: ResearchForm;

const store = mockStore({});
const setDraftProperties = vi.fn();

describe('ResearchProperties component', () => {
  const setup = (
    renderOptions: RenderOptions & {
      initialForm: ResearchForm;
      confirmBeforeAdd?: (propertyForm: PropertyForm) => Promise<boolean>;
    } & Partial<IMapStateMachineContext>,
  ) => {
    // render component under test
    const component = render(
      <Formik initialValues={renderOptions.initialForm} onSubmit={noop}>
        <ResearchProperties confirmBeforeAdd={vi.fn()} />
      </Formik>,
      {
        ...renderOptions,
        store: store,
        claims: [],
        useMockAuthentication: true,
      },
    );

    return {
      store,
      component,
    };
  };

  beforeEach(() => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    testForm = new ResearchForm();
    testForm.name = 'Test name';
    testForm.properties = [
      PropertyForm.fromFeatureDataset({
        ...mockFeatureSet,
        pimsFeature: {
          ...mockFeatureSet.pimsFeature,
          properties: {
            ...mockFeatureSet.pimsFeature?.properties,
            PID_PADDED: '123-456-789',
          },
        },
      }),
      PropertyForm.fromFeatureDataset({
        ...mockFeatureSet,
        pimsFeature: {
          ...mockFeatureSet.pimsFeature,
          properties: {
            ...mockFeatureSet.pimsFeature?.properties,
            PIN: 1111222,
          },
        },
      }),
    ];
  });

  afterEach(() => {
    vi.clearAllMocks();
    setDraftProperties.mockReset();
  });

  it('renders as expected when provided no properties', async () => {
    const { component } = setup({ initialForm: testForm });
    await act(async () => {});
    expect(component.asFragment()).toMatchSnapshot();
  });

  it('renders as expected when provided a list of properties', async () => {
    const {
      component: { getByText },
    } = await setup({ initialForm: testForm });

    await act(async () => {});
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
    });

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('properties are prefixed by svg with incrementing id', async () => {
    const {
      component: { getByTitle },
    } = await setup({ initialForm: testForm });

    await act(async () => {});
    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });

  it.skip('properties with lat/lng are synchronized', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    await setup({ initialForm: formWithProperties });

    //TODO: correct assertions.
  });

  it.skip('multiple properties with lat/lng are synchronized', async () => {
    const formWithProperties = testForm;
    formWithProperties.properties[0].latitude = 1;
    formWithProperties.properties[0].longitude = 2;
    formWithProperties.properties[1].latitude = 3;
    formWithProperties.properties[1].longitude = 4;

    await setup({ initialForm: formWithProperties });

    //TODO: correct assertions.
  });
});
