import { FormikProps } from 'formik';
import { createMemoryHistory } from 'history';
import { createRef } from 'react';

import Claims from '@/constants/claims';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { render, RenderOptions } from '@/utils/test-utils';

import { DispositionFormModel } from '../models/DispositionFormModel';
import AddDispositionContainerView, {
  IAddDispositionContainerViewProps,
} from './AddDispositionContainerView';

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

const onCancel = jest.fn();
const onSave = jest.fn();
const onSubmit = jest.fn();
const confirmBeforeAdd = jest.fn();

const initialValues = new DispositionFormModel();

describe('Add Disposition Container View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IAddDispositionContainerViewProps> } = {},
  ) => {
    const ref = createRef<FormikProps<DispositionFormModel>>();
    const utils = render(
      <AddDispositionContainerView
        {...renderOptions.props}
        formikRef={ref}
        dispositionInitialValues={renderOptions.props?.dispositionInitialValues ?? initialValues}
        loading={renderOptions.props?.loading ?? false}
        displayFormInvalid={renderOptions.props?.displayFormInvalid ?? false}
        onCancel={onCancel}
        onSave={onSave}
        onSubmit={onSubmit}
        confirmBeforeAdd={confirmBeforeAdd}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_ADD],
        history: history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
        },
      },
    );

    return {
      ...utils,
      getCancelButton: () => utils.getByText(/Cancel/i),
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('matches snapshot', async () => {
    const { asFragment } = await setup();
    expect(asFragment()).toMatchSnapshot();
  });
});
