import { FormikProps } from 'formik';
import { createRef } from 'react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { FileForm, PropertyForm } from '../../models';
import { getMockSelectedFeatureDataset } from '@/mocks/featureset.mock';
import PropertiesListContainer from './PropertiesListContainer';
import { PIMS_Property_Location_View } from '@/models/layers/pimsPropertyLocationView';

const mockStore = configureMockStore([thunk]);

const customSetFilePropertyLocations = vi.fn();

const verifyCanRemove = vi.fn();
const confirmBeforeAdd = vi.fn();

describe('PropertiesListContainer component', () => {
  const setup = async (
    props: { properties: PropertyForm[] },
    renderOptions: RenderOptions = {},
  ) => {
    const ref = createRef<FormikProps<FileForm>>();
    const utils = render(
      <PropertiesListContainer
        properties={props.properties}
        verifyCanRemove={verifyCanRemove}
        needsConfirmationBeforeAdd={confirmBeforeAdd}
      />,
      {
        ...renderOptions,
        store: mockStore({}),
        claims: [],
        mockMapMachine: {
          ...mapMachineBaseMock,
          setFilePropertyLocations: customSetFilePropertyLocations,
        },
      },
    );

    // Wait for any async effects to complete
    await act(async () => {});

    return {
      ...utils,
      getFormikRef: () => ref,
    };
  };

  let testForm: FileForm;

  beforeEach(() => {
    const mockFeatureSet = getMockSelectedFeatureDataset();
    testForm = new FileForm();
    testForm.properties = [
      PropertyForm.fromLocationFeatureDataset({
        ...mockFeatureSet,
        pimsFeatures: [
          {
            ...mockFeatureSet.pimsFeatures[0],
            properties: {
              ...mockFeatureSet.pimsFeatures[0].properties,
              PID_PADDED: '123-456-789',
            },
          },
        ],
      }),
      PropertyForm.fromLocationFeatureDataset({
        ...mockFeatureSet,
        pimsFeatures: [
          {
            ...mockFeatureSet.pimsFeatures[0],
            properties: {
              ...mockFeatureSet.pimsFeatures[0]?.properties,
              PIN: 1111222,
            },
          },
        ],
      }),
    ];
    testForm.properties[0].pid = '123456789';
    testForm.properties[1].pin = '1111222';
  });

  afterEach(() => {
    vi.clearAllMocks();
    customSetFilePropertyLocations.mockReset();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({ properties: testForm.properties });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders list of properties', async () => {
    const { getByText } = await setup({ properties: testForm.properties });

    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked', async () => {
    const { getAllByTitle, queryByText } = await setup({ properties: testForm.properties });
    const pidRow = getAllByTitle('remove')[0];
    await act(async () => userEvent.click(pidRow));

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('should display properties with svg prefixed with incrementing id', async () => {
    const { getByTitle } = await setup({ properties: testForm.properties });

    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });
});
