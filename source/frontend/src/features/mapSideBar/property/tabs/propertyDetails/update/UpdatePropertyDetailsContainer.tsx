import { Formik, FormikHelpers, FormikProps } from 'formik';
import isNumber from 'lodash/isNumber';
import React, { useEffect, useMemo, useState } from 'react';
import styled from 'styled-components';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import * as API from '@/constants/API';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { useQueryMapLayersByLocation } from '@/hooks/repositories/useQueryMapLayersByLocation';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import useIsMounted from '@/hooks/util/useIsMounted';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { isValidId } from '@/utils';

import { UpdatePropertyDetailsFormModel } from './models';
import { UpdatePropertyDetailsForm } from './UpdatePropertyDetailsForm';
import { UpdatePropertyDetailsYupSchema } from './validation';

export interface IUpdatePropertyDetailsContainerProps {
  id: number;
  onSuccess: () => void;
}

export const UpdatePropertyDetailsContainer = React.forwardRef<
  FormikProps<any>,
  IUpdatePropertyDetailsContainerProps
>((props, ref) => {
  const isMounted = useIsMounted();

  const { getPropertyWrapper, updatePropertyWrapper } = usePimsPropertyRepository();
  const executeGetProperty = getPropertyWrapper.execute;

  const { queryAll } = useQueryMapLayersByLocation();

  const [initialForm, setForm] = useState<UpdatePropertyDetailsFormModel | undefined>(undefined);

  // Lookup codes
  const { getByType } = useLookupCodeHelpers();
  const countryCA = useMemo(
    () => getByType(API.COUNTRY_TYPES).find(c => c.code === 'CA'),
    [getByType],
  );
  const provinceBC = useMemo(
    () => getByType(API.PROVINCE_TYPES).find(p => p.code === 'BC'),
    [getByType],
  );

  useEffect(() => {
    async function fetchProperty() {
      if (isValidId(props.id)) {
        const retrieved = await executeGetProperty(props.id);
        if (retrieved !== undefined && isMounted()) {
          const formValues = UpdatePropertyDetailsFormModel.fromApi(retrieved);

          // This triggers API calls to DataBC map layers
          if (isNumber(retrieved.latitude) && isNumber(retrieved.longitude)) {
            const layers = await queryAll({ lat: retrieved.latitude, lng: retrieved.longitude });
            formValues.isALR = !!layers.isALR;
            formValues.motiRegion = layers.motiRegion;
            formValues.highwaysDistrict = layers.highwaysDistrict;
            formValues.electoralDistrict = layers.electoralDistrict;
            formValues.firstNations = layers.firstNations;
          }

          setForm(formValues);
        }
      }
    }
    fetchProperty();
  }, [isMounted, props.id, queryAll, executeGetProperty]);

  // save handler - sends updated property information to backend
  const savePropertyInformation = async (
    values: UpdatePropertyDetailsFormModel,
    formikHelpers: FormikHelpers<UpdatePropertyDetailsFormModel>,
  ) => {
    // default province and country to BC, Canada
    if (values.address !== undefined) {
      values.address.province = {
        id: Number(provinceBC?.id),
        code: null,
        description: null,
        displayOrder: null,
      };
      values.address.country = {
        id: Number(countryCA?.id),
        code: null,
        description: null,
        displayOrder: null,
      };
    }

    const apiProperty: ApiGen_Concepts_Property = values.toApi();
    const response = await updatePropertyWrapper.execute(apiProperty);

    formikHelpers.setSubmitting(false);

    if (isValidId(response?.id)) {
      formikHelpers.resetForm();
      props.onSuccess();
    }
  };

  if (getPropertyWrapper?.loading || !initialForm) {
    return <LoadingBackdrop show={true} parentScreen={true}></LoadingBackdrop>;
  }

  return (
    <Formik<UpdatePropertyDetailsFormModel>
      enableReinitialize
      innerRef={ref}
      validationSchema={UpdatePropertyDetailsYupSchema}
      initialValues={initialForm || new UpdatePropertyDetailsFormModel()}
      onSubmit={savePropertyInformation}
    >
      {formikProps => (
        <StyledFormWrapper>
          <UpdatePropertyDetailsForm formikProps={formikProps} />
        </StyledFormWrapper>
      )}
    </Formik>
  );
});

const StyledFormWrapper = styled.div`
  display: flex;
  flex-direction: column;
  flex-grow: 1;
  text-align: left;
  height: 100%;
  overflow-y: auto;
  padding-right: 1rem;
  padding-bottom: 1rem;
`;
