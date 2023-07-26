import { createMemoryHistory } from 'history';

import { mockLookups } from '@/mocks/lookups.mock';
import { lookupCodesSlice } from '@/store/slices/lookupCodes';
import { act, render, RenderOptions, waitFor } from '@/utils/test-utils';

import {
  IToggleSaveInputContainerProps,
  ToggleSaveInputContainer,
} from './ToggleSaveInputContainer';
import { IToggleSaveInputViewProps } from './ToggleSaveInputView';

const history = createMemoryHistory();
const storeState = {
  [lookupCodesSlice.name]: { lookupCodes: mockLookups },
};

let viewProps: IToggleSaveInputViewProps;
const View = (props: IToggleSaveInputViewProps) => {
  viewProps = props;
  return <></>;
};

const onSave = jest.fn();

describe('ToggleSaveInputContainer component', () => {
  // render component under test

  const setup = (renderOptions: RenderOptions & { props: IToggleSaveInputContainerProps }) => {
    const utils = render(<ToggleSaveInputContainer {...renderOptions.props} View={View} />, {
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

  it('works as expected when promise returns success', async () => {
    onSave.mockResolvedValue('test');
    setup({ props: { onSave, View } as IToggleSaveInputContainerProps });
    await act(async () => viewProps.onClick('test'));

    expect(onSave).toHaveBeenCalledWith('');
    await waitFor(() => expect(viewProps.isSaving).toBe(false));
    expect(viewProps.isEditing).toBe(false);
    expect(viewProps.value).toBe('test');
  });

  it('works as expected when promise rejects', async () => {
    onSave.mockRejectedValue('fail');
    setup({ props: { onSave, View } as IToggleSaveInputContainerProps });
    await act(async () => viewProps.onClick('test'));

    expect(onSave).toHaveBeenCalledWith('');
    await waitFor(() => expect(viewProps.isSaving).toBe(false));
    expect(viewProps.value).toBe('');
    expect(viewProps.isEditing).toBe(false);
  });
});
