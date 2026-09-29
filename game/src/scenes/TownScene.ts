import Phaser from 'phaser';
import { CITY, TILE, urbanCharacterFrame } from '../data/atlas';
import { DEFAULT_PLAYER } from '../data/player';
import { NPC_DIALOGUE } from '../data/dialogue';

type TiledProperty = { name: string; type: string; value: string | number | boolean };
type MapObject = {
  id: number;
  name: string;
  type: string;
  x: number;
  y: number;
  width?: number;
  height?: number;
  point?: boolean;
  properties?: TiledProperty[];
};
type ObjectLayer = { name: string; objects: MapObject[] };
type TownMap = { width: number; height: number; tilewidth: number; tileheight: number; layers: ObjectLayer[] };
type Direction = 'down' | 'left' | 'right' | 'up';
type NpcRuntime = { object: MapObject; sprite: Phaser.GameObjects.Sprite; label: Phaser.GameObjects.Text; line: number };

const WORLD_W = 1280;
const WORLD_H = 960;

const prop = (object: MapObject, key: string, fallback = '') => {
  const value = object.properties?.find((p) => p.name === key)?.value;
  return value === undefined ? fallback : String(value);
};

export class TownScene extends Phaser.Scene {
  private playerBody!: Phaser.Physics.Arcade.Sprite;
  private playerSprite!: Phaser.GameObjects.Sprite;
  private playerShadow!: Phaser.GameObjects.Ellipse;
  private cursorKeys!: Phaser.Types.Input.Keyboard.CursorKeys;
  private wasd!: Record<'W'|'A'|'S'|'D', Phaser.Input.Keyboard.Key>;
  private interactKey!: Phaser.Input.Keyboard.Key;
  private enterKey!: Phaser.Input.Keyboard.Key;
  private jumpKey!: Phaser.Input.Keyboard.Key;
  private mapKey!: Phaser.Input.Keyboard.Key;
  private statusKey!: Phaser.Input.Keyboard.Key;
  private escKey!: Phaser.Input.Keyboard.Key;
  private mapCamera!: Phaser.Cameras.Scene2D.Camera;
  private miniCamera!: Phaser.Cameras.Scene2D.Camera;
  private mapOpen = false;
  private statusOpen = false;
  private jumpStarted = 0;
  private jumpOffset = 0;
  private facing: Direction = 'down';
  private districts: MapObject[] = [];
  private gameplay: MapObject[] = [];
  private npcs: NpcRuntime[] = [];
  private hudObjects: Phaser.GameObjects.GameObject[] = [];
  private zoneText!: Phaser.GameObjects.Text;
  private promptText!: Phaser.GameObjects.Text;
  private dialogueBox!: Phaser.GameObjects.Rectangle;
  private dialogueName!: Phaser.GameObjects.Text;
  private dialogueBody!: Phaser.GameObjects.Text;
  private statusObjects: Phaser.GameObjects.GameObject[] = [];
  private mobileDirections = new Set<Direction>();
  private cleanupMobile: Array<() => void> = [];

  constructor() {
    super('TownScene');
  }

  preload() {
    this.load.spritesheet('city', 'assets/kenney/tiles/city_tilemap.png', {
      frameWidth: 16,
      frameHeight: 16
    });
    this.load.spritesheet('urban', 'assets/kenney/urban/urban_tilemap.png', {
      frameWidth: 16,
      frameHeight: 16
    });
    this.load.json('first-town', 'maps/first-town.tmj');
  }

