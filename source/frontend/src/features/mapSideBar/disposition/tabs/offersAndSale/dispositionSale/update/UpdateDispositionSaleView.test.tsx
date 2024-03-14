import { createMemoryHistory } from 'history';

import Claims from '@/constants/claims';
import { DispositionSaleFormModel } from '@/features/mapSideBar/disposition/models/DispositionSaleFormModel';
import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes/lookupCodesSlice';
import { systemConstantsSlice } from '@/store/slices/systemConstants';
import { render, RenderOptions, waitFor, act } from '@/utils/test-utils';

import UpdateDispositionSaleView, {
  IUpdateDispositionSaleViewProps,
} from './UpdateDispostionSaleView';

const defaultInitialValues = new DispositionSaleFormModel(null, 1, 0);

const history = createMemoryHistory();
jest.mock('@react-keycloak/web');

const onSave = jest.fn();
const onCancel = jest.fn();
const onSuccess = jest.fn();
const onError = jest.fn();

describe('Update Disposition Sale View', () => {
  const setup = async (
    renderOptions: RenderOptions & { props?: Partial<IUpdateDispositionSaleViewProps> } = {},
  ) => {
    // const ref = createRef<FormikProps<DispositionFormModel>>();
    const utils = render(
      <UpdateDispositionSaleView
        {...renderOptions.props}
        initialValues={renderOptions.props?.initialValues ?? defaultInitialValues}
        loading={renderOptions.props?.loading ?? false}
        onCancel={onCancel}
        onSave={onSave}
        onSuccess={onSuccess}
        onError={onError}
      />,
      {
        ...renderOptions,
        useMockAuthentication: true,
        claims: renderOptions?.claims ?? [Claims.DISPOSITION_EDIT],
        history: history,
        store: {
          [lookupCodesSlice.name]: { lookupCodes: mockLookups },
          [systemConstantsSlice.name]: { systemConstants: [{ name: 'GST', value: '5.0' }] },
        },
      },
    );

    return {
      ...utils,
      getCancelButton: () => utils.getByText(/Cancel/i),
      getFinalSaleAmountTextbox: () =>
        utils.container.querySelector(`input[name="finalSaleAmount"]`) as HTMLInputElement,
      getGSTCollectedAmountTextbox: () =>
        utils.container.querySelector(`input[name="gstCollectedAmount"]`) as HTMLInputElement,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', async () => {
    
    const { asFragment } = await setup();
    await act(async ()=>{});
    const fragment = await waitFor(async () => asFragment());
    expect(fragment).toMatchSnapshot();
    
  });
});
