import { IWfsCqlFlags } from './useWfsLayer';

/*
 * OR all object keys together within the generated cql filter string.
 *
 * The resulting filter is URL-encoded
 * @param object an object to convert to a cql filter string.
 */
export const toCqlFilter = (object: Record<string, any>, forceExactMatch?: boolean) => {
  const cqlValue: string = toCqlFilterValue(object, {
    forceExactMatch: forceExactMatch,
    useCqlOr: true,
  });
  return cqlValue.length ? `cql_filter=${encodeURIComponent(cqlValue)}` : '';
};

/**
 * Convert any object to a cql filter string value, assuming the object's keys should be used as CQL filter properties.
 * NOTE: The resulting string value is left as-is. It will not be URL-encoded
 * @param object an object to convert to a cql filter string.
 */
export const toCqlFilterValue = (object: Record<string, string>, flags?: IWfsCqlFlags) => {
  const cql: string[] = [];
  Object.keys(object).forEach((key: string) => {
    if (object[key]) {
      if (
        (key === 'PID' || key === 'PID_PADDED') &&
        (object[key]?.length === 9 || flags?.forceExactMatch)
      ) {
        cql.push(`${key} = '${object[key]}'`);
      } else if (key === 'PIN' && flags?.forceExactMatch) {
        cql.push(`${key}='${object[key].replace(/[^0-9]/g, '')}'`);
      } else {
        cql.push(`${key} ilike '%${object[key]}%'`);
      }
    }
  });

  return cql.length > 0 ? (flags?.useCqlOr ? cql.join(' OR ') : cql.join(' AND ')) : '';
};
