import React from 'react';
import ReactDOM from 'react-dom';
import 'react-datepicker/dist/react-datepicker.css';
import './index.css';
import theme from './theme';
import { ColorModeScript } from '@chakra-ui/react';
import App from './App';

ReactDOM.render(
    <>
        <ColorModeScript initialColorMode={theme.config.initialColorMode} />
        <React.StrictMode>
            <App />
        </React.StrictMode>
    </>,
    document.getElementById('root')
);
