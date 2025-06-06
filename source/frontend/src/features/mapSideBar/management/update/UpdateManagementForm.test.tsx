import { FormikProps } from 'formik';
import { createRef } from 'react';

import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitForEffects } from '@/utils/test-utils';

import { ManagementFormModel } from '../models/ManagementFormModel';
import UpdateManagementForm, { IUpdateManagementFormProps } from './UpdateManagementForm';

const onSubmit = vi.fn();
const ref = createRef<FormikProps<ManagementFormModel>>();

// Need to mock this library for unit tests
vi.mock('react-visibility-sensor', () => {
  return {
    default: vi.fn().mockImplementation(({ children }) => {
      if (children instanceof Function) {
        return children({ isVisible: true });
      }
      return children;
    }),
  };
});

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider).mockReturnValue({
  retrieveProjectProducts: vi.fn(),
} as unknown as ReturnType<typeof useProjectProvider>);

describe('UpdateManagementForm component', () => {
  // render component under test
  const setup = async (props: IUpdateManagementFormProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateManagementForm
        formikRef={ref}
        initialValues={props.initialValues}
        onSubmit={props.onSubmit}
        loading={props.loading}
      />,
      {
        ...renderOptions,
        claims: [],
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    await act(async () => {});

    return {
      ...utils,
      getFormikRef: () => ref,
      getTeamMemberProfileDropDownList: (index: number = 0) =>
        utils.container.querySelector(
          `select[name="team.${index}.teamProfileTypeCode"]`,
        ) as HTMLSelectElement,
      getManagementFileStatusDropDownList: (index: number = 0) =>
        utils.container.querySelector(`select[name="fileStatusTypeCode"]`) as HTMLSelectElement,
      getCompletionDate: () => utils.container.querySelector(`input[name="completionDate"]`),
      getRemoveProjectButton: () =>
        utils.container.querySelector(
          `div[data-testid="typeahead-project"] button`,
        ) as HTMLSelectElement,
      getProductDropDownList: (index: number = 0) =>
        utils.container.querySelector(`select[name="productId"]`) as HTMLSelectElement,
    };
  };

  let initialValues: ManagementFormModel;

  beforeEach(() => {
    initialValues = ManagementFormModel.fromApi(mockManagementFileResponse());
  });

  afterEach(() => {
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = await setup({ initialValues, loading: false, formikRef: ref, onSubmit });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders loading spinner', async () => {
    const { getByTestId } = await setup({ initialValues, loading: true, formikRef: ref, onSubmit });
    expect(getByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('it validates that only profile is not repeated on another team member', async () => {
    const { getByTestId, queryByTestId, getTeamMemberProfileDropDownList } = await setup({
      initialValues,
      loading: false,
      formikRef: ref,
      onSubmit,
    });

    // Set duplicate should fail
    await act(async () => {
      userEvent.selectOptions(getTeamMemberProfileDropDownList(0), 'MINSTAFF');
    });

    expect(getByTestId('team-profile-dup-error')).toBeVisible();

    // Set unique should pass
    await act(async () => {
      userEvent.selectOptions(getTeamMemberProfileDropDownList(0), 'CONTRACT');
    });

    expect(queryByTestId('team-profile-dup-error')).toBeNull();
  });

  it('it clears the product field when a project is removed', async () => {
    const {
      getRemoveProjectButton,
      getProductDropDownList,
      getFormikRef,
      getTeamMemberProfileDropDownList,
    } = await setup({
      initialValues,
      loading: false,
      formikRef: ref,
      onSubmit,
    });

    await act(async () => userEvent.click(getRemoveProjectButton()));

    initialValues.productId = '';
    initialValues.fileName = 'test';
    initialValues.purposeTypeCode = 'ENGINEER';
    initialValues.project = '' as unknown as IAutocompletePrediction;

    expect(getProductDropDownList()).toBeNull();

    await act(async () => {
      userEvent.selectOptions(getTeamMemberProfileDropDownList(0), 'MINSTAFF');
    });
    await waitForEffects();

    await act(async () => getFormikRef().current?.submitForm());

    expect(onSubmit).toHaveBeenCalled();
  });
});
