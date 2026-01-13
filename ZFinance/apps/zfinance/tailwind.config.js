const { createGlobPatternsForDependencies } = require('@nx/angular/tailwind');
const { join } = require('path');

/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    join(__dirname, 'src/**/!(*.stories|*.spec).{ts,html}'),
    ...createGlobPatternsForDependencies(__dirname),
    '../../../GitHub/ZLibraries/libs/**/!(*.stories|*.spec).{ts,html}'
  ],
  theme: {
    extend: {
      colors: {
        'primary': {
          50: '#e6f4ff',
          100: '#9acaed',
          200: '#59a5db',
          300: '#2585c8',
          400: '#006bb6',
          500: '#005794',
          600: '#004372',
          700: '#002f50',
          800: '#001b2f',
          900: '#00070d',
        }
      }
    },
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
};