import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { MdArrowLeft, MdArrowRight } from 'react-icons/md';

import { Button } from '@/components/common/buttons';
import TooltipIcon from '@/components/common/TooltipIcon';
import { exists } from '@/utils';

import { InventoryTabNames } from '../property/InventoryTabs';
import { LayerContent } from './LayerContent';
import { LayerData } from './LayerTabContainer';

export interface ILayerTabViewProps {
  layersData?: LayerData[];
  activePage?: number;
  setActivePage?: React.Dispatch<React.SetStateAction<number>>;
}

export interface TabLayerView {
  content: React.ReactNode;
  key: InventoryTabNames;
  name: string;
}

/**
 * This view should be used to display multiple layers on a single screen wrapped up in a single carousel, suitable for layers with large amounts of properties.
 * Note that all layer information should be collapsed on a LayerData.tab.
 */
export const LayerTabView: React.FC<React.PropsWithChildren<ILayerTabViewProps>> = ({
  layersData = [],
  activePage = 0,
  setActivePage,
}) => {
  // Converts to an object that can be consumed by the file creation process
  const currentLayer = activePage < layersData.length ? layersData[activePage] : null;
  const heading = currentLayer?.title;

  return (
    <>
      <Row noGutters className="justify-content-around">
        <Col xs={1}>
          <Button
            variant="link"
            disabled={activePage === 0}
            onClick={() => setActivePage(currentValue => --currentValue)}
            title="previous page"
          >
            <MdArrowLeft size={36} />
          </Button>
        </Col>
        <Col xs="auto" className="d-flex align-items-center">
          <b>
            {heading}
            <TooltipIcon
              toolTipId="layer-tab-tooltip"
              toolTip="Lists all of the intersections of the named layer and shape of the current parcel."
            />
          </b>
        </Col>
        <Col xs={1} className="d-flex justify-content-end">
          <Button
            variant="link"
            disabled={activePage >= layersData.length - 1}
            onClick={() => setActivePage(currentValue => ++currentValue)}
            title="next page"
          >
            <MdArrowRight size={36} />
          </Button>
        </Col>
      </Row>
      {exists(currentLayer) ? (
        <LayerContent config={currentLayer.config} data={currentLayer.data} />
      ) : (
        <p>No Layer Data exists.</p>
      )}
    </>
  );
};
