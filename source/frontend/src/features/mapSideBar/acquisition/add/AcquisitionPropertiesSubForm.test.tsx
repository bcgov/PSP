import { Formik } from 'formik';
import noop from 'lodash/noop';
import configureMockStore from 'redux-mock-store';
import thunk from 'redux-thunk';

import { mapMachineBaseMock } from '@/mocks/mapFSM.mock';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { PropertyForm } from '../../shared/models';
import { AcquisitionPropertiesSubForm } from './AcquisitionPropertiesSubForm';
import { AcquisitionForm } from './models';

const mockStore = configureMockStore([thunk]);

const customSetFilePropertyLocations = vi.fn();

describe('AcquisitionProperties component', () => {
  // render component under test
  const setup = async (
    props: {
      initialForm: AcquisitionForm;
      confirmBeforeAdd?: (propertyForm: PropertyForm) => Promise<boolean>;
    },
    renderOptions: RenderOptions = {},
  ) => {
    const utils = render(
      <>
        <Formik initialValues={props.initialForm} onSubmit={noop}>
          {formikProps => (
            <AcquisitionPropertiesSubForm
              formikProps={formikProps}
              confirmBeforeAdd={props.confirmBeforeAdd ?? vi.fn()}
            />
          )}
        </Formik>
      </>,
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
