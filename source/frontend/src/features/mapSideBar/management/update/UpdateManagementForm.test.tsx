import { FormikProps } from 'formik';
import { createRef } from 'react';

import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { mockLookups } from '@/mocks/lookups.mock';
import { mockManagementFileResponse } from '@/mocks/managementFiles.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor, waitForEffects } from '@/utils/test-utils';

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

const retrieveProjectProductsFn = vi.fn();
vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider).mockReturnValue({
  retrieveProjectProducts: retrieveProjectProductsFn,
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
        canEditDetails={props.canEditDetails ?? true}
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
    const { asFragment } = await setup({
      initialValues,
      loading: false,
      formikRef: ref,
      canEditDetails: true,
      onSubmit,
    });
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders loading spinner', async () => {
    const { getByTestId } = await setup({
      initialValues,
      loading: true,
      formikRef: ref,
      canEditDetails: true,
      onSubmit,
    });
    expect(getByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('it validates that only profile is not repeated on another team member', async () => {
    const { getByTestId, queryByTestId, getTeamMemberProfileDropDownList } = await setup({
      initialValues,
      loading: false,
      formikRef: ref,
      canEditDetails: true,
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
      canEditDetails: true,
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

  it('it disables fields when file is not editable', async () => {
    retrieveProjectProductsFn.mockResolvedValue([]);
    const { container } = await setup({
      initialValues,
      loading: false,
      formikRef: ref,
      canEditDetails: false,
      onSubmit,
    });
    await waitForEffects();

    const projectInput = container.querySelector(`#typeahead-project`);
    expect(projectInput).toBeDisabled();

    const productInput = container.querySelector(`select#input-productId`);
    expect(productInput).toBeDisabled();

    const fundingInput = container.querySelector(`#input-fundingTypeCode`);
    expect(fundingInput).toBeDisabled();

    const fileNameInput = container.querySelector(`#input-fileName`);
    expect(fileNameInput).toBeDisabled();

    const legacyFileInput = container.querySelector(`#input-legacyFileNum`);
    expect(legacyFileInput).toBeDisabled();

    const pourposeInput = container.querySelector(`#input-purposeTypeCode`);
    expect(pourposeInput).toBeDisabled();

    const additionalDetailsInput = container.querySelector(`#input-additionalDetails`);
    expect(additionalDetailsInput).toBeDisabled();
  });
});
