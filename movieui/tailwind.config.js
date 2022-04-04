module.exports = {
  content: ['./public/**/*.html', './src/**/*.{js,jsx,ts,tsx,vue}'],
  theme: {
    extend: {},
    screens: {
      sm: '600px',
      md: '1024px',
      lg: '1440px',
      xl: '1920px',
    },
  },
  plugins: [],
  prefix: 'tw-',
};
