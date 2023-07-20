import convert, { Area, Volume } from 'convert';

import { AreaUnitTypes } from '@/constants/areaUnitTypes';
import { VolumeUnitTypes } from '@/constants/volumeUnitTypes';

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

export function getAreaUnit(unit: string): Area {
  switch (unit) {
    case AreaUnitTypes.Acres:
      return 'ac';
    case AreaUnitTypes.Hectares:
      return 'ha';
    case AreaUnitTypes.SquareFeet:
      return 'sq ft';
    case AreaUnitTypes.SquareMeters:
    default:
      return 'm2';
  }
}

export function getVolumeUnit(unit: string): Volume {
  switch (unit) {
    case VolumeUnitTypes.CubicFeet:
      return 'ft3';
    case VolumeUnitTypes.CubicMeters:
    default:
      return 'm3';
  }
}
