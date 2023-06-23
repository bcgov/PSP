import { useEffect, useMemo, useState } from 'react';

import { SelectOption } from '@/components/common/form';
import * as API from '@/constants/API';
import { CountryCodes } from '@/constants/countryCodes';
import useLookupCodeHelpers from '@/hooks/useLookupCodeHelpers';
import { Dictionary } from '@/interfaces/Dictionary';

const formLabelMap = new Map<string, Dictionary<string>>([
  [
    CountryCodes.Canada,
    { province: 'Province', provincePlaceholder: 'Select province', postal: 'Postal Code' },
  ],
  [CountryCodes.US, { province: 'State', provincePlaceholder: 'Select state', postal: 'Zip Code' }],
  [CountryCodes.Other, { province: '', provincePlaceholder: '', postal: 'Postal Code' }],
]);

/**
 * Hook that provides several helpers to the Address component.
 */
export default function useAddressHelpers() {
  const { getOptionsByType } = useLookupCodeHelpers();

  const countries = useMemo(() => getOptionsByType(API.COUNTRY_TYPES), [getOptionsByType]);
  const allProvinces = useMemo(() => getOptionsByType(API.PROVINCE_TYPES), [getOptionsByType]);

  const [provinces, setProvinces] = useState<SelectOption[]>([]);
  const [formLabels, setFormLabels] = useState<Dictionary<string>>({});

  const [selectedCountryId, setSelectedCountryId] = useState<string>('');
  const [selectedCountryCode, setSelectedCountryCode] = useState<string>(CountryCodes.Canada);

  // address forms default to Canadian addresses
  const defaultCountryId = useMemo(
    () => countries.find(c => c.code === CountryCodes.Canada)?.value?.toString(),
    [countries],
  );

  const otherCountryId = useMemo(
    () => countries.find(c => c.code === CountryCodes.Other)?.value?.toString(),
    [countries],
  );

  // adjust form labels and province/states based on selected country
  useEffect(() => {
    // find country code (e.g. 'CA' for currently selected country id)
    if (selectedCountryId) {
      const countryCode = countries.find(c => Number(c.value) === Number(selectedCountryId))
        ?.code as CountryCodes;
      const labels = formLabelMap.get(countryCode) ?? formLabelMap.get(CountryCodes.Canada);
      const filteredProvinces = allProvinces.filter(
        p => Number(p.parentId) === Number(selectedCountryId),
      );
      setSelectedCountryCode(countryCode);
      setFormLabels(labels as Dictionary<string>);
      setProvinces(filteredProvinces);
    }
  }, [allProvinces, countries, selectedCountryId]);

  return {
    defaultCountryId,
    otherCountryId,
    countries,
    provinces,
    formLabels,
    selectedCountryCode,
    selectedCountryId,
    setSelectedCountryId,
  };
}
