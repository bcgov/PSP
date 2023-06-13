/**
 * Convert any object to a cql filter string, assuming the object's keys should be used as CQL filter properties.
 * AND all object keys together within the generated cql filter string.
 *
 * The resulting filter is URL-encoded
 * @param object an object to convert to a cql filter string.
 */
export const toCqlFilter = (
  object: Record<string, any>,
  pidOverride?: boolean,
  forcePerfectMatch?: boolean,
) => {
  const cqlValue: string = toCqlFilterValue(object, pidOverride, forcePerfectMatch);
  return cqlValue.length ? `cql_filter=${encodeURIComponent(cqlValue)}` : '';
};

/**
 * Convert any object to a cql filter string value, assuming the object's keys should be used as CQL filter properties.
 * NOTE: The resulting string value is left as-is. It will not be URL-encoded
 * @param object an object to convert to a cql filter string.
 */
export const toCqlFilterValue = (
  object: Record<string, string>,
  forceSimplePid?: boolean,
  forceExactMatch?: boolean,
) => {
  const cql: string[] = [];
  Object.keys(object).forEach((key: string) => {
    if (object[key]) {
      if (
        ((key === 'PID' || key === 'PID_PADDED') && object[key]?.length === 9) ||
        forceExactMatch
      ) {
        cql.push(`${key} = '${object[key]}'`);
      } else if ((key === 'PID' || key === 'PID_PADDED') && object[key] && !forceSimplePid) {
        cql.push(
          `PIN ilike '%${object[key]}%' OR PID ilike '%${object[key]}%' OR PID_PADDED ilike '%${object[key]}%'`,
        );
      } else {
        cql.push(`${key} ilike '%${object[key]}%'`);
      }
    }
  });

  return cql.length ? (forceExactMatch ? cql.join(' OR ') : cql.join(' AND ')) : '';
};
