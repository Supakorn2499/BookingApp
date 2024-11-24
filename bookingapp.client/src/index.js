import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import '@fontsource/ibm-plex-sans-thai/300.css'  // font-light
import '@fontsource/ibm-plex-sans-thai/200.css'  // font-extralight
import '@fontsource/ibm-plex-sans-thai/100.css'  // font-thin
import '@fontsource/noto-sans-thai/300.css'
import '@fontsource/sarabun/300.css'
import '@fontsource/prompt/300.css'
import '@fontsource/kanit/300.css'

const root = ReactDOM.createRoot(document.getElementById('root'));
root.render(
  <React.StrictMode>
    <App />
  </React.StrictMode>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
