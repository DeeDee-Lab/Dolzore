import Phaser from 'phaser';
import './style.css';
import { TownScene } from './scenes/TownScene';

const config: Phaser.Types.Core.GameConfig = {
  type: Phaser.AUTO,
  parent: 'game-root',
  width: 640,
  height: 360,
  backgroundColor: '#111018',
  pixelArt: true,
  antialias: false,
  roundPixels: true,
  physics: {
    default: 'arcade',
    arcade: { gravity: { x: 0, y: 0 }, debug: false }
  },
  scale: {
    mode: Phaser.Scale.FIT,
    autoCenter: Phaser.Scale.CENTER_BOTH,
    width: 640,
    height: 360
  },
  scene: [TownScene]
};

new Phaser.Game(config);
