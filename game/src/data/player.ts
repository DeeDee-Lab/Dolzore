export type PlayerStats = {
  name: string;
  level: number;
  job: string;
  heart: number;
  maxHeart: number;
  focus: number;
  maxFocus: number;
  power: number;
  guard: number;
  speed: number;
  sense: number;
  luck: number;
};

export const DEFAULT_PLAYER: PlayerStats = {
  name: 'SORA',
  level: 1,
  job: 'WANDERER',
  heart: 120,
  maxHeart: 120,
  focus: 60,
  maxFocus: 60,
  power: 8,
  guard: 7,
  speed: 10,
  sense: 12,
  luck: 6
};
