import './App.css';
import { useState } from 'react';
import SheduleSection from './Components/SheduleSection';
import TabSection from './Components/TabSection';
import TeacherSection from './Components/TeacherSection';
import ExportSection from './Components/ExportSection';

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
                    <TeacherSection />
                )}
                {tab == 'export' && (
                    <ExportSection />
                )}
            </main>
        </>
    );
}

export default App;