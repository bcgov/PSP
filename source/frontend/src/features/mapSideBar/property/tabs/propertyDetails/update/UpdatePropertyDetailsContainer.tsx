import axios, { AxiosError } from 'axios';
import { Formik, FormikHelpers, FormikProps } from 'formik';
import isNumber from 'lodash/isNumber';
import React, { useCallback, useEffect, useMemo, useState } from 'react';
import { toast } from 'react-toastify';

import LoadingBackdrop from '@/components/common/LoadingBackdrop';
import * as API from '@/constants/API';
import { StyledFormWrapper } from '@/features/mapSideBar/shared/styles';
import { useHistoricalNumberRepository } from '@/hooks/repositories/useHistoricalNumberRepository';
import { usePimsPropertyRepository } from '@/hooks/repositories/usePimsPropertyRepository';
import { useQueryMapLayersByLocation } from '@/hooks/repositories/useQueryMapLayersByLocation';
import { useLookupCodeHelpers } from '@/hooks/useLookupCodeHelpers';
import { useModalContext } from '@/hooks/useModalContext';
import useIsMounted from '@/hooks/util/useIsMounted';
import { IApiError } from '@/interfaces/IApiError';
import { ApiGen_Concepts_Property } from '@/models/api/generated/ApiGen_Concepts_Property';
import { exists, isValidId } from '@/utils';

import { HistoricalNumberForm, UpdatePropertyDetailsFormModel } from './models';
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
  const { id, onSuccess } = props;
  const isMounted = useIsMounted();
  const { setModalContent, setDisplayModal } = useModalContext();

  const {
    getPropertyWrapper: { execute: executeGetProperty, loading: loadingGetProperty },
    updatePropertyWrapper: { execute: executeUpdateProperty },
  } = usePimsPropertyRepository();

  const {
    getPropertyHistoricalNumbers: {
      execute: executeGetHistoricalNumbers,
      loading: loadingGetHistoricalNumbers,
    },
    updatePropertyHistoricalNumbers: { execute: executeUpdateHistoricalNumbers },
  } = useHistoricalNumberRepository();

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

  const fetchProperty = useCallback(
    async (id: number) => {
      const retrieved = await executeGetProperty(id);
      if (exists(retrieved) && isMounted()) {
        // fetch historical file numbers for the retrieved property
        const apiHistoricalNumbers = await executeGetHistoricalNumbers(id);
        // create formik form model
        const formValues = UpdatePropertyDetailsFormModel.fromApi(retrieved);
        formValues.historicalNumbers =
          apiHistoricalNumbers?.map(hn => HistoricalNumberForm.fromApi(hn)) ?? [];

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
    },
    [executeGetHistoricalNumbers, executeGetProperty, isMounted, queryAll],
  );

  useEffect(() => {
    if (isValidId(id)) {
      fetchProperty(id);
    }
  }, [fetchProperty, id]);

  // save handler - sends updated property information to backend
  const savePropertyInformation = useCallback(
    async (
      values: UpdatePropertyDetailsFormModel,
      formikHelpers: FormikHelpers<UpdatePropertyDetailsFormModel>,
    ) => {
      try {
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

        // save changes to the concept property
        const apiProperty: ApiGen_Concepts_Property = values.toApi();
        const response = await executeUpdateProperty(apiProperty);

        // update list of historical numbers for this property
        if (isValidId(response?.id)) {
          const apiHistoricalNumbers = (values.historicalNumbers ?? [])
            .filter(hn => !hn.isEmpty())
            .map(hn => hn.toApi());
          await executeUpdateHistoricalNumbers(apiProperty.id, apiHistoricalNumbers);

          formikHelpers.resetForm();
          if (typeof onSuccess === 'function') {
            onSuccess();
          }
        }
      } catch (e) {
        if (axios.isAxiosError(e)) {
          const axiosError = e as AxiosError<IApiError>;
          if (axiosError.response?.status === 409) {
            setModalContent({
              variant: 'error',
              title: 'Error',
              message: axiosError.response.data as unknown as string,
              okButtonText: 'Close',
              handleOk: () => {
                setDisplayModal(false);
                fetchProperty(id);
              },
            });
            setDisplayModal(true);
          } else {
            if (axiosError.response?.status === 400) {
              toast.error(axiosError.response.data.error);
            } else {
              toast.error('Unable to save. Please try again.');
            }
          }
        }
      } finally {
        formikHelpers.setSubmitting(false);
      }
    },
    [
      countryCA?.id,
      executeUpdateHistoricalNumbers,
      executeUpdateProperty,
      fetchProperty,
      id,
      onSuccess,
      provinceBC?.id,
      setDisplayModal,
      setModalContent,
    ],
  );

  if (loadingGetProperty || loadingGetHistoricalNumbers || !initialForm) {
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
