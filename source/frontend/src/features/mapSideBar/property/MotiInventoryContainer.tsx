import { point } from '@turf/turf';
import { Geometry } from 'geojson';
import { LatLngLiteral } from 'leaflet';
import React, { useCallback, useEffect, useState } from 'react';
import { generatePath, useHistory, useRouteMatch } from 'react-router-dom';
import styled from 'styled-components';

import LotSvg from '@/assets/images/icon-lot.svg?react';
import ConfirmNavigation from '@/components/common/ConfirmNavigation';
import { useMapStateMachine } from '@/components/common/mapFSM/MapStateMachineContext';
import { PROPERTY_TYPES, useComposedProperties } from '@/hooks/repositories/useComposedProperties';
import { useQuery } from '@/hooks/use-query';
import { useFormikCancel } from '@/hooks/useFormikCancel';
import { exists, firstOrNull, pinParser } from '@/utils';

import MapSideBarLayout from '../layout/MapSideBarLayout';
import SidebarFooter from '../shared/SidebarFooter';
import { MotiInventoryHeader } from './MotiInventoryHeader';
import PropertyRouter from './PropertyRouter';

export interface IMotiInventoryContainerProps {
  id?: number;
  pid?: string;
  pin?: string;
  location?: LatLngLiteral;
  onClose: () => void;
}

/**
 * container responsible for logic related to map sidebar display. Synchronizes the state of the parcel detail forms with the corresponding query parameters (push/pull).
 */
export const MotiInventoryContainer: React.FunctionComponent<
  React.PropsWithChildren<IMotiInventoryContainerProps>
> = props => {
  const query = useQuery();
  const { push } = useHistory();
  const match = useRouteMatch();
  const tabMatch = useRouteMatch<{ tab: string; propertyId: string }>(`${match.path}/:tab`);
  const isEditing = query.get('edit') === 'true';
  const [isValid, setIsValid] = useState<boolean>(true);

  const mapMachine = useMapStateMachine();
  const selectedFeatureData = mapMachine.mapLocationFeatureDataset;

  const { formikRef, handleCancelClick } = useFormikCancel<any>();
  let boundary: Geometry = null;
  if (exists(props.id)) {
    boundary = firstOrNull(selectedFeatureData?.pimsFeatures)?.geometry;
  } else if (exists(props.pid || props.pin)) {
    boundary = firstOrNull(selectedFeatureData?.parcelFeatures)?.geometry;
  } else if (exists(props.location?.lng) && exists(props.location?.lat)) {
    boundary = point([props.location?.lng, props.location?.lat])?.geometry;
  }
  const composedPropertyState = useComposedProperties({
    id: props.id,
    pid:
      props?.pid === undefined || props?.pid === '' || isNaN(Number(props.pid))
        ? undefined
        : Number(props.pid),
    pin: pinParser(props?.pin),
    boundary: boundary,
    propertyTypes: propertyTabData,
  });

  useEffect(() => {
    push({ search: '' });
  }, [props.pid, props.pin, push]);

  const onSuccess = () => {
    props.id && composedPropertyState.apiWrapper?.execute(props.id);
    stripEditFromPath();
  };

  const handleSaveClick = async () => {
    if (formikRef !== undefined) {
      formikRef.current?.setSubmitting(true);
      await formikRef.current?.submitForm();
      setIsValid(formikRef.current?.isValid ?? false);
    }
  };

  const stripEditFromPath = useCallback(() => {
    if (!tabMatch) {
      return;
    }
    const path = generatePath('/mapview/sidebar/property/:propertyId/:tab', {
      propertyId: tabMatch?.params.propertyId,
      tab: tabMatch?.params.tab,
    });
    push(path, { search: query.toString() });
  }, [tabMatch, push, query]);

  const handleCancel = useCallback(() => {
    handleCancelClick(() => stripEditFromPath());
  }, [handleCancelClick, stripEditFromPath]);

  const shouldBlockNavigation = useCallback(() => {
    const current = formikRef.current;
    return !!current && current.dirty && !current.isSubmitting;
  }, [formikRef]);

  return (
    <MapSideBarLayout
      title="Property Information"
      header={
        <MotiInventoryHeader
          composedProperty={composedPropertyState.composedProperty}
          isLoading={composedPropertyState.apiWrapper?.loading}
        />
      }
      footer={
        isEditing && (
          <SidebarFooter
            isOkDisabled={formikRef?.current?.isSubmitting}
            onSave={handleSaveClick}
            onCancel={handleCancel}
            displayRequiredFieldError={!isValid}
          />
        )
      }
      icon={<LotIcon className="mr-1" />}
      showCloseButton
      onClose={props.onClose}
    >
      <PropertyRouter
        composedPropertyState={composedPropertyState}
        onSuccess={onSuccess}
        ref={formikRef}
      />
      <ConfirmNavigation navigate={push} shouldBlockNavigation={shouldBlockNavigation} />
    </MapSideBarLayout>
  );
};

export default MotiInventoryContainer;

const LotIcon = styled(LotSvg)`
  width: 3rem;
  height: 3rem;
`;

const propertyTabData = [
  PROPERTY_TYPES.ASSOCIATIONS,
  PROPERTY_TYPES.LTSA,
  PROPERTY_TYPES.PIMS_API,
  PROPERTY_TYPES.BC_ASSESSMENT,
  PROPERTY_TYPES.PARCEL_MAP,
  PROPERTY_TYPES.PIMS_GEOSERVER,
  PROPERTY_TYPES.CROWN_TENURES,
  PROPERTY_TYPES.CROWN_INCLUSIONS,
  PROPERTY_TYPES.CROWN_INVENTORY,
  PROPERTY_TYPES.CROWN_LEASES,
  PROPERTY_TYPES.CROWN_LICENSES,
  PROPERTY_TYPES.CROWN_SURVEYS,
  PROPERTY_TYPES.MUNICIPALITY,
  PROPERTY_TYPES.HIGHWAYS,
  PROPERTY_TYPES.ALR,
  PROPERTY_TYPES.ELECTORAL,
  PROPERTY_TYPES.FIRST_NATION,
];
