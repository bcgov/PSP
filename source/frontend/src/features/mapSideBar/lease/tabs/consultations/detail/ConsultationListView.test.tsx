import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { forwardRef } from 'react';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import {
  act,
  findByTitle,
  getByTestId,
  getByTitle,
  mockKeycloak,
  render,
  RenderOptions,
  userEvent,
  waitFor,
} from '@/utils/test-utils';

import { vi } from 'vitest';
import ConsultationListView, { IConsultationListViewProps } from './ConsultationListView';
import { getMockApiConsultation } from '@/mocks/consultations.mock';
import Claims from '@/constants/claims';
import { user } from '@/constants/toasts';
import { useApiContacts } from '@/hooks/pims-api/useApiContacts';
import { usePersonRepository } from '@/features/contacts/repositories/usePersonRepository';
import { useOrganizationRepository } from '@/features/contacts/repositories/useOrganizationRepository';
import { ApiGen_CodeTypes_ConsultationOutcomeTypes } from '@/models/api/generated/ApiGen_CodeTypes_ConsultationOutcomeTypes';
import { toTypeCodeNullable } from '@/utils/formUtils';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const onAdd = vi.fn();
const onEdit = vi.fn();
const onDelete = vi.fn();

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

describe('ConsultationListView component', () => {
  // render component under test

  const setup = (
    renderOptions: RenderOptions & { props?: Partial<IConsultationListViewProps> },
  ) => {
    const utils = render(
      <ConsultationListView
        {...renderOptions.props}
        loading={renderOptions?.props?.loading ?? false}
        consultations={renderOptions?.props?.consultations ?? []}
        onAdd={onAdd}
        onDelete={onDelete}
        onEdit={onEdit}
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
    const consultations = [
      { ...getMockApiConsultation(), id: 1 },
      { ...getMockApiConsultation(), id: 2 },
    ];
    const { asFragment } = setup({ props: { consultations } });
    expect(asFragment()).toMatchSnapshot();
  });

  it('displays loading spinner', async () => {
    const { getByTestId } = setup({ props: { loading: true } });

    const loading = getByTestId('filter-backdrop-loading');
    expect(loading).toBeVisible();
  });

  it('has all expected categories', async () => {
    const consultations = [
      { ...getMockApiConsultation(), id: 1 },
      { ...getMockApiConsultation(), id: 2 },
    ];
    const { getByText } = setup({ props: { consultations } });

    expect(getByText('District', { exact: false })).toBeVisible();
    expect(getByText('First Nation', { exact: false })).toBeVisible();
    expect(getByText('Headquarter (HQ)', { exact: false })).toBeVisible();
    expect(getByText('Regional planning', { exact: false })).toBeVisible();
    expect(getByText('Regional property services', { exact: false })).toBeVisible();
    expect(getByText('Strategic Real Estate (SRE)', { exact: false })).toBeVisible();
    expect(getByText('Other', { exact: false })).toBeVisible();
  });

  it('calls onAdd when clicked', async () => {
    const consultations = [
      { ...getMockApiConsultation(), id: 1 },
      { ...getMockApiConsultation(), id: 2 },
    ];
    const { getByText } = setup({ props: { consultations } });

    act(() => {
      getByText('Add Approval / Consultation', { exact: false }).click();
    });

    expect(onAdd).toHaveBeenCalledTimes(1);
  });

  it('displays the outcome as a header when clicked', async () => {
    const consultations = [{ ...getMockApiConsultation(), id: 1 }];
    const { getByText } = setup({ props: { consultations } });

    expect(getByText('Approval Denied')).toBeVisible();
  });

  it('calls onEdit when clicked', async () => {
    const consultations = [{ ...getMockApiConsultation(), id: 1 }];
    const { findByTitle } = setup({ props: { consultations } });

    const editButton = await findByTitle('Edit Consultation');

    act(() => {
      userEvent.click(editButton);
    });

    expect(onEdit).toHaveBeenCalledTimes(1);
  });

  it('calls onDelete when clicked', async () => {
    const consultations = [{ ...getMockApiConsultation(), id: 1 }];
    const { findByTitle, getByText } = setup({ props: { consultations } });

    const deleteButton = await findByTitle('Delete Consultation');

    act(() => {
      userEvent.click(deleteButton);
    });

    expect(
      getByText('You have selected to delete this Consultation. Do you want to proceed?'),
    ).toBeVisible();
  });

  it('displays error consultation icon when there is at lease one consultation in error status', async () => {
    const consultations = [
      {
        ...getMockApiConsultation(),
        id: 1,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRDENIED,
        ),
      },
      {
        ...getMockApiConsultation(),
        id: 2,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.CONSDISCONT,
        ),
      },
      {
        ...getMockApiConsultation(),
        id: 3,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRGRANTED,
        ),
      },
      {
        ...getMockApiConsultation(),
        id: 4,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.CONSCOMPLTD,
        ),
      },
      {
        ...getMockApiConsultation(),
        id: 5,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.INPROGRESS,
        ),
      },
    ];
    const { getByTestId } = setup({ props: { consultations } });

    expect(getByTestId('error-icon')).toBeVisible();
  });

  it('displays info consultation icon when there is at least one in progress outcome and no error outcomes', async () => {
    const consultations = [
      {
        ...getMockApiConsultation(),
        id: 3,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRGRANTED,
        ),
      },
      {
        ...getMockApiConsultation(),
        id: 4,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.CONSCOMPLTD,
        ),
      },
      {
        ...getMockApiConsultation(),
        id: 5,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.INPROGRESS,
        ),
      },
    ];
    const { getByTestId } = setup({ props: { consultations } });

    expect(getByTestId('info-icon')).toBeVisible();
  });

  it('displays ok consultation icon when all outcomes are in a completed status', async () => {
    const consultations = [
      {
        ...getMockApiConsultation(),
        id: 3,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.APPRGRANTED,
        ),
      },
      {
        ...getMockApiConsultation(),
        id: 4,
        consultationOutcomeTypeCode: toTypeCodeNullable(
          ApiGen_CodeTypes_ConsultationOutcomeTypes.CONSCOMPLTD,
        ),
      },
    ];
    const { getByTestId } = setup({ props: { consultations } });

    expect(getByTestId('ok-icon')).toBeVisible();
  });
});
