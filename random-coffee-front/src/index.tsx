import React from 'react';
import ReactDOM from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom'
import { Provider } from 'react-redux';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';

import store, { persistor } from './store';
import { PersistGate } from 'redux-persist/integration/react';


const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);


root.render(
    <BrowserRouter>
        <Provider store={store}>
            <PersistGate persistor={persistor}>
                <App/>
            </PersistGate>
        </Provider>
    </BrowserRouter>
);


reportWebVitals();
