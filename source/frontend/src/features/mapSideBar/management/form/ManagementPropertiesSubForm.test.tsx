import { Formik, FormikProps } from 'formik';
import { createRef } from 'react';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { ManagementFormModel } from '../models/ManagementFormModel';
import ManagementPropertiesSubForm from './ManagementPropertiesSubForm';

const mockStore = configureMockStore([thunk]);

const customSetFilePropertyLocations = vi.fn();

const confirmBeforeAdd = vi.fn();

describe('ManagementPropertiesSubForm component', () => {
  const setup = async (
    props: { initialForm: ManagementFormModel },
    renderOptions: RenderOptions = {},
  ) => {
    const ref = createRef<FormikProps<ManagementFormModel>>();
    const utils = render(
      <Formik innerRef={ref} initialValues={props.initialForm} onSubmit={vi.fn()}>
        {formikProps => (
          <ManagementPropertiesSubForm
            formikProps={formikProps}
            confirmBeforeAdd={confirmBeforeAdd}
          />
        )}
      </Formik>,
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

  let testForm: ManagementFormModel;

  beforeEach(() => {
    testForm = new ManagementFormModel();
    testForm.fileProperties = [
      PropertyForm.fromMapProperty({ pid: '123-456-789' }),
      PropertyForm.fromMapProperty({ pin: '1111222' }),
    ];
  });

  afterEach(() => {
    vi.clearAllMocks();
    customSetFilePropertyLocations.mockReset();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({ initialForm: testForm });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders list of properties', async () => {
    const { getByText } = await setup({ initialForm: testForm });
    expect(getByText('PID: 123-456-789')).toBeVisible();
    expect(getByText('PIN: 1111222')).toBeVisible();
  });

  it('should remove property from list when Remove button is clicked', async () => {
    const { getAllByTitle, queryByText } = await setup({ initialForm: testForm });
    const pidRow = getAllByTitle('remove')[0];
    await act(async () => userEvent.click(pidRow));

    expect(queryByText('PID: 123-456-789')).toBeNull();
  });

  it('should display properties with svg prefixed with incrementing id', async () => {
    const { getByTitle } = await setup({ initialForm: testForm });
    expect(getByTitle('1')).toBeInTheDocument();
    expect(getByTitle('2')).toBeInTheDocument();
  });
});