  create() {
    const map = this.cache.json.get('first-town') as TownMap;
    const layer = (name: string) => map.layers.find((l) => l.name === name)?.objects ?? [];

    this.districts = layer('Districts');
    this.gameplay = layer('Gameplay');

    this.physics.world.setBounds(0, 0, WORLD_W, WORLD_H);
    this.createTerrain(layer('Roads'), layer('Water'));
    this.createBuildings(layer('Buildings'));
    this.createCollisions(layer('Collisions'));
    this.createProps();
    this.createNpcs(layer('NPCs'));

    const spawn = layer('Gameplay').find((o) => o.type === 'spawn') ?? { x: 288, y: 288 } as MapObject;
    this.createPlayer(spawn.x, spawn.y);
    this.createInput();
    this.createHud();
    this.createCameras();
    this.bindMobileControls();

    this.showDialogue('DOLZORE', '最初の街。目的地を決めずに歩いてもいい。違和感があれば、覚えておこう。');

    const debugWindow = window as Window & {
      __DOLZORE_GAME__?: {
        ready: boolean;
        state: () => {
          x: number; y: number; zone: string; mapOpen: boolean; statusOpen: boolean;
          worldWidth: number; worldHeight: number;
        };
        teleport: (x: number, y: number) => void;
        openMap: () => void;
        openStatus: () => void;
      }
    };
    debugWindow.__DOLZORE_GAME__ = {
      ready: true,
      state: () => ({
        x: this.playerBody.x,
        y: this.playerBody.y,
        zone: this.zoneText.text,
        mapOpen: this.mapOpen,
        statusOpen: this.statusOpen,
        worldWidth: WORLD_W,
        worldHeight: WORLD_H
      }),
      teleport: (x: number, y: number) => {
        this.playerBody.setPosition(x, y);
        this.syncPlayerVisuals();
        this.cameras.main.centerOn(x, y);
        this.updateZone();
      },
      openMap: () => { if (!this.mapOpen) this.toggleMap(); },
      openStatus: () => { if (!this.statusOpen) this.toggleStatus(); }
    };

    this.events.once(Phaser.Scenes.Events.SHUTDOWN, () => {
      this.cleanupMobile.forEach((fn) => fn());
      delete debugWindow.__DOLZORE_GAME__;
    });
  }

  private createTerrain(roads: MapObject[], water: MapObject[]) {
    const rt = this.add.renderTexture(0, 0, WORLD_W, WORLD_H).setOrigin(0).setDepth(-100);

    for (let y = 0; y < WORLD_H; y += TILE) {
      for (let x = 0; x < WORLD_W; x += TILE) {
        const frame = ((x / TILE + y / TILE) % 13 === 0) ? CITY.grassDark : CITY.grass;
        rt.drawFrame('city', frame, x, y);
      }
    }

    for (const road of roads) {
      this.paintRect(rt, road, CITY.road);
      const horizontal = (road.width ?? 0) > (road.height ?? 0);
      this.paintRoadEdges(rt, road);
      if (horizontal) {
        const y = Math.floor((road.y + (road.height ?? 0) / 2) / TILE) * TILE;
        for (let x = road.x; x < road.x + (road.width ?? 0); x += TILE) rt.drawFrame('city', CITY.roadHorizontal, x, y);
      } else {
        const x = Math.floor((road.x + (road.width ?? 0) / 2) / TILE) * TILE;
        for (let y = road.y; y < road.y + (road.height ?? 0); y += TILE) rt.drawFrame('city', CITY.roadVertical, x, y);
      }
    }

    for (const object of water) {
      if (object.type === 'water') this.paintRect(rt, object, CITY.water);
      if (object.type === 'bridge') this.paintRect(rt, object, CITY.plaza);
    }
  }

  private paintRect(rt: Phaser.GameObjects.RenderTexture, object: MapObject, frame: number) {
    const width = object.width ?? TILE;
    const height = object.height ?? TILE;
    for (let y = object.y; y < object.y + height; y += TILE) {
      for (let x = object.x; x < object.x + width; x += TILE) rt.drawFrame('city', frame, x, y);
    }
  }

  private paintRoadEdges(rt: Phaser.GameObjects.RenderTexture, object: MapObject) {
    const width = object.width ?? TILE;
    const height = object.height ?? TILE;
    for (let x = object.x; x < object.x + width; x += TILE) {
      rt.drawFrame('city', CITY.sidewalkTan, x, object.y);
      rt.drawFrame('city', CITY.sidewalkTan, x, object.y + height - TILE);
    }
    for (let y = object.y; y < object.y + height; y += TILE) {
      rt.drawFrame('city', CITY.sidewalkTan, object.x, y);
      rt.drawFrame('city', CITY.sidewalkTan, object.x + width - TILE, y);
    }
  }

  private createBuildings(buildings: MapObject[]) {
    for (const building of buildings) this.createBuilding(building);
  }

