import './App.css';
import { BrowserRouter, Routes, Route, useSearchParams } from 'react-router-dom';
import ScheduleSection from './Components/Schedule/ScheduleSection';
import Header from './Components/Header';
import TeacherSection from './Components/Teacher/TeacherSection';
import ExportSection from './Components/Export/ExportSection';
import NearestSection from './Components/Nearest/NearestSection';
import SettingsSection from './Components/Settings/SettingsSection';
import { useSettings } from './hooks/useSettings';

function ScheduleSectionWrapper() {
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
    return <ScheduleSection defaultGroup={group} />;
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
                    <Route path='/' element={<ScheduleSectionWrapper />} />
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