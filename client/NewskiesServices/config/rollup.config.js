const path = require('path');
const typescript = require('rollup-plugin-typescript2');

const sourcePath = path.resolve(__dirname, '../src');
const destPath = path.resolve(__dirname, '../lib');

export default {
    input: path.resolve(sourcePath, 'index.ts'),
    external: ['underscore', 'moment', 'axios'],
    sourcemap: true,
    exports: 'named',
    banner: '/* eslint-disable */',
    name: 'newskies-services',
    output: [
        {
            file: path.resolve(destPath, 'index.js'),
            format: 'es'
        }
    ],
    plugins: [
        typescript()
    ]
};