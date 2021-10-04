import variables from '_variables.module.scss';
import clsx from 'classnames';
import TooltipWrapper from 'components/common/TooltipWrapper';
import { PropertyPopUpContext } from 'components/maps/providers/PropertyPopUpProvider';
import { MAX_ZOOM } from 'constants/strings';
import useKeycloakWrapper from 'hooks/useKeycloakWrapper';
import L from 'leaflet';
import React, { useContext, useEffect, useState } from 'react';
import Button from 'react-bootstrap/Button';
import { FaInfo } from 'react-icons/fa';
import { useMap } from 'react-leaflet';
import { toast } from 'react-toastify';
import { useProperties } from 'store/slices/properties';
import styled from 'styled-components';

import Control from '../Control/Control';
import FilterBackdrop from '../FilterBackdrop';
import HeaderActions from './HeaderActions';
import { InfoContent } from './InfoContent';

const InfoContainer = styled.div`
  margin-right: -1rem;
  width: 34.1rem;
  min-height: 5.2rem;
  height: auto;
  background-color: #fff;
  position: relative;
  border-radius: 0.4rem;
  box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
  z-index: 1000;
  &.closed {
    width: 0rem;
    height: 0rem;
  }
`;

const InfoHeader = styled.div`
  width: 100%;
  height: 5.2rem;
  background-color: ${variables.slideOutBlue};
  color: #fff;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: space-evenly;
  padding-top: 1.6rem;
  border-top-right-radius: 0.4rem;
  border-top-left-radius: 0.4rem;
`;

const InfoMain = styled.div`
  width: 100%;
  padding-left: 1rem;
  padding: 0rem 1rem 0.5rem 1rem;
  position: relative;

  &.open {
    overflow-y: scroll;
    max-height: calc(100vh - 38rem);
  }
`;

const InfoIcon = styled(FaInfo)`
  font-size: 3rem;
`;

const InfoButton = styled(Button)`
  width: 5.2rem;
  height: 5.2rem;
  position: absolute;
  left: -5.1rem;
  background-color: #fff;
  color: ${variables.slideOutBlue};
  border-color: ${variables.slideOutBlue};
  box-shadow: -0.2rem 0.1rem 0.4rem rgba(0, 0, 0, 0.2);
  &.open {
    border-top-right-radius: 0;
    border-bottom-right-radius: 0;
    top: 0rem;
  }
`;

const Title = styled.p`
  font-size: 1.8rem;
  color: #ffffff;
  text-decoration: none solid rgb(255, 255, 255);
  line-height: 1.8rem;
  font-weight: bold;
`;

export type InfoControlProps = {
  /** whether the slide out is open or closed */
  open: boolean;
  /** set the slide out as open or closed*/
  setOpen: (state: boolean) => void;
  /** additional action for when a link is clicked */
  onHeaderActionClick?: () => void;
};

/**
 * Component to display the popup information of a parcel/building
 * @param open open/closed state of the slideout
 * @param setOpen function to set the slideout as open or closed
 * @param onHeaderActionClick action to be taken when a menu item is clicked
 */
const InfoControl: React.FC<InfoControlProps> = ({ open, setOpen, onHeaderActionClick }) => {
  const popUpContext = useContext(PropertyPopUpContext);
  const { getProperty } = useProperties();
  const mapInstance = useMap();
  const { propertyInfo } = popUpContext;
  const jumpToView = () =>
    mapInstance.setView(
      [propertyInfo?.latitude as number, propertyInfo?.longitude as number],
      Math.max(MAX_ZOOM, mapInstance.getZoom()),
    );
  const zoomToView = () =>
    mapInstance.flyTo(
      [propertyInfo?.latitude as number, propertyInfo?.longitude as number],
      Math.max(MAX_ZOOM, mapInstance.getZoom()),
      { animate: false },
    );

  useEffect(() => {
    const elem = L.DomUtil.get('infoContainer');
    if (elem) {
      L.DomEvent.on(elem, 'mousewheel', L.DomEvent.stopPropagation);
    }
  });

  //whether the general info is open
  const [generalInfoOpen, setGeneralInfoOpen] = useState<boolean>(true);

  const keycloak = useKeycloakWrapper();
  const canViewProperty = keycloak.canUserViewProperty(propertyInfo);
  const canEditProperty = keycloak.canUserEditProperty(propertyInfo);

  const renderContent = () => {
    if (popUpContext.propertyInfo) {
      if (generalInfoOpen) {
        return (
          <>
            <FilterBackdrop show={open && popUpContext.loading}></FilterBackdrop>
            <HeaderActions
              propertyInfo={popUpContext.propertyInfo}
              propertyTypeId={popUpContext.propertyTypeId}
              onLinkClick={onHeaderActionClick}
              jumpToView={jumpToView}
              zoomToView={zoomToView}
              canViewDetails={canViewProperty}
              canEditDetails={canEditProperty}
            />
            <InfoContent
              propertyInfo={popUpContext.propertyInfo}
              propertyTypeId={popUpContext.propertyTypeId}
              canViewDetails={canViewProperty}
            />
          </>
        );
      }
    } else {
      return <p id="emptySlideOut">Click a pin to view the property details</p>;
    }
  };

  return (
    <Control position="topright">
      <InfoContainer id="infoContainer" className={clsx({ closed: !open })}>
        {open && (
          <InfoHeader>
            <Title>Property Info</Title>
          </InfoHeader>
        )}
        <TooltipWrapper toolTipId="info-slideout-id" toolTip="Property Information">
          <InfoButton
            id="slideOutInfoButton"
            variant="outline-secondary"
            onClick={() => {
              if (popUpContext.propertyInfo?.id && !open) {
                popUpContext.setLoading(true);
                getProperty(popUpContext.propertyInfo.id)
                  .then(property => {
                    popUpContext.setPropertyInfo(property);
                  })
                  .catch(() => {
                    toast.error('Unable to load property details, refresh the page and try again.');
                  })
                  .finally(() => {
                    popUpContext.setLoading(false);
                  });
              }
              if (!open) {
                setOpen(true);
                setGeneralInfoOpen(true);
              } else if (open && !generalInfoOpen) {
                setGeneralInfoOpen(true);
              } else {
                setOpen(false); //close the slide out
              }
            }}
            className={clsx({ open })}
          >
            <InfoIcon />
          </InfoButton>
        </TooltipWrapper>
        {open && <InfoMain className={clsx({ open })}>{renderContent()}</InfoMain>}
      </InfoContainer>
    </Control>
  );
};
export default InfoControl;
