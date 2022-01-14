import { initialState, systemConstantsSlice } from './systemConstantsSlice';
describe('system constants slice reducer functionality', () => {
  const systemConstantsReducer = systemConstantsSlice.reducer;
  it('saves the list of system constants', () => {
    const result = systemConstantsReducer(undefined, {
      type: systemConstantsSlice.actions.storeSystemConstants,
      payload: [
        {
          name: 'DBVERSION',
          value: '17.00',
        },
      ],
    });
    expect(result).toEqual({
      ...initialState,
      systemConstants: [
        {
          name: 'DBVERSION',
          value: '17.00',
        },
      ],
    });
  });
});
