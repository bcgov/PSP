import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  fillInput,
  findByTitle,
  getByTitle,
  mockKeycloak,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { vi } from 'vitest';
import { getMockApiConsultation } from '@/mocks/consultations.mock';
import Claims from '@/constants/claims';
import { user } from '@/constants/toasts';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import ConsultationListView, { IConsultationListViewProps } from '../detail/ConsultationListView';
import ConsultationEditForm, { IConsultationEditFormProps } from './ConsultationEditForm';
import { ConsultationFormModel } from './models';
import { getEmptyPerson } from '@/mocks/contacts.mock';
import { ApiGen_CodeTypes_ConsultationOutcomeTypes } from '@/models/api/generated/ApiGen_CodeTypes_ConsultationOutcomeTypes';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onSubmit = vi.fn();
const onCancel = vi.fn();

const mockGetPersonApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

const mockGetOrganizationApi = {
  error: undefined,
  response: undefined,
  execute: vi.fn(),
  loading: false,
  status: 200,
};

vi.mock('@/features/contacts/repositories/usePersonRepository');
vi.mocked(usePersonRepository).mockImplementation(() => ({
  getPersonDetail: mockGetPersonApi,
}));

vi.mock('@/features/contacts/repositories/useOrganizationRepository');
vi.mocked(useOrganizationRepository).mockImplementation(() => ({
  getOrganizationDetail: mockGetOrganizationApi,
}));

describe('ConsultationEditForm component', () => {
  // render component under test

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IConsultationEditFormProps> },
  ) => {
    const utils = render(
      <ConsultationEditForm
        {...renderOptions.props}
        isLoading={renderOptions?.props?.isLoading ?? false}
        initialValues={
          renderOptions?.props?.initialValues ??
          ConsultationFormModel.fromApi(getMockApiConsultation(), getEmptyPerson(), null)
        }
        onSubmit={onSubmit}
        onCancel={onCancel}
      />,
      {
        ...renderOptions,
        claims: [Claims.LEASE_EDIT],
        useMockAuthentication: true,
        store: storeState,
        history,
      },
    );

    return {
      ...utils,
    };
  };

  it('renders as expected', () => {
    const { asFragment } = setup({});
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays loading spinner', async () => {
    const { getByTestId } = setup({ props: { isLoading: true } });

    const loading = getByTestId('filter-backdrop-loading');
    expect(loading).toBeVisible();
  });

  it('cancels if no changes have been made', async () => {
    const { getByText } = setup({});

    act(() => {
      userEvent.click(getByText('Cancel'));
    });

    expect(onCancel).toHaveBeenCalledTimes(1);
  });

  it('does not cancel if changes have been made', async () => {
    const { getByText, findAllByDisplayValue, container } = setup({});

    await act(async () => {
      await fillInput(container, 'comment', 'test comments', 'textarea');
    });

    await findAllByDisplayValue('test comments');

    act(() => {
      userEvent.click(getByText('Cancel'));
    });

    expect(onCancel).not.toHaveBeenCalled();
  });

  it('cannot save when form has not been edited', async () => {
    const { getByText } = setup({});

    expect(getByText('Save').parentElement).toBeDisabled();
  });

  it('submit calls onSubmit when changes have been made', async () => {
    const { getByText, container } = setup({
      props: {
        initialValues: ConsultationFormModel.fromApi(
          {
            ...getMockApiConsultation(),
            consultationOutcomeTypeCode: {
              id: ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRGRANTED,
              description: 'Approved',
              isDisabled: false,
              displayOrder: 1,
            },
          },
          getEmptyPerson(),
          null,
        ),
      },
    });

    await act(async () => {
      await fillInput(container, 'comment', 'test comments', 'textarea');
    });

    act(() => {
      userEvent.click(getByText('Save'));
    });

    await waitFor(() => {
      expect(onSubmit).toHaveBeenCalledTimes(1);
    });
  });

  it('Comments become required if certain outcomes are selected', async () => {
    const { getByText, container } = setup({
      props: {
        initialValues: ConsultationFormModel.fromApi(
          {
            ...getMockApiConsultation(),
            consultationOutcomeTypeCode: {
              id: ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRDENIED,
              description: 'Denied',
              isDisabled: false,
              displayOrder: 1,
            },
          },
          getEmptyPerson(),
          null,
        ),
      },
    });

    await act(async () => {
      await fillInput(container, 'comment', '', 'textarea');
    });

    expect(getByText('Please add comments')).toBeVisible();
  });
});