  private createBuilding(building: MapObject) {
    const width = building.width ?? 96;
    const height = building.height ?? 80;
    const style = prop(building, 'style', 'brick');
    const important = prop(building, 'important', 'false') === 'true';
    const frames = style === 'stone'
      ? [CITY.stoneTop, CITY.stoneBody, CITY.stoneBottom]
      : style === 'glass'
        ? [CITY.glassTop, CITY.glassBody, CITY.glassBottom]
        : [CITY.brickTop, CITY.brickBody, CITY.brickBottom];

    const shadow = this.add.rectangle(
      building.x + width / 2 + 6,
      building.y + height / 2 + 7,
      width,
      height,
      0x111018,
      0.23
    ).setDepth(building.y + height - 2);

    for (let y = 0; y < height; y += TILE) {
      const rowFrame = y === 0 ? frames[0] : y >= height - TILE ? frames[2] : frames[1];
      for (let x = 0; x < width; x += TILE) {
        this.add.image(building.x + x + 8, building.y + y + 8, 'city', rowFrame)
          .setDepth(building.y + height);
      }
    }

    const windowFrame = CITY.glassBody;
    for (const wx of [building.x + 24, building.x + width - 24]) {
      this.add.image(wx, building.y + Math.min(48, height / 2), 'city', windowFrame)
        .setScale(1.3)
        .setDepth(building.y + height + 1);
    }

    this.add.image(building.x + width / 2, building.y + height - 8, 'city', CITY.door)
      .setDepth(building.y + height + 2);

    const sign = prop(building, 'sign');
    if (sign) {
      const signText = this.add.text(building.x + width / 2, building.y + 14, sign, {
        fontFamily: 'Arial, sans-serif',
        fontSize: important ? '11px' : '9px',
        fontStyle: 'bold',
        color: important ? '#201b25' : '#f7edcf',
        backgroundColor: important ? '#e9bd53' : '#28232d',
        padding: { x: 6, y: 3 }
      }).setOrigin(0.5).setDepth(building.y + height + 3);
      signText.setStroke(important ? '#f8e8b0' : '#151219', 1);
    }

    shadow.setData('building', building.name);
  }

  private createCollisions(collisions: MapObject[]) {
    for (const object of collisions) {
      const width = object.width ?? TILE;
      const height = object.height ?? TILE;
      const zone = this.add.zone(object.x + width / 2, object.y + height / 2, width, height);
      this.physics.add.existing(zone, true);
      this.physics.add.collider(this.playerBody, zone);
      zone.setData('collision', object.name);
    }
  }

  private createProps() {
    const props: Array<[number, number, number, number]> = [
      [CITY.treeRound, 64, 192, 2], [CITY.treeCone, 432, 128, 2], [CITY.treeRound, 448, 464, 2],
      [CITY.treeRound, 224, 688, 2], [CITY.treeCone, 624, 744, 2], [CITY.treeRound, 880, 736, 2],
      [CITY.bench, 608, 488, 1.5], [CITY.bench, 512, 720, 1.5], [CITY.bench, 1144, 704, 1.5],
      [CITY.lamp, 448, 336, 1.4], [CITY.lamp, 656, 336, 1.4], [CITY.lamp, 1008, 336, 1.4],
      [CITY.mailbox, 240, 304, 1.2], [CITY.trash, 744, 432, 1.2], [CITY.hydrant, 928, 448, 1.2],
      [CITY.carGreen, 384, 392, 1.8], [CITY.carGray, 1040, 400, 1.8], [CITY.carOrange, 832, 664, 1.8]
    ];
    for (const [frame, x, y, scale] of props) {
      this.add.image(x, y, 'city', frame).setScale(scale).setDepth(y);
    }

    const fountain = this.add.circle(664, 552, 26, 0x4b9caf, 0.75).setDepth(550);
    fountain.setStrokeStyle(5, 0xd7c69a, 1);
    this.add.circle(664, 552, 8, 0xe4d9b9, 1).setDepth(551);
  }

