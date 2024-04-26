import axios from 'axios';
import MockAdapter from 'axios-mock-adapter';
import { FormikProps } from 'formik';
import { createRef } from 'react';

import { DispositionFormModel } from '@/features/mapSideBar/disposition/models/DispositionFormModel';
import { IAutocompletePrediction } from '@/interfaces/IAutocomplete';
import { mockDispositionFileResponse } from '@/mocks/dispositionFiles.mock';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent, waitFor, waitForEffects } from '@/utils/test-utils';

import UpdateDispositionForm, { IUpdateDispositionFormProps } from './UpdateDispositionForm';
import { useUserInfoRepository } from '@/hooks/repositories/useUserInfoRepository';
import { ApiGen_Concepts_RegionUser } from '@/models/api/generated/ApiGen_Concepts_RegionUser';
import { ApiGen_Concepts_User } from '@/models/api/generated/ApiGen_Concepts_User';
import { useProjectProvider } from '@/hooks/repositories/useProjectProvider';

const mockAxios = new MockAdapter(axios);

// mock auth library

const onSubmit = vi.fn();
const ref = createRef<FormikProps<DispositionFormModel>>();

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

vi.mock('@/hooks/repositories/useUserInfoRepository');
vi.mocked(useUserInfoRepository).mockReturnValue({
  retrieveUserInfo: vi.fn(),
  retrieveUserInfoLoading: true,
  retrieveUserInfoResponse: {
    userRegions: [
      {
        id: 1,
        userId: 5,
        regionCode: 1,
      } as ApiGen_Concepts_RegionUser,
      {
        id: 2,
        userId: 5,
        regionCode: 2,
      } as ApiGen_Concepts_RegionUser,
    ],
  } as ApiGen_Concepts_User,
});

vi.mock('@/hooks/repositories/useProjectProvider');
vi.mocked(useProjectProvider).mockReturnValue({
  retrieveProjectProducts: vi.fn(),
} as unknown as ReturnType<typeof useProjectProvider>);

describe('UpdateDispositionForm component', () => {
  // render component under test
  const setup = (props: IUpdateDispositionFormProps, renderOptions: RenderOptions = {}) => {
    const utils = render(
      <UpdateDispositionForm
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

    return {
      ...utils,
      getFormikRef: () => ref,
      getTeamMemberProfileDropDownList: (index: number = 0) =>
        utils.container.querySelector(
          `select[name="team.${index}.teamProfileTypeCode"]`,
        ) as HTMLSelectElement,
      getDispositionFileStatusDropDownList: (index: number = 0) =>
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

  let initialValues: DispositionFormModel;

  beforeEach(() => {
    initialValues = DispositionFormModel.fromApi(mockDispositionFileResponse());
  });

  afterEach(() => {
    mockAxios.resetHistory();
    vi.clearAllMocks();
  });

  it('renders as expected', async () => {
    const { asFragment } = setup({ initialValues, loading: false, formikRef: ref, onSubmit });
    await act(async () => {});
    expect(asFragment()).toMatchSnapshot();
  });

  it('renders loading spinner', async () => {
    const { getByTestId } = setup({ initialValues, loading: true, formikRef: ref, onSubmit });
    await act(async () => {});
    expect(getByTestId('filter-backdrop-loading')).toBeVisible();
  });

  it('it validates that only profile is not repeated on another team member', async () => {
    const { getByTestId, queryByTestId, getTeamMemberProfileDropDownList } = setup({
      initialValues,
      loading: false,
      formikRef: ref,
      onSubmit,
    });

    // Set duplicate should fail
    await act(async () => {
      userEvent.selectOptions(getTeamMemberProfileDropDownList(0), 'NEGOTAGENT');
    });

    expect(getByTestId('team-profile-dup-error')).toBeVisible();

    // Set unique should pass
    await act(async () => {
      userEvent.selectOptions(getTeamMemberProfileDropDownList(0), 'MOTILEAD');
    });

    expect(queryByTestId(/team-profile-dup-error/i)).toBeNull();
  });

  it('it clears the product field when a project is removed', async () => {
    const { getRemoveProjectButton, getProductDropDownList, getFormikRef } = setup({
      initialValues,
      loading: false,
      formikRef: ref,
      onSubmit,
    });

    await waitFor(() => userEvent.click(getRemoveProjectButton()));
    await waitForEffects();

    initialValues.productId = '';
    initialValues.project = '' as unknown as IAutocompletePrediction;

    expect(getProductDropDownList()).toBeNull();

    await waitFor(() => getFormikRef().current?.submitForm());

    expect(onSubmit).toHaveBeenLastCalledWith(initialValues, expect.anything());
  });

  it('it enables and makes required the file completion date when status set to complete', async () => {
    const { getDispositionFileStatusDropDownList, getCompletionDate } = setup({
      initialValues,
      loading: false,
      formikRef: ref,
      onSubmit,
    });

    expect(getCompletionDate()).toBeDisabled();

    // Set duplicate should fail
    await act(async () => {
      userEvent.selectOptions(getDispositionFileStatusDropDownList(0), 'COMPLETE');
    });

    expect(getCompletionDate()).toBeEnabled();
  });
});
