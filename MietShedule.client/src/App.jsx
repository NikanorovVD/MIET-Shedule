import './App.css';
import { useState } from 'react';
import { BrowserRouter, Routes, Route } from 'react-router-dom';
import SheduleSection from './Components/Shedule/SheduleSection';
import Header from './Components/Header';
import TeacherSection from './Components/Teacher/TeacherSection';
import ExportSection from './Components/Export/ExportSection';

function App() {
    return (
        <>
            <BrowserRouter>
                <Header />
                <Routes>
                    <Route path='/' element={<SheduleSection />} />
                    <Route path='/teacher' element={<TeacherSection />} />
                    <Route path='/export' element={<ExportSection />} />
                </Routes>
            </BrowserRouter>
        </>
    );
}

export default App;