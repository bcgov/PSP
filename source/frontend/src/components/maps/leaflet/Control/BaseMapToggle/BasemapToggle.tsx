import './BasemapToggle.scss';

import React, { useState } from 'react';

export type BaseLayer = {
  name: string;
  urls: string[];
  attribution: string;
  thumbnail: string;
};

export type BasemapToggleEvent = {
  current: BaseLayer;
  previous: BaseLayer;
};

export type BasemapToggleProps = {
  baseLayers: BaseLayer[];
  onToggle?: (e: BasemapToggleEvent) => void;
};

const BasemapToggle: React.FC<React.PropsWithChildren<BasemapToggleProps>> = props => {
  const [updating] = useState(false);
  const [currentBasemap, secondaryBasemap] = props.baseLayers;

  const toggle = () => {
    const e: BasemapToggleEvent = {
      current: secondaryBasemap,
      previous: currentBasemap,
    };
    props.onToggle?.(e);
  };

  return (
    <div className={updating ? 'basemap-container view-busy' : 'basemap-container'}>
      <button className="basemap-item current"></button>
      <label className="caption">{secondaryBasemap?.name}</label>
      <div className="basemap-item basemap-item-button secondary" onClick={toggle}>
        <img src={secondaryBasemap?.thumbnail} role="presentation" alt="Map Thumbnail" />
      </div>
    </div>
  );
};

export default BasemapToggle;
