import React from 'react';
import { fireEvent, render } from '@/utils/test-utils';
import { LayerTabCollapsedView } from './LayerTabCollapsedView';
import { LayerData } from './LayerTabContainer';
import { vi } from 'vitest';

describe('LayerTabCollapsedView', () => {
  const setActiveGroupedPages = vi.fn();

  it('matches snapshot (empty layersData)', () => {
    const { container } = render(
      <LayerTabCollapsedView
        layersData={[]}
        activeGroupedPages={{}}
        setActiveGroupedPages={setActiveGroupedPages}
      />,
    );
    expect(container).toMatchSnapshot();
  });

  it('renders with no layers', () => {
    const { getByText } = render(
      <LayerTabCollapsedView
        layersData={[]}
        activeGroupedPages={{}}
        setActiveGroupedPages={setActiveGroupedPages}
      />,
    );
    expect(getByText(/no layer data exists/i)).toBeInTheDocument();
  });

  it('renders layers with no group', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any },
      { title: 'Layer 2', data: {}, config: {}, tab: 'test' as any },
    ];
    const { getByText, queryByText } = render(
      <LayerTabCollapsedView
        layersData={layersData}
        activeGroupedPages={{}}
        setActiveGroupedPages={setActiveGroupedPages}
      />,
    );
    expect(getByText('Layer 1')).toBeInTheDocument();
    expect(queryByText('Layer 2')).toBeNull();
  });

  it('renders layers with each layer having a unique group', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any, group: 'A' },
      { title: 'Layer 2', data: {}, config: {}, tab: 'test' as any, group: 'B' },
    ];
    const { getByText } = render(
      <LayerTabCollapsedView
        layersData={layersData}
        activeGroupedPages={{ A: 0, B: 0 }}
        setActiveGroupedPages={setActiveGroupedPages}
      />,
    );
    expect(getByText('Layer 1')).toBeInTheDocument();
    expect(getByText('Layer 2')).toBeInTheDocument();
  });

  it('renders layers with multiple layers in the same group', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any, group: 'A' },
      { title: 'Layer 2', data: {}, config: {}, tab: 'test' as any, group: 'A' },
      { title: 'Layer 3', data: {}, config: {}, tab: 'test' as any, group: 'B' },
    ];
    const { getByText, queryByText } = render(
      <LayerTabCollapsedView
        layersData={layersData}
        activeGroupedPages={{ A: 0, B: 0 }}
        setActiveGroupedPages={setActiveGroupedPages}
      />,
    );
    expect(getByText('Layer 1')).toBeInTheDocument();
    expect(queryByText('Layer 2')).toBeNull();
  });

  it('calls setActiveGroupedPages when right arrow clicked', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any, group: 'A' },
      { title: 'Layer 2', data: {}, config: {}, tab: 'test' as any, group: 'A' },
      { title: 'Layer 3', data: {}, config: {}, tab: 'test' as any, group: 'B' },
    ];
    const { getByText, getAllByTitle } = render(
      <LayerTabCollapsedView
        layersData={layersData}
        activeGroupedPages={{ A: 0, B: 0 }}
        setActiveGroupedPages={setActiveGroupedPages}
      />,
    );
    expect(getByText('Layer 1')).toBeInTheDocument();
    const rightButton = getAllByTitle('next page')[0];
    fireEvent.click(rightButton);
    expect(setActiveGroupedPages).toHaveBeenCalledWith({ A: 1, B: 0 });
  });

  it('disables right arrow when there is only one item in a group', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any, group: 'A' },
    ];
    const { getByText, getAllByTitle } = render(
      <LayerTabCollapsedView
        layersData={layersData}
        activeGroupedPages={{ A: 0, B: 0 }}
        setActiveGroupedPages={setActiveGroupedPages}
      />,
    );
    expect(getByText('Layer 1')).toBeInTheDocument();
    const rightButton = getAllByTitle('next page')[0];
    fireEvent.click(rightButton);
    expect(setActiveGroupedPages).not.toHaveBeenCalledWith({ A: 1 });
  });
});
