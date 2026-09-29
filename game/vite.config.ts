import { defineConfig } from 'vite';

export default defineConfig({
  base: '/Dolzore/game/',
  build: {
    outDir: '../docs/game',
    emptyOutDir: true,
    sourcemap: true,
    target: 'es2022'
  }
});
