import styled from 'styled-components';

export const MapContainer = styled.div`
  grid-area: map;

  .leaflet-div-icon {
    background: none;
    border: none;
    img {
      width: inherit;
      height: inherit;
      filter: drop-shadow(3px 1px 10px rgb(0 0 0 / 0.6));
    }
  }

  .leaflet-container {
    /* Subtract the header, nav-bar and footer sizes */
    height: 100%;
    margin: auto;
    font-size: 1.2rem;

    .leaflet-control-attribution {
      font-size: 1.1rem;
    }

    .leaflet-popup-content-wrapper {
      border-radius: 0;
    }
    .leaflet-popup-content {
      margin: 0rem;
    }

    .leaflet-control-container .leaflet-top {
      z-index: 500;
    }
  }

  .leaflet-control-layers {
    &.leaflet-control-layers-expanded {
      border-radius: 0rem;
    }
    .leaflet-control-layers-overlays {
      display: flex;
      flex-direction: column;
      padding: 1rem;
      width: 15rem;
      label {
        display: flex;
      }
    }
  }
`;

export const MapGrid = styled.div`
  width: 100%;
  display: grid;
  grid-template-rows: ${props => props.theme.css.mapfilterHeight} 1fr;
  grid-template-columns: min-content 1fr;
  grid-template-areas:
    'filter filter'
    'map map';
  &.hideSearchBar {
    grid-template-rows: 0 1fr;
    transition: 1s;
  }

  transition: margin 1s, width 1s;
  position: relative;
`;
