import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, userEvent } from '@/utils/test-utils';

import { IToggleSaveInputViewProps, ToggleSaveInputView } from './ToggleSaveInputView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

const setIsEditing = jest.fn();
const onClick = jest.fn();
const onSave = jest.fn();
const setValue = jest.fn();

describe('TogleSaveInputView component', () => {
  // render component under test

  const setup = (renderOptions: RenderOptions & { props: IToggleSaveInputViewProps }) => {
    const utils = render(<ToggleSaveInputView {...renderOptions.props} />, {
      ...renderOptions,
      store: storeState,
      history,
    });

    return {
      ...utils,
    };
  };

  afterEach(() => {
    jest.clearAllMocks();
  });

  it('renders as expected', () => {
    const { asFragment } = setup({ props: {} as IToggleSaveInputViewProps });
    expect(asFragment()).toMatchSnapshot();
  });

  it('value variable controls display', () => {
    const { getByText } = setup({ props: { value: 'test value' } as IToggleSaveInputViewProps });
    expect(getByText('test value')).toBeVisible();
  });

  it('value and ascurrency works as expected', () => {
    const { getByText } = setup({
      props: { asCurrency: true, value: '1' } as IToggleSaveInputViewProps,
    });
    expect(getByText('$1.00')).toBeVisible();
  });

  it('value and ascurrency works as expected when editing', () => {
    const { getByDisplayValue } = setup({
      props: { asCurrency: true, value: '1', isEditing: true } as IToggleSaveInputViewProps,
    });
    expect(getByDisplayValue('$1.00')).toBeVisible();
  });

  it('updating value with ascurrency works as expected', async () => {
    const { getByTitle } = setup({
      props: {
        asCurrency: true,
        value: '1',
        isEditing: true,
        setValue,
        onClick,
        setIsEditing,
      } as IToggleSaveInputViewProps,
    });
    await act(async () => userEvent.type(getByTitle('Enter a financial value'), '500'));
    expect(setValue).toHaveBeenCalled();
  });

  it('displays spinner when saving', () => {
    const { getByTestId } = setup({
      props: { isSaving: true, isEditing: true } as IToggleSaveInputViewProps,
    });
    expect(getByTestId('spinner')).toBeVisible();
  });

  it('displays confirm button when editing', () => {
    const { getByTitle } = setup({ props: { isEditing: true } as IToggleSaveInputViewProps });
    expect(getByTitle('confirm')).toBeVisible();
  });

  it('clicking on edit button calls setIsEdit', async () => {
    const { getByTitle } = setup({
      props: { setIsEditing, onClick, setValue, value: '' } as IToggleSaveInputViewProps,
    });
    await act(async () => userEvent.click(getByTitle('edit')));
    expect(setIsEditing).toBeCalledWith(true);
  });

  it('calls onSave when saving', async () => {
    const { getByTitle } = setup({
      props: {
        isEditing: true,
        setIsEditing,
        onSave,
        onClick,
        value: '1',
        setValue,
      } as IToggleSaveInputViewProps,
    });
    await act(async () => userEvent.click(getByTitle('confirm')));
    expect(onClick).toHaveBeenCalledWith('1');
  });
});
