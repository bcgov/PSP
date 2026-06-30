import { screen, render, waitFor, act, getByName, getByText } from '@/utils/test-utils';
import userEvent from '@testing-library/user-event';

import { ActivitiesFilter, IActivitiesFilterProps } from './ActivitiesFilter';
import { ManagementActivityFilterModel } from '../../models/ManagementActivityFilterModel';
import { Api_ManagementActivityFilter } from '@/models/api/ManagementActivityFilter';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { mockLookups } from '@/mocks/lookups.mock';

vi.mock('@/components/common/form', async () => {
  const actual = await vi.importActual<any>('@/components/common/form');
  const { useFormikContext } = await vi.importActual<any>('formik');

  return {
    ...actual,
    Multiselect: ({ field, options }: any) => {
      const { setFieldValue } = useFormikContext();

      return (
        <button
          type="button"
          data-testid={`multiselect-${field}`}
          onClick={() => setFieldValue(field, [options[0]])}
        >
          Select mock region
        </button>
      );
    },
  };
});

const setFilter = vi.fn();

const setup = (props: Partial<IActivitiesFilterProps> = {}) => {
    const initialValues = new ManagementActivityFilterModel();


  return render(
    <ActivitiesFilter
      initialValues={initialValues}
      setFilter={setFilter}
      activityStatusOptions={[]}
      activityTypesOptions={[]}
      fileStatusOptions={[]}
      managementPurposeOptions={[]}
      userRegionsOptions={[
        { id: '1', text: 'South Coast Region' },
        { id: '2', text: 'Northern Region' },
      ]}
      {...props}
    />,
  );
};

describe('Management Activities Filter', () => {
  beforeEach(() => {
    setFilter.mockClear();
  });

  it('matches snapshot', async () => {
    const { asFragment } = setup();

    const fragment = await waitFor(() => asFragment());
    expect(fragment).toMatchSnapshot();
  });
});

it('searches by PID', async () => {
    await setup();

    const searchBy = getByName('searchBy');
    await act(async () => userEvent.selectOptions(searchBy, 'pid'));

    const input = screen.getByPlaceholderText(/Enter a PID/i);
    await act(async () => userEvent.type(input, '123456'));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
    expect.objectContaining({
      projectNameOrNumber: '',
            searchBy: 'pid',
            pin: '',
            pid: '123456',
            regionCodes: [],
            address: '',
            fileNameOrNumberOrReference: '',
            activityStatusCode: '',
            activityTypeCode: '',
            managementFileStatusCode: '',
            managementFilePurposeCode: '',
    }),
  );
});

it('searches by PIN', async () => {
  await setup();

  const searchBy = document.getElementById('input-searchBy') as HTMLSelectElement;
  expect(searchBy).not.toBeNull();

  await act(async () => userEvent.selectOptions(searchBy, 'pin'));

  const input = await screen.findByPlaceholderText(/enter a pin/i);
  await act(async () => userEvent.type(input, '7890'));

  await act(async () => userEvent.click(screen.getByTestId('search')));

  expect(setFilter).toHaveBeenCalledWith(
    expect.objectContaining({
      projectNameOrNumber: '',
            searchBy: 'pin',
            pin: '7890',
            pid: '',
            regionCodes: [],
            address: '',
            fileNameOrNumberOrReference: '',
            activityStatusCode: '',
            activityTypeCode: '',
            managementFileStatusCode: '',
            managementFilePurposeCode: '',
    }),
  );
});

it('searches by file name or reference', async () => {
    await setup();

    const input = screen.getByPlaceholderText(/Management file number or name/i);
    await act(async () => userEvent.type(input, 'Activity File 123'));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

    expect(setFilter).toHaveBeenCalledWith(
          expect.objectContaining({
            projectNameOrNumber: '',
            searchBy: 'address',
            pin: '',
            pid: '',
            regionCodes: [],
            address: '',
            fileNameOrNumberOrReference: 'Activity File 123',
            activityStatusCode: '',
            activityTypeCode: '',
            managementFileStatusCode: '',
            managementFilePurposeCode: '',
          }),
        );
});

it('searches by project name', async () => {

    await setup();

    const input = screen.getByPlaceholderText(/Enter a project name/i);
    await act(async () => userEvent.type(input, 'Project X'));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

   expect(setFilter).toHaveBeenCalledWith(
          expect.objectContaining({
            projectNameOrNumber: 'Project X',
            searchBy: 'address',
            pin: '',
            pid: '',
            regionCodes: [],
            address: '',
            fileNameOrNumberOrReference: '',
            activityStatusCode: '',
            activityTypeCode: '',
            managementFileStatusCode: '',
            managementFilePurposeCode: '',
          }),
        );
});

it('searches by region codes', async () => {

    await setup();

    const regionsInput = screen.getByTestId('multiselect-regionCodes');
    await act(async () => userEvent.click(regionsInput));
    //await act(async () => userEvent.click(screen.getByText('South Coast Region')));

    const searchButton = screen.getByTestId('search');
    await act(async () => userEvent.click(searchButton));

   expect(setFilter).toHaveBeenCalledWith(
          expect.objectContaining({
            projectNameOrNumber: '',
            searchBy: 'address',
            pin: '',
            pid: '',
            regionCodes: ["1"],
            address: '',
            fileNameOrNumberOrReference: '',
            activityStatusCode: '',
            activityTypeCode: '',
            managementFileStatusCode: '',
            managementFilePurposeCode: '',
          }),
        );
});

it('resets filter when reset button clicked', async () => {
    await setup();

    const input = screen.getByPlaceholderText(/Enter a project name/i);
    await act(async () => userEvent.type(input, 'Project Y'));

    const resetButton = screen.getByTitle(/reset-button/i);
    await act(async () => userEvent.click(resetButton));

   expect(setFilter).toHaveBeenCalledWith(
          expect.objectContaining({
            projectNameOrNumber: '',
            searchBy: 'address',
            pin: '',
            pid: '',
            regionCodes: [],
            address: '',
            fileNameOrNumberOrReference: '',
            activityStatusCode: '',
            activityTypeCode: '',
            managementFileStatusCode: '',
            managementFilePurposeCode: '',
          }),
        );
});
