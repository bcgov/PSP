import { useState } from 'react';
import { toast } from 'react-toastify';

import { useApiAutocomplete } from '@/hooks/pims-api/useApiAutocomplete';
import { IAutocompletePrediction } from '@/interfaces';

interface IPersonOrganizationTypeahead {
  handleTypeaheadSearch: (query: string) => Promise<void>;
  isTypeaheadLoading: boolean;
  matchedOrgs: IAutocompletePrediction[];
}

export const usePersonOrganizationTypeahead = (): IPersonOrganizationTypeahead => {
  // organization type-ahead state
  const { getOrganizationPredictions } = useApiAutocomplete();
  const [isTypeaheadLoading, setIsTypeaheadLoading] = useState(false);
  const [matchedOrgs, setMatchedOrgs] = useState<IAutocompletePrediction[]>([]);

  // fetch autocomplete suggestions from server
  const handleTypeaheadSearch = async (query: string) => {
    try {
      setIsTypeaheadLoading(true);
      const { data } = await getOrganizationPredictions(query);
      setMatchedOrgs(data.predictions);
      setIsTypeaheadLoading(false);
    } catch (e) {
      setMatchedOrgs([]);
      toast.error('Failed to get autocomplete results for supplied organization', {
        autoClose: 7000,
      });
    } finally {
      setIsTypeaheadLoading(false);
    }
  };

  return { handleTypeaheadSearch, isTypeaheadLoading, matchedOrgs };
};
