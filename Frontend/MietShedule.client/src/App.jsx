import './App.css';
import { BrowserRouter, Routes, Route, useSearchParams } from 'react-router-dom';
import SheduleSection from './Components/Shedule/SheduleSection';
import Header from './Components/Header';
import TeacherSection from './Components/Teacher/TeacherSection';
import ExportSection from './Components/Export/ExportSection';

function SheduleSectionWrapper() {
    const [searchParams] = useSearchParams();
    const group = searchParams.get('group') || '';
    return <SheduleSection defaultGroup={group} />;
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
                </Routes>
            </BrowserRouter>
        </>
    );
}

export default App;