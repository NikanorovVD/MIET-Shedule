import './App.css';
import Header from './Components/Header';
import { useState } from 'react';

function App() {
    const [tab, setTab] = useState('weather')
    return (
        <>
            <Header />
            
        </>
    );
}

export default App;