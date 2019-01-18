const path = require('path');
const typescript = require('rollup-plugin-typescript2');
const commonjs = require('rollup-plugin-commonjs');

const sourcePath = path.resolve(__dirname, '../src');
const destPath = path.resolve(__dirname, '../lib');

export default {
    input: path.resolve(sourcePath, 'index.ts'),
    external: ['underscore', 'newskies-services', 'moment', 'ajv'],
    sourcemap: true,
    exports: 'named',
    banner: '/* eslint-disable */',
    name: 'flyadeal-services',
    output: [
        {
            file: path.resolve(destPath, 'index.js'),
            format: 'es'
        }
    ],
    plugins: [
        typescript(),
        commonjs()
    ]
};