  private createNpcs(npcObjects: MapObject[]) {
    for (const object of npcObjects) {
      const variant = Number(prop(object, 'variant', '4'));
      const sprite = this.add.sprite(object.x, object.y, 'urban', urbanCharacterFrame(variant, 'down'))
        .setScale(2)
        .setDepth(object.y);
      const label = this.add.text(object.x, object.y - 22, object.name, {
        fontFamily: 'Arial, sans-serif',
        fontSize: '8px',
        color: '#fff2cf',
        backgroundColor: '#17141dcc',
        padding: { x: 3, y: 1 }
      }).setOrigin(0.5).setDepth(object.y + 1);
      this.npcs.push({ object, sprite, label, line: 0 });
    }
  }

  private createPlayer(x: number, y: number) {
    const graphics = this.make.graphics({ x: 0, y: 0, add: false });
    graphics.fillStyle(0xffffff, 1).fillRect(0, 0, 12, 8).generateTexture('player-body', 12, 8);
    graphics.destroy();

    this.playerBody = this.physics.add.sprite(x, y, 'player-body').setVisible(false).setCollideWorldBounds(true);
    this.playerBody.setSize(12, 8);

    this.playerShadow = this.add.ellipse(x, y + 6, 22, 8, 0x141118, 0.34).setDepth(y - 1);
    this.playerSprite = this.add.sprite(x, y - 8, 'urban', urbanCharacterFrame(8, 'down'))
      .setScale(2)
      .setDepth(y);
  }

