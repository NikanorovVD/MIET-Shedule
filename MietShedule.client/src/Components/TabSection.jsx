import Button from "./Button/Button"
import "./TabSection.css"

export default function TabSection({ activeTab, OnChange }) {
    return (
        <section className="tab-section">
            <Button active={activeTab == 'shedule'} onClick={() => OnChange('shedule')}>Расписание</Button>
            <Button active={activeTab == 'teacher'} onClick={() => OnChange('teacher')}>Поиск преподавателя</Button>
            <Button active={activeTab == 'export'} onClick={() => OnChange('export')}>Экспорт расписания</Button>
            <a href="https://localhost:7056/swagger/index.html" target="_blank" rel="noopener noreferrer">
                <button type="button">API</button>
            </a>
        </section>
    )
}