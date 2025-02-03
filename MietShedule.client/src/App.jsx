import './App.css';
import Header from './Components/Header';
import { useState } from 'react';
import SheduleSection from './Components/SheduleSection';
import TabSection from './Components/TabSection';

function App() {
    const [tab, setTab] = useState('shedule')
    return (
        <>
            <TabSection activeTab={tab} OnChange={(current) => setTab(current)} />
            <main>
                {tab == 'shedule' && (
                    < SheduleSection />
                )}
                {tab == 'teacher' && (
                    <></>
                )}
            </main>
        </>
    );
}

export default App;