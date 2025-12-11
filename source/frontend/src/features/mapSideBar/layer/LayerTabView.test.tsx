import React from 'react';
import { render, fireEvent, getByTitle } from '@/utils/test-utils';
import { LayerTabView } from './LayerTabView';
import { LayerData } from './LayerTabContainer';
import { vi } from 'vitest';

describe('LayerTabView', () => {
  it('matches snapshot (empty layersData)', () => {
    const { container } = render(
      <LayerTabView layersData={[]} activePage={0} setActivePage={vi.fn()} />,
    );
    expect(container).toMatchSnapshot();
  });

  it('renders with no layers', () => {
    const { getByText } = render(
      <LayerTabView layersData={[]} activePage={0} setActivePage={vi.fn()} />,
    );
    expect(getByText(/no layer data exists/i)).toBeInTheDocument();
  });

  it('renders with one layer', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any },
    ];
    const { getByText } = render(
      <LayerTabView layersData={layersData} activePage={0} setActivePage={vi.fn()} />,
    );
    expect(getByText('Layer 1')).toBeInTheDocument();
  });

  it('renders with multiple layers, same tab, different titles', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any },
      { title: 'Layer 2', data: {}, config: {}, tab: 'test' as any },
      { title: 'Layer 3', data: {}, config: {}, tab: 'test' as any },
    ];
    const { getByText } = render(
      <LayerTabView layersData={layersData} activePage={0} setActivePage={vi.fn()} />,
    );
    expect(getByText('Layer 1')).toBeInTheDocument();
  });

  it('clicking MdArrowRight calls setActivePage', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any },
      { title: 'Layer 2', data: {}, config: {}, tab: 'test' as any },
    ];
    const setActivePage = vi.fn();
    const { getByTitle } = render(
      <LayerTabView layersData={layersData} activePage={0} setActivePage={setActivePage} />,
    );

    const rightButton = getByTitle('next page');
    fireEvent.click(rightButton);
    expect(setActivePage).toHaveBeenCalled();
  });

  it('disables MdArrowRight when there is only one page', () => {
    const layersData: LayerData[] = [
      { title: 'Layer 1', data: {}, config: {}, tab: 'test' as any },
    ];
    const setActivePage = vi.fn();
    const { getByTitle } = render(
      <LayerTabView layersData={layersData} activePage={0} setActivePage={setActivePage} />,
    );

    const rightButton = getByTitle('next page');
    fireEvent.click(rightButton);
    expect(setActivePage).not.toHaveBeenCalled();
  });
});
