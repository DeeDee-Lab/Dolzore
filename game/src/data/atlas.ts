export const TILE = 16;
export const CITY_COLS = 37;
export const URBAN_COLS = 27;

const city = (row: number, col: number) => row * CITY_COLS + col;

export const CITY = {
  grass: city(25, 0),
  grassDark: city(26, 0),
  road: city(21, 9),
  roadVertical: city(21, 11),
  roadHorizontal: city(21, 13),
  sidewalk: city(21, 0),
  sidewalkTan: city(21, 3),
  plaza: city(21, 6),
  water: city(5, 27),
  waterTop: city(4, 27),
  waterBottom: city(6, 27),
  brickTop: city(4, 1),
  brickBody: city(5, 1),
  brickBottom: city(8, 1),
  stoneTop: city(4, 5),
  stoneBody: city(5, 5),
  stoneBottom: city(8, 5),
  glassTop: city(4, 16),
  glassBody: city(5, 16),
  glassBottom: city(8, 16),
  door: city(27, 20),
  treeRound: city(8, 17),
  treeCone: city(7, 17),
  bush: city(10, 17),
  bench: city(11, 11),
  lamp: city(11, 3),
  trash: city(11, 8),
  mailbox: city(12, 8),
  hydrant: city(11, 13),
  carGreen: city(13, 17),
  carGray: city(15, 17),
  carOrange: city(17, 17)
} as const;

export const urbanCharacterFrame = (variant: number, direction: 'down'|'left'|'right'|'up') => {
  const col = { down: 23, left: 24, right: 25, up: 26 }[direction];
  return variant * URBAN_COLS + col;
};
