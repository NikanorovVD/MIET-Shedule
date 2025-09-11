import { fileURLToPath, URL } from 'node:url';
import { defineConfig, loadEnv } from 'vite';
import plugin from '@vitejs/plugin-react';
import mkcert from 'vite-plugin-mkcert'

process.env = {...process.env, ...loadEnv('', process.cwd())};
console.log(process.env.VITE_APP_URL)
export default defineConfig({
    plugins: [plugin(), mkcert()],
    resolve: {
        alias: {
            '@': fileURLToPath(new URL('./src', import.meta.url))
        }
    },
    server: {
        proxy: {
            '^/Groups': {
                target: process.env.VITE_APP_URL,
                secure: false
            },
            '^/Shedule': {
                target: process.env.VITE_APP_URL,
                secure: false
            },
            '^/Teachers': {
                target: process.env.VITE_APP_URL,
                secure: false
            },
            '^/Export': {
                target: process.env.VITE_APP_URL,
                secure: false
            },

        },
        host: '0.0.0.0',
        port: 5173
    }
})