  private createInput() {
    const keyboard = this.input.keyboard;
    if (!keyboard) throw new Error('Keyboard input unavailable');
    this.cursorKeys = keyboard.createCursorKeys();
    this.wasd = keyboard.addKeys('W,A,S,D') as Record<'W'|'A'|'S'|'D', Phaser.Input.Keyboard.Key>;
    this.interactKey = keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.E);
    this.enterKey = keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.ENTER);
    this.jumpKey = keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.SPACE);
    this.mapKey = keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.M);
    this.statusKey = keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.C);
    this.escKey = keyboard.addKey(Phaser.Input.Keyboard.KeyCodes.ESC);
  }

  private createHud() {
    const panel = this.add.rectangle(12, 12, 258, 62, 0x121019, 0.91)
      .setOrigin(0).setScrollFactor(0).setDepth(1000).setStrokeStyle(1, 0xe2bd68, 1);
    const name = this.add.text(24, 20, `${DEFAULT_PLAYER.name}  LV ${DEFAULT_PLAYER.level}  ${DEFAULT_PLAYER.job}`, {
      fontFamily: 'Arial, sans-serif', fontSize: '12px', fontStyle: 'bold', color: '#fff1c9'
    }).setScrollFactor(0).setDepth(1001);
    const heartLabel = this.add.text(24, 40, 'HEART', { fontFamily: 'monospace', fontSize: '9px', color: '#e76d75' })
      .setScrollFactor(0).setDepth(1001);
    const focusLabel = this.add.text(24, 58, 'FOCUS', { fontFamily: 'monospace', fontSize: '9px', color: '#68b7c7' })
      .setScrollFactor(0).setDepth(1001);
    const heartBg = this.add.rectangle(70, 44, 178, 7, 0x3a2630, 1).setOrigin(0).setScrollFactor(0).setDepth(1001);
    const heart = this.add.rectangle(70, 44, 178, 7, 0xc95b67, 1).setOrigin(0).setScrollFactor(0).setDepth(1002);
    const focusBg = this.add.rectangle(70, 62, 178, 7, 0x203640, 1).setOrigin(0).setScrollFactor(0).setDepth(1001);
    const focus = this.add.rectangle(70, 62, 178, 7, 0x4c9eb2, 1).setOrigin(0).setScrollFactor(0).setDepth(1002);

    this.zoneText = this.add.text(320, 18, 'RESIDENTIAL RIDGE', {
      fontFamily: 'Arial, sans-serif', fontSize: '11px', fontStyle: 'bold', color: '#fff1c9',
      backgroundColor: '#15121bd9', padding: { x: 8, y: 4 }
    }).setOrigin(0.5, 0).setScrollFactor(0).setDepth(1001);

    const miniFrame = this.add.rectangle(472, 10, 158, 104, 0x0f0d14, 0.94)
      .setOrigin(0).setScrollFactor(0).setDepth(1000).setStrokeStyle(2, 0xd6b45d, 1);
    const miniTitle = this.add.text(480, 16, 'MAP  [M]', {
      fontFamily: 'monospace', fontSize: '8px', color: '#f2dfa9'
    }).setScrollFactor(0).setDepth(1001);

    this.promptText = this.add.text(626, 326, '', {
      fontFamily: 'Arial, sans-serif', fontSize: '10px', fontStyle: 'bold', color: '#fff3ce',
      backgroundColor: '#191621e8', padding: { x: 7, y: 4 }
    }).setOrigin(1, 1).setScrollFactor(0).setDepth(1002).setVisible(false);

    this.dialogueBox = this.add.rectangle(16, 280, 416, 64, 0x121019, 0.94)
      .setOrigin(0).setScrollFactor(0).setDepth(1000).setStrokeStyle(2, 0xe2d4aa, 1);
    this.dialogueName = this.add.text(30, 290, 'DOLZORE', {
      fontFamily: 'monospace', fontSize: '9px', fontStyle: 'bold', color: '#e2bd68'
    }).setScrollFactor(0).setDepth(1001);
    this.dialogueBody = this.add.text(30, 307, '', {
      fontFamily: 'Arial, sans-serif', fontSize: '11px', color: '#fff7df',
      wordWrap: { width: 388 }, lineSpacing: 4
    }).setScrollFactor(0).setDepth(1001);

    const help = this.add.text(626, 346, 'WASD / SPACE JUMP / E TALK / M MAP / C STATUS', {
      fontFamily: 'monospace', fontSize: '7px', color: '#c8bea6'
    }).setOrigin(1, 1).setScrollFactor(0).setDepth(1001);

    this.hudObjects = [
      panel,name,heartLabel,focusLabel,heartBg,heart,focusBg,focus,miniFrame,miniTitle,
      this.zoneText,this.promptText,this.dialogueBox,this.dialogueName,this.dialogueBody,help
    ];

    this.createStatusPanel();
  }

  private createStatusPanel() {
    const bg = this.add.rectangle(320, 180, 360, 250, 0x121019, 0.98)
      .setScrollFactor(0).setDepth(1100).setStrokeStyle(2, 0xe2bd68, 1);
    const title = this.add.text(160, 72, 'STATUS', {
      fontFamily: 'Arial, sans-serif', fontSize: '18px', fontStyle: 'bold', color: '#fff0c8'
    }).setScrollFactor(0).setDepth(1101);
    const lines = [
      `${DEFAULT_PLAYER.name}  /  LV ${DEFAULT_PLAYER.level}  /  ${DEFAULT_PLAYER.job}`,
      `HEART  ${DEFAULT_PLAYER.heart} / ${DEFAULT_PLAYER.maxHeart}`,
      `FOCUS  ${DEFAULT_PLAYER.focus} / ${DEFAULT_PLAYER.maxFocus}`,
      '',
      `POWER   ${DEFAULT_PLAYER.power}`,
      `GUARD   ${DEFAULT_PLAYER.guard}`,
      `SPEED   ${DEFAULT_PLAYER.speed}`,
      `SENSE   ${DEFAULT_PLAYER.sense}`,
      `LUCK    ${DEFAULT_PLAYER.luck}`,
      '',
      'EQUIPMENT',
      'BODY   Mint Jacket',
      'SHOES  Canvas Sneakers',
      'CHARM  —',
      'TOOL   Field Pouch'
    ].join('\n');
    const body = this.add.text(160, 104, lines, {
      fontFamily: 'monospace', fontSize: '11px', color: '#e8dfc8', lineSpacing: 4
    }).setScrollFactor(0).setDepth(1101);
    const hint = this.add.text(480, 294, 'C / ESC CLOSE', {
      fontFamily: 'monospace', fontSize: '8px', color: '#d6b45d'
    }).setOrigin(1, 1).setScrollFactor(0).setDepth(1101);
    this.statusObjects = [bg,title,body,hint];
    this.statusObjects.forEach((o) => o.setVisible(false));
    this.hudObjects.push(...this.statusObjects);
  }

  private createCameras() {
    this.cameras.main.setBounds(0, 0, WORLD_W, WORLD_H);
    this.cameras.main.startFollow(this.playerBody, true, 0.11, 0.11);
    this.cameras.main.setRoundPixels(true);

    this.miniCamera = this.cameras.add(478, 18, 146, 88, false, 'minimap');
    this.miniCamera.setBounds(0, 0, WORLD_W, WORLD_H);
    this.miniCamera.setZoom(0.12);
    this.miniCamera.startFollow(this.playerBody, true, 0.22, 0.22);
    this.miniCamera.setBackgroundColor(0x18151d);
    this.miniCamera.ignore(this.hudObjects);

    this.mapCamera = this.cameras.add(68, 42, 504, 276, false, 'full-map');
    this.mapCamera.setBounds(0, 0, WORLD_W, WORLD_H);
    this.mapCamera.setZoom(0.285);
    this.mapCamera.centerOn(WORLD_W / 2, WORLD_H / 2);
    this.mapCamera.setBackgroundColor(0x111018);
    this.mapCamera.ignore(this.hudObjects);
    this.mapCamera.setVisible(false);
  }

  private bindMobileControls() {
    document.querySelectorAll<HTMLButtonElement>('[data-key]').forEach((button) => {
      const direction = button.dataset.key as Direction;
      const down = () => this.mobileDirections.add(direction);
      const up = () => this.mobileDirections.delete(direction);
      button.addEventListener('pointerdown', down);
      button.addEventListener('pointerup', up);
      button.addEventListener('pointercancel', up);
      button.addEventListener('pointerleave', up);
      this.cleanupMobile.push(() => {
        button.removeEventListener('pointerdown', down);
        button.removeEventListener('pointerup', up);
        button.removeEventListener('pointercancel', up);
        button.removeEventListener('pointerleave', up);
      });
    });

    document.querySelectorAll<HTMLButtonElement>('[data-action]').forEach((button) => {
      const action = button.dataset.action;
      const click = () => {
        if (action === 'interact') this.interact();
        if (action === 'jump') this.startJump();
        if (action === 'map') this.toggleMap();
      };
      button.addEventListener('click', click);
      this.cleanupMobile.push(() => button.removeEventListener('click', click));
    });
  }

  update(time: number) {
    if (Phaser.Input.Keyboard.JustDown(this.mapKey)) this.toggleMap();
    if (Phaser.Input.Keyboard.JustDown(this.statusKey)) this.toggleStatus();
    if (Phaser.Input.Keyboard.JustDown(this.escKey)) {
      if (this.mapOpen) this.toggleMap();
      if (this.statusOpen) this.toggleStatus();
    }

    if (this.mapOpen || this.statusOpen) {
      this.playerBody.setVelocity(0);
      return;
    }

    if (Phaser.Input.Keyboard.JustDown(this.jumpKey)) this.startJump();
    if (Phaser.Input.Keyboard.JustDown(this.interactKey) || Phaser.Input.Keyboard.JustDown(this.enterKey)) this.interact();

    const left = this.cursorKeys.left.isDown || this.wasd.A.isDown || this.mobileDirections.has('left');
    const right = this.cursorKeys.right.isDown || this.wasd.D.isDown || this.mobileDirections.has('right');
    const up = this.cursorKeys.up.isDown || this.wasd.W.isDown || this.mobileDirections.has('up');
    const down = this.cursorKeys.down.isDown || this.wasd.S.isDown || this.mobileDirections.has('down');

    let vx = 0;
    let vy = 0;
    const speed = 110;
    if (left) { vx -= speed; this.facing = 'left'; }
    if (right) { vx += speed; this.facing = 'right'; }
    if (up) { vy -= speed; this.facing = 'up'; }
    if (down) { vy += speed; this.facing = 'down'; }
    if (vx && vy) { vx *= 0.7071; vy *= 0.7071; }

    this.playerBody.setVelocity(vx, vy);
    this.playerSprite.setFrame(urbanCharacterFrame(8, this.facing));

    this.updateJump(time);
    this.syncPlayerVisuals();
    this.updateZone();
    this.updateInteractionPrompt();
  }

  private startJump() {
    if (this.jumpStarted) return;
    this.jumpStarted = this.time.now;
  }

  private updateJump(time: number) {
    if (!this.jumpStarted) {
      this.jumpOffset = 0;
      return;
    }
    const progress = Math.min(1, (time - this.jumpStarted) / 520);
    this.jumpOffset = Math.sin(progress * Math.PI) * 14;
    if (progress >= 1) {
      this.jumpStarted = 0;
      this.jumpOffset = 0;
    }
  }

  private syncPlayerVisuals() {
    this.playerSprite.setPosition(this.playerBody.x, this.playerBody.y - 8 - this.jumpOffset);
    this.playerSprite.setDepth(this.playerBody.y + 4);
    const shadowScale = Phaser.Math.Clamp(1 - this.jumpOffset / 30, 0.55, 1);
    this.playerShadow.setPosition(this.playerBody.x, this.playerBody.y + 5);
    this.playerShadow.setScale(shadowScale, shadowScale);
    this.playerShadow.setDepth(this.playerBody.y - 1);
  }

  private updateZone() {
    const zone = this.districts.find((d) =>
      this.playerBody.x >= d.x &&
      this.playerBody.x <= d.x + (d.width ?? 0) &&
      this.playerBody.y >= d.y &&
      this.playerBody.y <= d.y + (d.height ?? 0)
    );
    this.zoneText.setText(zone?.name ?? 'OUTER TOWN');
  }

  private nearestInteraction() {
    let nearest: { distance: number; npc?: NpcRuntime; object?: MapObject } | null = null;
    for (const npc of this.npcs) {
      const distance = Phaser.Math.Distance.Between(this.playerBody.x, this.playerBody.y, npc.object.x, npc.object.y);
      if (distance <= 44 && (!nearest || distance < nearest.distance)) nearest = { distance, npc };
    }
    for (const object of this.gameplay.filter((o) => o.type === 'interact')) {
      const distance = Phaser.Math.Distance.Between(this.playerBody.x, this.playerBody.y, object.x, object.y);
      if (distance <= 44 && (!nearest || distance < nearest.distance)) nearest = { distance, object };
    }
    return nearest;
  }

  private updateInteractionPrompt() {
    const target = this.nearestInteraction();
    if (!target) {
      this.promptText.setVisible(false);
      return;
    }
    const label = target.npc ? target.npc.object.name : target.object?.name.replaceAll('_', ' ');
    this.promptText.setText(`E / ENTER  ${label}`).setVisible(true);
  }

  private interact() {
    const target = this.nearestInteraction();
    if (!target) {
      this.showDialogue('SORA', '特に変わったものはない。');
      return;
    }
    if (target.npc) {
      const npc = target.npc;
      const lines = NPC_DIALOGUE[npc.object.name] ?? ['……。'];
      const text = lines[npc.line % lines.length];
      npc.line += 1;
      this.showDialogue(npc.object.name, text);
      return;
    }

    const action = prop(target.object as MapObject, 'action');
    const messages: Record<string, [string, string]> = {
      bar: ['BAR', 'ジュークボックスの音が聞こえる。BAR内部は次の完成単位で開放する。'],
      journal: ['JOURNAL', '町の記録が集まっている。古い住所の一件だけ、赤い印が付いている。'],
      fishing: ['RIVERSIDE', '魚影が見える。ここは釣りシステムの最初のポイントになる。'],
      'east-exit': ['EAST GATE', '街道は東へ続いている。今はまだ、その先へは行けない。']
    };
    const message = messages[action] ?? ['DOLZORE', 'まだ何も起きない。'];
    this.showDialogue(message[0], message[1]);
  }

  private showDialogue(name: string, body: string) {
    this.dialogueName.setText(name);
    this.dialogueBody.setText(body);
    this.dialogueBox.setVisible(true);
    this.dialogueName.setVisible(true);
    this.dialogueBody.setVisible(true);
  }

  private toggleMap() {
    this.mapOpen = !this.mapOpen;
    this.mapCamera.setVisible(this.mapOpen);
    this.miniCamera.setVisible(!this.mapOpen);
    this.playerBody.setVelocity(0);
  }

  private toggleStatus() {
    this.statusOpen = !this.statusOpen;
    this.statusObjects.forEach((o) => o.setVisible(this.statusOpen));
    this.playerBody.setVelocity(0);
  }
}
