import React from 'react';
import { Col, Row } from 'react-bootstrap';
import { AiOutlineExclamationCircle } from 'react-icons/ai';
import { FaSearchPlus } from 'react-icons/fa';
import { HiCube } from 'react-icons/hi2';
import styled from 'styled-components';

import { StyledIconButton } from '@/components/common/buttons';
import {
  HeaderContentCol,
  HeaderField,
  HeaderLabelCol,
} from '@/components/common/HeaderField/HeaderField';
import { StyledFiller } from '@/components/common/HeaderField/styles';
import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import { InlineFlexDiv } from '@/components/common/styles';
import TooltipWrapper from '@/components/common/TooltipWrapper';
import { IMapProperty } from '@/components/propertySelector/models';
import { ComposedProperty } from '@/features/mapSideBar/property/ComposedProperty';
import { exists, formatApiAddress, pidFormatter } from '@/utils';
import { mapFeatureToProperty } from '@/utils/mapPropertyUtils';

import HistoricalNumbersContainer from '../shared/header/HistoricalNumberContainer';
import { HistoricalNumberSectionView } from '../shared/header/HistoricalNumberSectionView';

export interface IMotiInventoryHeaderProps {
  isLoading: boolean;
  composedProperty: ComposedProperty;
  onZoom?: (latitude: number, longitude: number) => void;
}

export const MotiInventoryHeader: React.FunctionComponent<IMotiInventoryHeaderProps> = props => {
  const pid = pidFormatter(props.composedProperty.pid);

  const parcelMapData = props.composedProperty.parcelMapFeatureCollection;
  const geoserverMapData = props.composedProperty.geoserverFeatureCollection;
  const apiProperty = props.composedProperty.pimsProperty;

  let property: IMapProperty | null = null;

  if (parcelMapData?.features[0]) {
    property = mapFeatureToProperty(parcelMapData?.features[0]);
  }
  const pin = props.composedProperty?.pin ?? apiProperty?.pin ?? property?.pin ?? '-';

  const isLoading = props.isLoading;

  const isRetired = React.useMemo(() => {
    if (exists(apiProperty) && apiProperty.isRetired) {
      return true;
    }
    return false;
  }, [apiProperty]);

  const isDisposed = React.useMemo(() => {
    if (
      geoserverMapData?.features?.length &&
      exists(geoserverMapData?.features[0]) &&
      geoserverMapData?.features[0]?.properties?.IS_DISPOSED
    ) {
      return true;
    }
    return false;
  }, [geoserverMapData?.features]);

  let latitude: number | null = null;
  let longitude: number | null = null;

  if (exists(apiProperty)) {
    latitude = apiProperty.latitude ?? null;
    longitude = apiProperty.longitude ?? null;
  } else if (exists(property)) {
    latitude = property.latitude ?? null;
    longitude = property.longitude ?? null;
  }

  const hasLocation = exists(longitude) && exists(latitude);

  return (
    <>
      <LoadingBackdrop show={isLoading} parentScreen={true} />
      <Row className="no-gutters">
        <Col xs="7">
          <HeaderField label="Civic Address:" labelWidth={'3'} contentWidth="9">
            {exists(apiProperty?.address) ? formatApiAddress(apiProperty!.address) : '-'}
          </HeaderField>
          <HeaderField label="Plan #:" labelWidth={'3'} contentWidth="9">
            {property?.planNumber}
          </HeaderField>
          {exists(apiProperty) && (
            <HistoricalNumbersContainer
              View={HistoricalNumberSectionView}
              propertyIds={[apiProperty?.id]}
            />
          )}
        </Col>
        <Col className="text-right">
          <StyledFiller>
            <Row className="justify-content-end">
              <HeaderLabelCol label="PID:" />
              <HeaderContentCol>{pid}</HeaderContentCol>
              <HeaderLabelCol label={'PIN:'} />
              <HeaderContentCol>{pin}</HeaderContentCol>
            </Row>
            <HeaderField label="Land parcel type:" className="justify-content-end">
              {apiProperty?.propertyType?.description}
            </HeaderField>
            {(isRetired || isDisposed) && (
              <HeaderField label="" className="justify-content-end align-items-end mt-auto">
                <PropertyStyleStatus className={isRetired ? 'retired' : 'disposed'}>
                  {isRetired && <AiOutlineExclamationCircle size={16} />}
                  {isDisposed && <HiCube size={16} />}
                  {isRetired ? 'RETIRED' : isDisposed ? 'DISPOSED' : 'UNKNOWN STATUS'}
                </PropertyStyleStatus>
              </HeaderField>
            )}
          </StyledFiller>
        </Col>
        <Col xs="auto" className="d-flex p-0 align-items-center justify-content-end">
          {hasLocation && (
            <TooltipWrapper tooltipId="property-zoom-map" tooltip="Zoom Map">
              <StyledIconButton
                variant="info"
                disabled={!props.onZoom}
                title="Zoom Map"
                onClick={() => props?.onZoom && props?.onZoom(latitude, longitude)}
              >
                <FaSearchPlus size={22} />
              </StyledIconButton>
            </TooltipWrapper>
          )}
        </Col>
      </Row>
      <StyledDivider />
    </>
  );
};

const StyledDivider = styled.div`
  margin-top: 0.5rem;
  border-bottom-style: solid;
  border-bottom-color: grey;
  border-bottom-width: 0.1rem;
`;

export const PropertyStyleStatus = styled(InlineFlexDiv)`
  text-transform: uppercase;
  color: ${props => props.theme.css.textWarningColor};
  background-color: ${props => props.theme.css.warningBackgroundColor};
  border-radius: 0.4rem;
  letter-spacing: 0.1rem;
  padding: 0.2rem 0.5rem;
  gap: 0.5rem;
  font-family: 'BCSans-Bold';
  font-size: 1.4rem;
  align-items: center;
  width: fit-content;
  &.disposed {
    color: ${props => props.theme.css.fileStatusGreyColor};
    background-color: ${props => props.theme.css.fileStatusGreyBackgroundColor};
  }
`;
