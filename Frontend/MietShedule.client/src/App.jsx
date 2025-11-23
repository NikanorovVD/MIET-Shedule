import './App.css';
import { BrowserRouter, Routes, Route, useSearchParams } from 'react-router-dom';
import SheduleSection from './Components/Shedule/SheduleSection';
import Header from './Components/Header';
import TeacherSection from './Components/Teacher/TeacherSection';
import ExportSection from './Components/Export/ExportSection';
import NearestSection from './Components/Nearest/NearestSection';
import SettingsSection from './Components/Settings/SettingsSection';
import { useSettings } from './hooks/useSettings';

function SheduleSectionWrapper() {
    const [searchParams] = useSearchParams();
    const { settings, isLoading } = useSettings();

    if (isLoading) {
        return (
            <div className="loading-container">
                <div className="spinner">
                    <div className="spinner-circle"></div>
                </div>
            </div>
        );
    }

    const group = searchParams.get('group') || settings.defaultGroup || '';
    return <SheduleSection defaultGroup={group} />;
}

function NearestSectionWrapper() {
    const [searchParams] = useSearchParams();
    const { settings, isLoading } = useSettings();

    if (isLoading) {
        return (
            <div className="loading-container">
                <div className="spinner">
                    <div className="spinner-circle"></div>
                </div>
            </div>
        );
    }

    const group = searchParams.get('group') || settings.defaultGroup || '';
    return <NearestSection defaultGroup={group} settings={settings} />;
}

function App() {
    return (
        <>
            <BrowserRouter>
                <Header />
                <Routes>
                    <Route path='/' element={<SheduleSectionWrapper />} />
                    <Route path='/teacher' element={<TeacherSection />} />
                    <Route path='/export' element={<ExportSection />} />
                    <Route path='/nearest' element={<NearestSectionWrapper />} />
                    <Route path='/prefs' element={<SettingsSection />} />
                </Routes>
            </BrowserRouter>
        </>
    );
}

export default App;