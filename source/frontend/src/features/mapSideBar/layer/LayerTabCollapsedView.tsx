import { groupBy } from 'lodash';
import React, { useEffect, useMemo } from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdArrowLeft, MdArrowRight } from 'react-icons/md';

import { Button } from '@/components/common/buttons';
import TooltipIcon from '@/components/common/TooltipIcon';
import { exists } from '@/utils/utils';

import { InventoryTabNames } from '../property/InventoryTabs';
import { LayerContent } from './LayerContent';
import { LayerData } from './LayerTabContainer';

export interface ILayerTabCollapsedViewProps {
  layersData?: LayerData[];
  activeGroupedPages?: Record<string, number>;
  setActiveGroupedPages?: React.Dispatch<React.SetStateAction<Record<string, number>>>;
}

export interface TabLayerCollapsedView {
  content: React.ReactNode;
  key: InventoryTabNames;
  name: string;
}
/**
 * This view should be used to display multiple layers on a single screen stacked vertically, suitable for layers with limited data.
 * It should be used with the LayerData group property to create separate rows.
 */
export const LayerTabCollapsedView: React.FC<
  React.PropsWithChildren<ILayerTabCollapsedViewProps>
> = ({ layersData = [], activeGroupedPages = {}, setActiveGroupedPages }) => {
  const groupedLayersData = useMemo(() => groupBy(layersData, ld => ld.group), [layersData]);

  /** track the current page for each layer separately, ie layersData = [{..., group: 'group1' }, {..., group: 'group2' }] -> activeGroupedPages = {group1: 0, group2: 0} */
  useEffect(() => {
    const groupedPages = Object.keys(groupedLayersData).reduce((acc, key) => {
      acc[key] = 0; // start each group on page 0.
      return acc;
    }, {});
    setActiveGroupedPages(groupedPages);
  }, [groupedLayersData, setActiveGroupedPages]);

  return (
    <>
      {Object.keys(groupedLayersData).map(groupKey => (
        <React.Fragment key={groupKey}>
          <Row noGutters className="justify-content-around">
            <Col xs={1}>
              <Button
                variant="link"
                disabled={
                  !exists(activeGroupedPages[groupKey]) || activeGroupedPages[groupKey] === 0
                }
                onClick={() => {
                  setActiveGroupedPages({
                    ...activeGroupedPages,
                    [groupKey]: --activeGroupedPages[groupKey],
                  });
                }}
                title="previous page"
              >
                <MdArrowLeft size={36} />
              </Button>
            </Col>
            <Col xs="auto" className="d-flex align-items-center">
              <b>
                {groupedLayersData[groupKey][activeGroupedPages[groupKey]]?.title ??
                  groupedLayersData[groupKey][0]?.title}{' '}
                <TooltipIcon
                  toolTipId="layer-tab-tooltip"
                  toolTip="Lists all of the intersections of this layer and shape of the current parcel (see layers on map)."
                />
              </b>
            </Col>
            <Col xs={1} className="d-flex justify-content-end">
              <Button
                variant="link"
                disabled={
                  !exists(activeGroupedPages[groupKey]) ||
                  activeGroupedPages[groupKey] >= groupedLayersData[groupKey].length - 1
                }
                onClick={() => {
                  setActiveGroupedPages({
                    ...activeGroupedPages,
                    [groupKey]: ++activeGroupedPages[groupKey],
                  });
                }}
                title="next page"
              >
                <MdArrowRight size={36} />
              </Button>
            </Col>
          </Row>
          {exists(groupedLayersData[groupKey][activeGroupedPages[groupKey]]) ? (
            <LayerContent
              config={groupedLayersData[groupKey][activeGroupedPages[groupKey]]?.config}
              data={groupedLayersData[groupKey][activeGroupedPages[groupKey]]?.data}
            />
          ) : (
            <p>No Layer Data exists.</p>
          )}
        </React.Fragment>
      ))}
      {!exists(layersData?.length) || layersData.length === 0 ? <p>No Layer Data exists.</p> : null}
    </>
  );
};
