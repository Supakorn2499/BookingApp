module.exports = {
  content: [
    "./src/**/*.{js,jsx,ts,tsx}",
  ],
  theme: {
    extend: {
      fontFamily: {
        'ibm-plex-thai': ['IBM Plex Sans Thai', 'sans-serif'],
        'noto-sans-thai': ['Noto Sans Thai', 'sans-serif'],
        'sarabun': ['Sarabun', 'sans-serif'],
        'prompt': ['Prompt', 'sans-serif'],
        'kanit': ['Kanit', 'sans-serif'],
      },
    },
  },
  plugins: [
    require('@tailwindcss/forms')
  ],
}