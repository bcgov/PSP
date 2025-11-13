import convert, { Volume } from 'convert';

import { VolumeUnitTypes } from '@/constants/volumeUnitTypes';
import { ApiGen_CodeTypes_AreaUnitTypes } from '@/models/api/generated/ApiGen_CodeTypes_AreaUnitTypes';

import { exists } from './utils';

export function getVolumeUnit(unit: string): Volume {
  switch (unit) {
    case VolumeUnitTypes.CubicFeet:
      return 'ft3';
    case VolumeUnitTypes.CubicMeters:
    default:
      return 'm3';
  }
}

export function convertArea(value: number, from: string, to: string): number {
  const _from = getAreaUnit(from);
  const _to = getAreaUnit(to);
  return convert(value, _from).to(_to);
}

export function convertVolume(value: number, from: string, to: string): number {
  const _from = getVolumeUnit(from);
  const _to = getVolumeUnit(to);
  return convert(value, _from).to(_to);
}

export function getAreaUnit(unit: string | null | undefined): string {
  if (!exists(unit)) {
    return 'm2';
  }

  switch (unit) {
    case ApiGen_CodeTypes_AreaUnitTypes.ACRE:
      return 'ac';
    case ApiGen_CodeTypes_AreaUnitTypes.HA:
      return 'ha';
    case ApiGen_CodeTypes_AreaUnitTypes.FEET2:
      return 'sq ft';
    case ApiGen_CodeTypes_AreaUnitTypes.M2:
    default:
      return 'm2';
  }
}
